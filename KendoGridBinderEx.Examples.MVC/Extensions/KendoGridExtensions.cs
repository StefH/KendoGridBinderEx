using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Extensions
{
    public static class KendoGridExtensions
    {
        public static MvcHtmlString KendoGridDataSource<T>(this UrlHelper helper, string action, string datasource)
        {
            return KendoGridDataSource<T>(helper, null, action, datasource);
        }

        public static MvcHtmlString KendoGridDataSource<T>(this UrlHelper helper, string controller, string action, string datasource)
        {
            var sb = new StringBuilder();
            sb.AppendLine("var " + datasource + " = {");
            sb.AppendLine("    serverPaging: true,");
            sb.AppendLine("    serverSorting: true,");
            sb.AppendLine("    serverFiltering: true,");
            sb.AppendLine("    pageSize: 5,");
            sb.AppendLine("    transport: {");
            sb.AppendLine("        read: {");
            sb.AppendLine("            type: 'post',");
            sb.AppendLine("            dataType: 'json',");
            sb.AppendLine("            url: '" + helper.Action(action, controller) + "'");
            sb.AppendLine("        }");
            sb.AppendLine("    },");
            sb.AppendLine("    schema: {");
            sb.AppendLine("        data: 'Data',");
            sb.AppendLine("        total: 'Total',");
            sb.AppendLine("        model: {");
            sb.AppendLine("            id: 'Id',");
            sb.AppendLine("            fields: {");

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                sb.AppendLine("                " + propertyInfo.Name + ": { type: '" + GetPropertyType(propertyInfo.PropertyType) + "' }" + (propertyInfo != propertyInfos.Last() ? "," : ""));
            }
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("};");

            return MvcHtmlString.Create(sb.ToString());
        }

        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayAttribute)).Cast<DisplayAttribute>().ToList();

            return atts.Any() ? atts.First().Name : property.Name;
        }

        private static string GetPropertyType(Type propertyType)
        {
            string text = propertyType.Name.ToLower();

            switch (text)
            {
                case "int32":
                    text = "number";
                    break;

                case "int64":
                    text = "number";
                    break;

                case "datetime":
                    text = "date";
                    break;
            }

            return text;
        }
    }
}