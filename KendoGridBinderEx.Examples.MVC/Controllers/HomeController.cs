using System.Web.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class HomeController : Controller
    {
        // [Authorize(Roles="Guest")]
        public ActionResult Index()
        {
            return View();
        }
	}
}