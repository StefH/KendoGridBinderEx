using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_User")]
    public class User : Entity
    {
        public string IdentityName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        [InverseProperty("Users")]
        public virtual List<Role> Roles { get; set; }
    }
}