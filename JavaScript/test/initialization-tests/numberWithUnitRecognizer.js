var Recognizer = require('@microsoft/recognizers-text-number-with-unit');
var NumberWithUnitRecognizer = require('@microsoft/recognizers-text-number-with-unit').NumberWithUnitRecognizer;
var NumberWithUnitOptions = require('@microsoft/recognizers-text-number-with-unit').NumberWithUnitOptions;
var Culture = require('@microsoft/recognizers-text-number').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

var controlModel = new Recognizer.DimensionModel(new Map([[
    new Recognizer.NumberWithUnitExtractor(new Recognizer.EnglishDimensionExtractorConfiguration()), 
    new Recognizer.NumberWithUnitParser(new Recognizer.EnglishDimensionParserConfiguration())]]));

function clearCache() {
    var recognizer = new NumberWithUnitRecognizer();
    Object.getPrototypeOf(recognizer.modelFactory).constructor.cache.clear();
}

function getCache(recognizer) {
    return Object.getPrototypeOf(recognizer.modelFactory).constructor.cache;
}

module.exports = function (describe) {
    describe(`numberWithUnitRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getDimensionModel(), controlModel);
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.notDeepEqual(recognizer.getDimensionModel(SpanishCulture), controlModel);
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getDimensionModel(InvalidCulture), controlModel);
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberWithUnitRecognizer();
            t.throws(() => { recognizer.getDimensionModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new NumberWithUnitRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getDimensionModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new NumberWithUnitRecognizer();
            t.deepEqual(recognizer.getDimensionModel(), controlModel);
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & NumberWithUnitOptions.None) === NumberWithUnitOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new NumberWithUnitRecognizer(InvalidCulture, -1)});
        });
    });
    
    describe(`numberWithUnitRecognizer - cache -`, it => {
        it('WithLazyInitialization_CacheEmpty', t => {
            clearCache();
            var recognizer = new NumberWithUnitRecognizer(lazyInitialization = true);
            t.is(getCache(recognizer).size, 0);
        });

        it('WithoutLazyInitialization_CacheFull', t => {
            clearCache();
            var recognizer = new NumberWithUnitRecognizer(lazyInitialization = false);
            t.not(getCache(recognizer).size, 0);
        });
        
        it('WithoutLazyInitializationAndCulture_CacheWithCulture', t => {
            clearCache();
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, lazyInitialization = false);
            getCache(recognizer).forEach((value, key) => t.is(JSON.parse(key).culture, EnglishCulture));
        });
    });
}