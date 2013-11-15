using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icm.TaskManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repo = new TaskManager.Infrastructure.TaskRepository(new TaskManager.Infrastructure.TaskManagerContext());

            return View(repo);
        }
    }
}
