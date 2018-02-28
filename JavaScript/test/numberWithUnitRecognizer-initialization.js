var NumberWithUnitRecognizer = require('@microsoft/recognizers-text-number-with-unit').NumberWithUnitRecognizer;
var NumberWithUnitOptions = require('@microsoft/recognizers-text-number-with-unit').NumberWithUnitOptions;
var Culture = require('@microsoft/recognizers-text-number').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

module.exports = function (describe) {
    describe(`numberWithUnitRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.is(recognizer.getCurrencyModel(), recognizer.getCurrencyModel(EnglishCulture));
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.not(recognizer.getCurrencyModel(SpanishCulture), recognizer.getCurrencyModel());
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.is(recognizer.getCurrencyModel(InvalidCulture), recognizer.getCurrencyModel());
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberWithUnitRecognizer();
            t.throws(() => { recognizer.getCurrencyModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberWithUnitRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getCurrencyModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new NumberWithUnitRecognizer();
            t.is(recognizer.getCurrencyModel(), recognizer.getCurrencyModel(EnglishCulture));
        });

        it('InitializationNonLazy_CanGetModel', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, NumberWithUnitOptions.None, false);
            t.is(recognizer.getCurrencyModel(), recognizer.getCurrencyModel(EnglishCulture));
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & NumberWithUnitOptions.None) === NumberWithUnitOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new NumberWithUnitRecognizer(InvalidCulture, -1)});
        });
    });
}