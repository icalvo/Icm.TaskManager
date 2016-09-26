using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Rest.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using Icm.TaskManager.Application;
using NodaTime;
using Icm.TaskManager.Domain.Tests.Fakes;
using Icm.TaskManager.Infrastructure;
using Icm.TaskManager.Infrastructure.Interfaces;
using Icm.TaskManager.Rest.Controllers;
using NodaTime.Testing;

namespace Icm.TaskManager.Web.Tests
{
    [TestClass]
    public class TaskControllerTests
    {
        [TestMethod]
        public void GetTaskId_WhenNotFound_ReturnsNotFound()
        {
            var controller = BuildController(new FakeTaskRepository());
            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskId_WhenFound_ReturnsOk()
        {
            var repo = new FakeTaskRepository();
            TaskId newTaskId = repo.Add(new Task());
            var controller = BuildController(repo);
            var result = controller.GetTask(newTaskId.Value);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TaskInfoDto>));
        }

        private static TaskController BuildController(FakeTaskRepository repo)
        {
            var currentDateProvider = new FakeClock(new Instant(10000L * 3600L * 24L * 30L));
            var controller = new TaskController(repo, new TaskApplicationService(
                repo,
                currentDateProvider, 
                new MemoryEventBus(),
                new MessageDispatcher(new InMemoryEventStore())));
            return controller;
        }
    }
}
