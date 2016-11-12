using System;
using System.Linq.Expressions;

namespace KendoGridBinderEx
{
    public interface IKendoGridExValueResolver<TEntity>
    {
        Expression<Func<TEntity, object>> Expression { get; }

        string DestinationProperty { get; }
    }
}