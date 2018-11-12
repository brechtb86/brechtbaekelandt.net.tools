/// <reference path="../knockout/knockout.extensions.js" />
/// <reference path="../../lib/jquery/dist/jquery.js" />
/// <reference path="../../lib/knockout-mapping/build/output/knockout.mapping-latest.debug.js" />

var brechtbaekelandt = brechtbaekelandt || {};

brechtbaekelandt.home = (function ($, jQuery, ko, undefined) {
    "use strict";

    function HomeViewModel() {
        var self = this;

        self.newGuid = ko.observable();
        self.unminifiedJavascript = ko.observable().extend({
            required: { message: "you didn't paste any javascript!" }
        });
        self.unminifiedCSS = ko.observable().extend({
            required: { message: "you didn't paste any CSS!" }
        });
        self.minifiedJavascript = ko.observable();
        self.minifiedCSS = ko.observable();

        self.minifyJavascriptErrorMessage = ko.observable();
        self.minifyCSSErrorMessage = ko.observable();

        self.minifyJavascriptValidationErrors = ko.validation.group({ unminifiedJavascript: self.unminifiedJavascript });
        self.minifyCSSValidationErrors = ko.validation.group({ unminifiedCSS: self.unminifiedCSS });

        self.createGuid();
    };

    HomeViewModel.prototype.createGuid = function () {
        var self = this;

        self.newGuid(null);

        $.ajax({
            url: "../api/tools/guid",
            type: "GET",
            success: function (data, textStatus, jqXhr) { },
            async: false
        })
            .done(function (data, textStatus, jqXhr) {
                self.newGuid(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            })
            .always(function (data, textStatus, jqXhr) {

            });
    }

    HomeViewModel.prototype.minifyJavascript = function (javascript) {
        var self = this;

        if (self.minifyJavascriptValidationErrors().length > 0) {
            self.minifyJavascriptValidationErrors.showAllMessages();
            return;
        }

        self.minifyJavascriptErrorMessage(null);

        $.ajax({
            url: "../api/tools/js/minify",
            type: "POST",
            contentType: "application/json; charset=UTF-8",
            data: ko.toJSON(javascript()),
            dataType: "json",
            cache: false,
            processData: false,
            async: false,
            success: function (data, textStatus, jqXhr) { }
        })
            .done(function (data, textStatus, jqXhr) {
                self.minifiedJavascript(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {
                self.minifyJavascriptErrorMessage("oops, something went wrong, make sure your javascript is correct.");
            })
            .always(function (data, textStatus, jqXhr) {

            });
    }

    HomeViewModel.prototype.minifyCSS = function (css) {
        var self = this;

        if (self.minifyCSSValidationErrors().length > 0) {
            self.minifyCSSValidationErrors.showAllMessages();
            return;
        }

        self.minifyCSSErrorMessage(null);

        $.ajax({
            url: "../api/tools/css/minify",
            type: "POST",
            contentType: "application/json; charset=UTF-8",
            data: ko.toJSON(css()),
            dataType: "json",
            cache: false,
            processData: false,
            async: false,
            success: function (data, textStatus, jqXhr) { }
        })
            .done(function (data, textStatus, jqXhr) {
                self.minifiedCSS(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {
                self.minifyCSSErrorMessage("oops, something went wrong, make sure your CSS is correct.");
            })
            .always(function (data, textStatus, jqXhr) {

            });
    }

    function init() {

        var viewModel = new HomeViewModel();

        ko.applyBindings(viewModel);
    };

    return {
        HomeViewModel: HomeViewModel,
        init: init
    };

})($, jQuery, ko);