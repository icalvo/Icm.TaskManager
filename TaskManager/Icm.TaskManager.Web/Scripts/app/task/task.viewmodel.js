function TaskViewModel(taskListViewModel, data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.taskListViewModel = taskListViewModel;

    // Constructor
    // -----------
    self.taskListViewModel.activity.push(new ActivityViewModel("Loaded task: " + self.description()));
}