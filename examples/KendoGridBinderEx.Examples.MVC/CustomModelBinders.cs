using System;
using System.Globalization;
using System.Web.Mvc;

namespace KendoGridBinderEx.Examples.MVC
{
    [ModelBinder(typeof(MyDateTimeBinder))]
    public class MyDateTimeBinder : IModelBinder
    {
        private const string DatePattern = "MM/dd/yyyy";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var date = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));
            try
            {
                return DateTime.ParseExact(date, DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                return null;
            }
        }
    }
}