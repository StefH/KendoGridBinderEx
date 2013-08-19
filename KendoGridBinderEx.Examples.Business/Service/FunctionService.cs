using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public class FunctionService : BaseService<Function>, IFunctionService
    {
        public FunctionService(IRepositoryEx<Function> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }
    }
}