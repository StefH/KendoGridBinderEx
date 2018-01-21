using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KendoGridBinder;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.AutoMapper;
using KendoGridBinder.ModelBinder.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public abstract class BaseMvcGridController<TEntity, TViewModel> : BaseMvcController<TEntity, TViewModel>
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        private readonly KendoGridQueryableHelper _kendoGridExQueryableHelper;

        protected BaseMvcGridController(IBaseService<TEntity> service) : base(service)
        {
            _kendoGridExQueryableHelper = new KendoGridQueryableHelper(AutoMapperConfig.MapperConfiguration);
        }

        protected JsonResult GetKendoGridAsJson(KendoGridMvcRequest request, IQueryContext<TEntity> queryContext)
        {
            return Json(_kendoGridExQueryableHelper.ToKendoGridEx<TEntity, TViewModel>(queryContext.Query, request, queryContext.Includes));
        }

        protected JsonResult GetKendoGridAsJson(KendoGridMvcRequest request, IQueryable<TEntity> query, IEnumerable<string> includes = null)
        {
            return Json(_kendoGridExQueryableHelper.ToKendoGridEx<TEntity, TViewModel>(query, request, includes));
        }

        protected KendoGrid<TEntity, TViewModel> GetKendoGrid(KendoGridMvcRequest request, IQueryable<TEntity> query)
        {
            return _kendoGridExQueryableHelper.ToKendoGridEx<TEntity, TViewModel>(query, request);
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