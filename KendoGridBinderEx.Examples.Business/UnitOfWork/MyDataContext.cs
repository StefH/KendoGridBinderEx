using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.Business.UnitOfWork
{
    public class MyDataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<MainCompany> MainCompanies { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<SubFunction> SubFunctions { get; set; }

        public MyDataContext(string nameOrConnectionString, bool initDatabase)
            : base(nameOrConnectionString)
        {
            if (initDatabase)
            {
                Database.SetInitializer(new InitDatabase());
            }
            else
            {
                Database.SetInitializer<MyDataContext>(null); // must be turned off before mini profiler runs
            }
        }

        public ObjectContext ObjectContext
        {
            get
            {
                return (this as IObjectContextAdapter).ObjectContext;
            }
        }

        public bool TableExists(string table)
        {
            return ObjectContext.ExecuteStoreQuery<string>(string.Format("SELECT name FROM dbo.sysobjects WHERE xtype = 'U' AND name = '{0}'", table)).Any();
        }

        public void DeleteForeignKeys(string table)
        {
            string foreignKeys = string.Format("SELECT parent_object_id FROM sys.foreign_keys WHERE referenced_object_id = object_id('{0}')", table);
            if (ObjectContext.ExecuteStoreQuery<int>(foreignKeys).Any())
            {
                string dropForeignKeys = string.Format(
                    "SELECT 'ALTER TABLE ' + OBJECT_NAME(parent_object_id) + ' DROP CONSTRAINT ' + name FROM sys.foreign_keys WHERE referenced_object_id = object_id('{0}')",
                    table);
                Database.ExecuteSqlCommand(dropForeignKeys);
            }
        }
    }

    public class InitDatabase : IDatabaseInitializer<MyDataContext>
    {
        public void InitializeDatabase(MyDataContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }

            if (!dbExists)
            {
                throw new Exception();
            }

            // remove all tables if present
            var dropTables = new[] { "KendoGrid_Employee", "KendoGrid_SubFunction", "KendoGrid_Function", "KendoGrid_Product", "KendoGrid_Company", "KendoGrid_MainCompany", "KendoGrid_Country" };
            foreach (var table in dropTables)
            {
                if (context.TableExists(table))
                {
                    context.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}]", table));
                    context.DeleteForeignKeys(table);
                    context.Database.ExecuteSqlCommand(string.Format("DROP TABLE [{0}]", table));
                }
            }

            // recreate all tables
            var dbCreationScript = context.ObjectContext.CreateDatabaseScript();
            context.Database.ExecuteSqlCommand(dbCreationScript);

            Seed(context);
            context.SaveChanges();
        }

        private void Seed(MyDataContext context)
        {
            var functionIct = new Function { Id = 1, Code = "ICT", Name = "ICT" };
            var functionManagement = new Function { Id = 2, Code = "MAN", Name = "Management" };
            var functions = new List<Function> { functionIct, functionManagement };
            functions.ForEach(s => context.Functions.Add(s));

            for (int i = 1; i <= 10; i++)
            {
                var subFunctionIct = new SubFunction { Id = 1, Code = "ICT-" + i, Name = "ICT - " + i, Function = functionIct };
                context.SubFunctions.Add(subFunctionIct);
            }
            for (int i = 1; i <= 5; i++)
            {
                var subFunctionManagement = new SubFunction { Id = 1, Code = "MAN-" + i, Name = "Management - " + i, Function = functionManagement };
                context.SubFunctions.Add(subFunctionManagement);
            }

            var countryNL = new Country { Id = 1, Code = "NL", Name = "The Netherlands" };
            var countryBE = new Country { Id = 2, Code = "BE", Name = "Belgium" };
            var countries = new List<Country> { countryNL, countryBE };
            countries.ForEach(c => context.Countries.Add(c));

            var mainCompany1 = new MainCompany { Id = 10, Name = "M - 1" };
            var mainCompany2 = new MainCompany { Id = 20, Name = "M - 2" };
            var mainCompanies = new List<MainCompany> { mainCompany1, mainCompany2 };
            mainCompanies.ForEach(x => context.MainCompanies.Add(x));

            var companyA = new Company { Id = 1, Name = "A", MainCompany = mainCompany1 };
            var companyB = new Company { Id = 2, Name = "B", MainCompany = mainCompany1 };
            var companyC = new Company { Id = 3, Name = "C", MainCompany = mainCompany2 };
            var companies = new List<Company> { companyA, companyB, companyC };
            companies.ForEach(x => context.Companies.Add(x));

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Country = countryNL, Company = companyA, FirstName = "Bill", LastName = "Smith", Email = "bsmith@email.com", EmployeeNumber = 1001, HireDate = Convert.ToDateTime("01/12/1990"), Function = functionManagement, SubFunction = functionManagement.SubFunctions.First()},
                new Employee { Id = 2, Country = countryNL, Company = companyB, FirstName = "Jack", LastName = "Smith", Email = "jsmith@email.com", EmployeeNumber = 1002, HireDate = Convert.ToDateTime("12/12/1997"), Function = functionManagement, SubFunction = functionManagement.SubFunctions.First()},
                new Employee { Id = 3, Country = countryNL, Company = companyC, FirstName = "Mary", LastName = "Gates", Email = "mgates@email.com", EmployeeNumber = 1003, HireDate = Convert.ToDateTime("03/03/2000"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[1]},
                new Employee { Id = 4, Country = countryNL, Company = companyA, FirstName = "John", LastName = "Doe", Email = "jd@email.com", EmployeeNumber = 1004, HireDate = Convert.ToDateTime("11/11/2011"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[2]},
                new Employee { Id = 5, Country = countryBE, Company = companyB, FirstName = "Chris", LastName = "Cross", Email = "cc@email.com", EmployeeNumber = 1005, HireDate = Convert.ToDateTime("05/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[3]},
                new Employee { Id = 6, Country = countryBE, Company = companyC, FirstName = "Niki", LastName = "Myers", Email = "nm@email.com", EmployeeNumber = 1006, HireDate = Convert.ToDateTime("06/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[4]},
                new Employee { Id = 7, Country = countryBE, Company = companyA, FirstName = "Joseph", LastName = "Hall", Email = "jh@email.com", EmployeeNumber = 1007, HireDate = Convert.ToDateTime("07/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[5]},
                new Employee { Id = 8, Country = countryBE, Company = companyB, FirstName = "Daniel", LastName = "Wells", Email = "cc@email.com", EmployeeNumber = 1008, HireDate = Convert.ToDateTime("08/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[6]},
                new Employee { Id = 9, Country = countryNL, Company = companyC, FirstName = "Robert", LastName = "Lawrence", Email = "cc@email.com", EmployeeNumber = 1009, HireDate = Convert.ToDateTime("09/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[7]},
                new Employee { Id = 10, Country = countryNL, Company = companyA, FirstName = "Reginald", LastName = "Quinn", Email = "cc@email.com", EmployeeNumber = 1010, HireDate = Convert.ToDateTime("10/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[8]},
                new Employee { Id = 11, Country = countryNL, Company = companyB, FirstName = "Quinn", LastName = "Quick", Email = "cc@email.com", EmployeeNumber = 1011, HireDate = Convert.ToDateTime("11/05/1995"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[9]},
                new Employee { Id = 12, Country = countryNL, Company = companyC, FirstName = "Test", LastName = "User", Email = "tu@email.com", EmployeeNumber = 1012, HireDate = Convert.ToDateTime("11/05/2012"), Function = functionIct, SubFunction = functionIct.SubFunctions.ToArray()[0]},
            };
            employees.ForEach(x => context.Employees.Add(x));

            var products = new List<Product>
            {
                new Product { Id = 1, Code = "AR-5381", Name = "Adjustable Race"},
                new Product { Id = 2, Code = "BA-8327", Name = "Bearing Ball"},
                new Product { Id = 3, Code = "BE-2349", Name = "BB Ball Bearing"},
                new Product { Id = 4, Code = "BE-2908", Name = "Headset Ball Bearings"},
                new Product { Id = 316, Code = "BL-2036", Name = "Blade"},
                new Product { Id = 317, Code = "CA-5965", Name = "LL Crankarm"},
                new Product { Id = 318, Code = "CA-6738", Name = "ML Crankarm"},
                new Product { Id = 319, Code = "CA-7457", Name = "HL Crankarm"},
                new Product { Id = 320, Code = "CB-2903", Name = "Chainring Bolts"},
                new Product { Id = 321, Code = "CN-6137", Name = "Chainring Nut"},
                new Product { Id = 322, Code = "CR-7833", Name = "Chainring"}
            };
            products.ForEach(x => context.Products.Add(x));
        }
    }
}