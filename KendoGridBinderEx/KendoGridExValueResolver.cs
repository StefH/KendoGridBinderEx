using System;
using System.Linq.Expressions;
using AutoMapper;

namespace KendoGridBinderEx
{
    public abstract class KendoGridExValueResolver<TSource, TResult> : ValueResolver<TSource, TResult>
    {
        public abstract Expression<Func<TSource, TResult>> GetExpression();
    }
}