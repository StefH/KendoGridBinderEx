using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;
using System.Linq;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class UserController : BaseGridController<User, UserVM>
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
            : base(service)
        {
            _userService = service;
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<User, UserVM>()
                .ForMember(vm => vm.Id, opt => opt.Ignore())
                .ForMember(vm => vm.RolesAsCSVString, opt => opt.Ignore())
                ;

            Mapper.CreateMap<UserVM, User>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Role, RoleVM>()
               ;

            Mapper.CreateMap<RoleVM, Role>()
                .ForAllMembers(opt => opt.Ignore())
                ;
        }

        protected override IQueryable<User> GetQueryable()
        {
            return _userService.AsQueryable(e => e.Roles);
        }
    }
}