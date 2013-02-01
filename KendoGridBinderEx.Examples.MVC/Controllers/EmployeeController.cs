using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FluentValidation.Results;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;
using KendoGridBinder.Examples.MVC.Data.Validation;
using KendoGridBinder.Examples.MVC.Models;

namespace KendoGridBinder.Examples.MVC.Controllers
{
    public class EmployeeController : BaseGridController<Employee, EmployeeVM>
    {
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;
        private readonly EmployeeValidator _employeeValidator;

        public EmployeeController()
            : base(CompositionRoot.ResolveService<EmployeeService>())
        {
            _employeeService = (EmployeeService)Service;
            _companyService = CompositionRoot.ResolveService<CompanyService>();

            _employeeValidator = new EmployeeValidator(_employeeService);
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<Employee, EmployeeVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.Last, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))
                .ForMember(vm => vm.CompanyId, opt => opt.MapFrom(m => m.Company.Id))
                .ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
                .ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
                ;

            Mapper.CreateMap<EmployeeVM, Employee>()
                .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
                .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.Last))
                .ForMember(e => e.Company, opt => opt.Ignore())
                ;
        }

        protected override IQueryable<Employee> GetQueryable()
        {
            return _employeeService.AsQueryable(e => e.Company.MainCompany);
        }

        protected override Employee GetById(long id)
        {
            return _employeeService.GetById(id, e => e.Company.MainCompany);
        }

        protected override Employee Map(EmployeeVM viewModel)
        {
            var employee = Mapper.Map<Employee>(viewModel);
            employee.Company = viewModel.CompanyId > 0 ? _companyService.GetById(viewModel.CompanyId, c => c.MainCompany) : null;

            return employee;
        }

        [HttpPost]
        public JsonResult GridWithGroup(KendoGridRequest request)
        {
            var query = GetQueryable();
            return GetKendoGridAsJson(request, query);
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
        public JsonResult ValidateUniqueFullName(string first, string last, long? id)
        {
            var viewModel = new EmployeeVM
            {
                First = first,
                Last = last,
                Id = id ?? 0
            };

            var employee = Map(viewModel);
            var result = _employeeValidator.ValidateNames(employee);

            return JsonValidationResult(result);
        }
        #endregion
    }
}