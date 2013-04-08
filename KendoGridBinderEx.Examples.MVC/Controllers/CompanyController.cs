using System.Web.Mvc;
using KendoGridBinderEx.Examples.MVC.Data.Entities;
using KendoGridBinderEx.Examples.MVC.Data.Service;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class CompanyController : BaseGridController<Company, Company>
    {
        private readonly CompanyService _companyService;

        public CompanyController()
            : base(CompositionRoot.ResolveService<CompanyService>())
        {
            _companyService = (CompanyService)Service;
        }

        [HttpGet]
        public JsonResult GetJson()
        {
            var entities = _companyService.GetAll();

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}
