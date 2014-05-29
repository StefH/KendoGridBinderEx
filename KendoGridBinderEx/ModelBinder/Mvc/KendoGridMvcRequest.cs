using System.Web.Mvc;

namespace KendoGridBinderEx.ModelBinder.Mvc
{
    [ModelBinder(typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}