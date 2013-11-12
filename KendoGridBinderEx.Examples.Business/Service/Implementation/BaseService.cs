using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.QueryContext;
using KendoGridBinderEx.Examples.Business.Repository;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.UnitOfWork;

namespace KendoGridBinderEx.Examples.Business.Service.Implementation
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity>, IDisposable where TEntity : class, IEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;

        protected bool AutoCommit = false;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public IRepository<TEntity> Repository
        {
            get
            {
                return _repository;
            }
        }

        protected BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_unitOfWork != null)
            {
                //_unitOfWork.Dispose();
            }

            GC.SuppressFinalize(this);
        }
        #endregion

        public IQueryable<TEntity> AsQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.AsQueryable(includeProperties);
        }

        public IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetQueryContext(includeProperties);
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAll(includeProperties);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.Find(where, includeProperties);
        }

        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.First(where, includeProperties);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.Single(where, includeProperties);
        }

        public TEntity GetById(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstOrDefault(x => x.Id == id);
        }

        public void Insert(TEntity model)
        {
            _repository.Insert(model);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public void BulkInsert(IEnumerable<TEntity> enumerable, Func<IBaseService<TEntity>> createService, int step = 100)
        {
            using (var scope = new TransactionScope())
            {
                var service = createService.Invoke();

                try
                {
                    service.UnitOfWork.Configuration.AutoDetectChangesEnabled = false;

                    int count = 0;
                    foreach (var entityToInsert in enumerable)
                    {
                        ++count;
                        service = AddToUnitOfWork(service, entityToInsert, count, step, createService);
                    }

                    service.UnitOfWork.Commit();
                }
                finally
                {
                    if (service != null)
                    {
                        service.Dispose();
                    }
                }

                scope.Complete();
            }
        }

        private IBaseService<TEntity> AddToUnitOfWork(IBaseService<TEntity> service, TEntity entity, int count, int commitCount, Func<IBaseService<TEntity>> createService)
        {
            service.Insert(entity);

            if (count % commitCount == 0)
            {
                service.UnitOfWork.Commit();
                if (createService != null)
                {
                    service.Dispose();
                    service = createService.Invoke();
                    service.UnitOfWork.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return service;
        }
    }
}