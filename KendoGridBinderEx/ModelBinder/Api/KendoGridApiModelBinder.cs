using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using JetBrains.Annotations;

namespace KendoGridBinderEx.ModelBinder.Api
{
    public class KendoGridApiModelBinder : IModelBinder
    {
        private NameValueCollection _queryString;

        public bool BindModel([NotNull] HttpActionContext actionContext, [NotNull] ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string content = actionContext.Request.Content.ReadAsStringAsync().Result;

            try
            {
                // Try to parse as Json
                bindingContext.Model = GridHelper.Parse(content);
            }
            catch (Exception)
            {
                // Parse the QueryString
                _queryString = GetQueryString(content);
                bindingContext.Model = GridHelper.Parse<KendoGridApiRequest>(_queryString);
            }

            return true;
        }

        private NameValueCollection GetQueryString(string content)
        {
            return HttpUtility.ParseQueryString(content);
        }
    }
}