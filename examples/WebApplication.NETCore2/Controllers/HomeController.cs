using System.Collections.Generic;
using System.Linq;
using KendoGridBinder;
using KendoGridBinder.ModelBinder.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.NETCore2.Models;

namespace WebApplication.NETCore2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Grid(KendoGridMvcRequest request)
        {
            var employees = new List<EmployeeModel>
            {
                new EmployeeModel { EmployeeId = 1, FirstName = "Bill", LastName = "Jones", Email = "bill@email.com" },
                new EmployeeModel { EmployeeId = 2, FirstName = "Rob", LastName = "Johnson", Email = "rob@email.com" },
                new EmployeeModel { EmployeeId = 3, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com" }
            };

            for (int i = 10; i < 40; i++)
            {
                employees.Add(new EmployeeModel { EmployeeId = i, FirstName = "FN:" + i, LastName = "LN" + i, Email = $"user_{i}@email.com" });
            }

            var grid = new KendoGrid<EmployeeModel>(request, employees.AsQueryable());
            return Json(grid, new JsonSerializerSettings()); // Do no use CamelCasing
        }
    }
}