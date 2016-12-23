namespace Icm.TaskManager.Domain.Fs

open NodaTime

type Recurrence =
    | DueDate of Duration
    | FinishDate of Duration

type Task = {
    Description: string;
    CreationDate: Instant;
    DueDate: Instant;
    StartDate: Instant option;
    FinishDate: Instant option;
    Recurrence: Recurrence option;
    Priority: int;
    Notes: string;
    Labels: string[];
    Reminders: Instant[];
}

type Task with
    static member create desc dueDate now =
        {
                Description = desc;
                CreationDate = now;
                DueDate = dueDate;
                StartDate = None;
                Priority = 3;
                FinishDate = None;
                Recurrence = None;
                Notes = "";
                Labels = [||];
                Reminders = [||];
        }

    member t.isDone = t.FinishDate.IsSome

    member t.recur recurrence newDueDate now =
        match recurrence with
        | DueDate d -> { t with DueDate = newDueDate; CreationDate = now }
        | FinishDate d -> { t with DueDate = newDueDate; CreationDate = now }