$(function () {
    var app = new AppViewModel(new AppDataModel());

    app.addViewModel({
        name: "TaskList",
        bindingMemberName: "taskList",
        factory: TaskListViewModel
    });


    app.addViewModel({
        name: "Home",
        bindingMemberName: "home",
        factory: HomeViewModel
    });

    app.addViewModel({
        name: "Login",
        bindingMemberName: "login",
        factory: LoginViewModel,
        navigatorFactory: function (app) {
            return function () {
                app.errors.removeAll();
                app.user(null);
                app.view(app.Views.Login);
            };
        }
    });

    app.addViewModel({
        name: "Manage",
        bindingMemberName: "manage",
        factory: ManageViewModel,
        navigatorFactory: function (app) {
            return function (externalAccessToken, externalError) {
                app.errors.removeAll();
                app.view(app.Views.Manage);

                if (typeof (externalAccessToken) !== "undefined" || typeof (externalError) !== "undefined") {
                    app.manage().addExternalLogin(externalAccessToken, externalError);
                } else {
                    app.manage().load();
                };
            }
        }
    });

    app.addViewModel({
        name: "Register",
        bindingMemberName: "register",
        factory: RegisterViewModel
    });

    app.addViewModel({
        name: "RegisterExternal",
        bindingMemberName: "registerExternal",
        factory: RegisterExternalViewModel,
        navigatorFactory: function (app) {
            return function (userName, loginProvider, externalAccessToken, loginUrl, state) {
                app.errors.removeAll();
                app.view(app.Views.RegisterExternal);
                app.registerExternal().userName(userName);
                app.registerExternal().loginProvider(loginProvider);
                app.registerExternal().externalAccessToken = externalAccessToken;
                app.registerExternal().loginUrl = loginUrl;
                app.registerExternal().state = state;
            };
        }
    });

    app.initialize();
    ko.bindingHandlers.dateString = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor(),
                allBindings = allBindingsAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var pattern = allBindings.datePattern || 'yyyy-MM-dd';
            $(element).text(moment(valueUnwrapped).calendar());
        }
    }

    // Activate Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app, document.getElementById("htmlDoc"));
});
