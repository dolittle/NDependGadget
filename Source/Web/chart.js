Bifrost.namespace("Web", {
    chart: Bifrost.views.ViewModel.extend(function (measurements) {

        this.trend = measurements.all();

        /*
        this.trend = ko.observableArray([
            { something: 2006, val1: 42, val2: 80 },
            { something: 2007, val1: 52, val2: 100 },
            { something: 2008, val1: 142, val2: 70 },
            { something: 2009, val1: 32, val2: 50 }
        ]);*/
    })
});