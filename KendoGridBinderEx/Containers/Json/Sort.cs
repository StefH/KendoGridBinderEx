using System.Runtime.Serialization;

namespace KendoGridBinderEx.Containers.Json
{
    /// <summary>
    /// This class maps 1 : 1 to JSON Sort
    /// </summary>
    [DataContract(Name = "sort")]
    public class Sort
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        [DataMember(Name = "compare")]
        public object Compare { get; set; }
    }
}