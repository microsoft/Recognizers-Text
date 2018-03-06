var Recognizer = require('@microsoft/recognizers-text-choice');
var ChoiceRecognizer = require('@microsoft/recognizers-text-choice').ChoiceRecognizer;
var ChoiceOptions = require('@microsoft/recognizers-text-choice').ChoiceOptions;
var Culture = require('@microsoft/recognizers-text-choice').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

var controlModel = new Recognizer.BooleanModel(
    new Recognizer.BooleanParser(),
    new Recognizer.BooleanExtractor(new Recognizer.EnglishBooleanExtractorConfiguration()));

function clearCache() {
    var recognizer = new ChoiceRecognizer();
    Object.getPrototypeOf(recognizer.modelFactory).constructor.cache.clear();
}

function getCache(recognizer) {
    return Object.getPrototypeOf(recognizer.modelFactory).constructor.cache;
}

module.exports = function (describe) {
    describe(`choiceRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getBooleanModel(), controlModel);
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            t.pass();
            // This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning due to design.
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getBooleanModel(InvalidCulture), controlModel);
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
            t.deepEqual(recognizer.getBooleanModel(), controlModel);
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new ChoiceRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & ChoiceOptions.None) === ChoiceOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new ChoiceRecognizer(InvalidCulture, -1)});
        });
    });

    describe(`choiceRecognizer - cache -`, it => {
        it('WithLazyInitialization_CacheEmpty', t => {
            clearCache();
            var recognizer = new ChoiceRecognizer(lazyInitialization = true);
            t.is(getCache(recognizer).size, 0);
        });

        it('WithoutLazyInitialization_CacheFull', t => {
            clearCache();
            var recognizer = new ChoiceRecognizer(lazyInitialization = false);
            t.not(getCache(recognizer).size, 0);
        });
        
        it('WithoutLazyInitializationAndCulture_CacheWithCulture', t => {
            clearCache();
            var recognizer = new ChoiceRecognizer(EnglishCulture, lazyInitialization = false);
            getCache(recognizer).forEach((value, key) => t.is(JSON.parse(key).culture, EnglishCulture));
        });
    });
}