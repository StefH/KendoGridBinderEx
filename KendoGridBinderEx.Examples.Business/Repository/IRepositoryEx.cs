using System;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Patterns;

namespace KendoGridBinderEx.Examples.Business.Repository
{
    public interface IRepositoryEx<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}