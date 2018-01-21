using System.Web.Http.ModelBinding;
using KendoGridBinder.ModelBinder.Api;

namespace KendoGridBinderEx.Examples.MVC.Custom
{
    [ModelBinder(typeof(CustomApiModelBinder))]
    public class CustomApiRequest : KendoGridApiRequest
    {
        public string Custom { get; set; }
    }
}