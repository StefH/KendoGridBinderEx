using System.Linq;
using System.Web.Mvc;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;
using KendoGridBinderEx;

namespace KendoGridBinder.Examples.MVC.Controllers
{
    public abstract class BaseGridController<TEntity, TViewModel> : BaseController<TEntity, TViewModel>
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected BaseGridController(BaseService<TEntity> service)
            : base(service)
        {
        }

        protected KendoGridEx<TEntity, TViewModel> GetKendoGrid(KendoGridRequest request, IQueryable<TEntity> query)
        {
            return new KendoGridEx<TEntity, TViewModel>(request, query);
        }

        protected JsonResult GetKendoGridAsJson(KendoGridRequest request, IQueryable<TEntity> query)
        {
            return Json(GetKendoGrid(request, query));
        }

        #region MVC Grid Actions
        [HttpPost]
        public JsonResult Grid(KendoGridRequest request)
        {
            var entities = GetQueryable();
            return GetKendoGridAsJson(request, entities);
        }
        #endregion
    }
}