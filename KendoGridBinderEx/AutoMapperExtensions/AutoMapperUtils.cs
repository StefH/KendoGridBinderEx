using System;
using AutoMapper;
using KendoGridBinderEx.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KendoGridBinderEx.AutoMapperExtensions
{
    public static class AutoMapperUtils
    {
        private static string GetPath<T>(Expression<Func<T, object>> expr)
        {
            var stack = new Stack<string>();

            MemberExpression me;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expr.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expr.Body as MemberExpression;
                    break;
            }

            while (me != null)
            {
                stack.Push(me.Member.Name);
                me = me.Expression as MemberExpression;
            }

            return string.Join(".", stack.ToArray());
        }

        public static Dictionary<string, MapExpression<TEntity>> GetModelMappings<TEntity, TViewModel>(Dictionary<string, MapExpression<TEntity>> mappings = null)
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

            mappings = mappings ?? new Dictionary<string, MapExpression<TEntity>>();

            // Custom expressions because they do not map field to field
            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression != null))
            {
                // Get the linq expression body
                string body = propertyMap.CustomExpression.Body.ToString();

                // Get the item tag
                string tag = propertyMap.CustomExpression.Parameters[0].Name;

                string destination = body.Replace(string.Format("{0}.", tag), string.Empty);
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
                object customResolver = propertyMap.GetFieldValue("_customResolver");
                if (customResolver is IKendoGridExValueResolver)
                {
                    string source = propertyMap.DestinationProperty.Name;

                    var kendoResolver = customResolver as IKendoGridExValueResolver;
                    string destination = kendoResolver.GetDestinationProperty();

                    var customExpression = new MapExpression<TEntity>
                    {
                        Path = destination,
                        Expression = propertyMap.CustomExpression.ToTypedExpression<TEntity>(),
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