#if NETSTANDARD
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

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

            if (context.Metadata.ModelType == typeof(KendoGridApiRequest))
            {
                return new BinderTypeModelBinder(typeof(KendoGridApiModelBinder));
            }

            return null;
        }
    }
}
#endif