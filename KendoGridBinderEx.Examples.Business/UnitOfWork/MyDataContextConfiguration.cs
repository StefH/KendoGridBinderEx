
namespace KendoGridBinderEx.Examples.Business.UnitOfWork
{
    public class MyDataContextConfiguration
    {
        public bool InitDatabase { get; set; }

        public string NameOrConnectionString { get; set; }

        //public DbConnection DbConnection { get; set; }

        public MyDataContextConfiguration(string nameOrConnectionString, bool initDatabase)
        {
            NameOrConnectionString = nameOrConnectionString;
            InitDatabase = initDatabase;
            //DbConnection = existingConnection;
        }
    }
}