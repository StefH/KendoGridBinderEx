using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_Role")]
    public class Role : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [InverseProperty("Roles")]
        public virtual List<User> Users { get; set; }
    }
}