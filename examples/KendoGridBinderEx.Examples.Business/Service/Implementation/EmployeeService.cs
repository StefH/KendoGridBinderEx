using System.Linq;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public EmployeeService(IRepository<Employee> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            AutoCommit = true;
        }

        public IQueryable<Employee> GetManagers()
        {
            return Repository.AsQueryable().Where(e => e.IsManager);
        }

        public bool IsNumberUnique(Employee current, int number)
        {
            bool isProvided = current != null;
            long currentId = isProvided ? current.Id : 0;

            return !Repository.AsQueryable().Any(e => e.EmployeeNumber == number && (!isProvided || (isProvided && currentId != e.Id)));
        }

        public bool IsEmailUnique(Employee current, string email)
        {
            bool isProvided = current != null;
            long currentId = isProvided ? current.Id : 0;

            return !Repository.AsQueryable().Any(e => e.Email == email && (!isProvided || (isProvided && currentId != e.Id)));
        }

        public bool IsFullNameUnique(Employee current, string firstName, string lastName)
        {
            bool isProvided = current != null;
            long currentId = isProvided ? current.Id : 0;

            return !Repository.AsQueryable().Any(e => e.FirstName == firstName && e.LastName == lastName && (!isProvided || (isProvided && currentId != e.Id)));
        }
    }
}