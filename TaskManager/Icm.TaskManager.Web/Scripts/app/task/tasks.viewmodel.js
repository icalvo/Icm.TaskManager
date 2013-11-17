// Class to represent a row in the tasks list
function Task(id, name) {
    var self = this;

    self.id = ko.observable(id);

    self.name = name;
}

function TasksViewModel(app, dataModel) {
    var self = this;

    self.tasks = ko.observableArray([
        new Task(345, 'Lavavajillas'),
        new Task(441, 'Ensayo')]);
}

app.addViewModel({
    name: "Tasks",
    bindingMemberName: "tasks",
    factory: TasksViewModel
});
