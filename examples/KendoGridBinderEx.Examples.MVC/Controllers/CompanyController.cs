using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class CompanyController : BaseMvcGridController<Company, Company>
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
            : base(companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public JsonResult GetCompaniesAsJson()
        {
            var entities = _companyService.GetAll(c => c.MainCompany);

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
    }
}