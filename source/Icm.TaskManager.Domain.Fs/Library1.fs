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

    member t.recur now =
        match (t.Recurrence, t.FinishDate) with
        | (Some (DueDate duration), _) -> Some { t with DueDate = t.DueDate + duration; CreationDate = now }
        | (Some (FinishDate duration), Some finishDate) -> Some { t with DueDate = finishDate + duration; CreationDate = now }
        | _ -> None