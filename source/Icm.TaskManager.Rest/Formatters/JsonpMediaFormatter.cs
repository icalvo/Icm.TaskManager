using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Icm.TaskManager.Rest.Formatters
{
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private readonly string callbackQueryParameter;

        public JsonpMediaTypeFormatter(string callbackQueryParameter = "callback")
        {
            this.callbackQueryParameter = callbackQueryParameter;
            SupportedMediaTypes.Add(DefaultMediaType);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
            MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
        {
            string callback = Callback();

            if (callback != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(callback + "(");
                    writer.Flush();

                    base.WriteToStreamAsync(type, value, stream, content, transportContext).Wait();

                    writer.Write(")");
                    writer.Flush();
                });
            }

            return base.WriteToStreamAsync(type, value, stream, content, transportContext);
        }

        private string Callback()
        {
            if (HttpContext.Current.Request.HttpMethod != "GET")
            {
                return null;
            }

            string callback = HttpContext.Current.Request.QueryString[callbackQueryParameter];

            if (string.IsNullOrEmpty(callback))
            {
                return null;
            }

            return callback;
        }
    }
}