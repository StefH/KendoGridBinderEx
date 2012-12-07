using EntityFramework.Patterns;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Repository;

namespace KendoGridBinder.Examples.MVC.Data.Service
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