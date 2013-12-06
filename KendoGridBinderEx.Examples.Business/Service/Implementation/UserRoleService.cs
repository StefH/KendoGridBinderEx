using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<User> _userService;

        public UserRoleService(IRepository<Role> roleService, IRepository<User> userService,IUnitOfWork unitOfWork)
        {
            _roleService = roleService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Adds users to roles
        /// </summary>
        /// <param name="userNames">The identity names users to add to the roles</param>
        /// <param name="roleNames">The names of the roles to add the users to</param>
        public void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            var users = _userService.AsQueryable().Where(u => userNames.Contains(u.IdentityName));

            foreach (var user in users)
            {
                foreach (var role in _roleService.AsQueryable().Where(r => roleNames.Contains(r.Name)))
				{
					if (!user.Roles.Any(r=> r.Id == role.Id))
					{
						role.Users.Add(user);
					}
				}
            }

            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes the users from roles
        /// </summary>
        /// <param name="userNames">The identity names users to remove from the roles</param>
        /// <param name="roleNames">The names of the roles to remove the users from</param>
        public void DeleteUsersFromRoles(string[] userNames, string[] roleNames)
        {

        }
    }
}