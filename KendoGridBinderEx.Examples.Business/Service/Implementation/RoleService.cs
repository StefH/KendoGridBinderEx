using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IRepository<Role> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }

        /// <summary>
        /// Returns an existing Role
        /// </summary>
        /// <param name="name">The name of the role</param>
        /// <returns></returns>
        public Role GetByName(string name)
        {
            return AsQueryable().FirstOrDefault(r => r.Name == name);
        }
    }
}