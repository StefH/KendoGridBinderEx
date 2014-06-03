using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using KendoGridBinderEx.Containers.Json;
using KendoGridBinderEx.Extensions;
using Newtonsoft.Json;

namespace KendoGridBinderEx.ModelBinder.Api
{
    public class KendoGridApiModelBinder : IModelBinder
    {
        private NameValueCollection queryString;

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
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
                queryString = GetQueryString(content);
                bindingContext.Model = GridHelper.Parse<KendoGridApiRequest>(queryString);
            }

            return true;
        }

        private NameValueCollection GetQueryString(string content)
        {
            return HttpUtility.ParseQueryString(content);
        }
    }
}