function TaskManagerApi() {
    var self = this;

    self.restRoot = "http://localhost:53187";

    $.ajax(self.restRoot + "/api/metadata")
    .done(function(data) {
        var metadata = data;

        $.each(metadata, function (i, action) {
            if (!self[action.controllerName]) {
                self[action.controllerName] = {};
            }
            self[action.controllerName][action.actionName] = function (parameters) {
                var url = self.restRoot + '/' + action.url;
                var data;
                $.each(action.parameters, function (j, parameter) {
                    if (parameters[parameter.Name] === undefined) {
                        console.log('Missing parameter: ' + parameter.Name + ' for API: ' + action.controllerName + '/' + action.actionName);
                    } else if (parameter.IsUriParameter) {
                        url = url.replace("{" + parameter.Name + "}", parameters[parameter.Name]);
                    } else if (data === undefined) {
                        data = parameters[parameter.Name];
                    } else {
                        console.log('Detected multiple body-parameters for API: ' + action.controllerName + '/' + action.actionName);
                    }
                });
                return $.ajax({
                    type: action.method,
                    url: url,
                    data: data
                });
            };
        });
    })
    .fail(function () {
        alert("FATAL ERROR: Failed to load API metadata");
    });
}

var taskManagerApi = new TaskManagerApi();
