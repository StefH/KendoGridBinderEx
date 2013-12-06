using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using System.Threading.Tasks;

namespace KendoGridBinderEx.Examples.Business.Service.Interface
{
    public interface IBaseService<TEntity> where TEntity : class, IEntity
    {
        #region Common
        void Dispose();
        IUnitOfWork UnitOfWork { get; }
        IRepository<TEntity> Repository { get; }
        IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> AsQueryableNoTracking(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties);

        void Insert(TEntity model);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void BulkInsert(IEnumerable<TEntity> enumerable, Func<IBaseService<TEntity>> createService, int step);
        void Save();
        #endregion

        #region Synchronous
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(long id, params Expression<Func<TEntity, object>>[] includeProperties);
        #endregion

        #region Asynchronous
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(long id, params Expression<Func<TEntity, object>>[] includeProperties);
        #endregion
    }
}