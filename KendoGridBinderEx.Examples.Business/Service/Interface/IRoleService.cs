using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface IRoleService : IBaseService<Role>
    {
        Role GetByName(string name);
    }
}