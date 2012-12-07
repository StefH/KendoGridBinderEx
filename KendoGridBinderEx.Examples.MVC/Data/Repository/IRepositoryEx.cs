using System;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Patterns;

namespace KendoGridBinder.Examples.MVC.Data.Repository
{
    public interface IRepositoryEx<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}