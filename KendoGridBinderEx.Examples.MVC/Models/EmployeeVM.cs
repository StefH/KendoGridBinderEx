using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using KendoGridBinder.Examples.MVC.Data.Entities;

namespace KendoGridBinder.Examples.MVC.Models
{
    public class EmployeeVM : IEntity
    {
        public long Id { get; set; }

        [Remote("ValidateUniqueNumber", "Employee", AdditionalFields = "Id")]
        public int Number { get; set; }

        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "Last,Id")]
        public string First { get; set; }

        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "First,Id")]
        public string Last { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsManager { get; set; }

        public string Full { get; set; }

        [Display(Name = "Company Name")]
        public long CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string MainCompanyName { get; set; }
    }
}