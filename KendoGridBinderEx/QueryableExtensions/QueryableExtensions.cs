using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class QueryableExtensions
    {
        public static KendoGrid<TModel> ToKendoGrid<TModel>(this IQueryable<TModel> query, KendoGridRequest request)
        {
            return new KendoGrid<TModel>(request, query);
        }

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

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IQueryable<TEntity> query, IEnumerable<string> includes, Dictionary<string, string> mappings, bool canUseAutoMapperProjection, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, canUseAutoMapperProjection);
        }
    }
}