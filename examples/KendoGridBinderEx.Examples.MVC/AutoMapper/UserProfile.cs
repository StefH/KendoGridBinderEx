using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserVM>()
               .ForMember(vm => vm.Id, opt => opt.Ignore())
               .ForMember(vm => vm.RolesAsCSVString, opt => opt.Ignore())
               ;

            CreateMap<UserVM, User>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                ;

            CreateMap<Role, RoleVM>()
               ;

            CreateMap<RoleVM, Role>()
                .ForAllMembers(opt => opt.Ignore())
                ;
        }
    }
}