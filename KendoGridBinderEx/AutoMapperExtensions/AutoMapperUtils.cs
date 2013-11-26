using AutoMapper;
using KendoGridBinderEx.Extensions;
using System.Collections.Generic;
using System.Linq;

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

            // Custom expressions because they do not map field to field
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
                if (customResolver is IKendoGridExValueResolver)
                {
                    string source = propertyMap.DestinationProperty.Name;

                    var kendoResolver = customResolver as IKendoGridExValueResolver;
                    string destination = kendoResolver.GetDestinationProperty();

                    if (!mappings.ContainsKey(source))
                    {
                        mappings.Add(source, destination);
                    }
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