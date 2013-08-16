using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class CompanyController : BaseGridController<Company, Company>
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
            : base(companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public JsonResult GetJson()
        {
            var entities = _companyService.GetAll(c => c.MainCompany);

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}