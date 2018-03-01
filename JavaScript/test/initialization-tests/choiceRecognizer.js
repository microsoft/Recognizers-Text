var ChoiceRecognizer = require('@microsoft/recognizers-text-choice').ChoiceRecognizer;
var ChoiceOptions = require('@microsoft/recognizers-text-choice').ChoiceOptions;
var Culture = require('@microsoft/recognizers-text-choice').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

module.exports = function (describe) {
    describe(`choiceRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            t.is(recognizer.getBooleanModel(), recognizer.getBooleanModel(EnglishCulture));
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            t.pass();
            // This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning due to design.
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            t.is(recognizer.getBooleanModel(InvalidCulture), recognizer.getBooleanModel());
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new ChoiceRecognizer();
            t.throws(() => { recognizer.getBooleanModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new ChoiceRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getBooleanModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new ChoiceRecognizer();
            t.is(recognizer.getBooleanModel(), recognizer.getBooleanModel(EnglishCulture));
        });

        it('InitializationNonLazy_CanGetModel', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture, ChoiceOptions.None, false);
            t.is(recognizer.getBooleanModel(), recognizer.getBooleanModel(EnglishCulture));
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & ChoiceOptions.None) === ChoiceOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new ChoiceRecognizer(InvalidCulture, -1)});
        });
    });
}