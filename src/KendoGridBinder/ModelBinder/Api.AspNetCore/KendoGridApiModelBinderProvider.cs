#if NETSTANDARD
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// ReSharper disable once CheckNamespace
namespace KendoGridBinder.ModelBinder.Api
{
    public class KendoGridApiModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new KendoGridApiModelBinder();
        }
    }
}
#endif