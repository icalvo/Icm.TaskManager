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
            var repo = new TaskManager.Infrastructure.TaskRepository(new TaskManager.Infrastructure.TaskManagerContext());

            return View(repo);
        }
    }
}
