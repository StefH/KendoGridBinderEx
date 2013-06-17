
namespace KendoGridBinderEx.Examples.MVC.Data.Repository
{
    public class RepositoryConfig : IRepositoryConfig
    {
        private readonly bool _deleteAllowed;
        private readonly bool _insertAllowed;
        private readonly bool _updateAllowed;

        public RepositoryConfig(bool deleteAllowed, bool insertAllowed, bool updateAllowed)
        {
            _deleteAllowed = deleteAllowed;
            _insertAllowed = insertAllowed;
            _updateAllowed = updateAllowed;
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