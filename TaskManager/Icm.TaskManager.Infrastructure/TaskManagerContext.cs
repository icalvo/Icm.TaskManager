using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskManagerContext : DbContext
    {
        public IDbSet<Domain.Task> Tasks { get; set; }
    }
}
