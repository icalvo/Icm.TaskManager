using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeCurrentDateProvider : ICurrentDateProvider
    {
        private readonly DateTime now;

        public FakeCurrentDateProvider(DateTime now)
        {
            this.now = now;
        }

        public DateTime Now
        {
            get
            {
                return this.now;
            }
        }
    }
}
