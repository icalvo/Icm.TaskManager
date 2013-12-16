require.config({
    baseUrl: 'assets/js/lib',
    paths: {
        app: '../app',
        'knockout.validation': 'knockout.validation.debug'
    },
    shim: {
        'bootstrap': ['jquery']
    },
});

require(['knockout', 'app/app.viewmodel', 'app/task/tasklist.viewmodel', 'knockout.validation', /* Non-exporting -> */ 'app/infrastructure/koBindingHandlers'], function (ko, AppViewModel, TaskListViewModel) {
    var app = new AppViewModel();

    app.viewmodel(new TaskListViewModel(app));

    // Activate Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app, document.getElementById("htmlDoc"));
});