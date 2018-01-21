using System.Web;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using OfficeOpenXml;

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

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                using (var excel = new ExcelPackage(file.InputStream))
                {
                    var ws = excel.Workbook.Worksheets[1];

                    return Json(new { ExcelContent = ws.SelectedRange.Value });
                }
            }

            return RedirectToAction("Index");
        }
    }
}