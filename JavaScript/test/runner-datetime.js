var Recognizer = require('recognizers-text-datetime').DateTimeRecognizer;
var DateUtils = require('recognizers-text-datetime').DateUtils;
var _ = require('lodash');

var Constants = require('./constants');
var SupportedCultures = require('./cultures');
var Extractors = require('./datetime-extractors');
var Parsers = require('./datetime-parsers');

module.exports = function getDateTimeRunner(config) {
    var extractor = getExtractor(config);
    var parser = getParser(config);
    var model = getModel(config);

    // Extractor only test
    if (config.subType.includes(Constants.Extractor)) {
        if (!extractor) {
            return ignoredTest;
            // throw new Error(`Cannot found extractor for ${JSON.stringify(config)}. Please verify datetime-extractors.js is properly defined.`);
        }

        return getExtractorTestRunner(extractor);
    }

    // Parser test
    if (config.subType.includes(Constants.Parser)) {
        if (!extractor) {
            return ignoredTest;
            // throw new Error(`Cannot found extractor for ${JSON.stringify(config)}. Please verify datetime-extractors.js is properly defined.`);
        }

        if (!parser) {
            return ignoredTest;
            // throw new Error(`Cannot found parser for ${JSON.stringify(config)}. Please verify datetime-parsers.js is properly defined.`);
        }

        return getParserTestRunner(extractor, parser);
    }

    // Model test
    if (config.subType.includes(Constants.Model)) {
        if (!model) {
            return ignoredTest;
            // throw new Error(`Cannot found model for ${JSON.stringify(config)}.`);
        }

        return getModelTestRunner(model);
    }

    return ignoredTest;
};

function getExtractorTestRunner(extractor) {
    return function (t, testCase) {
        var expected = testCase.Results;

        if (testCase.Debug) {
            debugger;
        }

        var result = extractor.extract(testCase.Input);

        t.is(result.length, expected.length, 'Result count');
        _.zip(result, expected).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');
        });
    };
}

function getParserTestRunner(extractor, parser) {
    return function (t, testCase) {
        var expected = testCase.Results;
        var referenceDateTime = getReferenceDate(testCase);

        if (testCase.Debug) {
            debugger;
        }

        var extractResults = extractor.extract(testCase.Input);
        var result = extractResults.map(o => parser.parse(o, referenceDateTime));

        t.is(result.length, expected.length, 'Result count');
        _.zip(result, expected).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');

            if (actual.value) {
                // timex
                t.is(actual.value.timex, expected.Value.Timex, 'Result.Value.Timex');

                // resolutions
                var actualValue = {
                    timex: actual.value.timex,
                    futureResolution: toObject(actual.value.futureResolution),
                    pastResolution: toObject(actual.value.pastResolution),
                }

                t.deepEqual(actualValue.futureResolution, expected.Value.FutureResolution);
                t.deepEqual(actualValue.pastResolution, expected.Value.PastResolution);
            }
        });
    };
}

function getModelTestRunner(model) {
    return function (t, testCase) {
        var expected = testCase.Results;
        var referenceDateTime = getReferenceDate(testCase);

        if (testCase.Debug) {
            debugger;
        }

        var result = model.parse(testCase.Input, referenceDateTime);
        t.is(result.length, expected.length, 'Result count');
        _.zip(result, expected).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');

            if (actual.resolution) {
                var values = actual.resolution.get('values').map(toObject);
                t.is(values.length, expected.Resolution.values.length, 'Resolution.Values count');
                t.deepEqual(values, expected.Resolution.values, 'Resource.Value');
            }
        });
    };
}

function getExtractor(config) {
    var extractorName = config.subType.replace(Constants.Extractor, '').replace(Constants.Parser, '').replace(Constants.Base, '');
    var key = [config.language, extractorName].join('-');
    var extractor = Extractors[key];

    return extractor;
}

function getParser(config) {
    var parserName = config.subType.replace(Constants.Parser, '').replace(Constants.Base, '');
    var key = [config.language, parserName].join('-');
    var parser = Parsers[key];

    return parser;
}
function getModel(config) {
    return Recognizer.instance.getDateTimeModel(SupportedCultures[config.language].cultureCode);
}

function getReferenceDate(testCase) {
    if (testCase.Context && testCase.Context.ReferenceDateTime) {
        return new Date(Date.parse(testCase.Context.ReferenceDateTime));
    }

    return null;
}

function ignoredTest(t, testCase) {
    t.skip.true(true, 'Test case not supported.');
}

function toObject(map) {
    if (!map) return undefined;
    var keys = Array.from(map.keys());
    var values = Array.from(map.values()).map(asString);
    return _.zipObject(keys, values);
}

function asString(o) {
    if (!o) return o;

    if (_.isNumber(o)) {
        return o.toString();
    }

    if (_.isDate(o)) {
        var isoDate = new Date(o.getTime() - o.getTimezoneOffset() * 60000).toISOString();
        var parts = isoDate.split('T');
        var time = parts[1].split('.')[0].replace('00:00:00', '');
        return [parts[0], time].join(' ').trim();
    }


    // JS min Date is 1901, while .NET is 0001
    if (o === '1901-01-01') {
        return '0001-01-01';
    }

    return o;
}