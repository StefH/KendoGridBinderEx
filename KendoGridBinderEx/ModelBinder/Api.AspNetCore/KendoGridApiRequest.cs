#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinderEx.ModelBinder.Api
{
    [ModelBinder(BinderType = typeof(KendoGridApiModelBinder))]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}
#endif