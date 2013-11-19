function TasksViewModel(app, dataModel) {
    var self = this;
    var startedLoad = false;

    self.tasks = ko.observableArray();
    self.loading = ko.observable(true);
    self.message = ko.observable();
    self.errors = ko.observableArray();
    self.dataModel = new TaskDataModel();

    // For creating tasks
    self.newTaskDescription = ko.observable();

    // Operations
    self.load = function () { // Load user management data
        if (!startedLoad) {
            startedLoad = true;

            self.dataModel.getTasks(dataModel.returnUrl, true /* generateState */)
                .done(function (data) {
                    self.tasks.removeAll();
                    if (true) {
                        for (var i = 0; i < data.length; i++) {
                            var viewmodel = ko.mapping.fromJS(data[i]);
                            self.tasks.push(viewmodel);
                        }
                    } else {
                        app.errors.push("Error retrieving task list.");
                    }

                    self.loading(false);
                }).failJSON(function (data) {
                    var errors;

                    self.loading(false);
                    errors = dataModel.toErrorsArray(data);

                    if (errors) {
                        app.errors(errors);
                    } else {
                        app.errors.push("Error retrieving task list.");
                    }
                });
        }
    }

    self.addTask = function () {
        var data = { id: 0, description: this.newTaskDescription() };
        var observable = ko.mapping.fromJS(data);
        self.tasks.push(observable);
        self.dataModel.createTask(data, true /* generateState */)
                .done(function (data) {
                    //observable.creating(false);
                }).failJSON(function (data) {
                    var errors;

                    self.loading(false);
                    errors = dataModel.toErrorsArray(data);

                    if (errors) {
                        app.errors(errors);
                    } else {
                        app.errors.push("Error retrieving task list.");
                    }
                });
        self.newTaskDescription("");
    }

    self.doneTask = function () { };
    self.editTask = function () { };
    self.deleteTask = function () { };

    self.load();
}

app.addViewModel({
    name: "Tasks",
    bindingMemberName: "tasks",
    factory: TasksViewModel
});
