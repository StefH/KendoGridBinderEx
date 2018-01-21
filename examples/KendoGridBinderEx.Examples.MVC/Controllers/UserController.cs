using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;
using System.Linq;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class UserController : BaseMvcGridController<User, UserVM>
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
            : base(service)
        {
            _userService = service;
        }

        protected override IQueryable<User> GetQueryable()
        {
            return _userService.AsQueryable(e => e.Roles);
        }
    }
}