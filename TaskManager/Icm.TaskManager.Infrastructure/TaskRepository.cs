using Icm.TaskManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private TaskManagerContext context;

        public TaskRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        public void Create(Domain.Task task) {
            context.Tasks.Add(task);
            context.SaveChanges();
        }

        public Domain.Task GetById(int id)
        {
            return context.Tasks.Find(id);
        }

        public IEnumerator<Domain.Task> GetEnumerator()
        {
            return context.Tasks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return context.Tasks.GetEnumerator();
        }
    }
}
