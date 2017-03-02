using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using AutoMapper;
using KendoGridBinder.AutoMapperExtensions;
using KendoGridBinder.UnitTests.Entities;
using KendoGridBinder.UnitTests.Extensions;
using KendoGridBinder.UnitTests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace KendoGridBinder.UnitTests
{
    [TestFixture]
    public class KendoGridUnitTests : TestHelper
    {
        private MapperConfiguration _mapperConfiguration;
        private KendoGridQueryableHelper _instanceUnderTest;
        private IMapper _mapper;

        [Test]
        public void AutomapperUtilsTest()
        {
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
            };

            var companyIds = employees.Select(mappings.First().Value.Expression).ToList();
            Assert.AreEqual(12, companyIds.Count);
            Assert.AreEqual(1, companyIds.First());
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

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);
            Assert.IsNotNull(kendoGrid);

            Assert.AreEqual(employees.Count(), kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(5, kendoGrid.Data.Count());
        }

        [Test]
        public void Test_KendoGridModelBinder_Grid_Sort_Filter_EntitiesWithNullValues()
        {
            var form = new NameValueCollection
            {
                {"sort[0][field]", "Id"},
                {"sort[0][dir]", "asc"},

                {"filter[filters][0][field]", "LastName"},
                {"filter[filters][0][operator]", "contains"},
                {"filter[filters][0][value]", "s"},
                {"filter[filters][1][field]", "Email"},
                {"filter[filters][1][operator]", "contains"},
                {"filter[filters][1][value]", "r"},
                {"filter[logic]", "or"}
            };

            var gridRequest = SetupBinder(form, null);

            var employeeList = InitEmployeesWithData().ToList();
            foreach (var employee in employeeList.Where(e => e.LastName.Contains("e")))
            {
                employee.LastName = null;
                employee.FirstName = null;
            }

            var employees = employeeList.AsQueryable();

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);
            Assert.IsNotNull(kendoGrid);

            Assert.AreEqual(3, kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
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
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
                {
                    { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                    { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
                };


            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);
            Assert.IsNotNull(kendoGrid);

            Assert.AreEqual(4, kendoGrid.Total);
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
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
                { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, new[] { "Company", "Company.MainCompany", "Country" }, mappings);

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
        public void Test_KendoGridModelBinder_Aggregates_WithIncludes()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"aggregate[0][field]", "Id"},
                {"aggregate[0][aggregate]", "sum"},
                {"aggregate[1][field]", "Id"},
                {"aggregate[1][aggregate]", "min"},
                {"aggregate[2][field]", "Id"},
                {"aggregate[2][aggregate]", "max"},
                {"aggregate[3][field]", "Id"},
                {"aggregate[3][aggregate]", "count"},
                {"aggregate[4][field]", "Id"},
                {"aggregate[4][aggregate]", "average"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.IsNull(gridRequest.GroupObjects);
            Assert.AreEqual(5, gridRequest.AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, new[] { "Company", "Company.MainCompany", "Country" }, mappings);

            Assert.IsNull(kendoGrid.Groups);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(5, kendoGrid.Data.Count());

            Assert.IsNotNull(kendoGrid.Aggregates);
            var json = JsonConvert.SerializeObject(kendoGrid.Aggregates, Formatting.Indented);
            Assert.IsNotNull(json);

            var aggregatesAsDictionary = kendoGrid.Aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.IsNotNull(aggregatesAsDictionary);
            Assert.AreEqual(1, aggregatesAsDictionary.Keys.Count);
            Assert.AreEqual("Id", aggregatesAsDictionary.Keys.First());

            var aggregatesForId = aggregatesAsDictionary["Id"];
            Assert.AreEqual(5, aggregatesForId.Keys.Count);
            Assert.AreEqual(78, aggregatesForId["sum"]);
            Assert.AreEqual(1, aggregatesForId["min"]);
            Assert.AreEqual(12, aggregatesForId["max"]);
            Assert.AreEqual(12, aggregatesForId["count"]);
            Assert.AreEqual(6.5d, aggregatesForId["average"]);
        }

        [Test]
        public void Test_KendoGridModelBinder_Aggregates_WithIncludes_NoResults()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"filter[filters][0][field]", "LastName"},
                {"filter[filters][0][operator]", "equals"},
                {"filter[filters][0][value]", "xxx"},
                {"filter[filters][1][field]", "Email"},
                {"filter[filters][1][operator]", "contains"},
                {"filter[filters][1][value]", "r"},
                {"filter[logic]", "or"},

                {"aggregate[0][field]", "Id"},
                {"aggregate[0][aggregate]", "sum"},
                {"aggregate[1][field]", "Id"},
                {"aggregate[1][aggregate]", "min"},
                {"aggregate[2][field]", "Id"},
                {"aggregate[2][aggregate]", "max"},
                {"aggregate[3][field]", "Id"},
                {"aggregate[3][aggregate]", "count"},
                {"aggregate[4][field]", "Id"},
                {"aggregate[4][aggregate]", "average"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.IsNull(gridRequest.GroupObjects);
            Assert.AreEqual(5, gridRequest.AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, new[] { "Company", "Company.MainCompany", "Country" }, mappings);

            Assert.IsNull(kendoGrid.Groups);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(0, kendoGrid.Data.Count());

            Assert.IsNotNull(kendoGrid.Aggregates);
            var json = JsonConvert.SerializeObject(kendoGrid.Aggregates, Formatting.Indented);
            Assert.IsNotNull(json);

            var aggregatesAsDictionary = kendoGrid.Aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.IsNotNull(aggregatesAsDictionary);
            Assert.AreEqual(0, aggregatesAsDictionary.Keys.Count);
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
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.IsNotNull(employeeVMs);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

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
        }

        [Test]
        public void Test_KendoGridModelBinder_One_GroupBy_One_Aggregate_Count()
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

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(5, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);
        }

        [Test]
        public void Test_KendoGridModelBinder_One_GroupBy_One_Aggregate_Sum()
        {
            var form = new NameValueCollection
            {
                {"take", "10"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "10"},

                {"group[0][field]", "Last"},
                {"group[0][dir]", "asc"},
                {"group[0][aggregates][0][field]", "Number"},
                {"group[0][aggregates][0][aggregate]", "sum"},
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(9, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);

            var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
            Assert.IsNotNull(groupBySmith);

            var items = groupBySmith.items as List<EmployeeVM>;
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(2, items.Count(e => e.Last == "Smith"));

            var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.IsNotNull(aggregates);

            Assert.IsTrue(aggregates.ContainsKey("Number"));
            var aggregatesNumber = aggregates["Number"];
            Assert.IsNotNull(aggregatesNumber);
            Assert.AreEqual(1, aggregatesNumber.Count);

            var aggregateSum = aggregatesNumber.First();
            Assert.IsNotNull(aggregateSum);
            Assert.AreEqual("sum", aggregateSum.Key);
            Assert.AreEqual(2003, aggregateSum.Value);
        }

        /*
        take=10&skip=0&page=1&pageSize=10&
        group[0][field]=CompanyName&
        group[0][dir]=asc&
        group[0][aggregates][0][field]=Number&
        group[0][aggregates][0][aggregate]=min&
        group[0][aggregates][1][field]=Number&
        group[0][aggregates][1][aggregate]=max&
        group[0][aggregates][2][field]=Number&
        group[0][aggregates][2][aggregate]=average&
        group[0][aggregates][3][field]=Number&
        group[0][aggregates][3][aggregate]=count&

        group[1][field]=LastName&
        group[1][dir]=asc&
        group[1][aggregates][0][field]=Number&
        group[1][aggregates][0][aggregate]=min&
        group[1][aggregates][1][field]=Number&
        group[1][aggregates][1][aggregate]=max&
        group[1][aggregates][2][field]=Number&
        group[1][aggregates][2][aggregate]=average&
        group[1][aggregates][3][field]=Number&
        group[1][aggregates][3][aggregate]=count
         * */
        [Test]
        public void Test_KendoGridModelBinder_Two_GroupBy_One_Aggregate_Min()
        {
            var form = new NameValueCollection
            {
                {"take", "10"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "10"},

                {"group[0][field]", "CompanyName"},
                {"group[0][dir]", "asc"},
                {"group[0][aggregates][0][field]", "Number"},
                {"group[0][aggregates][0][aggregate]", "min"},

                {"group[1][field]", "LastName"},
                {"group[1][dir]", "asc"},
                {"group[1][aggregates][0][field]", "Number"},
                {"group[1][aggregates][0][aggregate]", "min"},
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(2, gridRequest.GroupObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.First().AggregateObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.Last().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
                { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(10, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);

            /*
            var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
            Assert.IsNotNull(groupBySmith);

            var items = groupBySmith.items as List<EmployeeVM>;
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(2, items.Count(e => e.Last == "Smith"));

            var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.IsNotNull(aggregates);

            Assert.IsTrue(aggregates.ContainsKey("Number"));
            var aggregatesNumber = aggregates["Number"];
            Assert.IsNotNull(aggregatesNumber);
            Assert.AreEqual(1, aggregatesNumber.Count);

            var aggregateSum = aggregatesNumber.First();
            Assert.IsNotNull(aggregateSum);
            Assert.AreEqual("sum", aggregateSum.Key);
            Assert.AreEqual(2003, aggregateSum.Value);
            */
        }

        //take=10&
        //skip=0&
        //page=1&
        //pageSize=10&
        //group[0][field]=Id&
        //group[0][dir]=asc&
        //group[0][aggregates][0][field]=Id&
        //group[0][aggregates][0][aggregate]=count
        [Test]
        public void Test_KendoGridModelBinder_A()
        {
            var form = new NameValueCollection
            {
                {"take", "10"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "10"},

                {"group[0][field]", "Id"},
                {"group[0][dir]", "asc"},
                {"group[0][aggregates][0][field]", "Id"},
                {"group[0][aggregates][0][aggregate]", "count"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.First().AggregateObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.Last().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
                { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(10, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);
        }

        [Test]
        //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[]}
        public void Test_KendoGridModelBinder_Json_WithoutIncludes()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group", "[]"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.IsNull(gridRequest.GroupObjects);

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.IsNotNull(employeeVMs);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.IsNotNull(kendoGrid);
            Assert.IsNull(kendoGrid.Groups);
            Assert.NotNull(kendoGrid.Data);

            Assert.AreEqual(employees.Count(), kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(5, kendoGrid.Data.Count());
        }

        [Test]
        public void Test_KendoGridModelBinder_Json_Filter()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group", "[]"},
                //{"filter", "{\"logic\":\"and\",\"filters\":[{\"field\":\"CompanyName\",\"operator\":\"eq\",\"value\":\"A\"}]}"},
                {"filter", "{\"logic\":\"and\",\"filters\":[{\"logic\":\"or\",\"filters\":[{\"field\":\"LastName\",\"operator\":\"contains\",\"value\":\"s\"},{\"field\":\"LastName\",\"operator\":\"endswith\",\"value\":\"ll\"}]},{\"field\":\"FirstName\",\"operator\":\"startswith\",\"value\":\"n\"}]}"},
                {"sort", "[{\"field\":\"FirstName\",\"dir\":\"asc\",\"compare\":null},{\"field\":\"LastName\",\"dir\":\"desc\",\"compare\":null}]"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.IsNull(gridRequest.GroupObjects);

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.IsNotNull(employeeVMs);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.IsNotNull(kendoGrid);
            Assert.IsNull(kendoGrid.Groups);
            Assert.NotNull(kendoGrid.Data);

            Assert.AreEqual(1, kendoGrid.Total);
            Assert.IsNotNull(kendoGrid.Data);
            Assert.AreEqual(1, kendoGrid.Data.Count());
        }

        [Test]
        //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[{"field":"LastName","dir":"asc","aggregates":[]}]}
        public void Test_KendoGridModelBinder_Json_One_GroupBy_WithoutIncludes()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group", "[{\"field\":\"LastName\",\"dir\":\"asc\",\"aggregates\":[]}]"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(0, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.IsNotNull(employeeVMs);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

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
        }

        [Test]
        //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[{"field":"LastName","dir":"asc","aggregates":["field":"Number","aggregate":"Sum"]}]}
        public void Test_KendoGridModelBinder_Json_One_GroupBy_One_Aggregate_Sum()
        {
            var form = new NameValueCollection
            {
                {"take", "10"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "10"},

                {"group", "[{\"field\":\"LastName\",\"dir\":\"asc\",\"aggregates\":[{\"field\":\"Number\",\"aggregate\":\"sum\"}]}]"}
            };

            var gridRequest = SetupBinder(form, null);
            Assert.AreEqual(1, gridRequest.GroupObjects.Count());
            Assert.AreEqual(1, gridRequest.GroupObjects.First().AggregateObjects.Count());

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.IsNull(kendoGrid.Data);
            Assert.IsNotNull(kendoGrid.Groups);
            var json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.IsNotNull(json);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.IsNotNull(groups);

            Assert.AreEqual(9, groups.Count());
            Assert.AreEqual(employees.Count(), kendoGrid.Total);

            var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
            Assert.IsNotNull(groupBySmith);

            var items = groupBySmith.items as List<EmployeeVM>;
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(2, items.Count(e => e.Last == "Smith"));

            var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.IsNotNull(aggregates);

            Assert.IsTrue(aggregates.ContainsKey("Number"));
            var aggregatesNumber = aggregates["Number"];
            Assert.IsNotNull(aggregatesNumber);
            Assert.AreEqual(1, aggregatesNumber.Count);

            var aggregateSum = aggregatesNumber.First();
            Assert.IsNotNull(aggregateSum);
            Assert.AreEqual("sum", aggregateSum.Key);
            Assert.AreEqual(2003, aggregateSum.Value);
        }

        #region InitAutoMapper
        private void InitAutoMapper()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeeProfile>();
            });

            _mapperConfiguration.AssertConfigurationIsValid();

            _mapper = _mapperConfiguration.CreateMapper();
        }
        #endregion

        public interface IKendoResolver
        {
            string GetM();
        }

        public class IdResolver : IValueResolver<Company, object, long>
        {
            public long Resolve(Company source, object destination, long destMember, ResolutionContext context)
            {
                return source?.Id ?? 0;
            }
        }

        public class IdResolver2 : IValueResolver<IEntity, object, long>, IKendoResolver
        {
            public string GetM()
            {
                return "xxx";
            }

            public long Resolve(IEntity source, object destination, long destMember, ResolutionContext context)
            {
                return source?.Id ?? 0;
            }
        }

        public class CompanyNameResolver : IValueResolver<Company, object, string>
        {
            public string Resolve(Company source, object destination, string destMember, ResolutionContext context)
            {
                return source != null ? source.Name : string.Empty;
            }
        }

        public class MainCompanyNameResolver : IValueResolver<Company, object, string>
        {
            public string Resolve(Company source, object destination, string destMember, ResolutionContext context)
            {
                return source.NullSafeGetValue(x => x.MainCompany.Name, null);
            }
        }

        public class CountryCodeResolver : IValueResolver<Country, object, string>
        {
            public string Resolve(Country source, object destination, string destMember, ResolutionContext context)
            {
                return source != null ? source.Code : null;
            }
        }

        public class CountryNameResolver : IValueResolver<Country, object, string>
        {
            public string Resolve(Country source, object destination, string destMember, ResolutionContext context)
            {
                return source != null ? source.Name : null;
            }
        }
    }
}