using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JetBrains.Annotations;
using KendoGridBinder.AutoMapperExtensions;

namespace KendoGridBinder
{
    public class KendoGridQueryableHelper
    {
        private readonly AutoMapperUtils _autoMapperUtils;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        public KendoGridQueryableHelper(MapperConfiguration mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration;
            _autoMapperUtils = new AutoMapperUtils(mapperConfiguration);
            _mapper = mapperConfiguration.CreateMapper();
        }

        public KendoGridEx<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>([NotNull] IQueryable<TEntity> query,
            KendoGridBaseRequest request,
            IEnumerable<string> includes = null,
            Dictionary<string, MapExpression<TEntity>> mappings = null,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
            bool canUseAutoMapperProjection = false)
        {
            var mappings2 = _autoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings);
            var conversion2 = conversion ?? GetAutoMapperConversion<TEntity, TViewModel>(canUseAutoMapperProjection);

            return new KendoGridEx<TEntity, TViewModel>(request, query, mappings2, conversion2, includes);
        }

        public IEnumerable<TViewModel> FilterBy<TEntity, TViewModel>([NotNull] IQueryable<TEntity> query,
            KendoGridBaseRequest request,
            IEnumerable<string> includes = null,
            Dictionary<string, MapExpression<TEntity>> mappings = null,
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
            bool canUseAutoMapperProjection = false)
        {
            var mappings2 = _autoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings);
            var conversion2 = conversion ?? GetAutoMapperConversion<TEntity, TViewModel>(canUseAutoMapperProjection);

            return new KendoGridEx<TEntity, TViewModel>(request, query, mappings2, conversion2, includes).Data;
        }

        private Func<IQueryable<TEntity>, IEnumerable<TViewModel>> GetAutoMapperConversion<TEntity, TViewModel>(bool canUseAutoMapperProjection = false)
        {
            Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion;

            if (AutoMapperUtils.SameTypes<TEntity, TViewModel>())
            {
                conversion = q => q.Cast<TViewModel>().ToList();
            }
            else
            {
                // https://github.com/AutoMapper/AutoMapper/issues/362
                // The idea behind Project().To is to be passed to a query provider like EF or NHibernate that will then do the appropriate SQL creation, 
                // not necessarily that the in-memory-execution will work.
                // Project.To has a TON of limitations as it's built explicitly for real query providers, and only does things like MapFrom etc.
                // To put it another way - don't use Project.To unless you're passing that to EF or NH or another DB query provider that knows what to do with the expression tree.
                if (canUseAutoMapperProjection)
                {
                    conversion = q => q.ProjectTo<TViewModel>(_mapperConfiguration).AsEnumerable();
                }
                else
                {
                    conversion = _mapper.Map<IEnumerable<TViewModel>>;
                }
            }

            return conversion;
        }
    }
}