#if !NETSTANDARD
using System;
using System.Web.Http.ModelBinding;

namespace KendoGridBinder.ModelBinder.Mvc
{
    [ModelBinder(typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }

    [Obsolete("Use KendoGridMvcRequest")]
    [ModelBinder(typeof(KendoGridMvcModelBinder))]
    public class KendoGridRequest : KendoGridBaseRequest
    {
    }
}
#endif