using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.MVC.Data.Entities;
using KendoGridBinderEx.Examples.MVC.Data.Repository;

namespace KendoGridBinderEx.Examples.MVC.Data.Service
{
    public class CompanyService : BaseService<Company>
    {
        public CompanyService(IRepositoryEx<Company> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = false;
        }
    }
}