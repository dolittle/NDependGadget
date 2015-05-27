google.load('visualization', '1.0', { 'packages': ['corechart'] });

ko.bindingHandlers.lineChart = {
    init: function (element, valueAccessor, allValueAccessors, viewModel, bindingContext) {
    },
    update: function (element, valueAccessor, allValueAccessors, viewModel, bindingContext) {
        optionsInput = valueAccessor();

        var options = {
            title: optionsInput.title,
            width: optionsInput.width,
            height: optionsInput.height,
            backgroundColor: 'transparent',
            animation: {
                duration: 1000,
                easing: 'out'
            },
            curveType: 'function',
            legend: { position: 'bottom' },
            x: optionsInput.x || { property: "", name: "" },
            series: optionsInput.series || {

            }
        };

        var chart = new google.visualization.LineChart(element);

        var array = [];
        var names = [];
        names.push(options.x.name);
        for (var serie in options.series) {
            names.push(options.series[serie]);
        }

        array.push(names);

        var inputData = ko.unwrap(optionsInput.data);
        inputData.forEach(function (item) {
            var row = [];
            row.push(item[options.x.property]);
            for (var serie in options.series) {
                var value = item[serie] || 0;
                row.push(value);
            }
            array.push(row);
        });

        if (Bifrost.isArray(inputData) && inputData.length > 0) {
            var data = google.visualization.arrayToDataTable(array);
            chart.draw(data, options);
        }
    }
};
