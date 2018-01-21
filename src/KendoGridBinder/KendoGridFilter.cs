using System.Collections.Generic;
using System.Linq;
using KendoGridBinder.Containers;
using Newtonsoft.Json;

namespace KendoGridBinder
{
    [JsonObject(MemberSerialization.OptIn)]
    public class KendoGridFilter
    {
        [JsonProperty(PropertyName = "logic")]
        public string Logic { get; set; }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "operator")]
        public string Operator { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "filters")]
        public List<KendoGridFilter> Filters { get; set; }

        public FilterObjectWrapper ToFilterObjectWrapper()
        {
            var filters = new List<FilterObject>();
            foreach (var filter in Filters)
            {
                FilterObject filterObject;
                if (filter.Filters != null && filter.Filters.Count == 2)
                {
                    var firstFilter = filter.Filters.First();
                    var lastFilter = filter.Filters.Last();
                    filterObject = new FilterObject
                    {
                        Field1 = firstFilter.Field,
                        Operator1 = firstFilter.Operator,
                        Value1 = firstFilter.Value,

                        Logic = filter.Logic,

                        Field2 = lastFilter.Field,
                        Operator2 = lastFilter.Operator,
                        Value2 = lastFilter.Value
                    };
                }
                else
                {
                    filterObject = new FilterObject
                    {
                        Field1 = filter.Field,
                        Operator1 = filter.Operator,
                        Value1 = filter.Value,
                    };
                }

                filters.Add(filterObject);
            }

            return new FilterObjectWrapper { Logic = Logic, FilterObjects = filters };
        }
    }
}