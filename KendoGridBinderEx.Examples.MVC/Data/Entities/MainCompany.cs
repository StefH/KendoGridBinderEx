using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinder.Examples.MVC.Data.Entities
{
    [Table("KendoGrid_MainCompany")]
    public class MainCompany : Entity
    {
        public string Name { get; set; }
    }
}