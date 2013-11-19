using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;

namespace KendoGridBinderEx.Extensions
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class Extensions
    {
        public class DataItem
        {
            public string Fieldname { get; set; }
            public string Prefix { get; set; }
            public object Value { get; set; }
        }

        public static object GetPropertyValue(this object self, string propertyName)
        {
            return self.GetPropertyValue<object>(propertyName);
        }

        public static T GetPropertyValue<T>(this object self, string propertyName)
        {
            var type = self.GetType();

            var propInfo = type.GetProperty(propertyName.Split('.').Last()); // In case the propertyName contains a . like Company.Name, take last part.
            try
            {
                return (T)propInfo.GetValue(self, null);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Combines the property into a list
        /// new(\"First\" as field__First, \"Last\" as field__Last) ==> Dictionary[string, string]
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IEnumerable<DataItem> GetDataItems(this DynamicClass self, string propertyName)
        {
            var propertyType = self.GetType();
            var propertyInfo = propertyType.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                return new List<DataItem>();
            }

            var property = propertyInfo.GetValue(self, null);
            var props = property.GetType().GetProperties().Where(p => p.Name.Contains("__"));

            return props
                // Split on __ to get the prefix and the field
                .Select(prop => new { PropertyInfo = prop, Data = prop.Name.Split(new[] { "__" }, StringSplitOptions.None) })

                // Return the Fieldname, Prefix and the the value ('First' , 'field' , 'First')
                .Select(x => new DataItem { Fieldname = x.Data.Last(), Prefix = x.Data.First(), Value = x.PropertyInfo.GetValue(property, null) })
            ;
        }

        /// <summary>
        /// Gets the aggregate properties and stores them into a Dictionary object.
        /// Property is defined as : {aggregate}__{field name}  Example : count__Firstname
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>Dictionary</returns>
        public static object GetAggregatesAsDictionary(this DynamicClass self)
        {
            var dataItems = self.GetDataItems("Aggregates");

            // Group by the field and return an anonymous dictionary
            return dataItems
                .GroupBy(groupBy => groupBy.Fieldname)
                .ToDictionary(x => x.Key, y => y.ToDictionary(k => k.Prefix, v => v.Value))
            ;
        }

        public static IDictionary<string, object> ToDictionary(this object a)
        {
            var type = a.GetType();
            var props = type.GetProperties();
            return props.ToDictionary(x => x.Name, y => y.GetValue(a, null));
        }
    }
}