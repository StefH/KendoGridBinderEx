using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Enums;

namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface IUserService : IBaseService<User>
    {
        bool UserHasRole(User user, Role role);
        bool UserHasRole(User user, string role);
        bool UserHasRole(User user, ERole role);
    }
}