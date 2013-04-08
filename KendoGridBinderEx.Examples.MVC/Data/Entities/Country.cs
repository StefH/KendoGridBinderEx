using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.MVC.Data.Entities
{
    [Table("KendoGrid_Country")]
    public class Country : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}