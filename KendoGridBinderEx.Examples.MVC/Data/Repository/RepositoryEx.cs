using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Patterns;
using PropertyTranslator;
using QueryInterceptor;

namespace KendoGridBinderEx.Examples.MVC.Data.Repository
{
    public class RepositoryEx<TEntity> : IRepositoryEx<TEntity> where TEntity : class
    {
        private readonly IRepositoryConfig _config;
        private readonly IObjectSet<TEntity> _objectSet;
        private readonly IObjectSetFactory _objectSetFactory;

        public RepositoryEx(IObjectSetFactory objectSetFactory, IRepositoryConfig config)
        {
            _objectSet = objectSetFactory.CreateObjectSet<TEntity>();
            _objectSetFactory = objectSetFactory;
            _config = config;
        }

        #region IRepositoryEx<T> Members
        public IQueryable<TEntity> AsQueryable()
        {
            return _objectSet.InterceptWith(new PropertyVisitor()).AsQueryable();
        }

        // ReSharper disable MethodOverloadWithOptionalParameter
        public IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
        // ReSharper restore MethodOverloadWithOptionalParameter
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
            return AsQueryable(includeProperties);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).Where(where);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable().Single(where);
        }

        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable().First(where);
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
                _objectSetFactory.ChangeObjectState(entity, EntityState.Modified);
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