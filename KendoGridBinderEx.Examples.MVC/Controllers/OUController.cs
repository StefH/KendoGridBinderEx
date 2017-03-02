using System.Data.Entity;
using System.Web.Mvc;
using KendoGridBinder;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.AutoMapper;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinder.ModelBinder.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class OUController : BaseMvcGridController<OU, OUVM>
    {
        private readonly IOUService _ouService;
        private readonly KendoGridQueryableHelper _kendoGridQueryableHelper;

        public OUController(IOUService service)
            : base(service)
        {
            _ouService = service;
            _kendoGridQueryableHelper = new KendoGridQueryableHelper(AutoMapperConfig.MapperConfiguration);
        }

        public ActionResult VirtualScrollable()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GridVirtualScrollable(KendoGridMvcRequest request)
        {
            var query = GetQueryable().AsNoTracking();
            var kendoGrid = _kendoGridQueryableHelper.ToKendoGridEx<OU, OUVM>(query, request);
            return Json(kendoGrid);
        }

        /*
        public void Bulk()
        {
            const int numOUs = 10000;
            var list = new List<OU>();
            for (int i = 1000000; i < 1000000 + numOUs; i++)
            {
                var ou = new OU
                {
                    Code = i.ToString(CultureInfo.InvariantCulture),
                    Name = "X_" + i
                };

                list.Add(ou);
            }

            Func<IOUService> createService = () => DependencyResolver.Current.GetService<IOUService>();
            _ouService.BulkInsert(list, createService, 100);
        }*/
    }
}