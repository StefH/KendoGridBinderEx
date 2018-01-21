using System.Data.Entity.Infrastructure;

namespace KendoGridBinderEx.Examples.Business.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Dispose();

        void Commit();

        DbContextConfiguration Configuration { get; }
    }
}