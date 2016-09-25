using System.Web.Mvc;
using System.Web.Routing;

namespace Icm.TaskManager.Web.Configuration.Routes
{
    /// <summary>
    /// Routing configuration for MVC
    /// </summary>
    public class MvcConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore the client directory which contains images, js, css & html
            routes.IgnoreRoute("client/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
