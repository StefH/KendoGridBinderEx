using JetBrains.Annotations;
using KendoGridBinder.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
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

      var collection = isForm ?
       bindingContext.HttpContext.Request.Form as IEnumerable<KeyValuePair<string, StringValues>> :
       bindingContext.HttpContext.Request.Query as IEnumerable<KeyValuePair<string, StringValues>>;

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