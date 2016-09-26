using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AutoMapper;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Rest.DTOs;

namespace Icm.TaskManager.Rest.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ReminderController : ApiController
    {
        private readonly ITaskRepository taskRepository;
        private ITaskService taskService;

        public ReminderController(ITaskRepository taskRepository, ITaskService taskService)
        {
            this.taskRepository = taskRepository;
            this.taskService = taskService;
            Mapper.CreateMap<Task, TaskInfoDto>();
        }

        // GET api/reminder
        [ResponseType(typeof(IEnumerable<ReminderDto>))]
        public IHttpActionResult GetActiveReminders()
        {
            return Ok(taskRepository.GetActiveReminders().Select(x => new ReminderDto { AlertDate = x.ToDateTimeUtc() }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}
