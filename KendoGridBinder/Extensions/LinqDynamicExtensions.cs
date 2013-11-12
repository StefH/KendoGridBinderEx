using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinder.Extensions
{
    public static class DynamicQueryableExtensions
    {
        public static IEnumerable<TEntity> Select<TEntity>(this IEnumerable<object> source, string propertyName)
        {
            return source.Select(x => GetPropertyValue<TEntity>(x, propertyName));
        }

        private static T GetPropertyValue<T>(object self, string propertyName)
        {
            var type = self.GetType();
            var propInfo = type.GetProperty(propertyName);

            try
            {
                return propInfo != null ? (T)propInfo.GetValue(self, null) : default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
}