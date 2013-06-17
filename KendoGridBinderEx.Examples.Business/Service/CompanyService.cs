using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        public CompanyService(IRepositoryEx<Company> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }
    }
}