var _ = require('lodash');
var Recognizer = require('@microsoft/recognizers-text-suite');
var SupportedCultures = require('./cultures.js');

var modelFunctions = {
    'PhoneNumberModel': (input, culture, options) => Recognizer.recognizePhoneNumber(input, culture, options),
    'IpAddressModel': (input, culture, options) => Recognizer.recognizeIpAddress(input, culture, options),
    'MentionModel': (input, culture, options) => Recognizer.recognizeMention(input, culture, options),
    'HashtagModel': (input, culture, options) => Recognizer.recognizeHashtag(input, culture, options),
    'EmailModel': (input, culture, options) => Recognizer.recognizeEmail(input, culture, options),
    'URLModel': (input, culture, options) => Recognizer.recognizeURL(input, culture, options),
    'GUIDModel': (input, culture, options) => Recognizer.recognizeGUID(input, culture, options),
};

module.exports = function getSequenceTestRunner(config) {
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
            if(!actual.resolution) return;

            t.deepEqual(actual.resolution, expected.Resolution, 'Result.Resolution');
        });
    };
}

function getResults(input, config) {
    var modelFunction = modelFunctions[config.subType];
    if(!modelFunction) {
        throw new Error(`Sequence model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`Sequence model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return modelFunction(input, culture, 0);
}