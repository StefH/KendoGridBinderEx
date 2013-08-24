using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class SubFunctionController : BaseGridController<SubFunction, SubFunctionVM>
    {
        private readonly ISubFunctionService _subFunctionService;

        public SubFunctionController(ISubFunctionService service)
            : base(service)
        {
            _subFunctionService = service;
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<SubFunction, SubFunctionVM>()
                ;

            Mapper.CreateMap<SubFunctionVM, SubFunction>()
                .ForMember(e => e.Function, opt => opt.Ignore())
                ;
        }

        [HttpGet]
        public JsonResult GetSubFunctionsByFunctionIdAsJson(long functionId)
        {
            var entities = base.Map(_subFunctionService.AsQueryable().Where(s => s.Function.Id == functionId));

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}