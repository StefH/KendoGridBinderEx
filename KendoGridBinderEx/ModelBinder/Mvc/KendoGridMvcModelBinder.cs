using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KendoGridBinderEx.Containers;
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
            var requestKeys = GetQueryKeys().ToList();
            var queryString = GetQueryString();

            int? take = GetQueryValue("take", (int?)null);
            int? page = GetQueryValue("page", (int?)null);
            int? skip = GetQueryValue("skip", (int?)null);
            int? pageSize = GetQueryValue("pageSize", (int?)null);

            string filterLogic = queryString["filter[logic]"];
            var filterKeys = requestKeys.Where(x => x.StartsWith("filter") && x != "filter[logic]").ToList();
            var filtering = FilterHelper.GetFilterObjects(queryString, filterKeys, filterLogic);

            var sortKeys = requestKeys.Where(x => x.StartsWith("sort")).ToList();
            var sorting = SortHelper.GetSortObjects(queryString, sortKeys);

            IEnumerable<GroupObject> groups = null;

            // If there is a group query parameter, try to parse the value as json
            if (requestKeys.Contains("group"))
            {
                string groupAsJson = queryString["group"];
                if (!string.IsNullOrEmpty(groupAsJson))
                {
                    groups = GroupHelper.GetGroupObjects(groupAsJson);
                }
            }
            else
            {
                // Just get the groups the old way
                groups = GroupHelper.GetGroupObjects(queryString, requestKeys.Where(k => k.StartsWith("group")));
            }

            return new KendoGridMvcRequest
            {
                Take = take,
                Skip = skip,
                Page = page,
                PageSize = pageSize,
                FilterObjectWrapper = filtering,
                SortObjects = sorting,
                GroupObjects = groups
            };
        }

        private NameValueCollection GetQueryString()
        {
            return _request.HttpMethod.ToUpper() == "POST" ? _request.Form : _request.QueryString;
        }

        private T GetQueryValue<T>(string key, T defaultValue)
        {
            var stringValue = GetQueryString()[key];

            return !string.IsNullOrEmpty(stringValue) ? (T)TypeExtensions.ChangeType(stringValue, typeof(T)) : defaultValue;
        }

        private IEnumerable<string> GetQueryKeys()
        {
            return GetQueryString().AllKeys;
        }
    }
}