using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FluentValidation.Results;
using KendoGridBinder;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;
using KendoGridBinderEx.Examples.Business.Validation;
using KendoGridBinderEx.Examples.MVC.Models;
using StackExchange.Profiling;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class EmployeeController : BaseGridController<Employee, EmployeeVM>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;
        private readonly IFunctionService _functionService;
        private readonly ISubFunctionService _subfunctionService;
        private readonly EmployeeValidator _employeeValidator;

        public EmployeeController(IEmployeeService employeeService, ICompanyService companyService, IFunctionService functionService, ISubFunctionService subfunctionService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _companyService = companyService;
            _functionService = functionService;
            _subfunctionService = subfunctionService;

            _employeeValidator = new EmployeeValidator(_employeeService);
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<Employee, EmployeeVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))
                .ForMember(vm => vm.CompanyId, opt => opt.MapFrom(m => m.Company.Id))
                .ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
                .ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
                .ForMember(vm => vm.CountryId, opt => opt.MapFrom(m => m.Country.Id))
                .ForMember(vm => vm.CountryCode, opt => opt.MapFrom(m => m.Country.Code))
                .ForMember(vm => vm.CountryName, opt => opt.MapFrom(m => m.Country.Name))
                .ForMember(vm => vm.FunctionId, opt => opt.MapFrom(m => m.Function.Id))
                .ForMember(vm => vm.FunctionCode, opt => opt.MapFrom(m => m.Function.Code))
                .ForMember(vm => vm.FunctionName, opt => opt.MapFrom(m => m.Function.Name))
                .ForMember(vm => vm.SubFunctionId, opt => opt.MapFrom(m => m.SubFunction.Id))
                .ForMember(vm => vm.SubFunctionCode, opt => opt.MapFrom(m => m.SubFunction.Code))
                .ForMember(vm => vm.SubFunctionName, opt => opt.MapFrom(m => m.SubFunction.Name))
                ;

            Mapper.CreateMap<EmployeeVM, Employee>()
                .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
                .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.LastName))
                .ForMember(e => e.Company, opt => opt.Ignore())
                .ForMember(e => e.Country, opt => opt.Ignore())
                .ForMember(e => e.Function, opt => opt.Ignore())
                .ForMember(e => e.SubFunction, opt => opt.Ignore())
                ;
        }

        protected override IQueryable<Employee> GetQueryable()
        {
            return _employeeService.AsQueryable(e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction);
        }

        protected override Employee GetById(long id)
        {
            return _employeeService.GetById(id, e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction);
        }

        protected override Employee Map(EmployeeVM viewModel)
        {
            var employee = Mapper.Map<Employee>(viewModel);
            employee.Company = viewModel.CompanyId > 0 ? _companyService.GetById(viewModel.CompanyId, c => c.MainCompany) : null;
            employee.Function = viewModel.FunctionId > 0 ? _functionService.GetById(viewModel.FunctionId) : null;
            employee.SubFunction = viewModel.SubFunctionId > 0 ? _subfunctionService.GetById(viewModel.SubFunctionId) : null;

            return employee;
        }

        [HttpPost]
        public JsonResult GridWithGroup(KendoGridRequest request)
        {
            using (MiniProfiler.Current.Step("GridWithGroup"))
            {
                var queryContext = _employeeService.GetQueryContext(e => e.Company, e => e.Company.MainCompany, e => e.Country, e => e.Function, e => e.SubFunction);
                return GetKendoGridAsJson(request, queryContext.Query, queryContext.Includes);
            }
        }

        [HttpPost]
        public JsonResult GridManagers(KendoGridRequest request)
        {
            var entities = _employeeService.GetManagers();
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