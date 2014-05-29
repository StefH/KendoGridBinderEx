using System.Web.Http.ModelBinding;

namespace KendoGridBinderEx.ModelBinder.Api
{
    [ModelBinder(typeof(KendoGridApiModelBinder))]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}