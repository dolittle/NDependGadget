Bifrost.namespace("Web", {
    chart: Bifrost.views.ViewModel.extend(function (measurements) {
        measurements.buildIdentifier("Default");
        this.trend = measurements.all();
    })
});