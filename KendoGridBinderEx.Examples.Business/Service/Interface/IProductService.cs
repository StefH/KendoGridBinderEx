using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface IProductService : IBaseService<Product>
    {
        IQueryable<Product> GetTop3Products();
        bool IsCodeUnique(Product current, string code);
    }
}