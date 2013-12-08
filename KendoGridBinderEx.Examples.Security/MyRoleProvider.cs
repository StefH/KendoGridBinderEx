using System;
using System.Linq;
using System.Web.Security;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Unity;

namespace KendoGridBinderEx.Examples.Security
{
    public class MyRoleProvider : RoleProvider
    {
        private string _applicationName;

        /// <inheritdoc />
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        /// <inheritdoc />
        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            UserRoleService.AddUsersToRoles(userNames, roleNames);
        }

        /// <inheritdoc />
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            UserRoleService.DeleteUsersFromRoles(userNames, roleNames);
        }

        /// <inheritdoc />
        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return RoleService.GetByName(roleName).Users
                .Where(u => u.DisplayName != null && u.DisplayName.Contains(usernameToMatch))
                .Select(u => u.IdentityName).ToArray();
        }

        /// <inheritdoc />
        public override string[] GetAllRoles()
        {
            return RoleService.GetAll().Select(r => r.Name).ToArray();
        }

        /// <inheritdoc />
        public override string[] GetRolesForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName is null or empty");
            }

            return RoleService.AsQueryableNoTracking().Where(r => r.Users.Any(u => u.IdentityName == userName)).Select(r => r.Name).ToArray();
        }

        /// <inheritdoc />
        public override string[] GetUsersInRole(string roleName)
        {
            return RoleService.GetByName(roleName).Users.Select(u => u.IdentityName).ToArray();
        }

        /// <inheritdoc />
        public override bool IsUserInRole(string userName, string roleName)
        {
            return GetRolesForUser(userName).Contains(roleName);
        }

        /// <inheritdoc />
        public override bool RoleExists(string roleName)
        {
            return RoleService.AsQueryableNoTracking().Any(r => r.Name == roleName);
        }

        private static IRoleService RoleService
        {
            get
            {
                return UnityResolver.Resolve<IRoleService>();
            }
        }

        private static IUserRoleService UserRoleService
        {
            get
            {
                return UnityResolver.Resolve<IUserRoleService>();
            }
        }
    }
}