using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KendoGridBinderEx.Examples.Business.QueryContext;
using PropertyTranslator;
using QueryInterceptor;

namespace KendoGridBinderEx.Examples.Business.Repository
{
    public class RepositoryImpl<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IRepositoryConfig _config;
        private readonly IObjectSet<TEntity> _objectSet;
        private readonly ObjectContext _objectContext;

        public RepositoryImpl([NotNull] DbContext dbContext, [NotNull] IRepositoryConfig config)
        {
            _objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            _config = config;

            _objectSet = _objectContext.CreateObjectSet<TEntity>();
        }

        #region IRepositoryEx<T> Members
        public IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return PerformInclusions(includeProperties, _objectSet.AsQueryable()).InterceptWith(new PropertyVisitor());
        }

        public IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var includes = includeProperties != null ? includeProperties
                .Select(p => new { Parameter = p.Parameters.First().ToString(), Body = p.Body.ToString() })
                .Select(x => x.Body.Substring(x.Parameter.Length + 1, x.Body.Length - x.Parameter.Length - 1))
                .ToList() : null;

            return new QueryContext<TEntity>
            {
                Query = AsQueryable(includeProperties),
                IncludeProperties = includeProperties,
                Includes = includes
            };
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).AsEnumerable();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(AsQueryable(includeProperties).AsEnumerable());
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).Where(where);
        }

        public Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(AsQueryable(includeProperties).Where(where).AsEnumerable());
        }

        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).First(where);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstAsync(where);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstOrDefault(where);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstOrDefaultAsync(where);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).Single(where);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).SingleAsync(where);
        }

        public void Delete(TEntity entity)
        {
            if (_config.DeleteAllowed)
            {
                _objectSet.DeleteObject(entity);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Insert(TEntity entity)
        {
            if (_config.InsertAllowed)
            {
                _objectSet.AddObject(entity);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Update(TEntity entity)
        {
            if (_config.InsertAllowed)
            {
                _objectSet.Attach(entity);
                _objectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        #endregion

        private static IQueryable<TEntity> PerformInclusions(IEnumerable<Expression<Func<TEntity, object>>> includeProperties, IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}