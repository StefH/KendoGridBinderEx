using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KendoGridBinderEx.Containers;

namespace KendoGridBinderEx.ModelBinder
{
    public static class FilterHelper
    {
        public static FilterObjectWrapper GetFilterObjects(NameValueCollection queryString, IList<string> filterKeys, string filterLogic)
        {
            var list = new List<FilterObject>();

            var fieldKeys = filterKeys.Where(x => x.Contains("field"));

            foreach (int index in GetIndexArr(fieldKeys))
            {
                var group = filterKeys.Where(x => GetFilterIndex(x) == index && !x.Contains("logic")).ToList();
                var field1 = group.First(g => g.Contains("field"));
                var operator1 = group.First(g => g.Contains("operator"));
                var value1 = group.First(g => g.Contains("value"));
                var ignoreCase1 = group.FirstOrDefault(g => g.Contains("ignoreCase"));

                var filterObject = new FilterObject
                {
                    Field1 = queryString[field1],
                    Operator1 = queryString[operator1],
                    Value1 = queryString[value1],
                    IgnoreCase1 = ignoreCase1 != null ? queryString[ignoreCase1] : null
                };

                if (group.Count == 6 || group.Count == 8)
                {
                    var field2 = group.Last(g => g.Contains("field"));
                    var operator2 = group.Last(g => g.Contains("operator"));
                    var value2 = group.Last(g => g.Contains("value"));
                    var ignoreCase2 = group.LastOrDefault(g => g.Contains("ignoreCase"));

                    filterObject.Field2 = queryString[field2];
                    filterObject.Operator2 = queryString[operator2];
                    filterObject.Value2 = queryString[value2];
                    filterObject.IgnoreCase2 = ignoreCase2 != null ? queryString[ignoreCase2] : null;
                    filterObject.Logic = GetFilterLogic(queryString, filterKeys, index, "logic");
                }

                list.Add(filterObject);
            }

            return new FilterObjectWrapper(filterLogic, list);
        }

        private static IEnumerable<int> GetIndexArr(IEnumerable<string> fieldKeys)
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

        private static int GetFilterIndex(string qString)
        {
            var splitArr = qString.Split('[');

            foreach (var s in splitArr)
            {
                int result;
                var strippedVal = s.Replace("]", "");
                if (int.TryParse(strippedVal, out result))
                {
                    return result;
                }
            }

            return 0;
        }

        private static string GetFilterLogic(NameValueCollection queryString, IEnumerable<string> filterKeys, int index, string type)
        {
            var fieldKeys = filterKeys.Where(x => x.Contains(type));

            foreach (var fieldKey in fieldKeys)
            {
                var filterIndex = GetFilterIndex(fieldKey);
                if (filterIndex == index)
                {
                    return queryString[fieldKey];
                }
            }

            return null;
        }
    }
}