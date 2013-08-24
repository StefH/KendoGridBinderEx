using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_OU")]
    public class OU : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}