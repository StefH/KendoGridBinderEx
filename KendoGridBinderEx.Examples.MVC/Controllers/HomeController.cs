using System.Web.Mvc;
using StackExchange.Profiling;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Set page title"))
            {
                ViewBag.Title = "Home Page";
            }

            return View();
        }
    }
}