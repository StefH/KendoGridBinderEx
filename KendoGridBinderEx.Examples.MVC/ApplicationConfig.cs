using System.Collections.Specialized;
using System.Configuration;

namespace KendoGridBinderEx.Examples.MVC
{
    public static class ApplicationConfig
    {
        private static readonly NameValueCollection Config = ConfigurationManager.AppSettings;

        public static bool MiniProfilerEnabled
        {
            get { return bool.Parse(Config["MiniProfiler.Enabled"]); }
        }

        public static bool InitDatabase
        {
            get { return bool.Parse(Config["InitDatabase"]); }
        }
    }
}