using System.Web.Mvc;
using AutoMapper;
using FluentValidation.Results;
using KendoGridBinder;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;
using KendoGridBinderEx.Examples.Business.Validation;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class ProductController : BaseGridController<Product, ProductVM>
    {
        private readonly IProductService _productService;
        private readonly ProductValidator _productValidator;

        public ProductController(IProductService productService) : base(productService)
        {
            _productService = productService;

            _productValidator = new ProductValidator(_productService);
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<Product, ProductVM>()
                ;

            Mapper.CreateMap<ProductVM, Product>()
                ;
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