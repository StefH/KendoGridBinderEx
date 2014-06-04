using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace KendoGridBinderEx.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source != null ?
                source.Cast<string>().Select(s => new { Key = s, Value = source.GetValues(s)[0] }).ToDictionary(p => p.Key, p => p.Value) :
                null;
        }

        public static T GetQueryValue<T>(this NameValueCollection queryString, string key, T defaultValue)
        {
            string stringValue = queryString[key];

            return !string.IsNullOrEmpty(stringValue) ? (T)TypeExtensions.ChangeType(stringValue, typeof(T)) : defaultValue;
        }
    }
}