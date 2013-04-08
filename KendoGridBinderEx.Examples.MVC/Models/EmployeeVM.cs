using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.MVC.Data.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class EmployeeVM : IEntity
    {
        public long Id { get; set; }

        [Remote("ValidateUniqueNumber", "Employee", AdditionalFields = "Id")]
        public int Number { get; set; }

        [Display(ResourceType = typeof(GlobalResources), Name = GlobalResourceLiterals.FirstName)]
        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "Last,Id")]
        public string First { get; set; }

        [Display(ResourceType = typeof(GlobalResources), Name = GlobalResourceLiterals.LastName)]
        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "First,Id")]
        public string Last { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsManager { get; set; }

        public string Full { get; set; }

        [Display(ResourceType = typeof(GlobalResources), Name = GlobalResourceLiterals.CompanyName)]
        public long CompanyId { get; set; }

        [Display(ResourceType = typeof(GlobalResources), Name = GlobalResourceLiterals.CompanyName)]
        public string CompanyName { get; set; }

        public long CountryId { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string MainCompanyName { get; set; }
    }
}