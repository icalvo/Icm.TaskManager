using Icm.TaskManager.Web.Configuration;
using Icm.TaskManager.Web.Configuration.Routes;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icm.TaskManager.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            MvcConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
