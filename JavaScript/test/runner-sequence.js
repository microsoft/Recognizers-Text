var _ = require('lodash');
var SequenceRecognizer = require('@microsoft/recognizers-text-sequence').SequenceRecognizer;
var SupportedCultures = require('./cultures.js');

var modelGetters = {
    'PhoneNumberModel': new SequenceRecognizer().getPhoneNumberModel,
    'IpAddressModel': new SequenceRecognizer().getIpAddressModel
};

module.exports = function getSequenceTestRunner(config) {
    try {
        var model = getSequenceModel(config);
    } catch (err) {
        return null;
    }

    return function (t, testCase) {

        if (testCase.Debug) debugger;
        var result = model.parse(testCase.Input);

        t.is(result.length, testCase.Results.length, 'Result count');
        _.zip(result, testCase.Results).forEach(o => {
            var actual = o[0];
            var expected = o[1];
            t.is(actual.text, expected.Text, 'Result.Text');
            t.is(actual.typeName, expected.TypeName, 'Result.TypeName');

            // Check resolution is defined (or not) in both actual and expected
            t.is(!!actual.resolution, !!expected.Resolution, 'Result.Resolution is defined');
            if(!actual.resolution) return;

            t.deepEqual(actual.resolution, expected.Resolution, 'Result.Resolution');
        });
    };
}

function getSequenceModel(config) {
    var getModel = modelGetters[config.subType];
    if (!getModel) {
        throw new Error(`Sequence model of ${config.subType} not supported.`);
    }

    var culture = SupportedCultures[config.language].cultureCode;
    if (!culture) {
        throw new Error(`Sequence model of ${config.subType} with culture ${config.language} not supported.`);
    }

    return getModel.bind(new SequenceRecognizer(culture))();
}