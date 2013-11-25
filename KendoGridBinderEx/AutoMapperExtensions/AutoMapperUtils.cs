using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KendoGridBinderEx.Extensions;

namespace KendoGridBinderEx.AutoMapperExtensions
{
    public static class AutoMapperUtils
    {
        public static Dictionary<string, string> GetModelMappings<TEntity, TViewModel>(Dictionary<string, string> mappings = null)
        {
            if (SameTypes<TEntity, TViewModel>())
            {
                return null;
            }

            var map = Mapper.FindTypeMapFor<TEntity, TViewModel>();
            if (map == null)
            {
                return null;
            }

            mappings = mappings ?? new Dictionary<string, string>();

            // We are only interested in custom expressions because they do not map field to field
            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression != null))
            {
                // Get the linq expression body
                string body = propertyMap.CustomExpression.Body.ToString();

                // Get the item tag
                string tag = propertyMap.CustomExpression.Parameters[0].Name;

                string destination = body.Replace(string.Format("{0}.", tag), string.Empty);
                string source = propertyMap.DestinationProperty.Name;

                if (!mappings.ContainsKey(source))
                {
                    mappings.Add(source, destination);
                }
            }

            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression == null))
            {
                object customResolver = propertyMap.GetFieldValue("_customResolver");
                if (customResolver is KendoGridExValueResolver<TEntity,TViewModel>)
                {
                    var kendoResolver = customResolver as KendoGridExValueResolver<TEntity, TViewModel>;

                    var expression = kendoResolver.GetExpression();
                    string param = expression.Body.ToString().Replace(expression.Parameters[0] + ".", string.Empty);
                }
            }

            return mappings;
        }

        public static bool SameTypes<TEntity, TViewModel>()
        {
            return typeof(TEntity) == typeof(TViewModel);
        }
    }
}
