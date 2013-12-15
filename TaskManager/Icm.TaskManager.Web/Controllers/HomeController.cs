using System.Web.Mvc;

namespace Icm.TaskManager.Web.Controllers
{
    /// <summary>
    /// Home controller, loads the SPA view.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
