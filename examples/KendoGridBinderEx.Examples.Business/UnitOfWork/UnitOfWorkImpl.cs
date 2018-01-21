using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using JetBrains.Annotations;

namespace KendoGridBinderEx.Examples.Business.UnitOfWork
{
    public class UnitOfWorkImpl : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;

        public UnitOfWorkImpl([NotNull] DbContext dbContext)
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