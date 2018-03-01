var DateTimeRecognizer = require('@microsoft/recognizers-text-date-time');
var DateTimeOptions = require('@microsoft/recognizers-text-date-time').DateTimeOptions;
var Culture = require('@microsoft/recognizers-text-date-time').Culture;

module.exports = function (describe) {
    describe(`dateTimeRecognizer - english -`, it => {
        it('recognizeDateTime', t => {
            var input = 'I\'ll go back Oct/2';
            var refTime = new Date('2016-11-07T00:00:00')

            var actual = DateTimeRecognizer.recognizeDateTime(input, Culture.English, DateTimeOptions.None, refTime);

            t.is(actual.length, 1, 'Result.length')
            t.is(actual[0].text, 'oct/2', 'Result.Text');
            t.is(actual[0].typeName, 'datetimeV2.date', 'Result.TypeName');
            t.is(actual[0].resolution.values.length, 2, 'Result.Resolution.Values.length')
            t.is(actual[0].resolution.values[0]["timex"], "XXXX-10-02");
            t.is(actual[0].resolution.values[0]["type"], "date");
            t.is(actual[0].resolution.values[0]["value"], "2016-10-02");

            t.is(actual[0].resolution.values[1]["timex"], "XXXX-10-02");
            t.is(actual[0].resolution.values[1]["type"], "date");
            t.is(actual[0].resolution.values[1]["value"], "2017-10-02");
        });
    });
}