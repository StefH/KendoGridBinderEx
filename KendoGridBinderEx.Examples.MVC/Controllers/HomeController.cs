using System.Web.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize(Roles="Administrator, SuperUser, ApplicationUser")]
        public ActionResult Index()
        {
            return View();
        }
	}
}