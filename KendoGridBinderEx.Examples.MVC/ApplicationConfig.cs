using System.Collections.Specialized;
using System.Configuration;

namespace KendoGridBinderEx.Examples.MVC
{
    public static class ApplicationConfig
    {
        private static readonly NameValueCollection Config = ConfigurationManager.AppSettings;

        public static bool RepositoryDeleteAllowed
        {
            get { return bool.Parse(Config["Repository.ChangeAllowed"]); }
        }

        public static bool RepositoryInsertAllowed
        {
            get { return bool.Parse(Config["Repository.ChangeAllowed"]); }
        }

        public static bool RepositoryUpdateAllowed
        {
            get { return bool.Parse(Config["Repository.ChangeAllowed"]); }
        }

        public static bool MiniProfilerEnabled
        {
            get { return bool.Parse(Config["MiniProfiler.Enabled"]); }
        }

        public static bool DatabaseInit
        {
            get { return bool.Parse(Config["Database.Init"]); }
        }
    }
}