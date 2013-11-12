using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace KendoGridBinderEx.Examples.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }

            GC.SuppressFinalize(this);
        }
        #endregion

        #region IUnitOfWork Members
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public DbContextConfiguration Configuration
        {
            get
            {
                return _dbContext.Configuration;
            }
        }
        #endregion
    }
}