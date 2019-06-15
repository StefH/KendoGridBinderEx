using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace KendoGridBinder.ModelBinder.Api
{
    [ModelBinder(BinderType = typeof(KendoGridApiModelBinder))]
    public class KendoGridApiRequest : KendoGridBaseRequest
    {
    }
}