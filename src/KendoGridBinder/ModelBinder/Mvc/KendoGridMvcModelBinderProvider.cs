#if !NETSTANDARD
using System;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace KendoGridBinder.ModelBinder.Mvc
{
    public class KendoGridMvcModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type == typeof(KendoGridMvcRequest))
            {
                return new KendoGridMvcModelBinder();
            }

            return null;
        }
    }
}
#endif