using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using KendoGridBinderEx.Containers.Json;
using Newtonsoft.Json;

namespace KendoGridBinderEx.ModelBinder
{
    public static class GroupHelper
    {
        private static readonly Regex GroupRegex = new Regex(@"^group\[(\d*)\]\[(field|dir)\]$", RegexOptions.IgnoreCase);
        private static readonly Regex GroupAggregateRegex = new Regex(@"^group\[(\d*)\]\[aggregates\]\[(\d*)\]\[(field|aggregate)\]$", RegexOptions.IgnoreCase);

        public static IEnumerable<GroupObject> Parse(NameValueCollection queryString)
        {
            // If there is a group query parameter, try to parse the value as json
            if (queryString.AllKeys.Contains("group"))
            {
                string groupAsJson = queryString["group"];
                if (!string.IsNullOrEmpty(groupAsJson))
                {
                    return GetGroupObjects(groupAsJson);
                }
            }
            else
            {
                // Just get the groups the old way
                return GetGroupObjects(queryString, queryString.AllKeys.Where(k => k.StartsWith("group")));
            }

            return null;
        }

        public static IEnumerable<GroupObject> Map(IEnumerable<GroupObject> groups)
        {
            if (groups == null)
            {
                return null;
            }

            var enumerable = groups as IList<GroupObject> ?? groups.ToList();
            return enumerable.Any() ? enumerable : null;
        }

        private static IEnumerable<GroupObject> GetGroupObjects(string groupAsJson)
        {
            var result = JsonConvert.DeserializeObject<List<GroupObject>>(groupAsJson);

            return Map(result);
        }

        private static IEnumerable<GroupObject> GetGroupObjects(NameValueCollection queryString, IEnumerable<string> requestKeys)
        {
            var result = new Dictionary<int, GroupObject>();

            var enumerable = requestKeys as IList<string> ?? requestKeys.ToList();
            foreach (var x in enumerable
                .Select(k => new { Key = k, Match = GroupRegex.Match(k) })
                .Where(x => x.Match.Success)
            )
            {
                int groupId = int.Parse(x.Match.Groups[1].Value);
                if (!result.ContainsKey(groupId))
                {
                    result.Add(groupId, new GroupObject());
                }

                string value = queryString[x.Key];
                if (x.Key.Contains("field"))
                {
                    result[groupId].Field = value;
                }
                if (x.Key.Contains("dir"))
                {
                    result[groupId].Direction = !string.IsNullOrEmpty(value) ? value : "asc";
                }
            }

            foreach (var groupObject in result)
            {
                var aggregates = new Dictionary<int, AggregateObject>();
                var aggregateKey = string.Format("group[{0}][aggregates]", groupObject.Key);

                foreach (var x in enumerable
                    .Where(k => k.StartsWith(aggregateKey))
                    .Select(k => new { Key = k, Match = GroupAggregateRegex.Match(k) })
                    .Where(x => x.Match.Success)
                    )
                {
                    int aggregateId = int.Parse(x.Match.Groups[2].Value);
                    if (!aggregates.ContainsKey(aggregateId))
                    {
                        aggregates.Add(aggregateId, new AggregateObject());
                    }

                    string value = queryString[x.Key];
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

            return result.Any() ? result.Values.ToList() : null;
        }
    }
}