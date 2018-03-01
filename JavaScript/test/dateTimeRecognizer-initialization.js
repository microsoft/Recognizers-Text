var DateTimeRecognizer = require('@microsoft/recognizers-text-date-time').DateTimeRecognizer;
var DateTimeOptions = require('@microsoft/recognizers-text-date-time').DateTimeOptions;
var Culture = require('@microsoft/recognizers-text-date-time').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

module.exports = function (describe) {
    describe(`dateTimeRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.is(recognizer.getDateTimeModel(), recognizer.getDateTimeModel(EnglishCulture));
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.not(recognizer.getDateTimeModel(SpanishCulture), recognizer.getDateTimeModel());
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.is(recognizer.getDateTimeModel(InvalidCulture), recognizer.getDateTimeModel());
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new DateTimeRecognizer();
            t.throws(() => { recognizer.getDateTimeModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new DateTimeRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getDateTimeModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new DateTimeRecognizer();
            t.is(recognizer.getDateTimeModel(), recognizer.getDateTimeModel(EnglishCulture));
        });

        it('InitializationNonLazy_CanGetModel', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture, DateTimeOptions.None, false);
            t.is(recognizer.getDateTimeModel(), recognizer.getDateTimeModel(EnglishCulture));
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture, 5);
            t.true((recognizer.Options & DateTimeOptions.SkipFromToMerge) === DateTimeOptions.SkipFromToMerge);
            t.true((recognizer.Options & DateTimeOptions.Calendar) === DateTimeOptions.Calendar);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new DateTimeRecognizer(InvalidCulture, -1)});
        });
    });
}