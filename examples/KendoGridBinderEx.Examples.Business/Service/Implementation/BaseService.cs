using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public IQueryable<TEntity> AsQueryableNoTracking(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).AsNoTracking();
        }

        public IQueryContext<TEntity> GetQueryContext(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetQueryContext(includeProperties);
        }


        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAll(includeProperties);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAllAsync(includeProperties);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.Where(where, includeProperties);
        }

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.WhereAsync(where, includeProperties);
        }

        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.First(where, includeProperties);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FirstAsync(where, includeProperties);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FirstOrDefault(where, includeProperties);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FirstOrDefaultAsync(where, includeProperties);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.Single(where, includeProperties);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.SingleAsync(where, includeProperties);
        }

        public TEntity GetById(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AsQueryable(includeProperties).FirstOrDefault(x => x.Id == id);
        }

        public async Task<TEntity> GetByIdAsync(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await AsQueryable(includeProperties).FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(TEntity model)
        {
            _repository.Insert(model);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }
        }

        public Task<TEntity> InsertAsync(TEntity model)
        {
            _repository.Insert(model);

            if (AutoCommit)
            {
                _unitOfWork.Commit();
            }

            return Task.FromResult(model);
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

        public void Save()
        {
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