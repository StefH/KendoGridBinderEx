using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KendoGridBinderEx.AutoMapperExtensions;

namespace KendoGridBinderEx
{
    public class KendoGridEx<TModel> : KendoGrid<TModel>
    {
        public KendoGridEx(KendoGridRequest request, IQueryable<TModel> query)
            : base(request, query)
        {
        }

        public KendoGridEx(KendoGridRequest request, IEnumerable<TModel> list)
            : base(request, list)
        {
        }

        public KendoGridEx(IEnumerable<TModel> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }

    public class KendoGridEx<TEntity, TViewModel> : KendoGrid<TEntity, TViewModel>
    {
        public KendoGridEx(KendoGridRequest request, IQueryable<TEntity> query)
            : this(request, query, null)
        {
        }

        public KendoGridEx(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes)
            : base(request, query, includes, AutoMapperUtils.GetModelMappings<TEntity, TViewModel>(), GetAutoMapperConversion(query))
        {
        }

        public KendoGridEx(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes, Dictionary<string, string> mappings, bool canUseAutoMapperProjection = true)
            : base(request, query, includes, AutoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings), GetAutoMapperConversion(query, canUseAutoMapperProjection))
        {
        }

        public KendoGridEx(KendoGridRequest request, IEnumerable<TEntity> entities)
            : this(request, entities, null)
        {
        }

        public KendoGridEx(KendoGridRequest request, IEnumerable<TEntity> entities, IEnumerable<string> includes)
            : this(request, entities.AsQueryable(), includes)
        {
        }

        public KendoGridEx(IEnumerable<TViewModel> list, int totalCount)
            : base(list, totalCount)
        {
        }

        public static Func<IQueryable<TEntity>, IEnumerable<TViewModel>> GetAutoMapperConversion(IQueryable<TEntity> query, bool canUseAutoMapperProjection = true)
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
                    conversion = q => q.Project().To<TViewModel>().ToList();
                }
                else
                {
                    conversion = Mapper.Map<IEnumerable<TViewModel>>;
                }
            }

            return conversion;
        }
    }
}