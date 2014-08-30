using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    /// <summary>
    /// A <see cref="JsonResult"/> implementation that uses JSON.NET to perform the serialization.
    /// https://gist.github.com/jpoehls/1424538
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        // public JsonRequestBehavior JsonRequestBehavior { get; set; }
        // public Encoding ContentEncoding { get; set; }
        // public string ContentType { get; set; }
        // public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings
            {
                // http://stackoverflow.com/questions/1153385/a-circular-reference-was-detected-while-serializing-an-object-of-type-subsonic
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output)
                {
                    Formatting = Formatting
                };
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}