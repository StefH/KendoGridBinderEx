using System.ComponentModel;
using System.Linq;

namespace KendoGridBinderEx.Extensions
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class QueryProviderExtensions
    {
        public static bool IsQueryTranslatorProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName.Contains("QueryInterceptor.QueryTranslatorProvider");
        }

        public static bool IsEntityFrameworkProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName == "System.Data.Objects.ELinq.ObjectQueryProvider" || provider.GetType().FullName.StartsWith("System.Data.Entity.Internal.Linq");
        }

        public static bool IsLinqToObjectsProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName.Contains("EnumerableQuery");
        }
    }
}