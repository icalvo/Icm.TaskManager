using System.Web.Http;
using Icm.TaskManager.Rest.Formatters;
using Newtonsoft.Json.Serialization;

namespace Icm.TaskManager.Rest.Configuration.Routes
{
    /// <summary>
    /// Routing configuration for Web API
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            config.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}