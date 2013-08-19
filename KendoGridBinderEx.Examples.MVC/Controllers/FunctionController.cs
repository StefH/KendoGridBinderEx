using System.Linq;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class FunctionController : BaseGridController<Function, FunctionVM>
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
            var entities = _functionService.GetAll().Select(x => new Function { Id = x.Id, Code = x.Code, Name = x.Name });

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}