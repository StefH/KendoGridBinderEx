using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using KendoGridBinder.ModelBinder.Mvc;
using KendoGridBinder.UnitTests.Entities;
using NUnit.Framework;

namespace KendoGridBinder.UnitTests.Helpers
{
    public class TestHelper
    {

        #region InitEmployees
        protected static IEnumerable<Employee> InitEmployeesWithData()
        {
            var countryNL = new Country { Id = 1, Code = "NL", Name = "The Netherlands" };
            var countryBE = new Country { Id = 2, Code = "BE", Name = "Belgium" };

            var mainCompany1 = new MainCompany { Id = 1, Name = "m-1" };
            var mainCompany2 = new MainCompany { Id = 2, Name = "m-2" };

            var companyA = new Company { Id = 1, Name = "A", MainCompany = mainCompany1 };
            var companyB = new Company { Id = 2, Name = "B", MainCompany = mainCompany1 };
            var companyC = new Company { Id = 3, Name = "C", MainCompany = mainCompany2 };

            return InitEmployees(countryNL, countryBE, companyA, companyB, companyC);
        }

        protected static IEnumerable<Employee> InitEmployees(Country countryNL = null, Country countryBE = null, Company companyA = null, Company companyB = null, Company companyC = null)
        {
            return new List<Employee>
            {
                new Employee { Id = 1, Country = countryNL, Company = companyA, FirstName = "Bill", LastName = "Smith", Email = "bsmith@email.com", EmployeeNumber = 1001, HireDate = Convert.ToDateTime("01/12/1990")},
                new Employee { Id = 2, Country = countryNL, Company = companyB, FirstName = "Jack", LastName = "Smith", Email = "jsmith@email.com", EmployeeNumber = 1002, HireDate = Convert.ToDateTime("12/12/1997")},
                new Employee { Id = 3, Country = countryNL, Company = companyC, FirstName = "Mary", LastName = "Gates", Email = "mgates@email.com", EmployeeNumber = 1003, HireDate = Convert.ToDateTime("03/03/2000")},
                new Employee { Id = 4, Country = countryNL, Company = companyA, FirstName = "John", LastName = "Doe", Email = "jd@email.com", EmployeeNumber = 1004, HireDate = Convert.ToDateTime("11/11/2011")},
                new Employee { Id = 5, Country = countryBE, Company = companyB, FirstName = "Chris", LastName = "Cross", Email = "cc@email.com", EmployeeNumber = 1005, HireDate = Convert.ToDateTime("05/05/1995")},
                new Employee { Id = 6, Country = countryBE, Company = companyC, FirstName = "Niki", LastName = "Myers", Email = "nm@email.com", EmployeeNumber = 1006, HireDate = Convert.ToDateTime("06/05/1995")},
                new Employee { Id = 7, Country = countryBE, Company = companyA, FirstName = "Joseph", LastName = "Hall", Email = "jh@email.com", EmployeeNumber = 1007, HireDate = Convert.ToDateTime("07/05/1995")},
                new Employee { Id = 8, Country = countryBE, Company = companyB, FirstName = "Daniel", LastName = "Wells", Email = "cc@email.com", EmployeeNumber = 1008, HireDate = Convert.ToDateTime("08/05/1995")},
                new Employee { Id = 9, Country = countryNL, Company = companyC, FirstName = "Robert", LastName = "Lawrence", Email = "cc@email.com", EmployeeNumber = 1009, HireDate = Convert.ToDateTime("09/05/1995")},
                new Employee { Id = 10, Country = countryNL, Company = companyA, FirstName = "Reginald", LastName = "Quinn", Email = "cc@email.com", EmployeeNumber = 1010, HireDate = Convert.ToDateTime("10/05/1995")},
                new Employee { Id = 11, Country = countryNL, Company = companyB, FirstName = "Quinn", LastName = "Quick", Email = "cc@email.com", EmployeeNumber = 1011, HireDate = Convert.ToDateTime("11/05/1995")},
                new Employee { Id = 12, Country = countryNL, Company = companyC, FirstName = "Test", LastName = "User", Email = "tu@email.com", EmployeeNumber = 1012, HireDate = Convert.ToDateTime("11/05/2012")},
            };
        }
        #endregion

        protected static KendoGridBaseRequest SetupBinder(NameValueCollection form, NameValueCollection queryString)
        {
            var fakeRequest = new FakeRequest("POST", form, queryString ?? new NameValueCollection());
            var httpContext = new FakeContext(fakeRequest);

            var controller = new FakeController();
            var controllerContext = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
            var modelBindingContext = new ModelBindingContext();

            var binder = new KendoGridMvcModelBinder();
            var model = binder.BindModel(controllerContext, modelBindingContext);
            Assert.IsNotNull(model);

            var gridRequest = model as KendoGridMvcRequest;
            Assert.IsNotNull(gridRequest);

            return gridRequest;
        }
    }
}
