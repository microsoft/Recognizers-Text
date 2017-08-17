var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/extractorConfiguration').EnglishDurationExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/extractors').BaseDurationExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Duration Extractor', it => {
    let extractor = new baseExtractor(new configuration());

    BasicTest(it, extractor, "I'll leave for 3h", 15, 2);
    BasicTest(it, extractor, "I'll leave for 3day", 15, 4);
    BasicTest(it, extractor, "I'll leave for 3.5years", 15, 8);

    BasicTest(it, extractor, "I'll leave for 3 h", 15, 3);
    BasicTest(it, extractor, "I'll leave for 3 hours", 15, 7);
    BasicTest(it, extractor, "I'll leave for 3 hrs", 15, 5);
    BasicTest(it, extractor, "I'll leave for 3 hr", 15, 4);
    BasicTest(it, extractor, "I'll leave for 3 day", 15, 5);
    BasicTest(it, extractor, "I'll leave for 3 months", 15, 8);
    BasicTest(it, extractor, "I'll leave for 3 minutes", 15, 9);
    BasicTest(it, extractor, "I'll leave for 3 min", 15, 5);
    BasicTest(it, extractor, "I'll leave for 3.5 second ", 15, 10);
    BasicTest(it, extractor, "I'll leave for 123.45 sec", 15, 10);
    BasicTest(it, extractor, "I'll leave for two weeks", 15, 9);
    BasicTest(it, extractor, "I'll leave for twenty min", 15, 10);
    BasicTest(it, extractor, "I'll leave for twenty and four hours", 15, 21);

    BasicTest(it, extractor, "I'll leave for all day", 15, 7);
    BasicTest(it, extractor, "I'll leave for all week", 15, 8);
    BasicTest(it, extractor, "I'll leave for all month", 15, 9);
    BasicTest(it, extractor, "I'll leave for all year", 15, 8);

    BasicTest(it, extractor, "I'll leave for an hour", 15, 7);
    BasicTest(it, extractor, "I'll leave for a year", 15, 6);

    BasicTest(it, extractor, "half year", 0, 9);
    BasicTest(it, extractor, "half an year", 0, 12);

    BasicTest(it, extractor, "I'll leave for 3-min", 15, 5);
    BasicTest(it, extractor, "I'll leave for 30-minutes", 15, 10);
    BasicTest(it, extractor, "I'll leave for a half hour", 15, 11);
    BasicTest(it, extractor, "I'll leave for half an hour", 15, 12);
    BasicTest(it, extractor, "I'll leave for an hour and half", 15, 16);
    BasicTest(it, extractor, "I'll leave for an hour and a half", 15, 18);
    BasicTest(it, extractor, "I'll leave for an hour and half", 15, 16);
    BasicTest(it, extractor, "I'll leave for half hour", 15, 9);
    
    BasicTest(it, extractor, "I'll leave for two hours", 15, 9);
    BasicTest(it, extractor, "I'll leave for two and a half hours", 15, 20);
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DURATION, results[0].type);
    });
}