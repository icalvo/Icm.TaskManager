$(function () {
    var app = new AppViewModel();

    app.viewmodel(new TaskListViewModel(app));

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
