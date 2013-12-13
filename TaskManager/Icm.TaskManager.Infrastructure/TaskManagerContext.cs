using Icm.TaskManager.Domain.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskManagerContext : DbContext
    {
        public IDbSet<Domain.Tasks.Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Ignore(task => task.Recurrence);
        }
    }
}
