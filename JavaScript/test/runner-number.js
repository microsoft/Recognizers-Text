var _ = require('lodash');
var NumberRecognizer = require('../compiled/number/numberRecognizer').default;
var SupportedCultures = require('./runner-cultures.js');

module.exports = function getNumberTestRunner(config) {
    return function (t, testCase) {
        var expected = testCase.Results;

        if (testCase.Debug) {
            debugger;
        }

        var model = getNumberModel(config);
        var result = model.parse(testCase.Input);

        t.is(result.length, expected.length, 'Result count');
        _.zip(result, expected).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');
            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
        });
    };
}

function getNumberModel(config) {
    var getModelFunc = getNumberModelFunc(config).bind(NumberRecognizer.instance);
    var culture = SupportedCultures[config.language];
    if (!culture) {
        throw new Error(`Number model with culture ${config.language} is not supported.`);
    }

    return getModelFunc(culture, false);
}

function getNumberModelFunc(config) {
    switch (config.subType) {
        case 'NumberModel':
            return NumberRecognizer.instance.getNumberModel;
        case 'OrdinalModel':
            return NumberRecognizer.instance.getOrdinalModel;
        case 'PercentModel':
            return NumberRecognizer.instance.getPercentageModel;
        default:
            throw new Error(`Number model ${config.subType} is not supported.`);
    }
}
