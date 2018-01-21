using System;
using System.Linq.Expressions;
using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.AutoMapper
{
    //public class NullSafeResolver<TEntity, TViewModel, TResult> : IValueResolver<TEntity, TViewModel, TResult>, IKendoGridExValueResolver<TEntity>
    //{
    //    private readonly Expression<Func<TEntity, TResult>> _expression;

    //    public NullSafeResolver(Expression<Func<TEntity, TResult>> expression)
    //    {
    //        _expression = expression;
    //    }

    //    public Expression<Func<TEntity, object>> Expression => _expression.ToTypedExpression<TEntity>();

    //    public string DestinationProperty => _expression.Body.ToString().Replace(_expression.Parameters[0] + ".", string.Empty);

    //    public TResult Resolve(TEntity source, TViewModel destination, TResult destMember, ResolutionContext context)
    //    {
    //        return source.NullSafeGetValue(_expression);
    //    }
    //}

    //ValueResolver<long, TEntity> where TEntity : class, IEntity, new()
    public class EntityResolver<TResultEntity> : IValueResolver<IEntity, object, TResultEntity> where TResultEntity : class, IEntity, new()
    {
        readonly IRepository<TResultEntity> _service;

        public EntityResolver(IRepository<TResultEntity> service)
        {
            _service = service;
        }

        public TResultEntity Resolve(IEntity source, object destination, TResultEntity destMember, ResolutionContext context)
        {
            return source.Id > 0 ? _service.FirstOrDefault(x => x.Id == source.Id) : null;
        }
    }
}