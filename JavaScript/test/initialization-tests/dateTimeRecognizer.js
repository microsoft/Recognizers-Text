var Recognizer = require('@microsoft/recognizers-text-date-time');
var DateTimeRecognizer = require('@microsoft/recognizers-text-date-time').DateTimeRecognizer;
var DateTimeOptions = require('@microsoft/recognizers-text-date-time').DateTimeOptions;
var Culture = require('@microsoft/recognizers-text-date-time').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

var controlModel = new Recognizer.DateTimeModel(
    new Recognizer.BaseMergedParser(new Recognizer.EnglishMergedParserConfiguration(new Recognizer.EnglishCommonDateTimeParserConfiguration()), DateTimeOptions.None),
    new Recognizer.BaseMergedExtractor(new Recognizer.EnglishMergedExtractorConfiguration(), DateTimeOptions.None));

function clearCache() {
    var recognizer = new DateTimeRecognizer();
    Object.getPrototypeOf(recognizer.modelFactory).constructor.cache.clear();
}

function getCache(recognizer) {
    return Object.getPrototypeOf(recognizer.modelFactory).constructor.cache;
}

module.exports = function (describe) {
    describe(`dateTimeRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getDateTimeModel(), controlModel);
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.notDeepEqual(recognizer.getDateTimeModel(SpanishCulture), controlModel);
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getDateTimeModel(InvalidCulture), controlModel);
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
            t.deepEqual(recognizer.getDateTimeModel(), controlModel);
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
    
    describe(`dateTimeRecognizer - cache -`, it => {
        it('WithLazyInitialization_CacheEmpty', t => {
            clearCache();
            var recognizer = new DateTimeRecognizer(lazyInitialization = true);
            t.is(getCache(recognizer).size, 0);
        });

        it('WithoutLazyInitialization_CacheFull', t => {
            clearCache();
            var recognizer = new DateTimeRecognizer(lazyInitialization = false);
            t.not(getCache(recognizer).size, 0);
        });
        
        it('WithoutLazyInitializationAndCulture_CacheWithCulture', t => {
            clearCache();
            var recognizer = new DateTimeRecognizer(EnglishCulture, lazyInitialization = false);
            getCache(recognizer).forEach((value, key) => t.is(JSON.parse(key).culture, EnglishCulture));
        });
    });
}