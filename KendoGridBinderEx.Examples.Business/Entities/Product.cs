using FluentValidation.Attributes;
using KendoGridBinderEx.Examples.Business.Validation;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Validator(typeof(ProductValidator))]
    public class Product : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}