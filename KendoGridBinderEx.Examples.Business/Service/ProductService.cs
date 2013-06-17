using System.Linq;
using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IRepositoryEx<Product> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = true;
        }

        public IQueryable<Product> GetTop3Products()
        {
            return Repository.AsQueryable().OrderBy(p => p.Id).Take(3);
        }

        public bool IsCodeUnique(Product current, string code)
        {
            bool isProvided = current != null;
            long currentId = isProvided ? current.Id : 0;

            return !Repository.AsQueryable().Any(p => p.Code == code && (!isProvided || (isProvided && currentId != p.Id)));
        }
    }
}