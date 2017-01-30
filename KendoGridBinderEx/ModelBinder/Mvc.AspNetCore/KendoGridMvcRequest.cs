#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinderEx.ModelBinder.AspNetCore
{
    [ModelBinder]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}
#endif