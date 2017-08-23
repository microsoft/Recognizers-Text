var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/extractorConfiguration').EnglishMergedExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/extractors').BaseMergedExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Merged Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
    
    BasicTest(it, extractor, "this is before 4pm tomorrow", 8, 19);

    BasicTest(it, extractor, "after 7/2 ", 0, 9);
    BasicTest(it, extractor, "since 7/2 ", 0, 9);

    BasicTest(it, extractor, "this is 2 days", 8, 6);

    BasicTest(it, extractor, "this is before 4pm", 8, 10);
    BasicTest(it, extractor, "this is before 4pm tomorrow", 8, 19);
    BasicTest(it, extractor, "this is before tomorrow 4pm ", 8, 19);

    BasicTest(it, extractor, "this is after 4pm", 8, 9);
    BasicTest(it, extractor, "this is after 4pm tomorrow", 8, 18);
    BasicTest(it, extractor, "this is after tomorrow 4pm ", 8, 18);

    //Unit tests for text should not extract datetime
    BasicTestNone(it, extractor, "which email have gotten a reply");
    BasicTestNone(it, extractor, "He is often alone");
    BasicTestNone(it, extractor, "often a bird");
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
    });
}

function BasicTestNone(it, extractor, text) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(0, results.length);
    });
}