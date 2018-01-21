using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using KendoGridBinder.Extensions;
using KendoGridBinder.ModelBinder;
using Newtonsoft.Json;

namespace KendoGridBinderEx.Examples.MVC.Custom
{
    public class CustomApiModelBinder : IModelBinder
    {
        private NameValueCollection _queryString;

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
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
                bindingContext.Model = Parse(content);
            }
            catch (Exception)
            {
                // Parse the QueryString
                _queryString = GetQueryString(content);
                bindingContext.Model = Parse(_queryString);
            }

            return true;
        }

        private NameValueCollection GetQueryString(string content)
        {
            return HttpUtility.ParseQueryString(content);
        }

        private CustomApiRequest Parse(string jsonRequest)
        {
            var kendoJsonRequest = JsonConvert.DeserializeObject<CustomGridRequest>(jsonRequest);

            return new CustomApiRequest
            {
                Custom = kendoJsonRequest.Custom,
                Take = kendoJsonRequest.Take,
                Page = kendoJsonRequest.Page,
                PageSize = kendoJsonRequest.PageSize,
                Skip = kendoJsonRequest.Skip,
                Logic = kendoJsonRequest.Logic,
                GroupObjects = GroupHelper.Map(kendoJsonRequest.Groups),
                AggregateObjects = AggregateHelper.Map(kendoJsonRequest.AggregateObjects),
                FilterObjectWrapper = FilterHelper.MapRootFilter(kendoJsonRequest.Filter),
                SortObjects = SortHelper.Map(kendoJsonRequest.Sort)
            };
        }

        private CustomApiRequest Parse(NameValueCollection queryString)
        {
            var request = GridHelper.Parse<CustomApiRequest>(queryString);
            request.Custom = queryString.GetQueryValue("custom", (string)null);

            return request;
        }
    }
}