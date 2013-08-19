using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Function")]
    public class Function : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public virtual List<SubFunction> SubFunctions { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}