var NumberTestRunner = require('./runner-number');
var NumberWithUnitTestRunner = require('./runner-numberWithUnit');
var DateTimeTestRunner = require('./runner-datetime');
var ChoiceTestRunner = require('./runner-choice');
var SequenceTestRunner = require('./runner-sequence');

module.exports = function (describe, specs) {
    specs.forEach(suite => {
        describe(`${suite.config.type} - ${suite.config.language} - ${suite.config.subType} -`, it => {
            suite.specs.forEach(testCase => {
                var caseName = `"${testCase.Input}"`;
                if(suite.config.type === "DateTime" && testCase.Context){
                    caseName += ` - "${testCase.Context.ReferenceDateTime}"`;
                }   

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
        case 'Choice':
            return ChoiceTestRunner(config);
        case 'Sequence':
            return SequenceTestRunner(config);
        default:
            throw new Error(`Recognizer type unknown: ${JSON.stringify(config)}`);
    }
}