using Icm.TaskManager.Domain.Tasks;
using System.Data.Entity;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskManagerContext : DbContext
    {
        public IDbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Ignore(task => task.Recurrence);
        }
    }
}
