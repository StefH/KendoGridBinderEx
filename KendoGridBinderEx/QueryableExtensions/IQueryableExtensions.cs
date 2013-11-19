using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class IQueryableExtensions
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
    }

    public static class IEnumerableExtensions
    {
        public static KendoGrid<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, KendoGridRequest request)
        {
            return new KendoGrid<TModel>(request, query);
        }

        public static KendoGrid<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, int totalCount)
        {
            return new KendoGrid<TModel>(query, totalCount);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, IEnumerable<string> includes, KendoGridRequest request)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGrid<TEntity, TViewModel>(this IEnumerable<TViewModel> query, int totalCount)
        {
            return new KendoGridEx<TEntity, TViewModel>(query, totalCount);
        }
    }
}