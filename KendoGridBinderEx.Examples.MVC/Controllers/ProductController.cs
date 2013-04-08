using System.Web.Mvc;
using FluentValidation.Results;
using KendoGridBinder;
using KendoGridBinder.Examples.MVC.Data.Validation;
using KendoGridBinderEx.Examples.MVC.Data.Entities;
using KendoGridBinderEx.Examples.MVC.Data.Service;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class ProductController : BaseGridController<Product, Product>
    {
        private readonly ProductService _productService;
        private readonly ProductValidator _productValidator;

        public ProductController()
            : base(CompositionRoot.ResolveService<ProductService>())
        {
            _productService = (ProductService)Service;

            _productValidator = new ProductValidator(_productService);
        }

        [HttpPost]
        public JsonResult GridTop3(KendoGridRequest request)
        {
            var entities = _productService.GetTop3Products();
            return GetKendoGridAsJson(request, entities);
        }

        #region Validations
        protected override ValidationResult Validate(Product product, string ruleSet)
        {
            return _productValidator.ValidateAll(product);
        }
        [HttpGet]
        public JsonResult IsCodeUnique(string code, long? id)
        {
            var result = _productValidator.Validate(id, p => p.Code, code);

            return JsonValidationResult(result);
        }
        #endregion
    }
}