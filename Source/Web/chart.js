Bifrost.namespace("Web", {
    chart: Bifrost.views.ViewModel.extend(function (measurements, trendHub) {
        var self = this;
        measurements.buildIdentifier("Default");
        this.trend = measurements.all();

        this.trendingDirection = ko.observable("middle");
        this.showingTrend = ko.observable(false);

        this.calculateTrendDirection = function (measurements) {
            if (measurements.length <= 1) self.trendingDirection("middle");

            var last = measurements[measurements.length - 1];
            var secondToLast = measurements[measurements.length - 2];

            if( last.numberOfRulesViolations > secondToLast.numberOfRulesViolations || 
                last.numberOfCriticalRulesViolations > secondToLast.numberOfCriticalRulesViolations) {

                self.trendingDirection("down");
            } else {
                self.trendingDirection("up");
            }
        };

        this.trend.completed(this.calculateTrendDirection);

        trendHub.client(function (client) {
            client.changed = function () {
                self.trend.execute();
            };
        });

        setInterval(function () {
            self.showingTrend(self.showingTrend() == true ? false: true);
        },10000);
    })
});