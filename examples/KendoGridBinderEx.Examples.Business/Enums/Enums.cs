using System.ComponentModel;

namespace KendoGridBinderEx.Examples.Business.Enums
{
    public enum ERole
    {
        [Description("Administrator")]
        Administrator = 1,

        [Description("SuperUser")]
        SuperUser = 2,

        [Description("ApplicationUser")]
        ApplicationUser = 4
    }
}