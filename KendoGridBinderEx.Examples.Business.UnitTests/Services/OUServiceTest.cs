using System.Data.Entity;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Implementation;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;
using KendoGridBinderEx.Examples.Business.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KendoGridBinderEx.Examples.Business.UnitTests.Services
{
    [TestClass]
    public class OUServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var unityContainer = UnityBootstrapper.Container;

            unityContainer.RegisterType(typeof(IRepository<>), typeof(RepositoryImpl<>));
            unityContainer.RegisterType<IRepositoryConfig, RepositoryConfig>(new InjectionConstructor(true, true, true));

            unityContainer.RegisterType<IUnitOfWork, UnitOfWorkImpl>();


            var db = Effort.DbConnectionFactory.CreateTransient();
            var ctx = new MyDataContext(db, new MyDataContextConfiguration("", true));
            ctx.Database.CreateIfNotExists();
            ctx.Database.Initialize(true);

            //unityContainer.RegisterType<MyDataContextConfiguration>(new InjectionConstructor("", true));
            unityContainer.RegisterType<DbContext>(new InjectionFactory(con => ctx));


            unityContainer.RegisterType<IOUService, OUService>();

            var ouService = unityContainer.Resolve<IOUService>();

            var x = ouService.First(o => o.Id > 0);

            int y = 0;

        }
    }
}
