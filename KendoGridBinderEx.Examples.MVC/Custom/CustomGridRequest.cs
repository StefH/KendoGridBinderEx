using System.Runtime.Serialization;
using KendoGridBinder.Containers.Json;

namespace KendoGridBinderEx.Examples.MVC.Custom
{
    [DataContract]
    public class CustomGridRequest : GridRequest
    {
        [DataMember(Name = "custom")]
        public string Custom { get; set; }
    }
}