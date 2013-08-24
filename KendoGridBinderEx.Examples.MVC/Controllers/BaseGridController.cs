using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using KendoGridBinder;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Service.Interface;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public abstract class BaseGridController<TEntity, TViewModel> : BaseController<TEntity, TViewModel>
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected BaseGridController(IBaseService<TEntity> service)
            : base(service)
        {

        }

        protected JsonResult GetKendoGridAsJson(KendoGridRequest request, IQueryContext<TEntity> queryContext)
        {
            return Json(GetKendoGrid(request, queryContext.Query, queryContext.Includes));
        }

        protected JsonResult GetKendoGridAsJson(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return Json(GetKendoGrid(request, query, includes));
        }

        protected JsonResult GetKendoGridAsJson(KendoGridRequest request, IQueryable<TEntity> query)
        {
            return GetKendoGridAsJson(request, query, null);
        }

        protected KendoGridEx<TEntity, TViewModel> GetKendoGrid(KendoGridRequest request, IQueryable<TEntity> query)
        {
            return GetKendoGrid(request, query, null);
        }

        protected KendoGridEx<TEntity, TViewModel> GetKendoGrid(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query, includes);
        }

        #region MVC Grid Actions
        [HttpPost]
        public JsonResult Grid(KendoGridRequest request)
        {
            var entities = GetQueryable().AsNoTracking();
            return GetKendoGridAsJson(request, entities);
        }
        #endregion
    }
}