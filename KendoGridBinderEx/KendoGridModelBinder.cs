using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using KendoGridBinderEx.Containers;
using KendoGridBinderEx.Extensions;

namespace KendoGridBinderEx
{
    public class KendoGridModelBinder : IModelBinder
    {
        private static readonly Regex GroupRegex = new Regex(@"^group\[(\d*)\]\[(field|dir)\]$", RegexOptions.IgnoreCase);
        private static readonly Regex GroupAggregateRegex = new Regex(@"^group\[(\d*)\]\[aggregates\]\[(\d*)\]\[(field|aggregate)\]$", RegexOptions.IgnoreCase);

        private HttpRequestBase _request;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            _request = controllerContext.HttpContext.Request;
            var requestKeys = GetRequestKeys().ToList();

            var take = GetQueryValue("take", (int?)null);
            var page = GetQueryValue("page", (int?)null);
            var skip = GetQueryValue("skip", (int?)null);
            var pageSize = GetQueryValue("pageSize", (int?)null);

            var filterLogic = GetQueryStringValue("filter[logic]");
            var filterKeys = requestKeys.Where(x => x.StartsWith("filter") && x != "filter[logic]").ToList();
            var filtering = GetFilterObjects(filterKeys, filterLogic);

            var sortKeys = requestKeys.Where(x => x.StartsWith("sort")).ToList();
            var sorting = GetSortObjects(sortKeys);
            var groups = GetGroupObjects(requestKeys.Where(k => k.StartsWith("group")));

            return new KendoGridRequest
            {
                Take = take,
                Skip = skip,
                Page = page,
                PageSize = pageSize,
                FilterObjectWrapper = filtering,
                SortObjects = sorting,
                GroupObjects = groups
            };
        }

        private IEnumerable<GroupObject> GetGroupObjects(IEnumerable<string> requestKeys)
        {
            var dict = new Dictionary<int, GroupObject>();

            var enumerable = requestKeys as IList<string> ?? requestKeys.ToList();
            foreach (var x in enumerable
                .Select(k => new { Key = k, Match = GroupRegex.Match(k) })
                .Where(x => x.Match.Success)
            )
            {
                var groupId = int.Parse(x.Match.Groups[1].Value);
                if (!dict.ContainsKey(groupId))
                {
                    dict.Add(groupId, new GroupObject());
                }

                var value = GetQueryStringValue(x.Key);
                if (x.Key.Contains("field"))
                {
                    dict[groupId].Field = value;
                }
                if (x.Key.Contains("dir"))
                {
                    dict[groupId].Direction = !string.IsNullOrEmpty(value) ? value : "asc";
                }
            }

            foreach (var groupObject in dict)
            {
                var aggregates = new Dictionary<int, AggregateObject>();
                var aggregateKey = string.Format("group[{0}][aggregates]", groupObject.Key);

                foreach (var x in enumerable
                    .Where(k => k.StartsWith(aggregateKey))
                    .Select(k => new { Key = k, Match = GroupAggregateRegex.Match(k) })
                    .Where(x => x.Match.Success)
                    )
                {
                    var aggregateId = int.Parse(x.Match.Groups[2].Value);
                    if (!aggregates.ContainsKey(aggregateId))
                    {
                        aggregates.Add(aggregateId, new AggregateObject());
                    }

                    var value = GetQueryStringValue(x.Key);
                    if (x.Key.Contains("field"))
                    {
                        aggregates[aggregateId].Field = value;
                    }
                    if (x.Key.Contains("aggregate"))
                    {
                        aggregates[aggregateId].Aggregate = value;
                    }
                }

                groupObject.Value.AggregateObjects = aggregates.Values.ToList();
            }

            return dict.Any() ? dict.Values.ToList() : null;
        }

        private IEnumerable<SortObject> GetSortObjects(IEnumerable<string> sortKeys)
        {
            var list = new List<SortObject>();

            var enumerable = sortKeys as IList<string> ?? sortKeys.ToList();
            var fields = enumerable.Where(sk => sk.Contains("field")).Select(GetQueryStringValue).ToList();
            var directions = enumerable.Where(sk => sk.Contains("dir")).Select(GetQueryStringValue).ToList();

            foreach (string field in fields)
            {
                var index = fields.IndexOf(field);
                var direction = directions[index];
                var obj = new SortObject(field, direction);
                list.Add(obj);
            }

            return list;
        }

        private FilterObjectWrapper GetFilterObjects(IList<string> filterKeys, string filterLogic)
        {
            var list = new List<FilterObject>();

            var fieldKeys = filterKeys.Where(x => x.Contains("field"));

            foreach (int index in GetIndexArr(fieldKeys))
            {
                var group = filterKeys.Where(x => GetFilterIndex(x) == index && !x.Contains("logic")).ToList();

                var filterObject = new FilterObject
                {
                    Field1 = GetQueryStringValue(group[0]),
                    Operator1 = GetQueryStringValue(group[1]),
                    Value1 = GetQueryStringValue(group[2])
                };

                if (group.Count == 6)
                {
                    filterObject.Field2 = GetQueryStringValue(group[3]);
                    filterObject.Operator2 = GetQueryStringValue(group[4]);
                    filterObject.Value2 = GetQueryStringValue(group[5]);
                    filterObject.Logic = GetValue(filterKeys, index, "logic");
                }

                list.Add(filterObject);
            }

            return new FilterObjectWrapper(filterLogic, list);
        }

        private IEnumerable<int> GetIndexArr(IEnumerable<string> fieldKeys)
        {
            var list = new List<int>();

            foreach (var fieldKey in fieldKeys)
            {
                var index = GetFilterIndex(fieldKey);

                var existing = list.Where(x => x == index);

                if (!existing.Any())
                {
                    list.Add(index);
                }
            }

            return list;
        }

        public int GetFilterIndex(string qString)
        {
            var splitArr = qString.Split('[');

            foreach (var s in splitArr)
            {
                float result;
                var strippedVal = s.Replace("]", "");
                if (float.TryParse(strippedVal, out result))
                {
                    return (int)result;
                }
            }

            return 0;
        }

        public string GetValue(IList<string> filterKeys, int index, string type)
        {
            var fieldKeys = filterKeys.Where(x => x.Contains(type));

            foreach (var fieldKey in fieldKeys)
            {
                var filterIndex = GetFilterIndex(fieldKey);
                if (filterIndex == index)
                {
                    return GetQueryStringValue(fieldKey);
                }
            }

            return null;
        }

        private string GetQueryStringValue(string key)
        {
            return _request.HttpMethod.ToUpper() == "POST" ? _request.Form[key] : _request.QueryString[key];
        }

        private T GetQueryValue<T>(string key, T defaultValue)
        {
            var stringValue = GetQueryStringValue(key);

            return !string.IsNullOrEmpty(stringValue) ? (T)TypeExtensions.ChangeType(stringValue, typeof(T)) : defaultValue;
        }

        private IEnumerable<string> GetRequestKeys()
        {
            return _request.HttpMethod.ToUpper() == "POST" ? _request.Form.AllKeys : _request.QueryString.AllKeys;
        }
    }
}