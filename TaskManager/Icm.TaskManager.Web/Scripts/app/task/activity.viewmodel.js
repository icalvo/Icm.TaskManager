function ActivityViewModel(message) {
    var self = this;

    self.date = ko.observable(new Date());
    self.message = ko.observable(message);
}