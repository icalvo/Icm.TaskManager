using Icm.TaskManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Icm.TaskManager.Web.Controllers
{
    public class TaskController : ApiController
    {
        private Domain.ITaskRepository taskRepository;

        public TaskController(Domain.ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        // GET api/task
        public IQueryable<Task> GetTasks()
        {
            return this.taskRepository.AsQueryable();
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

            return Ok(task);
        }

        // PUT api/task/5
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            if (!this.taskRepository.Update(task))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/task
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            this.taskRepository.Create(task);

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
