using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinderEx.ModelBinder.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class SubFunctionController : BaseMvcGridController<SubFunction, SubFunctionVM>
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
                .ForMember(vm => vm.FunctionId, opt => opt.MapFrom(e => e.Function.Id))
                ;

            Mapper.CreateMap<SubFunctionVM, SubFunction>()
                .ForMember(e => e.Function, opt => opt.Ignore())
                ;
        }

        [HttpPost]
        public JsonResult GridByFunctionId(KendoGridMvcRequest request, long? functionId)
        {
            var entities = GetQueryable().Where(s => s.Function.Id == functionId).AsNoTracking();
            return GetKendoGridAsJson(request, entities);
        }

        [HttpGet]
        public JsonResult GetSubFunctionsAsJson()
        {
            var entities = base.ProjectToList(GetQueryable());

            return Json(entities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubFunctionsByFunctionIdAsJson(long functionId)
        {
            var entities = base.ProjectToList(GetQueryable().Where(s => s.Function.Id == functionId));

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}