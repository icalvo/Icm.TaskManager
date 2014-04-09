﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private TaskManagerContext context;

        public TaskRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        public void Create(Task task) {
            this.context.Tasks.Add(task);
            this.context.SaveChanges();
        }

        public bool Update(Task task)
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

        public void Delete(Task task)
        {
            this.context.Tasks.Remove(task);
            this.context.SaveChanges();
        }

        public Task GetById(int id)
        {
            return context.Tasks.Find(id);
        }

        public bool Exists(int id)
        {
            return context.Tasks.Any(task => task.Id == id);
        }

        public IEnumerable<Reminder> GetActiveReminders()
        {
            var now = SystemClock.Instance.Now;
            return context.Tasks.SelectMany(task => task.Reminders).Where(reminder => reminder.AlarmDate >= now);
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return context.Tasks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return context.Tasks.GetEnumerator();
        }
    }
}
