using System.Collections.Generic;
using System.Web.Http;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Unity;
using KendoGridBinderEx.Examples.MVC.AutoMapper;
using KendoGridBinderEx.Examples.MVC.Custom;
using KendoGridBinderEx.ModelBinder.Api;
using Microsoft.Practices.Unity;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class EmployeeApiController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly KendoGridExQueryableHelper _kendoGridExQueryableHelper;

        public EmployeeApiController()
        {
            _employeeService = UnityBootstrapper.Container.Resolve<IEmployeeService>();
            _kendoGridExQueryableHelper = new KendoGridExQueryableHelper(AutoMapperConfig.MapperConfiguration);
        }

        [HttpPost]
        public KendoGridEx<Employee, Employee> Grid(KendoGridApiRequest request)
        {
            return _kendoGridExQueryableHelper.ToKendoGridEx<Employee, Employee>(_employeeService.AsQueryable(), request);
        }

        [HttpPost]
        public KendoGridEx<Employee, Employee> GridWithJson(KendoGridApiRequest request)
        {
            return _kendoGridExQueryableHelper.ToKendoGridEx<Employee, Employee>(_employeeService.AsQueryable(), request);
        }

        [HttpPost]
        public KendoGridEx<Employee, Employee> GridWithJsonCustom(CustomApiRequest request)
        {
            return _kendoGridExQueryableHelper.ToKendoGridEx<Employee, Employee>(_employeeService.AsQueryable(), request);
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