using System;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyTranslator;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Employee")]
    public class Employee : Entity
    {
        private static readonly CompiledExpressionMap<Employee, bool> IsManagerExpr =
            DefaultTranslationOf<Employee>.Property(e => e.IsManager).Is(e => e.Email.Contains(("smith")));

        private static readonly CompiledExpressionMap<Employee, string> FullNameExpr =
            DefaultTranslationOf<Employee>.Property(e => e.FullName).Is(e => e.FirstName + " " + e.LastName);

        public int EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public Company Company { get; set; }

        public Country Country { get; set; }

        [NotMapped]
        public bool IsManager
        {
            get
            {
                return IsManagerExpr.Evaluate(this);
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FullNameExpr.Evaluate(this);
            }
        }
    }
}