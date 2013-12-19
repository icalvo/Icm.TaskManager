define(['knockout', 'app/task/activity.viewmodel', 'knockout.mapping'], function (ko, ActivityViewModel, mapping) {
    return function TaskViewModel(taskListViewModel, data) {
        var self = this;
        mapping.fromJS(data, {}, self);

        self.taskListViewModel = taskListViewModel;

        self.isDone = function () {
            return self.finishDate() != null;
        };

        // Constructor
        // -----------
        self.taskListViewModel.activity.push(new ActivityViewModel("Loaded task: " + self.description()));
    }
});