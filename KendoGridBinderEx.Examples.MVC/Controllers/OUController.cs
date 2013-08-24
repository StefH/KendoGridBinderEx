using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;

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
    }
}