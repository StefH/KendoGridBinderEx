using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KendoGridBinder.AutoMapperExtensions;

namespace KendoGridBinder
{
    public class KendoGrid : KendoGrid<object>
    {
        public KendoGrid(
            [NotNull] KendoGridBaseRequest request,
            [NotNull] IEnumerable<dynamic> source,
            [CanBeNull] Dictionary<string, MapExpression<dynamic>> mappings = null,
            [CanBeNull] Func<IQueryable<dynamic>, IEnumerable<dynamic>> conversion = null,
            [CanBeNull] IEnumerable<string> includes = null
            )
            : base(request, source.AsQueryable(), mappings, conversion, includes)
        {
        }

        public KendoGrid(IEnumerable<dynamic> list, int totalCount)
            : base(list, totalCount)
        {
        }
    }
}