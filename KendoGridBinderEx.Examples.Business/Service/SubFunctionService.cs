using System.Linq;
using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public class SubFunctionService : BaseService<SubFunction>, ISubFunctionService
    {
        public SubFunctionService(IRepositoryEx<SubFunction> repository, IUnitOfWork unitOfWork)
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