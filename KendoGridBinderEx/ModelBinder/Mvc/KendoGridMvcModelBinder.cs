using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using KendoGridBinderEx.Extensions;

namespace KendoGridBinderEx.ModelBinder.Mvc
{
    public class KendoGridMvcModelBinder : IModelBinder
    {
        private HttpRequestBase _request;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            _request = controllerContext.HttpContext.Request;
            var queryString = GetQueryString();

            var kendoGridRequest = new KendoGridMvcRequest
            {
                Take = queryString.GetQueryValue("take", (int?)null),
                Page = queryString.GetQueryValue("page", (int?)null),
                Skip = queryString.GetQueryValue("skip", (int?)null),
                PageSize = queryString.GetQueryValue("pageSize", (int?)null),

                FilterObjectWrapper = FilterHelper.Parse(queryString),
                GroupObjects = GroupHelper.Parse(queryString),
                AggregateObjects = AggregateHelper.Parse(queryString),
                SortObjects = SortHelper.Parse(queryString),
            };

            return kendoGridRequest;
        }

        private NameValueCollection GetQueryString()
        {
            return _request.HttpMethod.ToUpper() == "POST" ? _request.Form : _request.QueryString;
        }
    }
}