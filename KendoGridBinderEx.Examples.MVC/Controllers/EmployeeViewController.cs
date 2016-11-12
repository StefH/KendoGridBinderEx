using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class EmployeeViewController : BaseMvcGridController<EmployeeView, EmployeeViewVM>
    {
        private readonly IEmployeeViewService _employeeService;

        public EmployeeViewController(IEmployeeViewService employeeService)
            : base(employeeService)
        {
            _employeeService = employeeService;
        }

        protected override IQueryable<EmployeeView> GetQueryable()
        {
            return _employeeService.AsQueryable();
        }

        protected override EmployeeView GetById(long id)
        {
            return _employeeService.GetById(id);
        }
    }
}