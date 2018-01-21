
namespace KendoGridBinderEx.Examples.Business.Repository
{
    public interface IRepositoryConfig
    {
        bool DeleteAllowed { get; }
        bool InsertAllowed { get; }
        bool UpdateAllowed { get; }
    }
}
