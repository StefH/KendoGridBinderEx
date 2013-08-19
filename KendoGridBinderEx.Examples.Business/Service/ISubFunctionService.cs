using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public interface ISubFunctionService : IBaseService<SubFunction>
    {
        IQueryable<SubFunction> GetByFunctionId(long functionId);
    }
}