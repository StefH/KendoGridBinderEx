
namespace KendoGridBinderEx.Examples.MVC.Data.Repository
{
    public interface IRepositoryConfig
    {
        bool DeleteAllowed { get; }
        bool InsertAllowed { get; }
        bool UpdateAllowed { get; }
    }
}
