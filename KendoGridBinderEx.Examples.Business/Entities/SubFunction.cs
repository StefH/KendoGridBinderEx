using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_SubFunction")]
    public class SubFunction : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Function Function { get; set; }
    }
}