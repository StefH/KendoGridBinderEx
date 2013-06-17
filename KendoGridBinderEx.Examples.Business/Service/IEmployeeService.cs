using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        IQueryable<Employee> GetManagers();
        bool IsNumberUnique(Employee current, int number);
        bool IsEmailUnique(Employee current, string email);
        bool IsFullNameUnique(Employee current, string firstName, string lastName);
    }
}