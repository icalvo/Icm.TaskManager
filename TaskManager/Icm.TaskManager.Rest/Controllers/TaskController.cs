using Icm.TaskManager.Domain;
using Icm.TaskManager.Rest.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Icm.TaskManager.Domain.Tasks;
using System.Web.Http.Cors;

namespace Icm.TaskManager.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class TaskController : ApiController
    {
        private ITaskRepository taskRepository;
        private ITaskService taskService;

        public TaskController(ITaskRepository taskRepository, ITaskService taskService)
        {
            this.taskRepository = taskRepository;
            this.taskService = taskService;
        }

        // GET api/task
        [ResponseType(typeof(IEnumerable<TaskInfoDto>))]
        public IHttpActionResult GetTasks()
        {
            return Ok(this.taskRepository.Select(Mapper.Map<Task, TaskInfoDto>).ToList());
        }

        // GET api/task/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = this.taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            Mapper.Map<TaskInfoDto>(task);
            return Ok(task);
        }

        // PUT api/task/5
        public IHttpActionResult PutTask(int id, TaskModifyDto taskInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskInfo.Id)
            {
                return BadRequest();
            }

            Task task = Mapper.Map<Task>(taskInfo);

            if (!this.taskRepository.Update(task))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/task
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(TaskInfoDto taskInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = this.taskService.CreateTask(
                taskInfo.Description,
                taskInfo.StartDate,
                taskInfo.DueDate,
                taskInfo.RecurrenceType,
                taskInfo.RepeatInterval,
                taskInfo.Priority,
                taskInfo.Notes,
                taskInfo.Labels
            );

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        }

        // DELETE api/task/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = this.taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            this.taskRepository.Delete(task);

            return Ok(task);
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
