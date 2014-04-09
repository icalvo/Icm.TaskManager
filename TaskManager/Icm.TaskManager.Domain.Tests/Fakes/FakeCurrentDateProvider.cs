using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeCurrentDateProvider : ICurrentDateProvider
    {
        private readonly Instant now;

        public FakeCurrentDateProvider(Instant now)
        {
            this.now = now;
        }

        public Instant Now
        {
            get
            {
                return this.now;
            }
        }
    }
}
