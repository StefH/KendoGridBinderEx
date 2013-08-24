using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public class SubFunctionService : BaseService<SubFunction>, ISubFunctionService
    {
        public SubFunctionService(IRepository<SubFunction> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = true;
        }

        public IQueryable<SubFunction> GetByFunctionId(long id)
        {
            return Repository.AsQueryable(s => s.Function != null && s.Function.Id == id);
        }
    }
}