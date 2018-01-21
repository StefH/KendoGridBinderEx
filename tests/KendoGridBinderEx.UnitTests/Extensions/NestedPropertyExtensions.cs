﻿using System;
using System.Collections;
using System.Linq.Expressions;

namespace KendoGridBinder.UnitTests.Extensions
{
    public static class NestedPropertyExtensions
    {
        #region NullSafeGetValue
        /// <summary>
        /// http://www.codeproject.com/Tips/177125/Get-Nested-Property-value-using-reflection-and-Lin
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">Root Object - it must be a reference type or a sub class of IEnumerable</param>
        /// <param name="expression">Labmda expression to set the property value returned</param>
        /// <param name="defaultValue">The default value in the case the property is not reachable </param>
        /// <returns></returns>
        public static TResult NullSafeGetValue<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> expression, TResult defaultValue)
        {
            object value = GetValue(expression, source);
            return value == null ? defaultValue : (TResult)value;
        }

        /// <summary>
        /// http://www.codeproject.com/Tips/177125/Get-Nested-Property-value-using-reflection-and-Lin
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">Root Object - it must be a reference type or a sub class of IEnumerable</param>
        /// <param name="expression">Labmda expression to set the property value returned</param>
        /// <returns></returns>
        public static TResult NullSafeGetValue<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> expression)
        {
            object value = GetValue(expression, source);
            return value == null ? default(TResult) : (TResult)value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TCastResultType"></typeparam>
        /// <param name="source">Root Object</param>
        /// <param name="expression">Labmda expression to set the property value returned</param>
        /// <param name="defaultValue">The default value in the case the property is not reachable</param>
        /// <param name="convertToResultToAction">An action to cast the returned value</param>
        /// <returns></returns>
        public static TCastResultType NullSafeGetValue<TSource, TResult, TCastResultType>(this TSource source, Expression<Func<TSource, TResult>> expression, TCastResultType defaultValue, Func<object, TCastResultType> convertToResultToAction)
        {
            object value = GetValue(expression, source);
            return value == null ? defaultValue : convertToResultToAction.Invoke(value);
        }

        private static string GetFullPropertyPathName<TSource, TResult>(Expression<Func<TSource, TResult>> expression)
        {
            return expression.Body.ToString().Replace(expression.Parameters[0] + ".", string.Empty);
        }

        public static TResult GetValue<TSource, TResult>(Expression<Func<TSource, TResult>> expression, TSource source)
        {
            string fullPropertyPathName = GetFullPropertyPathName(expression);
            object value = GetNestedPropertyValue(fullPropertyPathName, source);
            return value == null ? default(TResult) : (TResult)value;
        }

        private static object GetNestedPropertyValue(string name, object obj)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }

                var type = obj.GetType();
                if (obj is IEnumerable)
                {
                    type = (obj as IEnumerable).GetType();
                    var methodInfo = type.GetMethod("get_Item");
                    var index = int.Parse(part.Split('(')[1].Replace(")", string.Empty));
                    try
                    {
                        obj = methodInfo.Invoke(obj, new object[] { index });
                    }
                    catch (Exception)
                    {
                        obj = null;
                    }
                }
                else
                {
                    var info = type.GetProperty(part);
                    if (info == null)
                    {
                        return null;
                    }
                    obj = info.GetValue(obj, null);
                }
            }

            return obj;
        }
        #endregion
    }
}
