using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using KendoGridBinder;
using KendoGridBinderEx.UnitTests.Entities;
using KendoGridBinderEx.UnitTests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace KendoGridBinderEx.UnitTests
{
    [TestFixture]
    public class KendoGridExUnitTests
    {
        [Test]
        public void Test_KendoGridModelBinder_Page()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);
        }

        [Test]
        public void Test_KendoGridModelBinder_Page_Filter()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"filter[filters][0][field]", "CompanyName"},
                {"filter[filters][0][operator]", "eq"},
                {"filter[filters][0][value]", "A"},
                {"filter[logic]", "and"},
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(false, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual(null, filter1.Logic);
            Assert.AreEqual(null, filter1.LogicToken);
        }

        [Test]
        public void Test_KendoGridModelBinder_Page_Filter_Sort()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},
                
                {"sort[0][field]", "First"},
                {"sort[0][dir]", "asc"},
                {"sort[1][field]", "Email"},
                {"sort[1][dir]", "desc"},

                {"filter[filters][0][logic]", "or"},
                {"filter[filters][0][filters][0][field]", "CompanyName"},
                {"filter[filters][0][filters][0][operator]", "eq"},
                {"filter[filters][0][filters][0][value]", "A"},
                {"filter[filters][0][filters][1][field]", "CompanyName"},
                {"filter[filters][0][filters][1][operator]", "contains"},
                {"filter[filters][0][filters][1][value]", "B"},
                
                {"filter[filters][1][field]", "Last"},
                {"filter[filters][1][operator]", "contains"},
                {"filter[filters][1][value]", "s"},
                {"filter[logic]", "and"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(2, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(true, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual("CompanyName", filter1.Field2);
            Assert.AreEqual("contains", filter1.Operator2);
            Assert.AreEqual("B", filter1.Value2);
            Assert.AreEqual("or", filter1.Logic);
            Assert.AreEqual("||", filter1.LogicToken);

            var filter2 = filterObjects[1];
            Assert.AreEqual(false, filter2.IsConjugate);
            Assert.AreEqual("Last", filter2.Field1);
            Assert.AreEqual("contains", filter2.Operator1);
            Assert.AreEqual("s", filter2.Value1);

            Assert.IsNotNull(gridRequest.SortObjects);
            Assert.AreEqual(2, gridRequest.SortObjects.Count());
            var sortObjects = gridRequest.SortObjects.ToList();

            var sort1 = sortObjects[0];
            Assert.AreEqual("First", sort1.Field);
            Assert.AreEqual("asc", sort1.Direction);

            var sort2 = sortObjects[1];
            Assert.AreEqual("Email", sort2.Field);
            Assert.AreEqual("desc", sort2.Direction);
        }

        [Test]
        public void Test_KendoGridModelBinder_Grid_Page()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"}
            };

            var gridRequest = SetupBinder(form, null);

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var kendoGrid = new KendoGridEx<Employee, EmployeeVM>(gridRequest, employees);
            Assert.IsNotNull(kendoGrid);

            Assert.AreEqual(employees.Count(), kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(5, kendoGrid.Data.Count());
        }

        [Test]
        public void Test_KendoGridModelBinder_Grid_Page_Filter_Sort()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},
                
                {"sort[0][field]", "First"},
                {"sort[0][dir]", "asc"},
                {"sort[1][field]", "Email"},
                {"sort[1][dir]", "desc"},

                {"filter[filters][0][logic]", "or"},
                {"filter[filters][0][filters][0][field]", "CompanyName"},
                {"filter[filters][0][filters][0][operator]", "eq"},
                {"filter[filters][0][filters][0][value]", "A"},
                {"filter[filters][0][filters][1][field]", "CompanyName"},
                {"filter[filters][0][filters][1][operator]", "contains"},
                {"filter[filters][0][filters][1][value]", "B"},
                
                {"filter[filters][1][field]", "Last"},
                {"filter[filters][1][operator]", "contains"},
                {"filter[filters][1][value]", "s"},
                {"filter[logic]", "and"}
            };

            var gridRequest = SetupBinder(form, null);

            InitAutoMapper();
            var employees = InitEmployeesWithData();
            var kendoGrid = new KendoGridEx<Employee, EmployeeVM>(gridRequest, employees);
            Assert.IsNotNull(kendoGrid);

            Assert.AreEqual(12, kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(4, kendoGrid.Data.Count());

            Assert.AreEqual("Bill Smith", kendoGrid.Data.First().Full);
            Assert.AreEqual("Jack Smith", kendoGrid.Data.Last().Full);

            var query = kendoGrid.AsQueryable();
            Assert.AreEqual("Bill Smith", query.First().FullName);
            Assert.AreEqual("Jack Smith", query.Last().FullName);
        }

        [Test]
        public void Test_KendoGridModelBinder_One_GroupBy_WithIncludes()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group[0][field]", "CountryName"},
                {"group[0][dir]", "asc"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(0, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var kendoGrid = new KendoGridEx<Employee, EmployeeVM>(gridRequest, employees, new[] { "Company", "Company.MainCompany", "Country" });

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(2, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);

            var employeesFromFirstGroup = groups.First().items as IEnumerable<EmployeeVM>;
            Assert.IsNotNull(employeesFromFirstGroup);

            var employeesFromFirstGroupList = employeesFromFirstGroup.ToList();
            Assert.AreEqual(4, employeesFromFirstGroupList.Count);

            var testEmployee = employeesFromFirstGroupList.First();
            Assert.AreEqual("Belgium", testEmployee.CountryName);
            Assert.AreEqual("B", testEmployee.CompanyName);
        }

        [Test]
        public void Test_KendoGridModelBinder_One_GroupBy_WithoutIncludes()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group[0][field]", "LastName"},
                {"group[0][dir]", "asc"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(0, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var kendoGrid = new KendoGridEx<Employee, EmployeeVM>(gridRequest, employees);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(5, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);

            var employeesFromFirstGroup = groups.First().items as IEnumerable<EmployeeVM>;
            Assert.IsNotNull(employeesFromFirstGroup);

            var employeesFromFirstGroupList = employeesFromFirstGroup.ToList();
            Assert.AreEqual(1, employeesFromFirstGroupList.Count);

            var testEmployee = employeesFromFirstGroupList.First();
            Assert.IsNull(testEmployee.CountryName);
            Assert.IsNull(testEmployee.CompanyName);
        }

        [Test]
        public void Test_KendoGridModelBinder_One_GroupBy_One_Aggregate()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"sort[0][field]", "Full"},
                {"sort[0][dir]", "asc"},

                {"group[0][field]", "First"},
                {"group[0][dir]", "asc"},
                {"group[0][aggregates][0][field]", "First"},
                {"group[0][aggregates][0][aggregate]", "count"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var kendoGrid = new KendoGridEx<Employee, EmployeeVM>(gridRequest, employees);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(5, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);
        }

        #region
        private static void InitAutoMapper()
        {
            Mapper.CreateMap<Employee, EmployeeVM>()
               .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
               .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
               .ForMember(vm => vm.Last, opt => opt.MapFrom(m => m.LastName))
               .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))
               .ForMember(vm => vm.CompanyId, opt => opt.MapFrom(m => m.Company.Id))
               .ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
               .ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
               .ForMember(vm => vm.CountryId, opt => opt.MapFrom(m => m.Country.Id))
               .ForMember(vm => vm.CountryCode, opt => opt.MapFrom(m => m.Country.Code))
               .ForMember(vm => vm.CountryName, opt => opt.MapFrom(m => m.Country.Name))
               ;

            Mapper.CreateMap<EmployeeVM, Employee>()
              .ForMember(e => e.Email, opt => opt.MapFrom(vm => vm.Email))
              .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
              .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
              .ForMember(e => e.HireDate, opt => opt.MapFrom(vm => vm.HireDate))
              .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.Last))
              .ForMember(e => e.Company, opt => opt.Ignore())
              .ForMember(e => e.Country, opt => opt.Ignore())
              ;

            Mapper.AssertConfigurationIsValid();
        }

        private static IEnumerable<Employee> InitEmployeesWithData()
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

        private static IEnumerable<Employee> InitEmployees(Country countryNL = null, Country countryBE = null, Company companyA = null, Company companyB = null, Company companyC = null)
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

        #region
        private static void CheckTake(KendoGridRequest gridRequest, int take)
        {
            Assert.IsNotNull(gridRequest.Take);
            Assert.AreEqual(take, gridRequest.Take.Value);
        }

        private static void CheckSkip(KendoGridRequest gridRequest, int skip)
        {
            Assert.IsNotNull(gridRequest.Skip);
            Assert.AreEqual(skip, gridRequest.Skip.Value);
        }

        private static void CheckPage(KendoGridRequest gridRequest, int page)
        {
            Assert.IsNotNull(gridRequest.Page);
            Assert.AreEqual(page, gridRequest.Page.Value);
        }

        private static void CheckPageSize(KendoGridRequest gridRequest, int pagesize)
        {
            Assert.IsNotNull(gridRequest.PageSize);
            Assert.AreEqual(pagesize, gridRequest.PageSize.Value);
        }

        private static KendoGridRequest SetupBinder(NameValueCollection form, NameValueCollection queryString)
        {
            var fakeRequest = new FakeRequest("POST", form, queryString ?? new NameValueCollection());
            var httpContext = new FakeContext(fakeRequest);

            var controller = new FakeController();
            var controllerContext = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
            var modelBindingContext = new ModelBindingContext();

            var binder = new KendoGridModelBinder();
            var model = binder.BindModel(controllerContext, modelBindingContext);
            Assert.IsNotNull(model);

            var gridRequest = model as KendoGridRequest;
            Assert.IsNotNull(gridRequest);

            return gridRequest;
        }
        #endregion
    }
}