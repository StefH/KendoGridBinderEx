using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Attributes;
using KendoGridBinderEx.Examples.Business.Validation;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Product")]
    [Validator(typeof(ProductValidator))]
    public class Product : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}