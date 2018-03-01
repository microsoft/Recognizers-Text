var NumberWithUnitRecognizer = require('@microsoft/recognizers-text-number-with-unit');
var Culture = require('@microsoft/recognizers-text-number-with-unit').Culture;

module.exports = function (describe) {
    describe(`numberWithUnitRecognizer - english -`, it => {
        it("RecognizeCurrency", t => {
            var input = "montgomery county , md . - - $ 75 million of general obligation , series b , consolidated public improvement bonds of 1989 , through a manufacturers hanover trust co . group .";

            var actual = NumberWithUnitRecognizer.recognizeCurrency(input, Culture.English);

            t.is(actual[0].typeName, "currency", 'Result.TypeName');
            t.is(actual[0].text, "$ 75 million", 'Result.Text');
            t.is(actual[0].resolution.value, "75000000", 'Result.Resolution.value');
            t.is(actual[0].resolution.unit, "Dollar" , 'Result.Resolution.unit');
        });

        it("RecognizeTemperature", t => {
            var input = "the temperature outside is 40 deg celsius";

            var actual = NumberWithUnitRecognizer.recognizeTemperature(input, Culture.English);

            t.is(actual[0].typeName, "temperature", 'Result.TypeName');
            t.is(actual[0].text, "40 deg celsius", 'Result.Text');
            t.is(actual[0].resolution.value, "40", 'Result.Resolution.value');
            t.is(actual[0].resolution.unit, "C", 'Result.Resolution.unit');
        });

        it("RecognizeDimension", t => {
            var input = "75ml";

            var actual = NumberWithUnitRecognizer.recognizeDimension(input, Culture.English);

            t.is(actual[0].typeName, "dimension", 'Result.TypeName');
            t.is(actual[0].text, "75ml", 'Result.Text');
            t.is(actual[0].resolution.value, "75", 'Result.Resolution.value');
            t.is(actual[0].resolution.unit, "Milliliter", 'Result.Resolution.unit');
        });

        it("RecognizeAge", t => {
            var input = "When she was five years old, she learned to ride a bike.";

            var actual = NumberWithUnitRecognizer.recognizeAge(input, Culture.English);

            t.is(actual[0].typeName, "age", 'Result.TypeName');
            t.is(actual[0].text, "five years old", 'Result.Text');
            t.is(actual[0].resolution.value, "5", 'Result.Resolution.value');
            t.is(actual[0].resolution.unit, "Year", 'Result.Resolution.unit');
        });
    });
}