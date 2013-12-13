function TaskListViewModel(app) {
    var self = this;
    var startedLoad = false;

    self.title = ko.observable("Mis tareas");
    self.tasks = ko.observableArray();
    self.loading = ko.observable(true);
    self.message = ko.observable();
    self.errors = ko.observableArray();
    self.activity = ko.observableArray();
    self.reminders = ko.observableArray();
    // For creating tasks
    self.newTaskDescription = ko.observable();

    // Operations
    self.load = function () { // Load user management data
        if (!startedLoad) {
            self.tasks.removeAll();
            var alert = moment().add("seconds", 5).toISOString();
            self.reminders.push(new ReminderViewModel(self, {
                "description": "MY REMINDER TEST",
                "alarmDate": alert
            }));
            taskManagerApi.Task.GetTasks()
                .done(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var viewmodel = new TaskViewModel(self, data[i]);
                        self.tasks.push(viewmodel);
                    }

                })
                .failJSON(function (data) { app.showErrors(data, "Error retrieving task list.") });
            taskManagerApi.Reminder.GetActiveReminders()
                .done(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var viewmodel = new ReminderViewModel(self, data[i]);
                        self.reminders.push(viewmodel);
                    }
                })
                .failJSON(function (data) { app.showErrors(data, "Error retrieving active reminders list.") });
        }
    }

    self.addTask = function () {
        var newTaskData = { Id: 0, Description: this.newTaskDescription() };
        taskManagerApi.Task.PostTask({ task: newTaskData })
                .done(function (newTask) {
                    var observable = ko.mapping.fromJS(newTask);
                    self.tasks.push(observable);
                })
                .failJSON(function (data) { app.showErrors(data, "Error adding a task.") });
        self.newTaskDescription("");
    }

    self.hideTask = function (element, index, data) {
        $(element).hide(function() { this.remove() } );
    }

    self.doneTask = function (task) {
        task.finishDate(new Date());
        task.isDone(true);
    };
    self.editTask = function (task) {
    };
    self.deleteTask = function (task) {
        taskManagerApi.Task.DeleteTask({ id: task.id() })
                .done(function (data) {
                    self.tasks.remove(task);
                })
                .failJSON(function (data) { app.showErrors(data, "Error deleting task.") });
    };

    self.load();
}
