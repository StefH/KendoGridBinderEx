using System.Data.Entity;
using System.Web.Mvc;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinderEx.QueryableExtensions;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class OUController : BaseGridController<OU, OUVM>
    {
        private readonly IOUService _ouService;

        public OUController(IOUService service)
            : base(service)
        {
            _ouService = service;
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<OU, OUVM>()
                ;

            Mapper.CreateMap<OUVM, OU>()
                ;
        }

        public ActionResult VirtualScrollable()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GridVirtualScrollable(KendoGridRequest request)
        {
            var query = GetQueryable().AsNoTracking();
            return Json(query.ToKendoGridEx<OU, OUVM>(request));
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