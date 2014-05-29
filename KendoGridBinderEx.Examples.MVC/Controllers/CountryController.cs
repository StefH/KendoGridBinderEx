using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class CountryController : BaseMvcGridController<Country, Country>
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
            : base(countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public JsonResult GetCountriesAsJson()
        {
            return Json(_countryService.GetAll(), JsonRequestBehavior.AllowGet);
        }
    }
}