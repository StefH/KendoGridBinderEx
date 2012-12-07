using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KendoGridBinder;

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
        private static readonly Func<IQueryable<TEntity>, IEnumerable<TViewModel>> AutoMapperConversion = query => 
            SameTypes ? query.Cast<TViewModel>().ToList() : Mapper.Map<IEnumerable<TViewModel>>(query);

        public KendoGridEx(KendoGridRequest request, IQueryable<TEntity> query)
            : base(request, query, GetModelMappings(), AutoMapperConversion)
        {
        }

        public KendoGridEx(KendoGridRequest request, IEnumerable<TEntity> entities)
            : this(request, entities.AsQueryable())
        {
        }

        public KendoGridEx(IEnumerable<TViewModel> list, int totalCount)
            : base(list, totalCount)
        {
        }

        public static Dictionary<string, string> GetModelMappings()
        {
            if (SameTypes)
            {
                return null;
            }

            var map = Mapper.FindTypeMapFor<TEntity, TViewModel>();
            if (map == null)
            {
                return null;
            }

            var mappings = new Dictionary<string, string>();
            
            // We are only interested in custom expressions because they do not map field to field
            foreach (var propertyMap in map.GetPropertyMaps().Where(pm => pm.CustomExpression != null))
            {
                // Get the linq expression body
                string body = propertyMap.CustomExpression.Body.ToString();

                // Get the item tag
                string tag = propertyMap.CustomExpression.Parameters[0].Name;

                string destination = body.Replace(string.Format("{0}.", tag), string.Empty);
                string source = propertyMap.DestinationProperty.Name;

                mappings.Add(source, destination);
            }

            return mappings;
        }

        private static bool SameTypes
        {
            get { return typeof(TEntity) == typeof(TViewModel); }
        }
    }
}