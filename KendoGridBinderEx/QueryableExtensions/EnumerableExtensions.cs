//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KendoGridBinderEx.AutoMapperExtensions;

//namespace KendoGridBinderEx.QueryableExtensions
//{
//    public static class EnumerableExtensions
//    {
//        public static KendoGridEx<TModel> ToKendoGrid<TModel>(this IEnumerable<TModel> query, KendoGridBaseRequest request)
//        {
//            return new KendoGridEx<TModel>(request, query.AsQueryable());
//        }

//        public static KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(this IEnumerable<TEntity> query, KendoGridBaseRequest request, IEnumerable<string> includes = null, Dictionary<string, MapExpression<TEntity>> mappings = null, Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null, bool canUseAutoMapperProjection = true)
//        {
//            return new KendoGridEx<TEntity, TViewModel>(request, query.AsQueryable(), includes, mappings, conversion, canUseAutoMapperProjection);
//        }
//    }
//}