require.config({
    baseUrl: 'Scripts/lib',
    //except, if the module ID starts with "app",
    //load it from the js/app directory. paths
    //config is relative to the baseUrl, and
    //never includes a ".js" extension since
    //the paths config could be for a directory.
    paths: {
        app: '../app',
        'knockout.validation': 'knockout.validation.debug'
    },
    shim: {
        'bootstrap': ['jquery']
    },
});

require(['knockout', 'app/app.viewmodel', 'app/task/tasklist.viewmodel', 'knockout.validation', /* Non-exporting -> */ 'app/koBindingHandlers'], function (ko, AppViewModel, TaskListViewModel) {
    var app = new AppViewModel();

    app.viewmodel(new TaskListViewModel(app));

    // Activate Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app, document.getElementById("htmlDoc"));
});