#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinderEx.ModelBinder.Api
{
    [ModelBinder()]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}
#endif