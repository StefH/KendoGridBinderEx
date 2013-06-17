using System.Web.Mvc;
using KendoGridBinderEx.Examples.MVC.Data.Entities;
using KendoGridBinderEx.Examples.MVC.Data.Service;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class CompanyController : BaseGridController<Company, Company>
    {
        private readonly ICompanyService _companyService;

        public CompanyController(CompanyService companyService)
            : base(companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public JsonResult GetJson()
        {
            var entities = _companyService.GetAll();

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}
