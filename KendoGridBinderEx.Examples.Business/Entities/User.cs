using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using KendoGridBinderEx.Examples.Business.Enums;
using PropertyTranslator;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public class User : Entity
    {
        #region CompiledExpressionMaps
        private static readonly CompiledExpressionMap<User, bool> IsAdministratorExpr =
            DefaultTranslationOf<User>.Property(u => u.IsAdministrator).Is(u => u.Roles.Any(r => r.Id == (long)ERole.Administrator));

        private static readonly CompiledExpressionMap<User, bool> IsSuperUserExpr =
            DefaultTranslationOf<User>.Property(u => u.IsSuperUser).Is(u => u.Roles.Any(r => r.Id == (long)ERole.SuperUser));

        private static readonly CompiledExpressionMap<User, bool> IsApplicationUserExpr =
            DefaultTranslationOf<User>.Property(u => u.IsApplicationUser).Is(u => u.Roles.Any(r => r.Id == (long)ERole.ApplicationUser));
        #endregion

        public string IdentityName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public ICollection<Role> Roles { get; set; }

        [NotMapped]
        public bool IsAdministrator
        {
            get
            {
                return IsAdministratorExpr.Evaluate(this);
            }
        }

        [NotMapped]
        public bool IsSuperUser
        {
            get
            {
                return IsSuperUserExpr.Evaluate(this);
            }
        }

        [NotMapped]
        public bool IsApplicationUser
        {
            get
            {
                return IsApplicationUserExpr.Evaluate(this);
            }
        }

        public bool HasRole(string role)
        {
            return Roles.Any(r => r.Name == role);
        }
    }
}