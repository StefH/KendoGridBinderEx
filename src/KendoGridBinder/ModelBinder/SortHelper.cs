using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KendoGridBinder.Containers;
using KendoGridBinder.Containers.Json;
using Newtonsoft.Json;

namespace KendoGridBinder.ModelBinder
{
    public static class SortHelper
    {
        public static IEnumerable<SortObject> Parse(NameValueCollection queryString)
        {
            // If there is a sort query parameter, try to parse the value as json
            if (queryString.AllKeys.Contains("sort"))
            {
                string sortAsJson = queryString["sort"];
                if (!string.IsNullOrEmpty(sortAsJson))
                {
                    return GetSortObjects(sortAsJson);
                }
            }
            else
            {
                // Just get the sort the old way
                return GetSortObjects(queryString, queryString.AllKeys.Where(k => k.StartsWith("sort")));
            }

            return null;
        }

        private static IEnumerable<SortObject> GetSortObjects(string sortAsJson)
        {
            var result = JsonConvert.DeserializeObject<IEnumerable<Sort>>(sortAsJson);

            return result != null ? Map(result) : null;
        }

        public static IEnumerable<SortObject> Map(IEnumerable<Sort> sort)
        {
            return sort != null ? sort.Select(s => new SortObject(s.Field, s.Dir)) : null;
        }

        public static IEnumerable<SortObject> GetSortObjects(NameValueCollection queryValues, IEnumerable<string> sortKeys)
        {
            var list = new List<SortObject>();

            var enumerable = sortKeys as IList<string> ?? sortKeys.ToList();
            var fields = enumerable.Where(sk => sk.Contains("field")).Select(x => queryValues[x]).ToList();
            var directions = enumerable.Where(sk => sk.Contains("dir")).Select(x => queryValues[x]).ToList();

            foreach (string field in fields)
            {
                var index = fields.IndexOf(field);
                var direction = directions[index];
                var obj = new SortObject(field, direction);
                list.Add(obj);
            }

            return list.Any() ? list : null;
        }
    }
}