using System.Collections.Generic;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class UserVM : IEntity
    {
        public long Id { get; set; }

        public string IdentityName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public List<RoleVM> Roles { get; set; }

        //public string RolesAsString { get; set; }

        public bool IsAdministrator { get; set; }

        public bool IsSuperUser { get; set; }
    }
}