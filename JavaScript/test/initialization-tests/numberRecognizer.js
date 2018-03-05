var Recognizer = require('@microsoft/recognizers-text-number');
var NumberRecognizer = require('@microsoft/recognizers-text-number').NumberRecognizer;
var NumberOptions = require('@microsoft/recognizers-text-number').NumberOptions;
var Culture = require('@microsoft/recognizers-text-number').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

var controlModel = new Recognizer.NumberModel(
    Recognizer.AgnosticNumberParserFactory.getParser(Recognizer.AgnosticNumberParserType.Number, new Recognizer.EnglishNumberParserConfiguration()),
    new Recognizer.EnglishNumberExtractor(Recognizer.NumberMode.PureNumber));

function clearCache() {
    var recognizer = new NumberRecognizer();
    Object.getPrototypeOf(recognizer.modelFactory).constructor.cache.clear();
}

function getCache(recognizer) {
    return Object.getPrototypeOf(recognizer.modelFactory).constructor.cache;
}

module.exports = function (describe) {
    describe(`numberRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new NumberRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getNumberModel(), controlModel);
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            var recognizer = new NumberRecognizer(EnglishCulture);
            t.notDeepEqual(recognizer.getNumberModel(SpanishCulture), controlModel);
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new NumberRecognizer(SpanishCulture);
            t.deepEqual(recognizer.getNumberModel(EnglishCulture), controlModel);
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
            t.deepEqual(recognizer.getNumberModel(), controlModel);
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new NumberRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & NumberOptions.None) === NumberOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new NumberRecognizer(InvalidCulture, -1)});
        });
    });
    
    describe(`numberRecognizer - cache -`, it => {
        it('WithLazyInitialization_CacheEmpty', t => {
            clearCache();
            var recognizer = new NumberRecognizer(lazyInitialization = true);
            t.is(getCache(recognizer).size, 0);
        });

        it('WithoutLazyInitialization_CacheFull', t => {
            clearCache();
            var recognizer = new NumberRecognizer(lazyInitialization = false);
            t.not(getCache(recognizer).size, 0);
        });
        
        it('WithoutLazyInitializationAndCulture_CacheWithCulture', t => {
            clearCache();
            var recognizer = new NumberRecognizer(EnglishCulture, lazyInitialization = false);
            getCache(recognizer).forEach((value, key) => t.is(JSON.parse(key).culture, EnglishCulture));
        });
    });
}