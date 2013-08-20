using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KendoGridBinderEx.Examples.Business.QueryContext;

namespace KendoGridBinderEx.Examples.Business.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        void Delete(TEntity entity);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}