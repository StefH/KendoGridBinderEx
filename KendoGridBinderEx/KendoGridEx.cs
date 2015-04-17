using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KendoGridBinderEx.AutoMapperExtensions;
using KendoGridBinderEx.Containers;
using KendoGridBinderEx.Containers.Json;
using KendoGridBinderEx.Extensions;

namespace KendoGridBinderEx
{
    public class KendoGridEx<TModel> : KendoGridEx<TModel, TModel>
    {
        public KendoGridEx(KendoGridBaseRequest request, IQueryable<TModel> query)
            : base(request, query, null, null)
        {
        }

        public KendoGridEx(KendoGridBaseRequest request, IEnumerable<TModel> list)
            : this(request, list.AsQueryable())
        {
        }

        public KendoGridEx(IEnumerable<TModel> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }

    public class KendoGridEx<TEntity, TViewModel>
    {
        private readonly Func<IQueryable<TEntity>, IEnumerable<TViewModel>> _conversion;
        private readonly Dictionary<string, string> _mappings;
        private readonly IQueryable<TEntity> _query;

        public object Groups { get; set; }
        public IEnumerable<TViewModel> Data { get; set; }
        public object Aggregates { get; set; }
        public int Total { get; set; }

        public KendoGridEx(KendoGridBaseRequest request,
            IQueryable<TEntity> query,
            IEnumerable<string> includes = null,
            Dictionary<string, string> mappings = null,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
            bool canUseAutoMapperProjection = true)
        {
            _mappings = AutoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings);
            _conversion = conversion ?? GetAutoMapperConversion(canUseAutoMapperProjection);

            IList<string> includesAsList = null;
            if (includes != null)
            {
                includesAsList = includes.ToList();
            }

            var tempQuery = request.FilterObjectWrapper != null ? ApplyFiltering(query, request.FilterObjectWrapper) : query;
            Total = tempQuery.Count();

            if (request.AggregateObjects != null)
            {
                Aggregates = ApplyAggregates(tempQuery, includesAsList, request);
            }

            if (request.GroupObjects != null)
            {
                Groups = ApplyGroupingAndSorting(tempQuery, includesAsList, request);

                _query = null;
                Data = null;
            }
            else
            {
                tempQuery = ApplySorting(tempQuery, request.SortObjects);

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

        protected KendoGridEx(KendoGridBaseRequest request, IEnumerable<TEntity> entities,
            Dictionary<string, string> mappings,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion
            )
            : this(request, entities.AsQueryable(), null, mappings, conversion)
        {
        }

        protected KendoGridEx(IEnumerable<TViewModel> list, int totalCount)
        {
            Data = list;
            Total = totalCount;
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _query;
        }

        private IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, FilterObjectWrapper filter)
        {
            var filtering = GetFiltering(filter);
            return filtering != null ? query.Where(filtering) : query;
        }

        private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, IEnumerable<SortObject> sortObjects)
        {
            var sorting = GetSorting(sortObjects) ?? query.ElementType.FirstSortableProperty();
            return query.OrderBy(sorting);
        }

        protected object ApplyAggregates(IQueryable<TEntity> query, IList<string> includes, KendoGridBaseRequest request)
        {
            // In case of average, sum, min or max: convert it to sum(TEntity__.XXX) as sum_XXX
            // In case of count, convert it to count() as count_XXX
            var convertedAggregateObjects = request.AggregateObjects
                .Select(a => a.GetLinqAggregate(MapFieldfromViewModeltoEntity))
                .Distinct()
                .ToList();

            // new (new (sum(TEntity__.EmployeeNumber) as sum__Number) as Aggregates)
            string aggregatesExpression = string.Format("new (new ({0}) as Aggregates)", string.Join(", ", convertedAggregateObjects));

            string includesX = string.Empty;

            if (includes != null && includes.Any())
            {
                includesX = ", " + string.Join(", ", includes.Select(i => "it." + i + " as TEntity__" + i.Replace(".", "_")));
            }

            // Execute the Dynamic Linq "Select" to get TEntity__ (and includes if needed). Also add a fake __Key__ property to allow grouping
            // Example : new ("__Key__" as __Key__, it AS TEntity__, it.Company as TEntity__Company, it.Company.MainCompany as TEntity__Company_MainCompany, it.Country as TEntity__Country)
            var selectTEntityQuery = query.Select(string.Format("new (\"__Key__\" as __Key__, it AS TEntity__{0})", includesX));

            // Group by the __Key__ to allow aggregates to be calculated
            var groupByQuery = selectTEntityQuery.GroupBy("__Key__");

            // Execute the Dynamic Linq "Select" to add the aggregates
            // Example : new (new (Sum(TEntity__.Id) as sum__Id, Min(TEntity__.Id) as min__Id, Max(TEntity__.Id) as max__Id, Count() as count__Id, Average(TEntity__.Id) as average__Id) as Aggregates)
            var aggregatesQuery = groupByQuery.Select(aggregatesExpression);

            // Try to get first result, cast to DynamicClass as use helper method to convert this to correct response
            var aggregates = (aggregatesQuery.FirstOrDefault() as DynamicClass).GetAggregatesAsDictionary();

            return aggregates;
        }

        protected IEnumerable<KendoGroup> ApplyGroupingAndSorting(IQueryable<TEntity> query, IList<string> includes, KendoGridBaseRequest request)
        {
            bool hasAggregates = request.GroupObjects.Any(g => g.AggregateObjects.Any());
            string aggregatesExpression = string.Empty;

            if (hasAggregates)
            {
                // In case of sum, min or max: convert it to sum(TEntity__.XXX) as sum_XXX
                // In case of count, convert it to count() as count_XXX
                var convertedAggregateObjects = request.GroupObjects
                    .SelectMany(g => g.AggregateObjects)
                    .Select(a => a.GetLinqAggregate(MapFieldfromViewModeltoEntity))
                    .Distinct()
                    .ToList();

                // , new (sum(TEntity__.EmployeeNumber) as sum__Number) as Aggregates
                aggregatesExpression = string.Format(", new ({0}) as Aggregates", string.Join(", ", convertedAggregateObjects));
            }

            var sort = request.SortObjects != null ? request.SortObjects.ToList() : new List<SortObject>();
            bool hasSortObjects = sort.Any();

            // List[0] = LastName as Last
            var groupByFields = request.GroupObjects.Select(s => string.Format("{0} as {1}", MapFieldfromViewModeltoEntity(s.Field), s.Field)).ToList();

            // new (new (LastName as Last) as GroupByFields)
            var groupByExpressionX = string.Format("new (new ({0}) as GroupByFields)", string.Join(",", groupByFields));

            // new (Key.GroupByFields, it as Grouping, new (sum(TEntity__.EmployeeNumber) as sum__TEntity___EmployeeNumber) as Aggregates)
            var selectExpressionBeforeOrderByX = string.Format("new (Key.GroupByFields, it as Grouping {0})", aggregatesExpression);
            var groupSort = string.Join(",", request.GroupObjects.ToList().Select(s => string.Format("{0} {1}", MapFieldfromViewModeltoEntity(s.Field), s.Direction)));

            // Adam Downs moved sort to items vs group
            var orderByFieldsExpression = hasSortObjects ?
                string.Join(",", sort.Select(s => string.Format("{0} {1}", MapFieldfromViewModeltoEntity(s.Field), s.Direction))) :
                MapFieldfromViewModeltoEntity(request.GroupObjects.First().Field);

            // new (GroupByFields, Grouping, Aggregates)
            var selectExpressionAfterOrderByX = string.Format("new (GroupByFields, Grouping{0})", hasAggregates ? ", Aggregates" : string.Empty);

            // 
            string includesX = string.Empty;
            if (includes != null && includes.Any())
            {
                includesX = ", " + string.Join(", ", includes.Select(i => "it." + i + " as TEntity__" + i.Replace(".", "_")));
            }

            var limitedQuery = query.OrderBy(string.Join(",", new[] { groupSort, orderByFieldsExpression }));

            // Execute the Dynamic Linq for Paging
            if (request.Skip.HasValue && request.Skip > 0)
            {
                limitedQuery = limitedQuery.Skip(request.Skip.Value);
            }
            if (request.Take.HasValue && request.Take > 0)
            {
                limitedQuery = limitedQuery.Take(request.Take.Value);
            }

            // Execute the Dynamic Linq "GroupBy"
            var groupByQuery = limitedQuery.GroupBy(groupByExpressionX, string.Format("new (it AS TEntity__{0})", includesX));

            // Execute the Dynamic Linq "Select"
            var selectQuery = groupByQuery.Select(selectExpressionBeforeOrderByX);

            // Execute the Dynamic Linq "OrderBy"
            var orderByQuery = selectQuery.OrderBy(string.Join(",", request.GroupObjects
                .Select(s => string.Format("GroupByFields.{0} {1}", s.Field, s.Direction)).ToList()));

            // Execute the Dynamic Linq "Select" to get back the TEntity objects
            var tempQuery = orderByQuery.Select(selectExpressionAfterOrderByX, typeof(TEntity));

            // Create a valid List<KendoGroup> object
            var list = new List<KendoGroup>();
            foreach (DynamicClass item in tempQuery)
            {
                var grouping = item.GetPropertyValue<IGrouping<object, object>>("Grouping");
                var groupByDictionary = item.GetPropertyValue("GroupByFields").ToDictionary();
                var aggregates = item.GetAggregatesAsDictionary();

                Process(request.GroupObjects, groupByDictionary, grouping, aggregates, list);
            }

            return list;
        }

        private void Process(IEnumerable<GroupObject> groupByFields, IDictionary<string, object> values, IEnumerable<object> grouping, object aggregates, List<KendoGroup> kendoGroups)
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
                var entities = grouping.Select<TEntity>("TEntity__").AsQueryable();
                kendoGroup.items = _conversion(entities).ToList();
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

        protected string GetSorting(IEnumerable<SortObject> sortObjects)
        {
            if (sortObjects == null)
            {
                return null;
            }

            var expression = string.Join(",", sortObjects.Select(s => MapFieldfromViewModeltoEntity(s.Field) + " " + s.Direction));
            return expression.Length > 1 ? expression : null;
        }

        protected string GetFiltering(FilterObjectWrapper filter)
        {
            var finalExpression = string.Empty;

            foreach (var filterObject in filter.FilterObjects)
            {
                filterObject.Field1 = MapFieldfromViewModeltoEntity(filterObject.Field1);
                filterObject.Field2 = MapFieldfromViewModeltoEntity(filterObject.Field2);

                if (finalExpression.Length > 0)
                {
                    finalExpression += " " + filter.LogicToken + " ";
                }

                if (filterObject.IsConjugate)
                {
                    var expression1 = filterObject.GetExpression1<TEntity>();
                    var expression2 = filterObject.GetExpression2<TEntity>();
                    var combined = string.Format("({0} {1} {2})", expression1, filterObject.LogicToken, expression2);
                    finalExpression += combined;
                }
                else
                {
                    var expression = filterObject.GetExpression1<TEntity>();
                    finalExpression += expression;
                }
            }

            return finalExpression.Length == 0 ? "true" : finalExpression;
        }

        protected string MapFieldfromViewModeltoEntity(string field)
        {
            return (_mappings != null && field != null && _mappings.ContainsKey(field)) ? _mappings[field] : field;
        }

        protected string MapFieldfromEntitytoViewModel(string field)
        {
            return (_mappings != null && field != null && _mappings.ContainsValue(field)) ? _mappings.First(kvp => kvp.Value == field).Key : field;
        }

        public static Func<IQueryable<TEntity>, IEnumerable<TViewModel>> GetAutoMapperConversion(bool canUseAutoMapperProjection = true)
        {
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion;

            if (AutoMapperUtils.SameTypes<TEntity, TViewModel>())
            {
                conversion = q => q.Cast<TViewModel>().ToList();
            }
            else
            {
                // https://github.com/AutoMapper/AutoMapper/issues/362
                // The idea behind Project().To is to be passed to a query provider like EF or NHibernate that will then do the appropriate SQL creation, 
                // not necessarily that the in-memory-execution will work.
                // Project.To has a TON of limitations as it's built explicitly for real query providers, and only does things like MapFrom etc.
                // To put it another way - don't use Project.To unless you're passing that to EF or NH or another DB query provider that knows what to do with the expression tree.
                if (canUseAutoMapperProjection)
                {
                    conversion = q => Enumerable.AsEnumerable(q.Project().To<TViewModel>());
                }
                else
                {
                    conversion = Mapper.Map<IEnumerable<TViewModel>>;
                }
            }

            return conversion;
        }
    }
}