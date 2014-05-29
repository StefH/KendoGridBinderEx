using System.Collections.Generic;
using System.Web.Http;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Unity;
using KendoGridBinderEx.ModelBinder.Api;
using KendoGridBinderEx.QueryableExtensions;
using Microsoft.Practices.Unity;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class EmployeeApiController : ApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeApiController()
        {
            _employeeService = UnityBootstrapper.Container.Resolve<IEmployeeService>();
        }

        [HttpPost]
        public KendoGridEx<Employee, Employee> Grid(KendoGridApiRequest request)
        {
            return _employeeService.AsQueryable().ToKendoGridEx<Employee, Employee>(request);
        }

        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }
    }
}