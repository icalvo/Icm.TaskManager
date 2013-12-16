define(['knockout', 'app/task/activity.viewmodel', 'app/infrastructure/modals', 'knockout.mapping', 'moment'], function (ko, ActivityViewModel, modals, mapping, moment) {
    return function ReminderViewModel(taskListViewModel, data) {
        var self = this;

        mapping.fromJS(data, {}, self);

        self.template = "reminder-template";
        self.modal = null;

        self.taskListViewModel = taskListViewModel;

        // Public methods
        // --------------
        self.showAlarm = function () {
            self.taskListViewModel.activity.push(new ActivityViewModel("ALARM: " + self.description()));
            modals.showModal({
                viewModel: self,
                context: this
            })
            .then(function () {
                // Executed after the modal is closed
            });
        }

        self.cancel = function () {
            console.assert(self.modal != null);
            self.modal.close();
        };

        // Constructor
        // -----------
        self.taskListViewModel.activity.push(new ActivityViewModel("Loaded reminder: " + self.description()));

        if (data.alarmDate != null) {
            var diff = -moment().diff(data.alarmDate);
            if (diff > 0) {
                self.taskListViewModel.activity.push(new ActivityViewModel("Set alarm for: " + self.description()));
                setTimeout(self.showAlarm, diff);
            }
        }
    }
});