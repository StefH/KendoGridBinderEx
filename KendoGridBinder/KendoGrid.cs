using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using KendoGridBinder.Containers;

namespace KendoGridBinder
{
    public class KendoGrid<TModel> : KendoGrid<TModel, TModel>
    {
        public KendoGrid(KendoGridRequest request, IQueryable<TModel> query)
            : base(request, query, null, null)
        {
        }

        public KendoGrid(KendoGridRequest request, IEnumerable<TModel> list)
            : this(request, list.AsQueryable())
        {
        }

        public KendoGrid(IEnumerable<TModel> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }

    public abstract class KendoGrid<TEntity, TViewModel>
    {
        private readonly Func<IQueryable<TEntity>, IEnumerable<TViewModel>> _defaultConversion = query => query.Cast<TViewModel>();
        private readonly Func<IQueryable<TEntity>, IEnumerable<TViewModel>> _conversion;
        private readonly Dictionary<string, string> _mappings;
        private readonly IQueryable<TEntity> _query;

        public object Groups { get; set; }
        public IEnumerable<TViewModel> Data { get; set; }
        public int Total { get; set; }

        protected KendoGrid(KendoGridRequest request, IQueryable<TEntity> query,
            Dictionary<string, string> mappings,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion)
        {
            _mappings = mappings;
            _conversion = conversion ?? _defaultConversion;

            Total = query.Count();

            var tempQuery = ApplyFiltering(query, request);

            if (request.GroupObjects != null)
            {
                Groups = ApplyGroupingAndSorting(tempQuery, request);

                _query = null;
                Data = null;
            }
            else
            {
                tempQuery = ApplySorting(tempQuery, request);

                // Paging
                if (request.Skip.HasValue && request.Skip > 0)
                {
                    tempQuery = tempQuery.Skip(request.Skip.Value);
                }
                if (request.Take.HasValue && request.Take > 0)
                {
                    tempQuery = tempQuery.Take(request.Take.Value);
                }

                _query = tempQuery;

                Data = _conversion(_query).ToList();
                Groups = null;
            }
        }

        protected KendoGrid(KendoGridRequest request, IEnumerable<TEntity> entities,
            Dictionary<string, string> mappings,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion
            )
            : this(request, entities.AsQueryable(), mappings, conversion)
        {
        }

        protected KendoGrid(IEnumerable<TViewModel> list, int totalCount)
        {
            Data = list;
            Total = totalCount;
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _query;
        }

        private IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, KendoGridRequest request)
        {
            var filtering = GetFiltering(request);
            return filtering != null ? query.Where(filtering) : query;
        }

        private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, KendoGridRequest request)
        {
            var sorting = GetSorting(request) ?? query.ElementType.FirstSortableProperty();
            return query.OrderBy(sorting);
        }

        private void Process(IEnumerable<GroupObject> groupByFields, IDictionary<string, object> values, IGrouping<object, TEntity> grouping, object aggregates, List<KendoGroup> kendoGroups)
        {
            var groupObjects = groupByFields as IList<GroupObject> ?? groupByFields.ToList();
            bool isLast = groupObjects.Count() == 1;

            var groupObject = groupObjects.First();

            var kendoGroup = new KendoGroup
            {
                field = groupObject.Field,
                aggregates = aggregates,
                value = values[groupObject.Field],
                hasSubgroups = !isLast,
            };

            if (isLast)
            {
                kendoGroup.items = _conversion(grouping.AsQueryable()).ToList();
            }
            else
            {
                var newGroupByFields = new List<GroupObject>(groupObjects);
                newGroupByFields.Remove(groupObject);

                var newList = new List<KendoGroup>();
                Process(newGroupByFields.ToArray(), values, grouping, aggregates, newList);
                kendoGroup.items = newList;
            }

            kendoGroups.Add(kendoGroup);
        }

        protected IEnumerable<KendoGroup> ApplyGroupingAndSorting(IQueryable<TEntity> query, KendoGridRequest request)
        {
            bool hasAggregates = request.GroupObjects.Any(g => g.AggregateObjects.Any());
            var aggregatesExpression = hasAggregates ?
               string.Format(",new ({0}) as Aggregates", string.Join(", ", request.GroupObjects.SelectMany(g => g.AggregateObjects.Select(a => a.LinqAggregate)))) :
               string.Empty;

            var newSort = request.SortObjects.ToList();
            /*var groupByFieldPresentInSort = newSort.FirstOrDefault(s => s.Field == groupByObject.Field);
            if (groupByFieldPresentInSort != null)
            {
                newSort.Remove(groupByFieldPresentInSort);
            }*/
            bool hasSortObjects = newSort.Any();

            var groupByOrderByFieldsExpressionX = hasSortObjects ?
                "," + string.Join(",", newSort.Select(s => string.Format("{0} as OrderBy__{1}", ReplaceVM2E(s.Field).Split('.').Last(), s.Field))) :
                string.Empty;

            var groupByExpressionX = string.Format("new (new ({0}{1}) as GroupByFields)",
                string.Join(",", request.GroupObjects.Select(s => string.Format("{0} as {1}", ReplaceVM2E(s.Field), s.Field))), groupByOrderByFieldsExpressionX);

            var selectExpressionBeforeOrderByX = string.Format("new (Key.GroupByFields, it as Grouping{0})", aggregatesExpression);

            var orderByFieldsExpression = hasSortObjects ?
                string.Join(",", newSort.Select(s => string.Format("GroupByFields.OrderBy__{0} {1}", s.Field, s.Direction))) :
                "GroupByFields." + request.GroupObjects.First().Field;

            var selectExpressionAfterOrderByX = string.Format("new (GroupByFields, Grouping{0})", hasAggregates ? ",Aggregates" : string.Empty);

            var groupByQuery = query.GroupBy(groupByExpressionX, "it");
            var selectQuery = groupByQuery.Select(selectExpressionBeforeOrderByX);
            var orderByQuery = selectQuery.OrderBy(orderByFieldsExpression);
            var tempQuery = orderByQuery.Select(selectExpressionAfterOrderByX, typeof(TEntity));

            // Paging
            if (request.Skip.HasValue && request.Skip > 0)
            {
                tempQuery = tempQuery.Skip(request.Skip.Value);
            }
            if (request.Take.HasValue && request.Take > 0)
            {
                tempQuery = tempQuery.Take(request.Take.Value);
            }

            var list = new List<KendoGroup>();
            foreach (DynamicClass item in tempQuery)
            {
                var grouping = item.GetPropertyValue<IGrouping<object, TEntity>>("Grouping");
                var groupByDictionary = item.GetPropertyValue("GroupByFields").ToDictionary();
                var aggregates = item.GetAggregatesAsDictionary();

                Process(request.GroupObjects, groupByDictionary, grouping, aggregates, list);
            }

            return list;
        }
        /*
        private class GroupResult
        {
            public IGrouping<object, TEntity> Grouping { get; set; }
            public object GroupByFields { get; set; }
            public object Aggregates { get; set; }

            public object AggregatesAsDictionary
            {
                get
                {
                    return Aggregates != null ? Aggregates.GetType().GetProperties().Where(p => p.Name.Contains("__"))

                                // Split on __ to get the prefix and the field
                                .Select(prop => new { PropertyInfo = prop, Data = prop.Name.Split(new[] {"__"}, StringSplitOptions.None)})

                                // Return the Fieldname, Prefix and the the value ('First' , 'field' , 'First')
                                .Select(x => new {Fieldname = x.Data.Last(),Prefix = x.Data.First(),Value = x.PropertyInfo.GetValue(Aggregates, null)})

                                // Group by the field
                                .GroupBy(groupBy => groupBy.Fieldname)

                                // and return an anonymous dictionary
                                .ToDictionary(x => x.Key, y => y.ToDictionary(k => k.Prefix, v => v.Value)) : null;
                }
            }
        }*/

        protected string GetSorting(KendoGridRequest request)
        {
            var expression = string.Join(",", request.SortObjects.Select(s => ReplaceVM2E(s.Field) + " " + s.Direction));

            return expression.Length > 1 ? expression : null;
        }

        protected string GetFiltering(KendoGridRequest request)
        {
            var finalExpression = string.Empty;

            foreach (var filterObject in request.FilterObjectWrapper.FilterObjects)
            {
                filterObject.Field1 = ReplaceVM2E(filterObject.Field1);
                filterObject.Field2 = ReplaceVM2E(filterObject.Field2);

                if (finalExpression.Length > 0)
                {
                    finalExpression += " " + request.FilterObjectWrapper.LogicToken + " ";
                }

                if (filterObject.IsConjugate)
                {
                    var expression1 = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    var expression2 = GetExpression(filterObject.Field2, filterObject.Operator2, filterObject.Value2);
                    var combined = string.Format("({0} {1} {2})", expression1, filterObject.LogicToken, expression2);
                    finalExpression += combined;
                }
                else
                {
                    var expression = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    finalExpression += expression;
                }
            }

            return finalExpression.Length == 0 ? "true" : finalExpression;
        }

        protected string ReplaceVM2E(string field)
        {
            return (_mappings != null && field != null && _mappings.ContainsKey(field)) ? _mappings[field] : field;
        }

        protected string ReplaceE2VM(string field)
        {
            return (_mappings != null && field != null && _mappings.ContainsValue(field)) ? _mappings.First(kvp => kvp.Value == field).Key : field;
        }

        protected static string GetPropertyType(Type type, string field)
        {
            foreach (var part in field.Split('.'))
            {
                if (type == null)
                {
                    return null;
                }

                var info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }

                type = info.PropertyType;
            }

            return type.Name.ToLower();
        }

        protected static string GetExpression(string field, string op, string param)
        {
            var dataType = GetPropertyType(typeof(TEntity), field);
            var caseMod = string.Empty;

            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()";
            }

            if (dataType == "datetime")
            {
                var date = DateTime.Parse(param);
                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            string exStr;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}{2}.Contains({1})", field, param, caseMod);
                    break;
					
				case "doesnotcontain":
                    exStr = string.Format("!{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;
					
                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;
					
                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;
					
                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;
					
                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;
					
                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }
    }
}
