using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public interface IBaseService<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(long id, params Expression<Func<TEntity, object>>[] includeProperties);
        void Insert(TEntity model);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}