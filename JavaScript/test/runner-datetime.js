var SupportedCultures = require('./runner-cultures.js');
var _ = require('lodash');

var Extractors = require('./datetime-extractors');

module.exports = function getDateTimeRunner(config) {
    var extractor = getExtractor(config);
    var parser = getParser(config);
    var model = getModel(config);

    // if(!extractor && !parser && !model) {
    //     return ignoredTest;
    // }

    // Extractor only test
    if (config.subType.includes('Extractor')) {
        if (!extractor) {
            throw new Error(`Cannot found extractor for ${JSON.stringify(config)}. Please verify datetime-extractors.js is properly defined.`);
        }

        return getExtractorTestRunner(extractor, config);
    }

    return ignoredTest;
};

function getExtractorTestRunner(extractor, config) {
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

function getExtractor(config) {
    var extractorName = config.subType.replace('Extractor', '').replace('Base', '');
    var key = [config.language, extractorName].join('-');
    var extractor = Extractors[key];

    return extractor;
}

function getParser() { return null; }
function getModel() { return null; }

function ignoredTest(t, testCase) {
    t.skip.true(true, 'Test case not supported.');
}