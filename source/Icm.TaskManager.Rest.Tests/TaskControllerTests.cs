using AutoMapper;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
using Icm.TaskManager.Rest.DTOs;
using Icm.TaskManager.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http.Results;
using NodaTime;

namespace Icm.TaskManager.Web.Tests
{
    [TestClass]
    public class TaskControllerTests
    {
        [TestMethod]
        public void GetTaskId_WhenNotFound_ReturnsNotFound()
        {
            var repo = new FakeTaskRepository();
            var currentDateProvider = new FakeCurrentDateProvider(new Instant(10000L * 3600L * 24L * 30L));
            var controller = new TaskController(repo, new TaskService(currentDateProvider));

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskId_WhenFound_ReturnsOk()
        {
            var repo = new FakeTaskRepository(new[] { new FakeTask(1) });
            var currentDateProvider = new FakeCurrentDateProvider(new Instant(10000L * 3600L * 24L * 30L));
            var controller = new TaskController(repo, new TaskService(currentDateProvider));

            var result = controller.GetTask(1);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TaskInfoDto>));
        }

    }
}
