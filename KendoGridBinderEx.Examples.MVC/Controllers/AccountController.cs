using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;
using KendoGridBinderEx.Examples.Security;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class AccountController : BaseMvcController<User, UserVM>
    {
        private readonly IUserService _userService;

        public AccountController(IUserService service)
            : base(service)
        {
            _userService = service;
        }

        public ActionResult Login(LoginUserVM model)
        {
            var user = _userService.GetByIdentityName(model.IdentityName);
            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(1, user.IdentityName, DateTime.Now, DateTime.Now.AddMinutes(30), true, String.Empty, FormsAuthentication.FormsCookiePath);
                string encryptedCookie = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie);
                cookie.Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies.Add(cookie);

                var appUser = new ApplicationUser(user);
                HttpContext.User = appUser;
            }

            //HttpContext.User.Identity.
            return View(model);
        }
    }
}