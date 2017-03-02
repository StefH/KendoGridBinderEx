using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KendoGridBinder.Containers.Json
{
    /// <summary>
    /// This class maps 1 : 1 to JSON Group
    /// </summary>
    [DataContract(Name = "group")]
    public class GroupObject
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "dir")]
        public string Direction { get; set; }

        [DataMember(Name = "aggregates")]
        public IEnumerable<AggregateObject> AggregateObjects { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupObject"/> class.
        /// </summary>
        public GroupObject()
        {
            Direction = "asc";
        }
    }
}