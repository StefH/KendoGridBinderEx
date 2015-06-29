using System.Data.Entity;
using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Implementation;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Unity
{
    public static class UnityMVCBootstrapper
    {
        public static void Initialise(IUnityContainer unityContainer)
        {
            unityContainer.LoadConfiguration();

            // Registering interfaces of Unit Of Work & Generic Repository
            unityContainer.RegisterType(typeof(IRepository<>), typeof(RepositoryImpl<>));
            unityContainer.RegisterType<IUnitOfWork, UnitOfWorkImpl>();

            unityContainer.RegisterType<MyDataContextConfiguration>(new InjectionConstructor(ApplicationConfig.ConnectionString, ApplicationConfig.InitDatabase));
            unityContainer.RegisterType<DbContext>(new InjectionFactory(con => con.Resolve<MyDataContext>()));

            unityContainer.RegisterInstance(unityContainer.Resolve<DbContext>(), new PerThreadLifetimeManager());

            unityContainer.RegisterType<IEmployeeService, EmployeeService>();
            unityContainer.RegisterType<IProductService, ProductService>();
            unityContainer.RegisterType<ICompanyService, CompanyService>();
            unityContainer.RegisterType<ICountryService, CountryService>();
            unityContainer.RegisterType<IMainCompanyService, MainCompanyService>();
            unityContainer.RegisterType<IFunctionService, FunctionService>();
            unityContainer.RegisterType<ISubFunctionService, SubFunctionService>();
            unityContainer.RegisterType<IOUService, OUService>();
            unityContainer.RegisterType<IUserService, UserService>();
            unityContainer.RegisterType<IRoleService, RoleService>();
            unityContainer.RegisterType<IUserRoleService, UserRoleService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(unityContainer));
        }
    }
}