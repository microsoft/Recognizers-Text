var _ = require('lodash');
var NumberWithUnitRecognizer = require('recognizers-text-number-with-unit').NumberWithUnitRecognizer;
var SupportedCultures = require('./cultures.js');

var modelGetters = {
    'AgeModel': NumberWithUnitRecognizer.instance.getAgeModel,
    'CurrencyModel': NumberWithUnitRecognizer.instance.getCurrencyModel,
    'TemperatureModel': NumberWithUnitRecognizer.instance.getTemperatureModel,
    'DimensionModel': NumberWithUnitRecognizer.instance.getDimensionModel
};

module.exports = function getNumberWithUnitTestRunner(config) {
    try {
        var model = getNumberWithUnitModel(config);
    } catch (err) {
        return null;
    }

    return function (t, testCase) {

        if (testCase.Debug) debugger;
        var result = model.parse(testCase.Input);

        t.is(result.length, testCase.Results.length, 'Result count');
        _.zip(result, testCase.Results).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');

            // Check resolution is defined (or not) in both actual and expected
            t.is(!!actual.resolution, !!expected.Resolution, 'Result.Resolution is defined');
            if(!actual.resolution) return;

            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
            t.is(actual.resolution.unit, expected.Resolution.unit, 'Result.Resolution.unit');
        });
    };
}

function getNumberWithUnitModel(config) {
    var getModel = modelGetters[config.subType];
    if (!getModel) {
        throw new Error(`NumberWithUnit model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`NumberWithUnit model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return getModel.bind(NumberWithUnitRecognizer.instance)(culture, false);
}