#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinder.ModelBinder.Mvc
{
    [ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}
#endif