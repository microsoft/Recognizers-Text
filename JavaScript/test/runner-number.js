var _ = require('lodash');
var NumberRecognizer = require('recognizers-text-number').NumberRecognizer;
var SupportedCultures = require('./cultures.js');

var modelGetters = {
    'NumberModel': NumberRecognizer.instance.getNumberModel,
    'OrdinalModel': NumberRecognizer.instance.getOrdinalModel,
    'PercentModel': NumberRecognizer.instance.getPercentageModel
};

module.exports = function getNumberTestRunner(config) {
    var model = getNumberModel(config);
    return function (t, testCase) {

        if (testCase.Debug) debugger;
        var result = model.parse(testCase.Input);

        t.is(result.length, testCase.Results.length, 'Result count');
        _.zip(result, testCase.Results).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');
            t.is(actual.resolution.value, expected.Resolution.value, 'Result.Resolution.value');
        });
    };
}

function getNumberModel(config) {
    var getModel = modelGetters[config.subType];
    if(!getModel) {
        throw new Error(`Number model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`Number model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return getModel.bind(NumberRecognizer.instance)(culture, false);
}