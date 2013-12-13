using System.Web;
using System.Web.Mvc;

namespace Icm.TaskManager.Web.Configuration
{
    /// <summary>
    /// MVC filter configuration
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
