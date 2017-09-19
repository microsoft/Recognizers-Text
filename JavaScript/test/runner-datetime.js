var SupportedCultures = require('./runner-cultures.js');
var _ = require('lodash');

var Extractors = require('./datetime-extractors');
var Parsers = require('./datetime-parsers');

module.exports = function getDateTimeRunner(config) {
    var extractor = getExtractor(config);
    var parser = getParser(config);
    var model = getModel(config);

    // Extractor only test
    if (config.subType.includes('Extractor')) {
        if (!extractor) {
            return testError(`Cannot found extractor for ${JSON.stringify(config)}. Please verify datetime-extractors.js is properly defined.`);
        }

        return getExtractorTestRunner(config, extractor);
    }

    // Parser test
    if (config.subType.includes('Parser')) {
        if (!extractor) {
            return testError(`Cannot found extractor for ${JSON.stringify(config)}. Please verify datetime-extractors.js is properly defined.`);
        }

        if (!parser) {
            return testError(`Cannot found parser for ${JSON.stringify(config)}. Please verify datetime-parsers.js is properly defined.`);
        }

        return getParserTestRunner(config, extractor, parser);
    }

    return ignoredTest;
};

function getExtractorTestRunner(config, extractor) {
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

var timezoneOffset = new Date().getTimezoneOffset() * 60 * 1000;

function getParserTestRunner(config, extractor, parser) {
    return function (t, testCase) {
        var expected = testCase.Results;

        // referenceDate parameter
        var referenceDateTime = null;
        if(testCase.Context && testCase.Context.ReferenceDateTime) {
            referenceDateTime = new Date(Date.parse(testCase.Context.ReferenceDateTime));
        }

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

            if(actual.value) {
                t.is(actual.value.timex, expected.Value.Timex, 'Result.Value.Timex');
            }
            /// TODO: check for resolution values
        });
    };
}

function getExtractor(config) {
    var extractorName = config.subType.replace('Extractor', '').replace('Parser', '').replace('Base', '');
    var key = [config.language, extractorName].join('-');
    var extractor = Extractors[key];

    return extractor;
}

function getParser(config) {
    var parserName = config.subType.replace('Parser', '').replace('Base', '');
    var key = [config.language, parserName].join('-');
    var parser = Parsers[key];

    return parser;
}
function getModel() { return null; }

function ignoredTest(t, testCase) {
    t.skip.true(true, 'Test case not supported.');
}

function testError(message) {
    return (t, testCase) => {
        t.fail(message);
    };
}