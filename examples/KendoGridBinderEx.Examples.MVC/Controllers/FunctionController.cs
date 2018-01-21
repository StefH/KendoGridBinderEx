using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class FunctionController : BaseMvcGridController<Function, FunctionVM>
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService service)
            : base(service)
        {
            _functionService = service;
        }

        [HttpGet]
        public JsonResult GetFunctionsAsJson()
        {
            var entities = ProjectToList(_functionService.AsQueryable());

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}