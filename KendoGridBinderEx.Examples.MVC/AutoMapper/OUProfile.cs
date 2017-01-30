using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public class OUProfile : Profile
    {
        public OUProfile()
        {
            CreateMap<OU, OUVM>()
                ;

            CreateMap<OUVM, OU>()
                ;
        }
    }
}