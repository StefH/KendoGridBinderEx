using System;
using System.Linq.Expressions;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Extensions;
using KendoGridBinderEx.Extensions;

namespace KendoGridBinderEx.UnitTests.Helpers
{
    public class NullSafeResolver<TEntity, TViewModel, TResult> : IValueResolver<TEntity, TViewModel, TResult>, IKendoGridExValueResolver<TEntity>
    {
        private readonly Expression<Func<TEntity, TResult>> _expression;

        public NullSafeResolver(Expression<Func<TEntity, TResult>> expression)
        {
            _expression = expression;
        }

        public Expression<Func<TEntity, object>> Expression => _expression.ToTypedExpression<TEntity>();

        public string DestinationProperty =>_expression.Body.ToString().Replace(_expression.Parameters[0] + ".", string.Empty);

        public TResult Resolve(TEntity source, TViewModel destination, TResult destMember, ResolutionContext context)
        {
            return source.NullSafeGetValue(_expression);
        }
    }
}