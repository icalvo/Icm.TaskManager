using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AutoMapper;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Rest.DTOs;

namespace Icm.TaskManager.Rest.Controllers
{
    [EnableCors("*", "*", "*")]
    public class TaskController : ApiController
    {
        private readonly ITaskRepository taskRepository;
        private readonly TaskApplicationService taskService;

        public TaskController(ITaskRepository taskRepository, TaskApplicationService taskService)
        {
            this.taskRepository = taskRepository;
            this.taskService = taskService;
            Mapper.CreateMap<Task, TaskInfoDto>();
        }

        // GET api/task/5
        [ResponseType(typeof(TaskInfoDto))]
        public IHttpActionResult GetTask(int id)
        {
            TaskId taskId = new TaskId(id);
            Task task = taskRepository.GetById(taskId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<TaskInfoDto>(task));
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

            var key = new TaskId(id);
            if (taskRepository.GetById(key) == null)
            {
                return NotFound();
            }

            taskRepository.Update(key, task);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/task
        [ResponseType(typeof(TaskInfoDto))]
        public IHttpActionResult PostTask(TaskInfoDto taskInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskId = taskService.CreateTaskParsing(
                taskInfo.Description,
                taskInfo.DueDate,
                taskInfo.RecurrenceType,
                taskInfo.RepeatInterval,
                taskInfo.Priority,
                taskInfo.Notes,
                taskInfo.Labels);

            return CreatedAtRoute("DefaultApi", new { id = taskId }, taskInfo);
        }

        // DELETE api/task/5
        [ResponseType(typeof(TaskInfoDto))]
        public IHttpActionResult DeleteTask(int id)
        {
            TaskId taskId = new TaskId(id);
            Task task = taskRepository.GetById(taskId);
            if (task == null)
            {
                return NotFound();
            }

            taskRepository.Delete(taskId);

            return Ok(Mapper.Map<TaskInfoDto>(task));
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
