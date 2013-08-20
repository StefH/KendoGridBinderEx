using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, IEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<TEntity> Repository;

        protected bool AutoCommit = false;

        protected BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.AsQueryable(includeProperties);
        }

        public IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.GetQueryContext(includeProperties);
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.GetAll(includeProperties);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.Find(where, includeProperties);
        }

        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.First(where, includeProperties);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.Single(where, includeProperties);
        }

        public TEntity GetById(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstOrDefault(x => x.Id == id);
        }

        public void Insert(TEntity model)
        {
            Repository.Insert(model);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public void Update(TEntity entity)
        {
            Repository.Update(entity);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public void Delete(TEntity entity)
        {
            Repository.Delete(entity);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }
    }
}