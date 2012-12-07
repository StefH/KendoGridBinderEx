using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;
using KendoGridBinder.Examples.MVC.Data.Validation;
using System.Web.Mvc;

namespace KendoGridBinder.Examples.MVC.Controllers
{
    public class ProductController : BaseGridController<Product, Product>
    {
        private readonly ProductService _productService;

        public ProductController()
            : base(CompositionRoot.ResolveService<ProductService>())
        {
            _productService = (ProductService)Service;
        }

        [HttpPost]
        public JsonResult GridTop3(KendoGridRequest request)
        {
            var entities = _productService.GetTop3Products();
            return GetKendoGridAsJson(request, entities);
        }

        [HttpGet]
        public JsonResult IsCodeUnique(string code)
        {
            var validator = new ProductValidator(_productService);
            //var result = validator.Validate(p => p.Code, code);

            //return JsonValidationResult(result);
            return null;
        }
    }
}
