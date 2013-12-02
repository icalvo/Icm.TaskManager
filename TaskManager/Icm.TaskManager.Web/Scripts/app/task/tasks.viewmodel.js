function TasksViewModel(app, dataModel) {
    var self = this;
    var startedLoad = false;

    self.title = ko.observable("Mis tareas");
    self.tasks = ko.observableArray();
    self.loading = ko.observable(true);
    self.message = ko.observable();
    self.errors = ko.observableArray();

    // For creating tasks
    self.newTaskDescription = ko.observable();

    // Operations
    self.load = function () { // Load user management data
        if (!startedLoad) {
            taskmanager.api.Task.GetTasks()
                .done(function (data) {
                    self.tasks.removeAll();
                    for (var i = 0; i < data.length; i++) {
                        var viewmodel = ko.mapping.fromJS(data[i]);
                        self.tasks.push(viewmodel);
                    }
                })
                .failJSON(function (data) { app.showErrors(data, "Error retrieving task list.") });
        }
    }

    self.addTask = function () {
        var newTaskData = { Id: 0, Description: this.newTaskDescription() };
        taskmanager.api.Task.PostTask({ task: newTaskData })
                .done(function (newTask) {
                    var observable = ko.mapping.fromJS(newTask);
                    self.tasks.push(observable);
                })
                .failJSON(function (data) { app.showErrors(data, "Error adding a task.") });
        self.newTaskDescription("");
    }

    self.doneTask = function (task) {
        task.finishDate(new Date());
        task.isDone(true);
    };
    self.editTask = function (task) {
    };
    self.deleteTask = function (task) {
        taskmanager.api.Task.DeleteTask({ id: task.id() })
                .done(function (data) {
                    self.tasks.remove(task);
                })
                .failJSON(function (data) { app.showErrors(data, "Error deleting task.") });
    };

    self.load();
}

app.addViewModel({
    name: "Tasks",
    bindingMemberName: "tasks",
    factory: TasksViewModel
});
