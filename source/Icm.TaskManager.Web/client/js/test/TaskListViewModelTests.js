require(['knockout', 'jquery', 'app/app.viewmodel', 'app/task/tasklist.viewmodel', 'app/infrastructure/templateLoader',/* Non-exporting -> */ 'knockout.validation', 'app/infrastructure/koBindingHandlers'], function (ko, $, AppViewModel, TaskListViewModel, loadExternalKoTemplates) {
    var app = new AppViewModel();

    var taskListViewModel = new TaskListViewModel(app);

    // Activate Knockout
    ko.validation.init({ grouping: { observable: false } });

    // When all templates are loaded, apply bindings
    loadExternalKoTemplates().done(function () {
        ko.applyBindings(app, $('html')[0]);
    });
});