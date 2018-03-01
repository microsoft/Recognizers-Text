var ChoiceRecognizer = require('@microsoft/recognizers-text-choice');
var ChoiceOptions = require('@microsoft/recognizers-text-choice').ChoiceOptions;
var Culture = require('@microsoft/recognizers-text-choice').Culture;

module.exports = function (describe) {
    describe(`choiceRecognizer - english -`, it => {
        it('choiceRecognizer', t => {
            var input = 'I don\'t thing so. no.';

            var actual = ChoiceRecognizer.recognizeBoolean(input, Culture.English);

            t.is(actual.length, 1, 'Result.length')
            t.is(actual[0].typeName, 'boolean', 'Result.TypeName');
            t.is(actual[0].text, 'no', 'Result.Text');
            t.is(actual[0].resolution.value, false);
            t.is(actual[0].resolution.score, 0.5);
        });
    });
}