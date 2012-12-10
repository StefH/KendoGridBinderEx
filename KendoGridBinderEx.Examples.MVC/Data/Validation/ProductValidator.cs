using FluentValidation;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;

namespace KendoGridBinder.Examples.MVC.Data.Validation
{
    public class ProductValidator : BaseValidator<Product>
    {
        private readonly ProductService _productService;

        public ProductValidator()
            : this(CompositionRoot.ResolveService<ProductService>())
        {
        }

        public ProductValidator(ProductService service)
            : base(service)
        {
            _productService = service;

            RuleFor(p => p.Code)
                .NotEmpty()
                .Must(IsCodeUnique).WithMessage(GlobalResources.Product_Code_NotUnique)
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