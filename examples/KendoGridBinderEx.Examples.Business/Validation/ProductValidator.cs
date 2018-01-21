using FluentValidation;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;

namespace KendoGridBinderEx.Examples.Business.Validation
{
    public class ProductValidator : BaseValidator<Product>
    {
        private readonly IProductService _productService;

        public ProductValidator(IProductService service)
            : base(service)
        {
            _productService = service;

            RuleFor(p => p.Code)
                .NotEmpty()
                .Must(IsCodeUnique).WithMessage(GlobalResources_Business.Product_Code_NotUnique)
                .Length(0, 10);

            RuleFor(p => p.Name)
                .NotEmpty()
                .Length(0, 50);
        }

        public bool IsCodeUnique(Product product, string code)
        {
            return _productService.IsCodeUnique(product, code);
        }
    }
}