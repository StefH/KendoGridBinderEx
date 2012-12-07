using FluentValidation.Attributes;
using KendoGridBinder.Examples.MVC.Data.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace KendoGridBinder.Examples.MVC.Data.Entities
{
    [Table("KendoGrid_Product")]
    [Validator(typeof(ProductValidator))]
    public class Product : Entity
    {
        [Remote("IsCodeUnique", "Product")]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}