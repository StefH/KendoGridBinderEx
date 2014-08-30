using System.Collections.Generic;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public class Function : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public virtual List<SubFunction> SubFunctions { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}