using Icm.TaskManager.Domain;
using Icm.TaskManager.Web.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.Web.Controllers
{
    public class ReminderController : ApiController
    {
        private ITaskRepository taskRepository;
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
            return Ok(this.taskRepository.GetActiveReminders());
        }

        #region IDisposable implementation

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //this.taskRepository.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion
    }
}
