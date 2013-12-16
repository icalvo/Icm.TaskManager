define(['knockout', 'moment'], function (ko, moment) {
    return function AppViewModel() {
        var self = this;

        self.title = ko.observable('TaskManagr');

        self.viewmodel = ko.observable();

        self.loading = ko.observable(false);
        self.loggedIn = ko.observable(true);

        self.errors = ko.observableArray();

        self.threeColumnLayout = true;
        self.user = "icalvo";

        self.copyrightYear = function () {
            return moment().year();
        }

        self.showErrors = function (data, defaultMessage) {
            var errors;

            //errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push(defaultMessage);
            }
        }
    };
});