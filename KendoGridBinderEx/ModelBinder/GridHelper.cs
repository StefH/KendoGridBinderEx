using System.Collections.Specialized;
using KendoGridBinderEx.Containers.Json;
using KendoGridBinderEx.Extensions;
using KendoGridBinderEx.ModelBinder.Api;
using Newtonsoft.Json;

namespace KendoGridBinderEx.ModelBinder
{
    public static class GridHelper
    {
        public static T Parse<T>(NameValueCollection queryString) where T : KendoGridBaseRequest, new()
        {
            return new T
            {
                Take = queryString.GetQueryValue("take", (int?)null),
                Page = queryString.GetQueryValue("page", (int?)null),
                Skip = queryString.GetQueryValue("skip", (int?)null),
                PageSize = queryString.GetQueryValue("pageSize", (int?)null),

                FilterObjectWrapper = FilterHelper.Parse(queryString),
                GroupObjects = GroupHelper.Parse(queryString),
                SortObjects = SortHelper.Parse(queryString)
            };
        }

        public static KendoGridApiRequest Parse(string jsonRequest)
        {
            var kendoJsonRequest = JsonConvert.DeserializeObject<GridRequest>(jsonRequest);

            return new KendoGridApiRequest
            {
                Take = kendoJsonRequest.Take,
                Page = kendoJsonRequest.Page,
                PageSize = kendoJsonRequest.PageSize,
                Skip = kendoJsonRequest.Skip,
                Logic = kendoJsonRequest.Logic,
                GroupObjects = GroupHelper.Map(kendoJsonRequest.Groups),
                FilterObjectWrapper = FilterHelper.MapRootFilter(kendoJsonRequest.Filter),
                SortObjects = SortHelper.Map(kendoJsonRequest.Sort)
            };
        }
    }
}