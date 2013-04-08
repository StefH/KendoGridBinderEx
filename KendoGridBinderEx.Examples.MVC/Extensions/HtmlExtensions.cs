using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace KendoGridBinderEx.Examples.MVC.Extensions
{
    public static class FormExtensions
    {
        public static MvcForm BeginPostForm(this HtmlHelper htmlHelper, string formName)
        {
            return htmlHelper.BeginForm(null, null, null, FormMethod.Post, new { name = "form", id = "form" });
        }

        public static MvcForm BeginPostForm(this HtmlHelper htmlHelper, string formName, object routeValues)
        {
            return htmlHelper.BeginForm(null, null, routeValues, FormMethod.Post, new { name = "form", id = "form" });
        }
    }

    public static class ImageActionLinkExtensions
    {
        public static MvcHtmlString SubmitFormLink(this HtmlHelper helper, string image, string altText)
        {
            return helper.SubmitFormLink("form", image, altText);
        }

        public static MvcHtmlString SubmitFormLink(this HtmlHelper helper, string formName, string image, string altText)
        {
            var imageUrl = string.Format("~/Content/Images/{0}.png", image.ToLower());

            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", VirtualPathUtility.ToAbsolute(imageUrl));
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.AddCssClass("iconLink");

            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", string.Format("javascript: submitform(document.{0})", formName));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += altText;
            text += linkBuilder.ToString(TagRenderMode.EndTag);

            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string image, string actionName, string text, object routeValues)
        {
            var imageUrl = string.Format("~/Content/Images/{0}.png", image.ToLower());

            return helper.ImageActionLink(VirtualPathUtility.ToAbsolute(imageUrl), text, actionName, routeValues, null);
        }

        /*
        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            string controllerName,
            object routeValues,
            object linkHtmlAttributes,
            object imgHtmlAttributes)
        {
            var linkAttributes = AnonymousObjectToKeyValue(linkHtmlAttributes);
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            linkBuilder.MergeAttributes(linkAttributes, true);
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }*/

        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues,
            object imgHtmlAttributes)
        {
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.AddCssClass("iconLink");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += altText;
            text += linkBuilder.ToString(TagRenderMode.EndTag);

            return MvcHtmlString.Create(text);
        }
        /*
        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }
        */
        private static Dictionary<string, object> AnonymousObjectToKeyValue(object anonymousObject)
        {
            var dictionary = new Dictionary<string, object>();
            if (anonymousObject != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
                {
                    dictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(anonymousObject));
                }
            }
            return dictionary;
        }
    }
}