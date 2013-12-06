using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using KendoGridBinderEx.Examples.Business.Enums;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IRepository<User> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }

        public bool UserHasRole(User user, Role role)
        {
            return user.Roles != null ? user.Roles.Any(r => r.Id == role.Id) : false;
        }
        
        public bool UserHasRole(User user, string role)
        {
            return user.Roles != null ? user.Roles.Any(r => r.Name == role) : false;
        }

        public bool UserHasRole(User user, ERole role)
        {
            return user.Roles != null ? user.Roles.Any(r => r.Id == (long) role) : false;
        }
    }
}