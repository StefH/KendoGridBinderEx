using System.Data.Entity;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Implementation;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
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
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));

            UnityContainer.RegisterType<DbContext>(new InjectionFactory(con => con.Resolve<MyDataContext>()));

            UnityContainer.RegisterInstance(UnityContainer.Resolve<DbContext>(), new PerThreadLifetimeManager());

            UnityContainer.RegisterType<IEmployeeService>(new InjectionFactory(con => con.Resolve<EmployeeService>()));
            UnityContainer.RegisterType<IProductService>(new InjectionFactory(con => con.Resolve<ProductService>()));
            UnityContainer.RegisterType<ICompanyService>(new InjectionFactory(con => con.Resolve<CompanyService>()));
            UnityContainer.RegisterType<IFunctionService>(new InjectionFactory(con => con.Resolve<FunctionService>()));
            UnityContainer.RegisterType<ISubFunctionService>(new InjectionFactory(con => con.Resolve<SubFunctionService>()));
            UnityContainer.RegisterType<IOUService>(new InjectionFactory(con => con.Resolve<OUService>()));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));
        }
    }
}
