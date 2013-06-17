using System;
using FluentValidation;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.MVC.Data.Entities;
using KendoGridBinderEx.Examples.MVC.Data.Service;

namespace KendoGridBinderEx.Examples.MVC.Data.Validation
{
    public class EmployeeValidator : BaseValidator<Employee>
    {
        private const string RuleSetNames = "Names";
        private readonly IEmployeeService _employeeService;

        public EmployeeValidator(IEmployeeService service)
            : base(service)
        {
            _employeeService = service;

            RuleFor(e => e.EmployeeNumber)
                .NotEmpty().WithMessage(GlobalResources.Employee_Number_Required)
                .Must(IsNumberUnique).WithMessage(GlobalResources.Employee_Number_NotUnique)
                ;

            RuleFor(e => e.Company)
                .NotNull().WithMessage(GlobalResources.Employee_Company_Required)
                ;

            RuleSet(RuleSetNames, () =>
            {
                RuleFor(e => e.FirstName)
                    .NotEmpty().WithMessage(GlobalResources.Employee_First_Required)
                    .Length(0, 10).WithName(GlobalResources.FirstName)
                    .Must(IsFirstNameUnique).WithMessage(GlobalResources.Employee_Fullname_NotUnique, e => e.FullName);

                RuleFor(e => e.LastName)
                    .NotEmpty().WithMessage(GlobalResources.Employee_Last_Required)
                    .Length(0, 50).WithName(GlobalResources.LastName)
                    .Must(IsLastNameUnique).WithMessage(GlobalResources.Employee_Fullname_NotUnique, e => e.FullName);
            });

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage(GlobalResources.Employee_Email_Required)
                .EmailAddress().WithMessage(GlobalResources.Employee_Email_Invalid)
                .Must(IsEmailUnique).WithMessage(GlobalResources.Employee_Email_NotUnique)
                ;

            RuleFor(e => e.HireDate)
                .NotNull().WithMessage(GlobalResources.Employee_HireDate_Required)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage(GlobalResources.Employee_HireDate_InvalidPast)
                ;
        }

        public ValidationResult ValidateNames(Employee employee)
        {
            return this.Validate(employee, ruleSet: RuleSetNames);
        }

        public bool IsNumberUnique(Employee employee, int number)
        {
            return _employeeService.IsNumberUnique(employee, number);
        }

        public bool IsEmailUnique(Employee employee, string email)
        {
            return _employeeService.IsEmailUnique(employee, email);
        }

        public bool IsFirstNameUnique(Employee employee, string firstName)
        {
            return _employeeService.IsFullNameUnique(employee, firstName, employee.LastName);
        }

        public bool IsLastNameUnique(Employee employee, string lastName)
        {
            return _employeeService.IsFullNameUnique(employee, employee.FirstName, lastName);
        }
    }
}