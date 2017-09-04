var describe = require('ava-spec').describe;
var EnglishDateExtractorConfiguration = require('../compiled/dateTime/english/dateConfiguration').EnglishDateExtractorConfiguration;
var BaseDateExtractor = require('../compiled/dateTime/baseDate').BaseDateExtractor;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Extractor', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    BasicTest(it, extractor, "I'll go back on 15", 16, 2);
    BasicTest(it, extractor, "I'll go back April 22", 13, 8);
    BasicTest(it, extractor, "I'll go back Jan-1", 13, 5);
    BasicTest(it, extractor, "I'll go back Jan/1", 13, 5);
    BasicTest(it, extractor, "I'll go back October. 2", 13, 10);
    BasicTest(it, extractor, "I'll go back January 12, 2016", 13, 16);
    BasicTest(it, extractor, "I'll go back January 12 of 2016", 13, 18);
    BasicTest(it, extractor, "I'll go back Monday January 12th, 2016", 13, 25);
    BasicTest(it, extractor, "I'll go back 02/22/2016", 13, 10);
    BasicTest(it, extractor, "I'll go back 21/04/2016", 13, 10);
    BasicTest(it, extractor, "I'll go back 21/04/16", 13, 8);
    BasicTest(it, extractor, "I'll go back 9-18-15", 13, 7);
    BasicTest(it, extractor, "I'll go back on 4.22", 16, 4);
    BasicTest(it, extractor, "I'll go back on 4-22", 16, 4);
    BasicTest(it, extractor, "I'll go back at 4.22", 16, 4);
    BasicTest(it, extractor, "I'll go back in 4-22", 16, 4);
    BasicTest(it, extractor, "I'll go back on    4/22", 19, 4);
    BasicTest(it, extractor, "I'll go back on 22/04", 16, 5);
    BasicTest(it, extractor, "I'll go back       4/22", 19, 4);
    BasicTest(it, extractor, "I'll go back 22/04", 13, 5);
    BasicTest(it, extractor, "I'll go back 2015/08/12", 13, 10);
    BasicTest(it, extractor, "I'll go back 11/12,2016", 13, 10);
    BasicTest(it, extractor, "I'll go back 11/12,16", 13, 8);
    BasicTest(it, extractor, "I'll go back 1st Jan", 13, 7);
    BasicTest(it, extractor, "I'll go back 1-Jan", 13, 5);
    BasicTest(it, extractor, "I'll go back 28-Nov", 13, 6);
    BasicTest(it, extractor, "I'll go back Wed, 22 of Jan", 13, 14);

    BasicTest(it, extractor, "I'll go back Jan first", 13, 9);
    BasicTest(it, extractor, "I'll go back May twenty-first", 13, 16);
    BasicTest(it, extractor, "I'll go back May twenty one", 13, 14);
    BasicTest(it, extractor, "I'll go back second of Aug", 13, 13);
    BasicTest(it, extractor, "I'll go back twenty second of June", 13, 21);

    BasicTest(it, extractor, "I'll go back on Friday", 16, 6);
    BasicTest(it, extractor, "I'll go back Friday", 13, 6);
    BasicTest(it, extractor, "I'll go back on Fridays", 16, 7);
    BasicTest(it, extractor, "I'll go back Fridays", 13, 7);
    BasicTest(it, extractor, "I'll go back today", 13, 5);
    BasicTest(it, extractor, "I'll go back tomorrow", 13, 8);
    BasicTest(it, extractor, "I'll go back yesterday", 13, 9);
    BasicTest(it, extractor, "I'll go back the day before yesterday", 13, 24);
    BasicTest(it, extractor, "I'll go back the day after tomorrow", 13, 22);
    BasicTest(it, extractor, "I'll go back the next day", 13, 12);
    BasicTest(it, extractor, "I'll go back next day", 13, 8);
    BasicTest(it, extractor, "I'll go back this Friday", 13, 11);
    BasicTest(it, extractor, "I'll go back next Sunday", 13, 11);
    BasicTest(it, extractor, "I'll go back last Sunday", 13, 11);
    BasicTest(it, extractor, "I'll go back last day", 13, 8);
    BasicTest(it, extractor, "I'll go back the last day", 13, 12);
    BasicTest(it, extractor, "I'll go back the day", 13, 7);
    BasicTest(it, extractor, "I'll go back this week Friday", 13, 16);
    BasicTest(it, extractor, "I'll go back next week Sunday", 13, 16);
    BasicTest(it, extractor, "I'll go back last week Sunday", 13, 16);
    BasicTest(it, extractor, "I'll go back 15 June 2016", 13, 12);
    BasicTest(it, extractor, "a baseball on may the eleventh", 14, 16);

    BasicTest(it, extractor, "I'll go back the first friday of july", 13, 24);
    BasicTest(it, extractor, "I'll go back the first friday in this month", 13, 30);

    BasicTest(it, extractor, "I'll go back two weeks from now", 13, 18);

    BasicTest(it, extractor, "I'll go back next week on Friday", 13, 19);
    BasicTest(it, extractor, "I'll go back on Friday next week", 13, 19);

    BasicTest(it, extractor, "past Monday", 0, 11);

    
    BasicTest(it, extractor, "I went back two months ago", 12, 14);
    BasicTest(it, extractor, "I'll go back two days later", 13, 14);
    BasicTest(it, extractor, "who did i email a month ago", 16, 11);
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(Constants.SYS_DATETIME_DATE, results[0].type);
    });
}