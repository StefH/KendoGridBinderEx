using System.Data.Entity;
using EntityFramework.Patterns;
using KendoGridBinder.Examples.MVC.Data.Repository;
using Microsoft.Practices.Unity;

namespace KendoGridBinder.Examples.MVC.Data.Service
{
    public static class CompositionRoot
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        public static IUnityContainer Container { get { return UnityContainer; } }

        public static void RegisterServices()
        {
            // Registering interfaces of Unit Of Work & Generic Repository
            UnityContainer.RegisterType(typeof(IRepositoryEx<>), typeof(RepositoryEx<>));
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));

            // Every time we ask for a EF context, we'll pass our own Context.
            UnityContainer.RegisterType(typeof(DbContext), typeof(MyDataContext));

            UnityContainer.RegisterType<IObjectSetFactory>(new InjectionFactory(con => con.Resolve<DbContextAdapter>()));

            UnityContainer.RegisterType<IObjectContext>(new InjectionFactory(con => con.Resolve<DbContextAdapter>()));

            UnityContainer.RegisterType<IRepositoryConfig>(new InjectionFactory(con => con.Resolve<RepositoryConfig>()));

            UnityContainer.RegisterInstance(new DbContextAdapter(UnityContainer.Resolve<DbContext>()), new PerThreadLifetimeManager());
        }

        public static T ResolveService<T>()
        {
            return Container.Resolve<T>();
        }
    }
}