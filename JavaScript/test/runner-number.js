var _ = require('lodash');
var Recognizer = require('@microsoft/recognizers-text-suite');
var NumberOptions = require('@microsoft/recognizers-text-suite').NumberOptions;
var RecognizerTextNumber = require('@microsoft/recognizers-text-number');
var SupportedCultures = require('./cultures.js');

var modelFunctions = {
    'NumberModel': (input, culture, options) => Recognizer.recognizeNumber(input, culture, options, false),
    'OrdinalModel': (input, culture, options) => Recognizer.recognizeOrdinal(input, culture, options, false),
    'PercentModel': (input, culture, options) => Recognizer.recognizePercentage(input, culture, options, false),
    // TODO: Implement number range model in javascript
    'NumberRangeModel': (input, culture, options) => null
};

module.exports = function getNumberTestRunner(config) {
    return function (t, testCase) {

        if (testCase.Debug) debugger;
        var result = getResults(testCase.Input, config);

        t.is(result.length, testCase.Results.length, 'Result count');
        _.zip(result, testCase.Results).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.start, expected.Start, 'Result.Start');
            t.is(actual.end, expected.End, 'Result.End')
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');
            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
        });
    };
}

function getResults(input, config) {
    var modelFunction = modelFunctions[config.subType];
    if(!modelFunction) {
        throw new Error(`Number model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`Number model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return modelFunction(input, culture, 0);
}