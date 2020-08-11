var Recognizer = require('@microsoft/recognizers-text-sequence');
var SequenceRecognizer = require('@microsoft/recognizers-text-sequence').SequenceRecognizer;
var SequenceOptions = require('@microsoft/recognizers-text-sequence').SequenceOptions;
var Culture = require('@microsoft/recognizers-text-sequence').Culture;

var EnglishCulture = Culture.English;
var SpanishCulture = Culture.Spanish;
var InvalidCulture = "vo-id";

var controlModel = new Recognizer.PhoneNumberModel(
    new Recognizer.PhoneNumberParser(),
    new Recognizer.BasePhoneNumberExtractor(new Recognizer.EnglishPhoneNumberExtractorConfiguration()));

function clearCache() {
    var recognizer = new SequenceRecognizer();
    Object.getPrototypeOf(recognizer.modelFactory).constructor.cache.clear();
}

function getCache(recognizer) {
    return Object.getPrototypeOf(recognizer.modelFactory).constructor.cache;
}

module.exports = function (describe) {
    describe(`sequenceRecognizer - initialization -`, it => {
        it('WithoutCulture_UseTargetCulture', t => {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getPhoneNumberModel(), controlModel);
        });

        it('WithOtherCulture_NotUseTargetCulture', t => {
            t.pass();
            // This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning due to design.
        });

        it('WithInvalidCulture_UseTargetCulture', t => {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            t.deepEqual(recognizer.getPhoneNumberModel(InvalidCulture), controlModel);
        });

        it('WithInvalidCultureAndWithoutFallback_ThrowError', t => {
            var recognizer = new SequenceRecognizer();
            t.throws(() => { recognizer.getPhoneNumberModel(InvalidCulture, false) });
        });

        it('WithInvalidCultureAsTargetAndWithoutFallback_ThrowError', t => {
            var recognizer = new SequenceRecognizer(InvalidCulture);
            t.throws(() => { recognizer.getPhoneNumberModel(null, false) });
        });

        it('WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture', t => {
            var recognizer = new SequenceRecognizer();
            t.deepEqual(recognizer.getPhoneNumberModel(), controlModel);
        });

        it('InitializationWithIntOption_ResolveOptionsEnum', t => {
            var recognizer = new SequenceRecognizer(EnglishCulture, 0);
            t.true((recognizer.Options & SequenceOptions.None) === SequenceOptions.None);
        });

        it('InitializationWithInvalidOptions_ThrowError', t => {
            t.throws(() => { new SequenceRecognizer(InvalidCulture, -1)});
        });
    });
    
    describe(`sequenceRecognizer - cache -`, it => {
        it('WithLazyInitialization_CacheEmpty', t => {
            clearCache();
            var recognizer = new SequenceRecognizer(lazyInitialization = true);
            t.is(getCache(recognizer).size, 0);
        });

        it('WithoutLazyInitialization_CacheFull', t => {
            clearCache();
            var recognizer = new SequenceRecognizer(lazyInitialization = false);
            t.not(getCache(recognizer).size, 0);
        });
        
        it('WithoutLazyInitializationAndCulture_CacheWithCulture', t => {
            clearCache();
            var recognizer = new SequenceRecognizer(EnglishCulture, lazyInitialization = false);
            getCache(recognizer).forEach((value, key) => t.is(JSON.parse(key).culture, EnglishCulture));
        });
    });
}