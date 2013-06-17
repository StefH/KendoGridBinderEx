using System.Linq;
using KendoGridBinderEx.Examples.MVC.Data.Entities;

namespace KendoGridBinderEx.Examples.MVC.Data.Service
{
    public interface IProductService : IBaseService<Product>
    {
        IQueryable<Product> GetTop3Products();
        bool IsCodeUnique(Product current, string code);
    }
}