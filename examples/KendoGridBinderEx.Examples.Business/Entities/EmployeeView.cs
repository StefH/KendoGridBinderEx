﻿using System;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public class EmployeeView : Entity
    {
        public int EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        //public long? CompanyId { get; set; }

        //public long? CountryId { get; set; }

        //public long? FunctionId { get; set; }

        //public long? SubFunctionId { get; set; }

        //public Company Company { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        //public Function Function { get; set; }

        //public SubFunction SubFunction { get; set; }

        public int? Assigned { get; set; }

        public bool IsAssigned { get; set; }

        public bool IsManager { get; set; }

        public string FullName { get; set; }
    }
}