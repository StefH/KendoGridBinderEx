using System;
using Linq.PropertyTranslator.Core;

namespace KendoGridBinder.UnitTests.Entities
{
    public class Employee : Entity
    {
        private static readonly CompiledExpressionMap<Employee, bool> IsManagerExpr =
            DefaultTranslationOf<Employee>.Property(e => e.IsManager).Is(e => e.Email != null && e.Email.Contains("smith"));

        private static readonly CompiledExpressionMap<Employee, string> FullNameExpr =
            DefaultTranslationOf<Employee>.Property(e => e.FullName).Is(e => e.FirstName + " " + e.LastName);

        public int EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public Company Company { get; set; }

        public Country Country { get; set; }

        public bool IsManager
        {
            get
            {
                return IsManagerExpr.Evaluate(this);
            }
        }

        public string FullName
        {
            get
            {
                return FullNameExpr.Evaluate(this);
            }
        }
    }
}