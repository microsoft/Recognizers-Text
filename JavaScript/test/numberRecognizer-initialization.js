var NumberRecognizer = require('@microsoft/recognizers-text-number').NumberRecognizer;
var NumberOptions = require('@microsoft/recognizers-text-number').NumberOptions;
var Culture = require('@microsoft/recognizers-text-number').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

module.exports = function (describe) {
    describe(`numberRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new NumberRecognizer(EnglishCulture);
            t.is(recognizer.getNumberModel(), recognizer.getNumberModel(EnglishCulture));
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new NumberRecognizer(EnglishCulture);
            t.not(recognizer.getNumberModel(SpanishCulture), recognizer.getNumberModel());
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new NumberRecognizer(EnglishCulture);
            t.is(recognizer.getNumberModel(InvalidCulture), recognizer.getNumberModel());
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberRecognizer();
            t.throws(() => { recognizer.getNumberModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getNumberModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new NumberRecognizer();
            t.is(recognizer.getNumberModel(), recognizer.getNumberModel(EnglishCulture));
        });

        it('InitializationNonLazy_CanGetModel', t => {
            var recognizer = new NumberRecognizer(EnglishCulture, NumberOptions.None, false);
            t.is(recognizer.getNumberModel(), recognizer.getNumberModel(EnglishCulture));
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new NumberRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & NumberOptions.None) === NumberOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new NumberRecognizer(InvalidCulture, -1)});
        });
    });
}