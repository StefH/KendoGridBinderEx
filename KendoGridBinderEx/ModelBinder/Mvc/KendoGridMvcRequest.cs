using System;
using System.Web.Mvc;

namespace KendoGridBinderEx.ModelBinder.Mvc
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