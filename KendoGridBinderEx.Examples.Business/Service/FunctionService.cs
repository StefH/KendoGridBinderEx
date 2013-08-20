using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public class FunctionService : BaseService<Function>, IFunctionService
    {
        public FunctionService(IRepository<Function> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }
    }
}