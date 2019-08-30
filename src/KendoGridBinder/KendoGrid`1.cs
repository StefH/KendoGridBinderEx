using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KendoGridBinder.AutoMapperExtensions;

namespace KendoGridBinder
{
    public class KendoGrid<TModel> : KendoGrid<TModel, TModel>
    {
        public KendoGrid(
            [NotNull] KendoGridBaseRequest request,
            [NotNull] IEnumerable<TModel> source,
            [CanBeNull] Dictionary<string, MapExpression<TModel>> mappings = null,
            [CanBeNull] Func<IQueryable<TModel>, IEnumerable<TModel>> conversion = null,
            [CanBeNull] IEnumerable<string> includes = null)
            : base(request, source.AsQueryable(), mappings, conversion, includes)
        {
        }

        public KendoGrid([NotNull] IEnumerable<TModel> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }
}