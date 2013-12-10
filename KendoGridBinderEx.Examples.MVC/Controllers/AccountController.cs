using System.Web.Mvc;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(LoginUserVM model)
        {
            return View();
        }
	}
}