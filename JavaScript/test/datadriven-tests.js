var specsPath = '../Specs';
var supportedLanguages = ['Eng', 'Spa'];

var fs = require('fs');
var path = require('path');
var describe = require('ava-spec').describe;

// get list of specs (.json)
var specFiles = getSpecFilePaths(specsPath)
    // Ignore non-supported languages
    .filter(s => supportedLanguages.find(l => s.indexOf(path.sep + l + path.sep) !== -1));

// parse specs
var specs = specFiles
    .map(s => ({
        config: getSuiteConfig(s),
        specs: require(path.join('../', s))
    }));

// console.log(specs)
// run suites
specs.forEach(suite => {
    describe(`${suite.config.language} - ${suite.config.type} - ${suite.config.subType} - `, it => {
        suite.specs.forEach(testCase => {
            var caseName = `"${testCase.Input}"`;

            var notSupported = (testCase.NotSupported || '').split(',').map(s => s.trim());
            var notSupportedByDesign = (testCase.NotSupportedByDesign || '').split(',').map(s => s.trim());

            if (notSupportedByDesign.includes('javascript')) {
                // Not Supported by Design - right now we don't care about implementing it
                return;
            }

            var testRunner = getTestRunner(suite.config);
            if (!testRunner || notSupported.includes('javascript')) {
                // test case or type not supported
                it.skip(caseName, t => { });
                return;
            }

            it(caseName, t => testRunner(t, testCase));
        });
    });
});

function getSpecFilePaths(specsPath) {
    return fs
        .readdirSync(specsPath).map(s => path.join(specsPath, s))
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .filter(s => s.indexOf('.json') !== -1);
}

function getSuiteConfig(jsonPath) {
    var parts = jsonPath.split(path.sep).slice(2);

    return {
        type: parts[0],
        subType: parts[2].split('.json')[0],
        language: parts[1]
    };
}

function getTestRunner(config) {
    switch (config.type) {
        case 'Number':
            return getNumberTestRunner(config);
        case 'NumberWithUnit':
            return getNumberWithUnitTestRunner(config);
        default:
            return null;
    }
}

var Culture = require('../compiled/culture').Culture;
var Cultures = {
    'Eng': Culture.English,
    'Spa': Culture.Spanish
};

// Number
var NumberRecognizer = require('../compiled/number/numberRecognizer').default;
function getNumberTestRunner(config) {
    return function (t, testCase) {
        var expected = testCase.Results;

        if (testCase.Debug) {
            debugger;
        }

        var model = getNumberModel(config);
        var result = model.parse(testCase.Input);

        t.is(result.length, expected.length, 'Result count');
        result.forEach((r, ix) => {
            var e = expected[ix];
            t.is(r.text, e.Text, 'Result.Text');
            t.is(r.typeName, e.TypeName, 'Result.TypeName');
            t.is(r.resolution.value, e.Resolution.value, 'Result.Resolution.value');
        });
    };
}

function getNumberModel(config) {
    var getModelFunc = getNumberModelFunc(config).bind(NumberRecognizer.instance);
    var culture = Cultures[config.language];
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

// NumberWithUnits
var NumberWithUnitRecognizer = require('../compiled/numberWithUnit/numberWithUnitRecognizer').default;
function getNumberWithUnitTestRunner(config) {
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
    var culture = Cultures[config.language];
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