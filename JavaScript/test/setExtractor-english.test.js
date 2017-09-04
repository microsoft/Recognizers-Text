var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/setConfiguration').EnglishSetExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/baseSet').BaseSetExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Set Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
    BasicTest(it, extractor, "I'll leave weekly", 11, 6);
    BasicTest(it, extractor, "I'll leave daily", 11, 5);
    BasicTest(it, extractor, "I'll leave every day", 11, 9);
    BasicTest(it, extractor, "I'll leave each month", 11, 10);
    BasicTest(it, extractor, "I'll leave annually", 11, 8);
    BasicTest(it, extractor, "I'll leave annual", 11, 6);

    BasicTest(it, extractor, "I'll leave each two days", 11, 13);
    BasicTest(it, extractor, "I'll leave every three week", 11, 16);

    BasicTest(it, extractor, "I'll leave 3pm every day", 11, 13);
    BasicTest(it, extractor, "I'll leave 3pm each day", 11, 12);

    BasicTest(it, extractor, "I'll leave each 4/15", 11, 9);
    BasicTest(it, extractor, "I'll leave every monday", 11, 12);
    BasicTest(it, extractor, "I'll leave each monday 4pm", 11, 15);

    BasicTest(it, extractor, "I'll leave every morning", 11, 13);
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_SET, results[0].type);
    });
}