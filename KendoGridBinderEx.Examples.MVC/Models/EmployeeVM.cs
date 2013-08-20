using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class EmployeeVM : IEntity
    {
        public long Id { get; set; }

        [Remote("ValidateUniqueNumber", "Employee", AdditionalFields = "Id")]
        public int Number { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.FirstName)]
        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "LastName,Id")]
        public string First { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.LastName)]
        [Remote("ValidateUniqueFullName", "Employee", AdditionalFields = "First,Id")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.Employee_IsManager)]
        public bool IsManager { get; set; }

        public string Full { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.CompanyName)]
        public long CompanyId { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.CompanyName)]
        public string CompanyName { get; set; }

        public long CountryId { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string MainCompanyName { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.Function)]
        public long FunctionId { get; set; }

        public string FunctionCode { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.Function)]
        public string FunctionName { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.SubFunction)]
        public long SubFunctionId { get; set; }

        public string SubFunctionCode { get; set; }

        [Display(ResourceType = typeof(GlobalResources_Business), Name = GlobalResources_BusinessLiterals.SubFunction)]
        public string SubFunctionName { get; set; }
    }
}