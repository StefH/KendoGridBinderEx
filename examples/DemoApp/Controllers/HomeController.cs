using DemoApp.Models;
using KendoGridBinder;
using KendoGridBinder.ModelBinder.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Grid(KendoGridMvcRequest request)
        {
            var employees = new[]
            {
                new Employee { EmployeeId = 1, FirstName = "Bill", LastName = "Jones", Email = "bill@email.com" },
                new Employee { EmployeeId = 2, FirstName = "Rob", LastName = "Johnson", Email = "rob@email.com" },
                new Employee { EmployeeId = 3, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com" }
            };

            var search = GetSearch(request);
            var viewModel = string.IsNullOrWhiteSpace(search) ?
              employees :
              employees.Where(e => e.FirstName.ToUpper(CultureInfo.CurrentCulture).Contains(search)).Select(e => e).AsEnumerable();
            return Json(viewModel);
        }

        [HttpPost]
        public JsonResult Grid2(KendoGridMvcRequest request)
        {
            var employees = new[]
            {
                new Employee { EmployeeId = 1, FirstName = "Bill", LastName = "Jones", Email = "bill@email.com" },
                new Employee { EmployeeId = 2, FirstName = "Rob", LastName = "Johnson", Email = "rob@email.com" },
                new Employee { EmployeeId = 3, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com" }
            };

            var search = GetSearch(request);
            var viewModel = string.IsNullOrWhiteSpace(search) ?
                employees :
                employees.Where(e => e.FirstName.ToUpper(CultureInfo.CurrentCulture).Contains(search)).Select(e => e).AsEnumerable();

            var grid = new KendoGrid<Employee>(request, viewModel);
            return Json(grid, new JsonSerializerSettings()); // Do no use CamelCasing
        }

        private string GetSearch(KendoGridMvcRequest request)
        {
            string search = string.Empty;
            var searchObject = request?.FilterObjectWrapper?.FilterObjects?.FirstOrDefault(o => !string.IsNullOrWhiteSpace(o.Field1));

            if (searchObject != null)
            {
                search = searchObject.Value1.ToUpper(CultureInfo.CurrentCulture);
            }

            return search;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}