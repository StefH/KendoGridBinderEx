using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Validation;
using KendoGridBinderEx.Examples.MVC.AutoMapper;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinder;
using KendoGridBinder.ModelBinder.Mvc;
using OfficeOpenXml;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class EmployeeController : BaseMvcGridController<Employee, EmployeeVM>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICountryService _countryService;
        private readonly ICompanyService _companyService;
        private readonly IFunctionService _functionService;
        private readonly ISubFunctionService _subfunctionService;
        private readonly EmployeeValidator _employeeValidator;
        private readonly KendoGridQueryableHelper _kendoGridQueryableHelper;

        public EmployeeController(IEmployeeService employeeService, ICompanyService companyService, IFunctionService functionService, ISubFunctionService subfunctionService, ICountryService countryService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _companyService = companyService;
            _functionService = functionService;
            _subfunctionService = subfunctionService;
            _countryService = countryService;

            _employeeValidator = new EmployeeValidator(_employeeService);
            _kendoGridQueryableHelper = new KendoGridQueryableHelper(AutoMapperConfig.MapperConfiguration);
        }

        protected override IQueryable<Employee> GetQueryable()
        {
            return _employeeService.AsQueryable(e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction, e => e.Country);
        }

        protected override Employee GetById(long id)
        {
            return _employeeService.GetById(id, e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction, e => e.Country);
        }

        public ActionResult IndexManagers()
        {
            return View();
        }

        public ActionResult IndexManagersAngular()
        {
            return View();
        }

        public ActionResult IndexPaulJame()
        {
            return View();
        }

        public ActionResult IndexApiGrouped()
        {
            return View();
        }

        public ActionResult IndexApiJsonGrouped()
        {
            return View();
        }

        public ActionResult IndexApiJsonCustom()
        {
            return View();
        }

        public ActionResult IndexGrouped()
        {
            return View();
        }

        public ActionResult IndexGrouped2()
        {
            return View();
        }

        public ActionResult IndexGrouped3()
        {
            return View();
        }

        public ActionResult IndexGroupedFixed()
        {
            return View();
        }

        public ActionResult IndexMasterDetail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Export(KendoGridFilter filter, string guid)
        {
            var gridRequest = new KendoGridMvcRequest();
            if (filter != null)
            {
                gridRequest.FilterObjectWrapper = filter.Filters != null ? filter.ToFilterObjectWrapper() : null;
                gridRequest.Logic = filter.Logic;
            }

            var query = GetQueryable().AsNoTracking();
            var results = _kendoGridQueryableHelper.FilterBy<Employee, EmployeeVM>(query, gridRequest);

            using (var stream = new MemoryStream())
            {
                using (var excel = new ExcelPackage(stream))
                {
                    excel.Workbook.Worksheets.Add("Employees");
                    var ws = excel.Workbook.Worksheets[1];
                    ws.Cells.LoadFromCollection(results, true);
                    ws.Cells.AutoFitColumns();

                    excel.Save();
                    Session[guid] = stream.ToArray();
                    return Json(new { success = true });
                }
            }
        }

        [HttpGet]
        public FileResult GetGeneratedExcel(string title, string guid)
        {
            // Is there a spreadsheet stored in session?
            if (Session[guid] == null)
            {
                throw new Exception($"{title} not found");
            }

            // Get the spreadsheet from session.
            var file = Session[guid] as byte[];
            string filename = $"{title}.xlsx";

            // Remove the spreadsheet from session.
            Session.Remove(guid);

            // Return the spreadsheet.
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", $"attachment; filename={filename}");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        [HttpPost]
        public JsonResult GridBySubFunctionId(KendoGridMvcRequest request, long? subFunctionId)
        {
            var query = GetQueryable().Where(s => s.SubFunction.Id == subFunctionId).AsNoTracking();
            var grid = _kendoGridQueryableHelper.ToKendoGridEx<Employee, EmployeeDetailVM>(query, request);
            return Json(grid);
        }

        [HttpPost]
        public JsonResult GridWithGroup(KendoGridMvcRequest request)
        {
            var queryContext = _employeeService.GetQueryContext(e => e.Company, e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction);

            int seven = 7;
            return GetKendoGridAsJson(request, queryContext.Query.Where(x => 7 == seven), queryContext.Includes);
        }

        [HttpPost]
        public JsonResult GridDetails(KendoGridMvcRequest request)
        {
            var queryContext = _employeeService.GetQueryContext(e => e.Company, e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction);
            return GetKendoGridAsJson(request, queryContext.Query, queryContext.Includes);
        }

        [HttpPost]
        public JsonResult GridManagers(KendoGridMvcRequest request)
        {
            var entities = _employeeService.GetManagers();
            return GetKendoGridAsJson(request, entities);
        }

        [HttpPost]
        public JsonResult GridManagersAngular(KendoGridMvcRequest request)
        {
            Thread.Sleep(2000); // Sleep 2 seconds to show loading gif
            return GridManagers(request);
        }

        [HttpPost]
        public JsonResult GridPaulJame(KendoGridMvcRequest request)
        {
            var entities = _employeeService.AsQueryable();
            return GetKendoGridAsJson(request, entities);
        }

        protected override ValidationResult Validate(Employee employee, string ruleSet)
        {
            //return _employeeValidator.Validate(employee, ruleSet: "*");
            return _employeeValidator.ValidateAll(employee);
        }

        #region Remote Validations
        [HttpGet]
        public JsonResult ValidateUniqueNumber(int number, long? id)
        {
            var result = _employeeValidator.Validate(id, e => e.EmployeeNumber, number);

            return JsonValidationResult(result);
        }

        [HttpGet]
        public JsonResult ValidateUniqueFullName(string first, string lastName, long? id)
        {
            var viewModel = new EmployeeVM
            {
                First = first,
                LastName = lastName,
                Id = id ?? 0
            };

            var employee = Map(viewModel);
            var result = _employeeValidator.ValidateNames(employee);

            return JsonValidationResult(result);
        }

        [HttpGet]
        public JsonResult IsManager(string last)
        {
            bool isManager = !string.IsNullOrEmpty(last) && _employeeService.GetManagers().Any(e => string.Compare(e.LastName, last, StringComparison.CurrentCultureIgnoreCase) == 0);

            return Json(isManager, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}