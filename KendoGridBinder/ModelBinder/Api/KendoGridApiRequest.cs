#if !NETSTANDARD
using System.Web.Http.ModelBinding;

namespace KendoGridBinder.ModelBinder.Api
{
    [ModelBinder(typeof(KendoGridApiModelBinder))]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}
#endif