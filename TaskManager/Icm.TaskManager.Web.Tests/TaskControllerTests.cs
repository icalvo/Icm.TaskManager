using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;

namespace Icm.TaskManager.Web.Tests
{
    [TestClass]
    public class TaskControllerTests
    {
        [TestMethod]
        public void GetTaskId_WhenNotFound_ReturnsNotFound()
        {
            var controller = new Web.Controllers.TaskController(new Domain.Tests.FakeRepository());

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskId_WhenFound_ReturnsOk()
        {
            var controller = new Web.Controllers.TaskController(new Domain.Tests.FakeRepository(new[] { new Domain.Task { Id = 1 } } ));

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Domain.Task>));
        }
    }
}
