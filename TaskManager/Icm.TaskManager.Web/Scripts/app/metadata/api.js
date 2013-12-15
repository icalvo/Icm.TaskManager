define(['knockout', 'jquery', 'http://localhost:53187/api/metadata?callback=define'], function (ko, module, data) {
    function TaskManagerApi() {
        var self = this;

        var restRoot = 'http://localhost:53187';

        $.each(data.methods, function (i, action) {
            if (!self[action.controllerName]) {
                self[action.controllerName] = {};
            }
            self[action.controllerName][action.actionName] = function (parameters) {
                var url = restRoot + '/' + action.url;
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
    }

    var result = new TaskManagerApi();

    return result;
});
