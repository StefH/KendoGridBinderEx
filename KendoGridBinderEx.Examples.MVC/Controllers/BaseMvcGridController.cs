using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.ModelBinder.Mvc;
using KendoGridBinderEx.QueryableExtensions;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public abstract class BaseMvcGridController<TEntity, TViewModel> : BaseMvcController<TEntity, TViewModel>
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected BaseMvcGridController(IBaseService<TEntity> service)
            : base(service)
        {
        }

        protected JsonResult GetKendoGridAsJson(KendoGridMvcRequest request, IQueryContext<TEntity> queryContext)
        {
            return Json(queryContext.ToKendoGrid<TViewModel>(request));
        }

        protected JsonResult GetKendoGridAsJson(KendoGridMvcRequest request, IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return Json(query.ToKendoGridEx<TEntity, TViewModel>(request, includes));
        }

        protected JsonResult GetKendoGridAsJson(KendoGridMvcRequest request, IQueryable<TEntity> query)
        {
            return GetKendoGridAsJson(request, query, null);
        }

        protected KendoGridEx<TEntity, TViewModel> GetKendoGrid(KendoGridMvcRequest request, IQueryable<TEntity> query)
        {
            return query.ToKendoGridEx<TEntity, TViewModel>(request);
        }

        #region MVC Grid Actions
        [HttpPost]
        public JsonResult Grid(KendoGridMvcRequest request)
        {
            var entities = GetQueryableNoTracking();
            return GetKendoGridAsJson(request, entities);
        }
        #endregion
    }
}