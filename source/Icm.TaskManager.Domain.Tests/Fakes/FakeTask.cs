using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeTask : Task
    {
        public FakeTask(int id)
        {
            this.Id = id;
        }
    }
}
