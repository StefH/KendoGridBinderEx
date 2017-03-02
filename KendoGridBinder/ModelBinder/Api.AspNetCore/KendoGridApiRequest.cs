#if NETSTANDARD
using Microsoft.AspNetCore.Mvc;

namespace KendoGridBinder.ModelBinder.Api
{
    [ModelBinder(BinderType = typeof(KendoGridApiModelBinder))]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}
#endif