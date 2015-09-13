using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace KendoGridBinderEx.Extensions
{
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// http://blog.cincura.net/232247-casting-expression-func-tentity-tproperty-to-expression-func-tentity-object/
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="expression">The LambdaExpression.</param>
        /// <returns>Expression{Func{T, object}}</returns>
        public static Expression<Func<T, object>> ToTypedExpression<T>([NotNull] this LambdaExpression expression)
        {
            Type propertyType = expression.Body.Type;

            if (!propertyType.IsValueType)
                return Expression.Lambda<Func<T, object>>(expression.Body, expression.Parameters);

            return Expression.Lambda<Func<T, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters);
        }
    }
}