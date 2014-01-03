using PropertyTranslator;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Employee")]
    public class Employee : Entity
    {
        private static readonly CompiledExpressionMap<Employee, bool> IsManagerExpr =
            DefaultTranslationOf<Employee>.Property(e => e.IsManager).Is(e => e.LastName != null && e.LastName.ToLower().Contains(("smith")));

        private static readonly CompiledExpressionMap<Employee, string> FullNameExpr =
            DefaultTranslationOf<Employee>.Property(e => e.FullName).Is(e => e.FirstName + " " + e.LastName);

        private static readonly CompiledExpressionMap<Employee, bool> IsAssignedExpr =
            DefaultTranslationOf<Employee>.Property(e => e.IsAssigned).Is(e => e.Assigned != null && e.Assigned >= 1);

        public int EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public Company Company { get; set; }

        public Country Country { get; set; }

        public Function Function { get; set; }

        public SubFunction SubFunction { get; set; }

        public int? Assigned { get; set; }

        [NotMapped]
        public bool IsAssigned
        {
            get
            {
                return IsAssignedExpr.Evaluate(this);
            }
        }

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