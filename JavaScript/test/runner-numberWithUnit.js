var _ = require('lodash');
var NumberWithUnitRecognizer = require('../compiled/numberWithUnit/numberWithUnitRecognizer').default;
var SupportedCultures = require('./runner-cultures.js');

module.exports = function getNumberWithUnitTestRunner(config) {
    return function (t, testCase) {
        var expected = testCase.Results;

        if (testCase.Debug) {
            debugger;
        }

        var model = getNumberWithUnitModel(config);
        var result = model.parse(testCase.Input);

        t.is(result.length, expected.length, 'Result count');
        _.zip(result, expected).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');
            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
            t.is(actual.resolution.unit, expected.Resolution.unit, 'Result.Resolution.unit');
        });
    };
}

function getNumberWithUnitModel(config) {
    var getModelFunc = getNumberWithUnitModelFunc(config).bind(NumberWithUnitRecognizer.instance);
    var culture = SupportedCultures[config.language];
    if (!culture) {
        throw new Error(`NumberWithUnit model with culture ${config.language} is not supported.`);
    }

    return getModelFunc(culture, false);
}

function getNumberWithUnitModelFunc(config) {
    switch (config.subType) {
        case 'AgeModel':
            return NumberWithUnitRecognizer.instance.getAgeModel;
        case 'CurrencyModel':
            return NumberWithUnitRecognizer.instance.getCurrencyModel;
        case 'TemperatureModel':
            return NumberWithUnitRecognizer.instance.getTemperatureModel;
        case 'DimensionModel':
            return NumberWithUnitRecognizer.instance.getDimensionModel;
        default:
            throw new Error(`NumberWithUnit model ${config.subType} is not supported.`);
    }
}