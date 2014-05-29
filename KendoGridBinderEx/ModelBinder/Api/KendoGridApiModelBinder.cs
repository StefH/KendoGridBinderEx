using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using KendoGridBinderEx.Containers;
using KendoGridBinderEx.Extensions;
using KendoGridBinderEx.ModelBinder.Mvc;

namespace KendoGridBinderEx.ModelBinder.Api
{
    public class KendoGridApiModelBinder : IModelBinder
    {
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

            var queryString = HttpContext.Current.Request.Params;
            var requestKeys = queryString.AllKeys;

            int? take = GetParamValue("take", (int?)null);
            int? page = GetParamValue("page", (int?)null);
            int? skip = GetParamValue("skip", (int?)null);
            int? pageSize = GetParamValue("pageSize", (int?)null);

            string filterLogic = GetParamStringValue("filter[logic]");
            var filterKeys = requestKeys.Where(x => x.StartsWith("filter") && x != "filter[logic]").ToList();
            var filtering = FilterHelper.GetFilterObjects(queryString, filterKeys, filterLogic);

            var sortKeys = requestKeys.Where(x => x.StartsWith("sort")).ToList();
            var sorting = SortHelper.GetSortObjects(queryString, sortKeys);

            IEnumerable<GroupObject> groups = null;

            // If there is a group query parameter, try to parse the value as json
            if (requestKeys.Contains("group"))
            {
                string groupAsJson = GetParamStringValue("group");
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

            bindingContext.Model = new KendoGridApiRequest
            {
                Take = take,
                Skip = skip,
                Page = page,
                PageSize = pageSize,
                FilterObjectWrapper = filtering,
                SortObjects = sorting,
                GroupObjects = groups
            };

            return true;
        }

        private T GetParamValue<T>(string key, T defaultValue)
        {
            var collection = HttpContext.Current.Request.Params;
            string stringValue = collection[key];

            return !string.IsNullOrEmpty(stringValue) ? (T)TypeExtensions.ChangeType(stringValue, typeof(T)) : defaultValue;
        }

        private string GetParamStringValue(string key)
        {
            return HttpContext.Current.Request.Params[key];
        }
    }
}