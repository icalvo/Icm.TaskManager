define(['jquery', 'module'], function ($, module) {
    // Recursively loads all the templates defined with the tag data-template-src.
    // Returns a promise for all the loads.
    var loadExternalKoTemplates = function () {
        var templateLoads = [];
        $('script[data-template-src]').each(function () {
            var templateElement = $(this);
            var templateUrl = templateElement.attr('data-template-src');

            if (templateUrl == '') {

                templateUrl = module.config().pathTemplate.replace('{0}', templateElement.attr('id'));
            }
            else {
                templateUrl = templateUrl.replace('~/', '');
            }
            // Load the template and push the promise to the templateLoads array
            templateLoads.push(
                $.get(templateUrl)
                .done(function (data) {
                    templateElement.html(data);
                })
            );
        });
        return $.when.apply($, templateLoads);
    }

    return loadExternalKoTemplates;
});