var _ = require('lodash');
var RecognizerTextNumber = require('@microsoft/recognizers-text-number');
var NumberRecognizer = require('@microsoft/recognizers-text-number').NumberRecognizer;
var NumberOptions = require('@microsoft/recognizers-text-number').NumberOptions;
var SupportedCultures = require('./cultures.js');


var recognizer = new NumberRecognizer()
var modelGetters = {
    'NumberModel': recognizer.getNumberModel,
    'OrdinalModel': recognizer.getOrdinalModel,
    'PercentModel': recognizer.getPercentageModel,
    // TODO: Implement number range model in javascript
    'NumberRangeModel': recognizer.getNumberModel,
    'CustomNumberModel': getCustomNumberModel
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

function getCustomNumberModel(culture) {
    switch (culture) {
        case SupportedCultures['Chinese'].cultureCode:
            return new RecognizerTextNumber.NumberModel(
                RecognizerTextNumber.AgnosticNumberParserFactory.getParser(
                    RecognizerTextNumber.AgnosticNumberParserType.Number,
                    new RecognizerTextNumber.ChineseNumberParserConfiguration()),
                new RecognizerTextNumber.ChineseNumberExtractor(RecognizerTextNumber.ChineseNumberMode.ExtractAll)
            );
            break;
    }
    return null;
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

    return getModel.bind(new NumberRecognizer(culture, NumberOptions.None))(culture, false);
}