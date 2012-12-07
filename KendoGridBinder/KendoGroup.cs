
namespace KendoGridBinder
{
    /// <summary>
    /// KendoGroup
    /// 
    /// Properties are lowercase to support Kendo UI Grid
    /// </summary>
    public class KendoGroup
    {
        public string field { get; set; }
        public object aggregates { get; set; }
        public object items { get; set; }
        public bool hasSubgroups { get; set; }
        public object value { get; set; }
    }
}