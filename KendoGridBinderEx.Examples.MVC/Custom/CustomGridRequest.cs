using System.Runtime.Serialization;
using KendoGridBinderEx.Containers.Json;

namespace KendoGridBinderEx.Examples.MVC.Custom
{
    [DataContract]
    public class CustomGridRequest : GridRequest
    {
        [DataMember(Name = "custom")]
        public string Custom { get; set; }
    }
}