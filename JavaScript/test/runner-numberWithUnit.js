var NumberWithUnitRecognizer = require('../compiled/numberWithUnit/numberWithUnitRecognizer').default;
var SupportedCultures = require('./runner-cultures.js');

module.exports = function getNumberWithUnitTestRunner(config) {
    return function (t, testCase) {
        var expected = testCase.Results;

        if (testCase.Debug) {
            debugger;
        }

        var model = getNumberWithUnitModel(config);
        var result = model.parse(testCase.Input);

        t.is(result.length, expected.length, 'Result count');
        result.forEach((r, ix) => {
            var e = expected[ix];
            t.is(r.text, e.Text, 'Result.Text');
            t.is(r.typeName, e.TypeName, 'Result.TypeName');
            t.is(r.resolution.value, e.Resolution.value, 'Result.Resolution.value');
            t.is(r.resolution.unit, e.Resolution.unit, 'Result.Resolution.unit');
        });
    };
}

function getNumberWithUnitModel(config) {
    var getModelFunc = getNumberWithUnitModelFunc(config).bind(NumberWithUnitRecognizer.instance);
    var culture = SupportedCultures[config.language];
    if (!culture) {
        throw new Error(`NumberWithUnit model with culture ${config.language} is not supported.`);
    }

    return getModelFunc(culture, false);
}

function getNumberWithUnitModelFunc(config) {
    switch (config.subType) {
        case 'AgeModel':
            return NumberWithUnitRecognizer.instance.getAgeModel;
        case 'CurrencyModel':
            return NumberWithUnitRecognizer.instance.getCurrencyModel;
        case 'TemperatureModel':
            return NumberWithUnitRecognizer.instance.getTemperatureModel;
        case 'DimensionModel':
            return NumberWithUnitRecognizer.instance.getDimensionModel;
        default:
            throw new Error(`NumberWithUnit model ${config.subType} is not supported.`);
    }
}