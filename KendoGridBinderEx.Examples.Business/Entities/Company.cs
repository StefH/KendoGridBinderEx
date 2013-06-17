using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Company")]
    public class Company : Entity
    {
        public string Name { get; set; }

        public MainCompany MainCompany { get; set; }
    }
}