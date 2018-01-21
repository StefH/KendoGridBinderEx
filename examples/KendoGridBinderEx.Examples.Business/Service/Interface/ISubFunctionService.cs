using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface ISubFunctionService : IBaseService<SubFunction>
    {
        IQueryable<SubFunction> GetByFunctionId(long functionId);
    }
}