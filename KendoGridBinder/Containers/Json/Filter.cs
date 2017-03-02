using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KendoGridBinder.Containers.Json
{
    /// <summary>
    /// This class maps 1 : 1 to JSON filter
    /// </summary>
    [DataContract]
    public class Filter
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "logic")]
        public string Logic { get; set; }

        [DataMember(Name = "ignoreCase")]
        public string IgnoreCase { get; set; }

        [DataMember(Name = "filters")]
        public List<Filter> Filters { get; set; }
    }
}