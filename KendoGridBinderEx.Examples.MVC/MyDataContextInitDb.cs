using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.MVC
{
    public class MyDataContextInitDb : MyDataContext
    {
        public MyDataContextInitDb(string nameOrConnectionString)
            : base(nameOrConnectionString, ApplicationConfig.InitDatabase)
        {

        }
    }
}