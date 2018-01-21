#if NETSTANDARD
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// ReSharper disable once CheckNamespace
namespace KendoGridBinder.ModelBinder.Mvc
{
    public class KendoGridMvcModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new KendoGridMvcModelBinder();
        }
    }
}
#endif