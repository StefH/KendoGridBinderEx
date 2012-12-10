using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using FluentValidation.Attributes;
using KendoGridBinder.Examples.MVC.Data.Validation;

namespace KendoGridBinder.Examples.MVC.Data.Entities
{
    [Table("KendoGrid_Product")]
    [Validator(typeof(ProductValidator))]
    public class Product : Entity
    {
        [Remote("IsCodeUnique", "Product", AdditionalFields = "Id")]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}