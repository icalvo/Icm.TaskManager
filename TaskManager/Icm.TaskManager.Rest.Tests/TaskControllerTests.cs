using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http.Results;

namespace Icm.TaskManager.Web.Tests
{
    [TestClass]
    public class TaskControllerTests
    {
        [TestMethod]
        public void GetTaskId_WhenNotFound_ReturnsNotFound()
        {
            var repo = new Domain.Tests.Fakes.FakeTaskRepository();
            var currentDateProvider = new FakeCurrentDateProvider(new DateTime(2013, 12, 7));
            var controller = new Web.Controllers.TaskController(repo, new Domain.Tasks.TaskService(currentDateProvider));

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskId_WhenFound_ReturnsOk()
        {
            var repo = new Domain.Tests.Fakes.FakeTaskRepository(new[] { new Domain.Tests.Fakes.FakeTask(1) });
            var currentDateProvider = new FakeCurrentDateProvider(new DateTime(2013, 12, 7));
            var controller = new Web.Controllers.TaskController(repo, new Domain.Tasks.TaskService(currentDateProvider));

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task>));
        }

    }
}
