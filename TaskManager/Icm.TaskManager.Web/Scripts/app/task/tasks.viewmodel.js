// Class to represent a row in the tasks list
function Task(id, name) {
    var self = this;

    self.name = name;

    self.id = ko.observable(initialMeal);
}

function TaskViewModel(app, title, dataModel) {
    var self = this;

    // Data
    self.title = ko.observable(title);

    // Operations
    self.complete = function () {
        dataModel.logout().done(function () {
            app.navigateToLoggedOff();
        }).fail(function () {
            app.errors.push("Log off failed.");
        });
    };

    self.manage = function () {
        app.navigateToManage();
    };
}
