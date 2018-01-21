using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KendoGridBinderEx.Examples.Business.QueryContext
{
    public class QueryContext<TEntity> : IQueryContext<TEntity> where TEntity : class
    {
        private List<Expression<Func<TEntity, object>>> _includeProperties;

        public IQueryable<TEntity> Query { get; set; }

        public IEnumerable<Expression<Func<TEntity, object>>> IncludeProperties
        {
            get
            {
                return _includeProperties;
            }

            set
            {
                _includeProperties = value.ToList();
            }
        }

        public IEnumerable<string> Includes { get; set; }

        public IQueryContext<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (_includeProperties == null)
            {
                _includeProperties = includeProperties.ToList();
            }
            else
            {
                _includeProperties.AddRange(includeProperties);
            }

            return this;
        }

        //public KendoGridEx<TEntity, TViewModel> ToKendoGrid<TViewModel>(KendoGridBaseRequest request)
        //{
        //    return new KendoGridEx<TEntity, TViewModel>(request, Query, Includes);
        //}
    }
}