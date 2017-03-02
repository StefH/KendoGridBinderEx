using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace KendoGridBinder.Extensions
{
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// http://blog.cincura.net/232247-casting-expression-func-tentity-tproperty-to-expression-func-tentity-object/
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The LambdaExpression.</param>
        /// <returns>Expression{Func{T, TProperty}}</returns>
        public static Expression<Func<T, TProperty>> ToTypedExpression<T, TProperty>([NotNull] this LambdaExpression expression)
        {
            Type propertyType = expression.Body.Type;

            if (!propertyType.GetTypeInfo().IsValueType)
                return Expression.Lambda<Func<T, TProperty>>(expression.Body, expression.Parameters);

            return Expression.Lambda<Func<T, TProperty>>(Expression.Convert(expression.Body, typeof(TProperty)), expression.Parameters);
        }

        /// <summary>
        /// Converts LambdaExpression to a typed expression (Expression{Func{T, object}}).
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Expression{Func{T, object}}</returns>
        public static Expression<Func<T, object>> ToTypedExpression<T>([NotNull] this LambdaExpression expression)
        {
            return expression.ToTypedExpression<T, object>();
        }
    }
}