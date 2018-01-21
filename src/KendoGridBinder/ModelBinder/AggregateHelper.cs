using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using KendoGridBinder.Containers.Json;
using Newtonsoft.Json;

namespace KendoGridBinder.ModelBinder
{
    public static class AggregateHelper
    {
        private static readonly Regex AggregateRegex = new Regex(@"^aggregate\[(\d*)\]\[(field|aggregate)\]$", RegexOptions.IgnoreCase);

        public static IEnumerable<AggregateObject> Parse(NameValueCollection queryString)
        {
            // If there is a aggregates query parameter, try to parse the value as json
            if (queryString.AllKeys.Contains("aggregate"))
            {
                string aggregateAsJson = queryString["aggregate"];
                if (!string.IsNullOrEmpty(aggregateAsJson))
                {
                    return GetAggregateObjects(aggregateAsJson);
                }
            }
            else
            {
                // Just get the aggregates the old way
                return GetAggregateObjects(queryString, queryString.AllKeys.Where(k => k.StartsWith("aggregate")));
            }

            return null;
        }

        public static IEnumerable<AggregateObject> Map(IEnumerable<AggregateObject> aggregates)
        {
            if (aggregates == null)
            {
                return null;
            }

            var enumerable = aggregates as IList<AggregateObject> ?? aggregates.ToList();
            return enumerable.Any() ? enumerable : null;
        }

        private static IEnumerable<AggregateObject> GetAggregateObjects(string aggregatesAsJson)
        {
            var result = JsonConvert.DeserializeObject<List<AggregateObject>>(aggregatesAsJson);

            return Map(result);
        }

        private static IEnumerable<AggregateObject> GetAggregateObjects(NameValueCollection queryString, IEnumerable<string> requestKeys)
        {
            var result = new Dictionary<int, AggregateObject>();

            var enumerable = requestKeys as IList<string> ?? requestKeys.ToList();
            foreach (var x in enumerable
                .Select(k => new { Key = k, Match = AggregateRegex.Match(k) })
                .Where(x => x.Match.Success)
            )
            {
                int aggregateId = int.Parse(x.Match.Groups[1].Value);
                if (!result.ContainsKey(aggregateId))
                {
                    result.Add(aggregateId, new AggregateObject());
                }

                string value = queryString[x.Key];
                if (x.Key.Contains("field"))
                {
                    result[aggregateId].Field = value;
                }
                if (x.Key.Contains("aggregate"))
                {
                    result[aggregateId].Aggregate = value;
                }
                if (x.Key.Contains("dir"))
                {
                    // TODO : is this supported?
                    result[aggregateId].Direction = !string.IsNullOrEmpty(value) ? value : "asc";
                }
            }

            return result.Any() ? result.Values.ToList() : null;
        }
    }
}