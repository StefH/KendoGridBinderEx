using System.Collections.Generic;
using KendoGridBinder.Containers;
using KendoGridBinder.Containers.Json;
using KendoGridBinder.ModelBinder.Mvc;

namespace KendoGridBinder
{
#if ASPNETCORE
    [Microsoft.AspNetCore.Mvc.ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
#else
    [System.Web.Http.ModelBinding.ModelBinder(typeof(KendoGridMvcModelBinder))]
#endif
    public abstract class KendoGridBaseRequest
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public string Logic { get; set; }

        public FilterObjectWrapper FilterObjectWrapper { get; set; }
        public IEnumerable<SortObject> SortObjects { get; set; }
        public IEnumerable<GroupObject> GroupObjects { get; set; }
        public IEnumerable<AggregateObject> AggregateObjects { get; set; }
    }
}