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
        /*
        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, IEnumerable<string> includes, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, IEnumerable<string> includes, Dictionary<string, string> mappings, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings);
        }
        */
        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, KendoGridRequest request, IEnumerable<string> includes = null, Dictionary<string, string> mappings = null, Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null, bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection);
        }
    }
}