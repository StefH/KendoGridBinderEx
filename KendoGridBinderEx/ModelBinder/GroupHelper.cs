using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using KendoGridBinderEx.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KendoGridBinderEx.ModelBinder
{
    public static class GroupHelper
    {
        private static readonly Regex GroupRegex = new Regex(@"^group\[(\d*)\]\[(field|dir)\]$", RegexOptions.IgnoreCase);
        private static readonly Regex GroupAggregateRegex = new Regex(@"^group\[(\d*)\]\[aggregates\]\[(\d*)\]\[(field|aggregate)\]$", RegexOptions.IgnoreCase);

        public static IEnumerable<GroupObject> GetGroupObjects(string groupAsJson)
        {
            var result = new List<GroupObject>();

            var groupList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(groupAsJson);
            foreach (var group in groupList)
            {
                var groupObject = new GroupObject();
                result.Add(groupObject);

                foreach (var groupKvp in group)
                {
                    if (groupKvp.Key == "field")
                    {
                        groupObject.Field = (string)groupKvp.Value;
                    }
                    if (groupKvp.Key == "dir")
                    {
                        groupObject.Direction = !string.IsNullOrEmpty((string)groupKvp.Value) ? (string)groupKvp.Value : "asc";
                    }
                    if (groupKvp.Key == "aggregates")
                    {
                        var aggregates = new List<AggregateObject>();

                        var aggregatesList = groupKvp.Value as JArray;
                        if (aggregatesList != null && aggregatesList.Count > 0)
                        {
                            foreach (var aggregate in aggregatesList)
                            {
                                var aggregateObject = new AggregateObject();
                                aggregates.Add(aggregateObject);

                                foreach (var aggregateToken in aggregate.Cast<JProperty>())
                                {
                                    if (aggregateToken.Name == "field")
                                    {
                                        aggregateObject.Field = (string)aggregateToken.Value;
                                    }
                                    if (aggregateToken.Name == "aggregate")
                                    {
                                        aggregateObject.Aggregate = (string)aggregateToken.Value;
                                    }
                                }
                            }
                        }

                        groupObject.AggregateObjects = aggregates;
                    }
                }
            }

            return result;
        }

        public static IEnumerable<GroupObject> GetGroupObjects(NameValueCollection queryString, IEnumerable<string> requestKeys)
        {
            var dict = new Dictionary<int, GroupObject>();

            var enumerable = requestKeys as IList<string> ?? requestKeys.ToList();
            foreach (var x in enumerable
                .Select(k => new { Key = k, Match = GroupRegex.Match(k) })
                .Where(x => x.Match.Success)
            )
            {
                int groupId = int.Parse(x.Match.Groups[1].Value);
                if (!dict.ContainsKey(groupId))
                {
                    dict.Add(groupId, new GroupObject());
                }

                string value = queryString[x.Key];
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

            return dict.Any() ? dict.Values.ToList() : null;
        }
    }
}