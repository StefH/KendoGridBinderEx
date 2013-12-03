using System.Data.Entity;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Implementation;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Xml;
using Unity.Mvc4;

namespace KendoGridBinderEx.Examples.MVC.Unity
{
    public static class UnityBootstrapper
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        public static IUnityContainer Container { get { return UnityContainer; } }

        public static void Initialise()
        {
            UnityContainer.LoadConfiguration();

            // Registering interfaces of Unit Of Work & Generic Repository
            UnityContainer.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            UnityContainer.RegisterType<IUnitOfWork, UnitOfWork>();

            UnityContainer.RegisterType<MyDataContextConfiguration>(new InjectionConstructor(ApplicationConfig.ConnectionString, ApplicationConfig.InitDatabase));
            UnityContainer.RegisterType<DbContext>(new InjectionFactory(con => con.Resolve<MyDataContext>()));

            UnityContainer.RegisterInstance(UnityContainer.Resolve<DbContext>(), new PerThreadLifetimeManager());

            UnityContainer.RegisterType<IEmployeeService, EmployeeService>();
            UnityContainer.RegisterType<IProductService, ProductService>();
            UnityContainer.RegisterType<ICompanyService, CompanyService>();
            UnityContainer.RegisterType<ICountryService, CountryService>();
            UnityContainer.RegisterType<IMainCompanyService, MainCompanyService>();
            UnityContainer.RegisterType<IFunctionService, FunctionService>();
            UnityContainer.RegisterType<ISubFunctionService, SubFunctionService>();
            UnityContainer.RegisterType<IOUService, OUService>();
            UnityContainer.RegisterType<IUserService, UserService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));
        }
    }
}