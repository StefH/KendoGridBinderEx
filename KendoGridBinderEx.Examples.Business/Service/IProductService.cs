using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;

namespace KendoGridBinderEx.Examples.MVC.Data.Service
{
    public interface IProductService : IBaseService<Product>
    {
        IQueryable<Product> GetTop3Products();
        bool IsCodeUnique(Product current, string code);
    }
}