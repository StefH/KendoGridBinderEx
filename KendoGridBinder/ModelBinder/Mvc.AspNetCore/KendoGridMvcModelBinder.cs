#if NETSTANDARD
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using KendoGridBinder.Extensions;

namespace KendoGridBinder.ModelBinder.Mvc
{
    public class KendoGridMvcModelBinder : IModelBinder
    {
        public Task BindModelAsync([NotNull] ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            bool isForm = bindingContext.HttpContext.Request.HasFormContentType &&
                          bindingContext.HttpContext.Request.Method.ToUpper() == "POST";

            IEnumerable<KeyValuePair<string, StringValues>> form = bindingContext.HttpContext.Request.Form;
            IEnumerable<KeyValuePair<string, StringValues>> query = bindingContext.HttpContext.Request.Query;
            IEnumerable<KeyValuePair<string, StringValues>> collection = isForm ? form : query;

            var queryString = new NameValueCollection();
            foreach (KeyValuePair<string, StringValues> entry in collection)
            {
                string key = entry.Key;
                string value = entry.Value.ToArray().FirstOrDefault();
                queryString.Add(key, value);
            }

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

            bindingContext.Result = ModelBindingResult.Success(kendoGridRequest);
            return Task.CompletedTask;
        }
    }
}
#endif