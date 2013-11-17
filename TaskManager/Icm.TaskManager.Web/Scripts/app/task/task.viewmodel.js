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
