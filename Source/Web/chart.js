Bifrost.namespace("Web", {
    chart: Bifrost.views.ViewModel.extend(function (measurements) {
        this.trend = ko.observableArray([
            ["12:00", 42, "12:00", 100],
            ["12:20", 43, "12:20", 70]
        ]);
    })
});