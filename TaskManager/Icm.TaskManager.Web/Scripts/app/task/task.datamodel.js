function TaskDataModel() {
    var self = this,
        // Routes
        taskUrl = "/api/Task";

    // Data access operations
    self.getTasks = function () {
        return $.ajax(taskUrl);
    };

    self.createTask = function (data) {
        return $.ajax(taskUrl, {
            type: "POST",
            data: data
        });
    };
}
