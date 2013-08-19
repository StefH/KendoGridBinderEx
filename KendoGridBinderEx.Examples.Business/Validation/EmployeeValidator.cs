using System;
using FluentValidation;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;

namespace KendoGridBinderEx.Examples.Business.Validation
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
                .NotEmpty().WithMessage(GlobalResources_Business.Employee_Number_Required)
                .Must(IsNumberUnique).WithMessage(GlobalResources_Business.Employee_Number_NotUnique)
                ;

            RuleFor(e => e.Company)
                .NotNull().WithMessage(GlobalResources_Business.Employee_Company_Required)
                ;

            RuleFor(e => e.Function)
                .NotNull().WithMessage(GlobalResources_Business.Employee_Function_Required)
                ;

            RuleFor(e => e.SubFunction)
                .NotNull().WithMessage(GlobalResources_Business.Employee_SubFunction_Required)
                ;

            RuleSet(RuleSetNames, () =>
            {
                RuleFor(e => e.FirstName)
                    .NotEmpty().WithMessage(GlobalResources_Business.Employee_First_Required)
                    .Length(0, 10).WithName(GlobalResources_Business.FirstName)
                    .Must(IsFirstNameUnique).WithMessage(GlobalResources_Business.Employee_Fullname_NotUnique, e => e.FullName);

                RuleFor(e => e.LastName)
                    .NotEmpty().WithMessage(GlobalResources_Business.Employee_Last_Required)
                    .Length(0, 50).WithName(GlobalResources_Business.LastName)
                    .Must(IsLastNameUnique).WithMessage(GlobalResources_Business.Employee_Fullname_NotUnique, e => e.FullName);
            });

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage(GlobalResources_Business.Employee_Email_Required)
                .EmailAddress().WithMessage(GlobalResources_Business.Employee_Email_Invalid)
                .Must(IsEmailUnique).WithMessage(GlobalResources_Business.Employee_Email_NotUnique)
                ;

            RuleFor(e => e.HireDate)
                .NotNull().WithMessage(GlobalResources_Business.Employee_HireDate_Required)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage(GlobalResources_Business.Employee_HireDate_InvalidPast)
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