define(['knockout', 'moment'], function (ko, moment) {
    ko.bindingHandlers.dateString = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor(),
                allBindings = allBindingsAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var pattern = allBindings.datePattern || 'yyyy-MM-dd';
            $(element).text(moment(valueUnwrapped).calendar());
        }
    }
});