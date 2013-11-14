
namespace KendoGridBinderEx
{
    /// <summary>
    /// KendoGroup
    /// 
    /// Properties are lowercase to support Kendo UI Grid
    /// </summary>
    public class KendoGroup
    {
        // ReSharper disable InconsistentNaming
        public string field { get; set; }
        public object aggregates { get; set; }
        public object items { get; set; }
        public bool hasSubgroups { get; set; }
        public object value { get; set; }
        // ReSharper restore InconsistentNaming
    }
}