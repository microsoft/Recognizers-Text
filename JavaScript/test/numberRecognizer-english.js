var NumberRecognizer = require('@microsoft/recognizers-text-number');
var Culture = require('@microsoft/recognizers-text-number').Culture;

module.exports = function (describe) {
    describe(`numberRecognizer - english -`, it => {
        it("recognizeNumber", t => {
            var input = "192";

            var actual = NumberRecognizer.recognizeNumber(input, Culture.English);

            t.is(actual[0].text, "192", 'Result.Text');
            t.is(actual[0].typeName, "number", 'Result.TypeName');
            t.is(actual[0].resolution.value, "192", 'Result.Resolution.value');
        });

        it("recognizeOrdinal", t => {
            var input = "a hundred trillionth";

            var actual = NumberRecognizer.recognizeOrdinal(input, Culture.English);

            t.is(actual[0].text, "a hundred trillionth", 'Result.Text');
            t.is(actual[0].typeName, "ordinal", 'Result.TypeName');
            t.is(actual[0].resolution.value, "100000000000000", 'Result.Resolution.value');
        });

        it("recognizePercentage", t => {
            var input = "100%";

            var actual = NumberRecognizer.recognizePercentage(input, Culture.English);

            t.is(actual[0].text, "100%", 'Result.Text');
            t.is(actual[0].typeName, "percentage", 'Result.TypeName');
            t.is(actual[0].resolution.value, "100%", 'Result.Resolution.value');
        });
    });
}