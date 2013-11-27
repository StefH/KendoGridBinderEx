using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using KendoGridBinderEx.Examples.Business.Enums;
using PropertyTranslator;
using System.Data.Entity.SqlServer;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    [Table("KendoGrid_User")]
    public class User : Entity
    {
        #region CompiledExpressionMaps
        private static readonly CompiledExpressionMap<User, bool> IsAdministratorExpr =
            DefaultTranslationOf<User>.Property(u => u.IsAdministrator).Is(u => u.Roles.Any(r => r.Id == (long)ERole.Administrator));

        private static readonly CompiledExpressionMap<User, bool> IsSuperUserExpr =
            DefaultTranslationOf<User>.Property(u => u.IsSuperUser).Is(u => u.Roles.Any(r => r.Id == (long)ERole.SuperUser));
        /*
        private static readonly CompiledExpressionMap<User, string> RolesAsCSVStringExpr =
            DefaultTranslationOf<User>.Property(u => u.RolesAsCSVString).Is(u => SqlFunctions.Stuff(;*/
        #endregion

        public string IdentityName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        [InverseProperty("Users")]
        public virtual List<Role> Roles { get; set; }

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
        /*
        [NotMapped]
        public string RolesAsCSVString
        {
            get
            {
                return RolesAsCSVStringrExpr.Evaluate(this);
            }
        }*/
    }
}