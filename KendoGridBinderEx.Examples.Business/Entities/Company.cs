using System.Collections.Generic;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }

        public long? MainCompanyId { get; set; }

        public virtual MainCompany MainCompany { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}