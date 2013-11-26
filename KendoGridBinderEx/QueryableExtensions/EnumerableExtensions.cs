using System;
using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class EnumerableExtensions
    {
        public static KendoGridEx<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, KendoGridRequest request)
        {
            return new KendoGridEx<TModel>(request, query.AsQueryable());
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, KendoGridRequest request, IEnumerable<string> includes = null, Dictionary<string, string> mappings = null, Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null, bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query.AsQueryable(), includes, mappings, conversion, canUseAutoMapperProjection);
        }
    }
}