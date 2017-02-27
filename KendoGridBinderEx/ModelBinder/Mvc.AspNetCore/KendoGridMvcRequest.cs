#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinderEx.ModelBinder.AspNetCore
{
    [ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}
#endif