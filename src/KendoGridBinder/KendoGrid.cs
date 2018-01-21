using System;
using System.Collections.Generic;
using System.Linq;
using KendoGridBinder.AutoMapperExtensions;

namespace KendoGridBinder
{
    public class KendoGrid : KendoGrid<object>
    {
        public KendoGrid(
            KendoGridBaseRequest request,
            IEnumerable<dynamic> source,
            Dictionary<string, MapExpression<dynamic>> mappings = null,
            Func<IQueryable<dynamic>, IEnumerable<dynamic>> conversion = null,
            IEnumerable<string> includes = null
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