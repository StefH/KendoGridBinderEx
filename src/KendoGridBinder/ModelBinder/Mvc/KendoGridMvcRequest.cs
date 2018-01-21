#if !NETSTANDARD
using System.Web.Http.ModelBinding;

namespace KendoGridBinder.ModelBinder.Mvc
{
    [ModelBinder(typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}
#endif