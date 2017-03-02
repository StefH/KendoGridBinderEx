using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KendoGridBinder.Containers;
using KendoGridBinder.Containers.Json;
using Newtonsoft.Json;

namespace KendoGridBinder.ModelBinder
{
    public static class FilterHelper
    {
        public static FilterObjectWrapper Parse(NameValueCollection queryString)
        {
            // If there is a filter query parameter, try to parse the value as json
            if (queryString.AllKeys.Contains("filter"))
            {
                string filterAsJson = queryString["filter"];
                if (!string.IsNullOrEmpty(filterAsJson))
                {
                    return GetFilterObjects(filterAsJson);
                }
            }
            else
            {
                // Just get the filters the old way
                string filterLogic = queryString["filter[logic]"];

                if (filterLogic != null)
                {
                    var filterKeys = queryString.AllKeys.Where(x => x.StartsWith("filter") && x != "filter[logic]").ToList();
                    return GetFilterObjects(queryString, filterKeys, filterLogic);
                }
            }

            return null;
        }

        /*
        "filter":{
            "logic":"and",
            "filters":[
                {
                "field":"LastName",
                "operator":"eq",
                "value":"d"
                },
                {
                "field":"LastName",
                "operator":"eq",
                "value":"ddd"
                },
                {
                "field":"Email",
                "operator":"startswith",
                "value":"d"
                },
                {
                "logic":"or",
                "filters":[
                    {
                        "field":"FirstName",
                        "operator":"contains",
                        "value":"s"
                    },
                    {
                        "field":"FirstName",
                        "operator":"contains",
                        "value":"d"
                    }
                ]
                }
            ]
        }
        */
        public static FilterObjectWrapper MapRootFilter(Filter mainFilter)
        {
            if (mainFilter != null)
            {
                var filters = new List<FilterObject>();

                foreach (var filter in mainFilter.Filters)
                {
                    FilterObject filterObject;
                    if (filter.Logic == "or" || filter.Logic == "and")
                    {
                        var filter1 = filter.Filters.First();
                        filterObject = Map(filter1);

                        var filter2 = filter.Filters.Last();
                        filterObject.Field2 = filter2.Field;
                        filterObject.Operator2 = filter2.Operator;
                        filterObject.Value2 = filter2.Value;
                        filterObject.IgnoreCase2 = filter2.IgnoreCase;
                        filterObject.Logic = filter.Logic;
                    }
                    else
                    {
                        filterObject = Map(filter);
                    }

                    filters.Add(filterObject);
                }

                return new FilterObjectWrapper(mainFilter.Logic, filters);
            }

            return null;
        }

        private static FilterObject Map(Filter filter)
        {
            return new FilterObject
            {
                Field1 = filter.Field,
                Operator1 = filter.Operator,
                Value1 = filter.Value,
                IgnoreCase1 = filter.IgnoreCase,
                Logic = filter.Logic
            };
        }

        // "filter":{"logic":"and","filters":[{"field":"LastName","operator":"contains","value":"s"}]}
        public static FilterObjectWrapper GetFilterObjects(string filterAsJson)
        {
            var parseResult = JsonConvert.DeserializeObject<Filter>(filterAsJson);

            return parseResult != null ? MapRootFilter(parseResult) : null;
        }

        private static FilterObjectWrapper GetFilterObjects(NameValueCollection queryString, IList<string> filterKeys, string filterLogic)
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