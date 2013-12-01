using System;
using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class QueryableExtensions
    {
        public static KendoGridEx<TModel> ToKendoGrid<TModel>(this IQueryable<TModel> query, KendoGridRequest request)
        {
            return new KendoGridEx<TModel>(request, query);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, KendoGridRequest request, IEnumerable<string> includes = null, Dictionary<string, string> mappings = null, Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null, bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection);
        }

        public static IEnumerable<TViewModel> FilterBy<TEntity, TViewModel>(this IQueryable<TEntity> query, KendoGridRequest request, IEnumerable<string> includes = null, Dictionary<string, string> mappings = null, Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null, bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection).Data;
        }
    }
}