using System.Collections.Specialized;
using System.Configuration;

namespace KendoGridBinderEx.Examples.MVC
{
    public static class ApplicationConfig
    {
        private static readonly NameValueCollection Config = ConfigurationManager.AppSettings;

        public static bool MiniProfilerEnabled => bool.Parse(Config["MiniProfiler.Enabled"]);

        public static bool InitDatabase => bool.Parse(Config["MyDataContextConfiguration.InitDatabase"]);

        public static string ConnectionString => ConfigurationManager.ConnectionStrings["MyDataContext"].ConnectionString;
    }
}