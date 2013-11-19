using Icm.TaskManager.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            task.CreationDate = DateTime.Now;
            this.context.Tasks.Add(task);
            this.context.SaveChanges();
        }

        public bool Update(Domain.Task task)
        {
            this.context.Entry(task).State = EntityState.Modified;

            try
            {
                this.context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(task.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public void Delete(Domain.Task task)
        {
            this.context.Tasks.Remove(task);
            this.context.SaveChanges();
        }

        public Domain.Task GetById(int id)
        {
            return context.Tasks.Find(id);
        }

        private bool Exists(int id)
        {
            return context.Tasks.Any(task => task.Id == id);
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
