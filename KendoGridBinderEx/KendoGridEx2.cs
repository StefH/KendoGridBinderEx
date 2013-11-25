using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KendoGridBinderEx.AutoMapperExtensions;

namespace KendoGridBinderEx
{
    public class KendoGridEx2<TModel> : KendoGridEx<TModel>
    {
        public KendoGridEx2(KendoGridRequest request, IQueryable<TModel> query)
            : base(request, query)
        {
        }

        public KendoGridEx2(KendoGridRequest request, IEnumerable<TModel> list)
            : base(request, list)
        {
        }

        public KendoGridEx2(IEnumerable<TModel> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }

    public class KendoGridEx2<TEntity, TViewModel> : KendoGridEx<TEntity, TViewModel>
    {
        public KendoGridEx2(KendoGridRequest request, IQueryable<TEntity> query)
            : this(request, query, null)
        {
        }

        public KendoGridEx2(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes)
            : base(request, query, includes, AutoMapperUtils.GetModelMappings<TEntity, TViewModel>(), GetAutoMapperConversion(query))
        {
        }

        public KendoGridEx2(KendoGridRequest request, IQueryable<TEntity> query, IEnumerable<string> includes, Dictionary<string, string> mappings, bool canUseAutoMapperProjection = true)
            : base(request, query, includes, AutoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings), GetAutoMapperConversion(query, canUseAutoMapperProjection))
        {
        }

        public KendoGridEx2(KendoGridRequest request, IEnumerable<TEntity> entities)
            : this(request, entities, null)
        {
        }

        public KendoGridEx2(KendoGridRequest request, IEnumerable<TEntity> entities, IEnumerable<string> includes)
            : this(request, entities.AsQueryable(), includes)
        {
        }

        public KendoGridEx2(IEnumerable<TViewModel> list, int totalCount)
            : base(list, totalCount)
        {
        }

       
    }
}