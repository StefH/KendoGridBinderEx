using AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Repository;

namespace KendoGridBinderEx.Examples.Business.AutoMapper
{
    public class EntityResolver<TEntity> : ValueResolver<long, TEntity> where TEntity : class, IEntity, new()
    {
        readonly IRepository<TEntity> _service;

        public EntityResolver(IRepository<TEntity> service)
        {
            _service = service;
        }

        protected override TEntity ResolveCore(long id)
        {
            return id > 0 ? _service.FirstOrDefault(x => x.Id == id) : null;
        }
    }
}