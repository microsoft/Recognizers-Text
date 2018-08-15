var _ = require('lodash');
var Recognizer = require('@microsoft/recognizers-text-suite');
var SupportedCultures = require('./cultures.js');

var modelFunctions = {
    'AgeModel': (input, culture, options) => Recognizer.recognizeAge(input, culture, options, false),
    'CurrencyModel': (input, culture, options) => Recognizer.recognizeCurrency(input, culture, options, false),
    'TemperatureModel': (input, culture, options) => Recognizer.recognizeTemperature(input, culture, options, false),
    'DimensionModel': (input, culture, options) => Recognizer.recognizeDimension(input, culture, options, false)
};

module.exports = function getNumberWithUnitTestRunner(config) {
    return function (t, testCase) {

        if (testCase.Debug) debugger;
        var result = getResults(testCase.Input, config);

        t.is(result.length, testCase.Results.length, 'Result count');
        _.zip(result, testCase.Results).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');

            // Check resolution is defined (or not) in both actual and expected
            t.is(!!actual.resolution, !!expected.Resolution, 'Result.Resolution is defined');
            if(!expected.Resolution) return;

            simpleAssert(t, actual, expected, 'value');
            simpleAssert(t, actual, expected, 'unit');
            simpleAssert(t, actual, expected, 'isoCurrency');
        });
    };
}

function simpleAssert(t, actual, expected, prop) {
    if (expected.Resolution[prop]) {
        t.is(actual.resolution[prop], expected.Resolution[prop], 'Result.Resolution.' + prop);
    }
}

function getResults(input, config) {
    var modelFunction = modelFunctions[config.subType];
    if(!modelFunction) {
        throw new Error(`NumberWithUnit model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`NumberWithUnit model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return modelFunction(input, culture, 0);
}