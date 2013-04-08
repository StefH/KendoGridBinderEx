
namespace KendoGridBinderEx.Examples.MVC.Data.Repository
{
    public class RepositoryConfig : IRepositoryConfig
    {
        private readonly bool _deleteAllowed;
        private readonly bool _insertAllowed;
        private readonly bool _updateAllowed;

        public RepositoryConfig()
        {
            _deleteAllowed = ApplicationConfig.RepositoryDeleteAllowed;
            _insertAllowed = ApplicationConfig.RepositoryInsertAllowed;
            _updateAllowed = ApplicationConfig.RepositoryUpdateAllowed;
        }

        public bool DeleteAllowed
        {
            get { return _deleteAllowed; }
        }

        public bool InsertAllowed
        {
            get { return _insertAllowed; }
        }

        public bool UpdateAllowed
        {
            get { return _updateAllowed; }
        }
    }
}