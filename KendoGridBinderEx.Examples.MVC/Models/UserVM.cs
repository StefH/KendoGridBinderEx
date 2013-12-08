using System.Linq;
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

        public string RolesAsCSVString
        {
            get
            {
                return Roles != null ? string.Join(",", Roles.Select(r => r.Name)) : string.Empty;
            }
        }

        public bool IsAdministrator { get; set; }

        public bool IsSuperUser { get; set; }

        public bool IsApplicationUser { get; set; }
    }
}