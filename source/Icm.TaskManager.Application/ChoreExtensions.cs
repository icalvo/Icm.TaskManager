using System.Collections.Generic;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;

namespace Icm.ChoreManager.Application
{
    internal static class ChoreExtensions
    {
        internal static ChoreDto ToDto(this Identified<ChoreId, Chore> identifiedChore)
        {
            var chore = identifiedChore.Value;
            return new ChoreDto
            {
                Id = identifiedChore.Id,
                CreationDate = chore.CreationDate,
                Description = chore.Description,
                DueDate = chore.DueDate,
                FinishDate = chore.FinishDate,
                Labels = chore.Labels,
                Notes = chore.Notes,
                Recurrence = chore.Recurrence,
                Reminders = new List<Instant>(chore.Reminders),
                Priority = chore.Priority,
                StartDate = chore.StartDate
            };
        }
    }
}