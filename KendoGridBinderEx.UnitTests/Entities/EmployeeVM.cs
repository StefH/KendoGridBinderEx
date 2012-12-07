using System;

namespace KendoGridBinderEx.UnitTests.Entities
{
    public class EmployeeVM : Entity
    {
        public int Number { get; set; }

        public string First { get; set; }

        public string Last { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsManager { get; set; }

        public string Full { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string MainCompanyName { get; set; }
    }
}