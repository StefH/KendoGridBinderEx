using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace KendoGridBinder.ModelBinder.Mvc
{
    [ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
    public class KendoGridMvcRequest : KendoGridBaseRequest
    {
    }
}