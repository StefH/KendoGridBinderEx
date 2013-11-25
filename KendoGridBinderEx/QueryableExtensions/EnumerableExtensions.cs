using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class EnumerableExtensions
    {
        public static KendoGridEx<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, KendoGridRequest request)
        {
            return new KendoGridEx<TModel>(request, query);
        }

        public static KendoGridEx<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, int totalCount)
        {
            return new KendoGridEx<TModel>(query, totalCount);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query.AsQueryable());
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, IEnumerable<string> includes, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query.AsQueryable(), includes);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TViewModel> query, int totalCount)
        {
            return new KendoGridEx<TEntity, TViewModel>(query, totalCount);
        }
    }
}