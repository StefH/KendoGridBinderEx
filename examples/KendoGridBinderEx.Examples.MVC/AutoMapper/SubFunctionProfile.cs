using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public class SubFunctionProfile : Profile
    {
        public SubFunctionProfile()
        {
            CreateMap<SubFunction, SubFunctionVM>()
                .ForMember(vm => vm.FunctionId, opt => opt.MapFrom(e => e.Function.Id))
                ;

            CreateMap<SubFunctionVM, SubFunction>()
                .ForMember(e => e.Function, opt => opt.Ignore())
                .ForMember(e => e.Employees, opt => opt.Ignore())
                ;
        }
    }
}