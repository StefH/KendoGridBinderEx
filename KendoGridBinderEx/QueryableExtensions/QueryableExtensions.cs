using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KendoGridBinderEx.AutoMapperExtensions;

namespace KendoGridBinderEx.QueryableExtensions
{
    public static class QueryableExtensions
    {
        public static KendoGridEx<TModel> ToKendoGrid<TModel>([NotNull] this IQueryable<TModel> query, KendoGridBaseRequest request)
        {
            return new KendoGridEx<TModel>(request, query);
        }

        public static KendoGridEx<TModel> ToKendoGridEx<TModel>([NotNull] this IQueryable<TModel> query,
            KendoGridBaseRequest request,
            IEnumerable<string> includes = null,
            Dictionary<string, MapExpression<TModel>> mappings = null,
            Func<IQueryable<TModel>, IEnumerable<TModel>> conversion = null,
            bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection);
        }

        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>([NotNull] this IQueryable<TEntity> query,
            KendoGridBaseRequest request,
            IEnumerable<string> includes = null,
            Dictionary<string, MapExpression<TEntity>> mappings = null,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
            bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection);
        }

        public static IEnumerable<TViewModel> FilterBy<TEntity, TViewModel>([NotNull] this IQueryable<TEntity> query,
            KendoGridBaseRequest request,
            IEnumerable<string> includes = null,
            Dictionary<string, MapExpression<TEntity>> mappings = null,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
            bool canUseAutoMapperProjection = true)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes, mappings, conversion, canUseAutoMapperProjection).Data;
        }
    }
}