// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

var _ = require('lodash');
var Recognizer = require('@microsoft/recognizers-text-suite');
var SupportedCultures = require('./cultures.js');

var modelFunctions = {
    'BooleanModel': (input, culture, options) => Recognizer.recognizeBoolean(input, culture, options, false)
};

module.exports = function getChoiceTestRunner(config) {
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

            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
            t.is(actual.resolution.score, expected.Resolution.score, 'Result.Resolution.unit');
        });
    };
}

function getResults(input, config) {
    var modelFunction = modelFunctions[config.subType];
    if(!modelFunction) {
        throw new Error(`Choice model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`Choice model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return modelFunction(input, culture, 0);
}