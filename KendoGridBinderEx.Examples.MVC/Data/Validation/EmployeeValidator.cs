using FluentValidation;
using FluentValidation.Results;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;

namespace KendoGridBinder.Examples.MVC.Data.Validation
{
    public class EmployeeValidator : BaseValidator<Employee>
    {
        private const string RuleSetNames = "Names";
        private readonly EmployeeService _employeeService;

        public EmployeeValidator()
            : this(CompositionRoot.ResolveService<EmployeeService>())
        {
        }

        public EmployeeValidator(EmployeeService service)
            : base(service)
        {
            _employeeService = service;

            RuleFor(e => e.EmployeeNumber)
                .NotEmpty().WithMessage(GlobalResources.Employee_Number_Required)
                .Must(IsNumberUnique).WithMessage(GlobalResources.Employee_Number_NotUnique, e => e.EmployeeNumber)
                ;

            RuleFor(e => e.Company)
                .NotNull().WithMessage(GlobalResources.Employee_Company_Required)
                ;    

            RuleSet(RuleSetNames, () =>
            {
                RuleFor(e => e.FirstName)
                    .NotEmpty()
                    .Length(0, 10)
                    .Must(IsFirstNameUnique).WithMessage(GlobalResources.Employee_Fullname_NotUnique, e => e.FullName);

                RuleFor(e => e.LastName)
                    .NotEmpty()
                    .Length(0, 50)
                    .Must(IsLastNameUnique).WithMessage(GlobalResources.Employee_Fullname_NotUnique, e => e.FullName);
            });
        }

        public ValidationResult ValidateNames(Employee employee)
        {
            return this.Validate(employee, ruleSet: RuleSetNames);
        }

        public bool IsNumberUnique(Employee employee, int number)
        {
            return _employeeService.IsNumberUnique(employee, number);
        }

        public bool IsFirstNameUnique(Employee employee, string firstName)
        {
            return _employeeService.IsFullNameUnique(employee, firstName, employee.LastName);
        }

        public bool IsLastNameUnique(Employee employee, string lastName)
        {
            return _employeeService.IsFullNameUnique(employee, employee.FirstName, lastName);
        }

        public bool IsFullNameUnique(Employee employee, string full)
        {
            return _employeeService.IsFullNameUnique(employee, full);
        }
    }
}