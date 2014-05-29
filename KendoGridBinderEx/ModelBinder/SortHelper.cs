using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KendoGridBinderEx.Containers;

namespace KendoGridBinderEx.ModelBinder
{
    public static class SortHelper
    {
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

            return list;
        }
    }
}