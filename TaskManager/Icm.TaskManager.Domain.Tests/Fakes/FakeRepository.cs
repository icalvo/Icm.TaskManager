using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain.Tests
{
    public class FakeRepository : ITaskRepository
    {
        #region Private fields

        #endregion

        public FakeRepository()
        {
            this.Items = new List<Task>();
        }

        public FakeRepository(IEnumerable<Task> items)
        {
            this.Items = new List<Task>();
            this.Items.AddRange(items);
        }

        protected List<Task> Items { get; set; }

        public void Create(Task task)
        {
            this.Items.Add(task);
        }

        public Task GetById(int id)
        {
            return this.Items.SingleOrDefault(task => task.Id == id);
        }

        public bool Update(Task task)
        {
            Delete(task);
            Create(task);
            return true;
        }

        public void Delete(Task task)
        {
            var dbTask = this.GetById(task.Id);
            this.Items.Remove(dbTask);
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}
