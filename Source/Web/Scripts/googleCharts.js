google.load('visualization', '1.0', { 'packages': ['corechart'] });

ko.bindingHandlers.lineChart = {
    init: function (element, valueAccessor, allValueAccessors, viewModel, bindingContext) {
        optionsInput = valueAccessor();

        var options = {
            title: optionsInput.title,
            width: optionsInput.width || 300,
            height: optionsInput.height || 300,
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

        /*
        var dataHash = {};

        
        var data = new google.visualization.DataTable();

        options.lines.forEach(function (line) {
            data.addColumn(line.x.type, line.x.name);
            data.addColumn(line.y.type, line.y.name);
        });

        function addRow(row, rowIndex) {
            var value = row[1];
            if (ko.isObservable(value)) {
                value.subscribe(function (newValue) {
                    data.setValue(rowIndex, 1, newValue);
                    chart.draw(data, options);
                });
            }

            var actualValue = ko.unwrap(value);
            data.addRow([row[0], actualValue]);

            dataHash[row[0]] = actualValue;
        };

        optionsInput.data().forEach(addRow);

        optionsInput.data.subscribe(function (newValue) {
            newValue.forEach(function (row, rowIndex) {
                if (!dataHash.hasOwnProperty(row[0])) {
                    addRow(row, rowIndex);
                }
            });

            chart.draw(data, options);
        });*/

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
                row.push(item[serie]);
            }
            array.push(row);
        });
        
        var data = google.visualization.arrayToDataTable(array);
        /*
        var data = google.visualization.arrayToDataTable([
          ['Year', 'Sales', 'Expenses'],
          ['2004', 1000, 400],
          ['2005', 1170, 460],
          ['2006', 660, 1120],
          ['2007', 1030, 540]
        ]);
        */

        chart.draw(data, options);
    },
    update: function (element, valueAccessor, allValueAccessors, viewModel, bindingContext) {
    }
};


//google.setOnLoadCallback(init);

ko.bindingHandlers.columnChart = {
    init: function (element, valueAccessor, allValueAccessors, viewModel, bindingContext) {
        optionsInput = valueAccessor();

        var options = {
            title: optionsInput.title,
            hAxis: { title: 'Year', titleTextStyle: { color: 'red' } },
            width: optionsInput.width || 300,
            height: optionsInput.height || 300,
            backgroundColor: 'transparent',
            animation: {
                duration: 1000,
                easing: 'out'
            }
        };

        var data = google.visualization.arrayToDataTable([
            ['Year', 'Sales', 'Expenses', 'Horse'],
            ['2004', 1000, 400, 2],
            ['2005', 1170, 460, 300],
            ['2006', 660, 1120, 100],
            ['2007', 1030, 540, 500]
        ]);

        var chart = new google.visualization.ColumnChart(element);
        chart.draw(data, options);
    },
    update: function () {
    }
};
