using System;
using System.Linq;
using System.Linq.Expressions;

namespace KendoGridBinderEx.Extensions
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// Creates the expression from path.
        /// http://stackoverflow.com/questions/26089918/dynamically-create-sort-lambda-expression
        /// </summary>
        /// <typeparam name="T">The type from the object</typeparam>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>Expression{Func{T, object}}</returns>
        public static Expression<Func<T, object>> CreateTypedExpressionFromPath<T>(string propertyPath)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var parts = propertyPath.Split('.');
            Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);

            if (parent.Type.IsValueType)
            {
                var converted = Expression.Convert(parent, typeof(object));
                return Expression.Lambda<Func<T, object>>(converted, param);
            }

            return Expression.Lambda<Func<T, object>>(parent, param);
        }
    }
}