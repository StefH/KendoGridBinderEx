using System.Web.Mvc;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
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

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<Function, FunctionVM>()
                ;

            Mapper.CreateMap<FunctionVM, Function>()
                .ForMember(e => e.Employees, opt => opt.Ignore())
                .ForMember(e => e.SubFunctions, opt => opt.Ignore())
                ;
        }

        [HttpGet]
        public JsonResult GetFunctionsAsJson()
        {
            var entities = Map(_functionService.AsQueryable());

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}