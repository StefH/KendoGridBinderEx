using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Validation;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinderEx.ModelBinder.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class ProductController : BaseMvcGridController<Product, ProductVM>
    {
        private readonly IProductService _productService;
        private readonly ProductValidator _productValidator;

        public ProductController(IProductService productService)
            : base(productService)
        {
            _productService = productService;

            _productValidator = new ProductValidator(_productService);
        }

        public async Task<ActionResult> DetailsByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("code");
            }

            var entity = await _productService.FirstAsync(p => p.Code == code);
            var viewModel = Map(entity);

            return View("Details", viewModel);
        }

        [HttpPost]
        public JsonResult GridTop3(KendoGridMvcRequest request)
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