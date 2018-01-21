using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KendoGridBinder.Extensions;

namespace KendoGridBinder.AutoMapperExtensions
{
    public class AutoMapperUtils
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperUtils(MapperConfiguration mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration;
        }

        public Dictionary<string, MapExpression<TEntity>> GetModelMappings<TEntity, TViewModel>(Dictionary<string, MapExpression<TEntity>> mappings = null)
        {
            if (SameTypes<TEntity, TViewModel>())
            {
                return null;
            }

            var map = _mapperConfiguration?.FindTypeMapFor<TEntity, TViewModel>();
            if (map == null)
            {
                return null;
            }

            mappings = mappings ?? new Dictionary<string, MapExpression<TEntity>>();

            // Custom expressions because they do not map field to field
            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression != null))
            {
                // Get the linq expression body
                string body = propertyMap.CustomExpression.Body.ToString();

                // Get the item tag
                string tag = propertyMap.CustomExpression.Parameters[0].Name;

                string destination = body.Replace($"{tag}.", string.Empty);
                string source = propertyMap.DestinationProperty.Name;

                var customExpression = new MapExpression<TEntity>
                {
                    Path = destination,
                    Expression = propertyMap.CustomExpression.ToTypedExpression<TEntity>()
                };

                if (!mappings.ContainsKey(source))
                {
                    mappings.Add(source, customExpression);
                }
            }

            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression == null))
            {
                var customResolver = propertyMap.ValueResolverConfig?.Instance;
                if (customResolver is IKendoGridExValueResolver<TEntity>)
                {
                    string source = propertyMap.DestinationProperty.Name;

                    IKendoGridExValueResolver<TEntity> kendoResolver = customResolver as IKendoGridExValueResolver<TEntity>;
                    string destination = kendoResolver.DestinationProperty;
                    var expression = propertyMap.CustomExpression != null ? propertyMap.CustomExpression.ToTypedExpression<TEntity>() : kendoResolver.Expression;

                    var customExpression = new MapExpression<TEntity>
                    {
                        Path = destination,
                        Expression = expression
                    };

                    if (!mappings.ContainsKey(source))
                    {
                        mappings.Add(source, customExpression);
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