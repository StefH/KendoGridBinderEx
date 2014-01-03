
namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface IUserRoleService
    {
        void AddUsersToRoles(string[] userNames, string[] roleNames);
        void DeleteUsersFromRoles(string[] userNames, string[] roleNames);
    }
}