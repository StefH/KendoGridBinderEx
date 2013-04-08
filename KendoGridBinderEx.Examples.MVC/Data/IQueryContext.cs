using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KendoGridBinderEx.Examples.MVC.Data
{
    public interface IQueryContext<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query { get; }

        IEnumerable<Expression<Func<TEntity, object>>> IncludeProperties { get; }

        IEnumerable<string> Includes { get; }

        IQueryContext<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}