var fs = require('fs');
var path = require('path');
var _ = require('lodash');
var describe = require('ava-spec').describe;

var SupportedCultures = require('./cultures');
var NumberTestRunner = require('./runner-number');
var NumberWithUnitTestRunner = require('./runner-numberWithUnit');
var DateTimeTestRunner = require('./runner-datetime');

var specsPath = '../Specs';
var supportedLanguages = _.keys(SupportedCultures);

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

// run suites
specs.forEach(suite => {
    describe(`${suite.config.language} - ${suite.config.type} - ${suite.config.subType} -`, it => {
        suite.specs.forEach(testCase => {
            var caseName = `"${testCase.Input}"`;

            // Not Supported by Design - right now we don't care about implementing it
            var notSupportedByDesign = (testCase.NotSupportedByDesign || '').split(',').map(s => s.trim());
            if (notSupportedByDesign.includes('javascript')) {
                return;
            }

            var notSupported = (testCase.NotSupported || '').split(',').map(s => s.trim());
            var testRunner = getTestRunner(suite.config);
            if (!testRunner || notSupported.includes('javascript')) {
                // test case or type not supported
                it.skip(caseName, t => { });
                return;
            }

            // Run test
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
            return NumberTestRunner(config);
        case 'NumberWithUnit':
            return NumberWithUnitTestRunner(config);
        case 'DateTime':
            return DateTimeTestRunner(config);
        default:
            return null;
    }
}