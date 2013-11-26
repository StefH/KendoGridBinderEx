using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class UserController : BaseGridController<User, UserVM>
    {
        private readonly IUserService _UserService;

        public UserController(IUserService service)
            : base(service)
        {
            _UserService = service;
        }

        public static void InitAutoMapper()
        {
            Mapper.CreateMap<User, UserVM>()
                .ForMember(vm => vm.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<UserVM, User>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Roles, opt => opt.Ignore())
                ;
        }
    }
}