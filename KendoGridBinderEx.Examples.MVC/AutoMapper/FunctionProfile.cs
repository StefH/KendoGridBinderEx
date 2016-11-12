using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public class FunctionProfile : Profile
    {
        public FunctionProfile()
        {
            CreateMap<Function, FunctionVM>()
               ;

            CreateMap<FunctionVM, Function>()
                .ForMember(e => e.Employees, opt => opt.Ignore())
                .ForMember(e => e.SubFunctions, opt => opt.Ignore())
                ;
        }
    }
}