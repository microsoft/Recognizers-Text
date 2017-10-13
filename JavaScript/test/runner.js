var NumberTestRunner = require('./runner-number');
var NumberWithUnitTestRunner = require('./runner-numberWithUnit');
var DateTimeTestRunner = require('./runner-datetime');

module.exports = function (describe, specs) {
    specs.forEach(suite => {
        describe(`${suite.config.type} - ${suite.config.language} - ${suite.config.subType} -`, it => {
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
            throw new Error(`Extractor type unknown: ${JSON.stringify(config)}`);
    }
}