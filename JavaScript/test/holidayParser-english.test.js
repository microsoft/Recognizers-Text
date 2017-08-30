var describe = require('ava-spec').describe;
var EnglishHolidayExtractorConfiguration = require('../compiled/dateTime/english/extractorConfiguration').EnglishHolidayExtractorConfiguration;
var BaseHolidayExtractor = require('../compiled/dateTime/extractors').BaseHolidayExtractor;
var EnglishHolidayParserConfiguration = require('../compiled/dateTime/english/parserConfiguration').EnglishHolidayParserConfiguration;
var BaseHolidayParser = require('../compiled/dateTime/parsers').BaseHolidayParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Time Parse', it => {
    let refrenceDay = new Date(2016, 11 - 1, 7);
    let extractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
    let parser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());

    // All date's month are zero-based (-1)
    BasicTest(it, extractor, parser, referenceDay, "I'll go back on new year eve",
        new Date(2016, 12-1, 31),
        new Date(2015, 12-1, 31));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on new year's eve",
        new Date(2016, 12-1, 31),
        new Date(2015, 12-1, 31));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on christmas",
        new Date(2016, 12-1, 25),
        new Date(2015, 12-1, 25));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on Yuandan",
        new Date(2017, 1-1, 1),
        new Date(2016, 1-1, 1));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanks giving day",
        new Date(2016, 11-1, 24),
        new Date(2015, 11-1, 26));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanksgiving",
        new Date(2016, 11-1, 24),
        new Date(2015, 11-1, 26));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on father's day",
        new Date(2017, 6-1, 18),
        new Date(2016, 6-1, 19));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on Yuandan of next year",
        new Date(2017, 1-1, 1),
        new Date(2017, 1-1, 1));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanks giving day 2010",
        new Date(2010, 11-1, 25),
        new Date(2010, 11-1, 25));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on father's day of 2015",
        new Date(2015, 6-1, 21),
        new Date(2015, 6-1, 21));
});

describe('Date Time Luis', it => {
    let year = 2016, month = 11 - 1, day = 7, hour = 0, min = 0, second = 0;
    let referenceTime = new Date(2016, 11 - 1, 7, 0, 0, 0);
    let extractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
    let parser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back as soon as possible", "FUTURE_REF");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00", "XXXX-XX-15T08:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00:24", "XXXX-XX-15T08:00:24");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15, 8pm", "XXXX-XX-15T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00pm", "2016-04-21T20:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00:24pm", "2016-04-21T20:00:24");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back Oct.23 at seven", "XXXX-10-23T07");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14 8:00am", "XXXX-10-14T08:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14 8:00:13am", "XXXX-10-14T08:00:13");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00am", "XXXX-10-14T08:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00:25am", "XXXX-10-14T08:00:25");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back May 5, 2016, 20 min past eight in the evening", "2016-05-05T20:20");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm on 15", "XXXX-XX-15T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back at seven on 15", "XXXX-XX-15T07");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm today", "2016-11-07T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back a quarter to seven tomorrow", "2016-11-08T06:45");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 19:00, 2016-12-22", "2016-12-22T19:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back now", "PRESENT_REF");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tomorrow 8:00am", "2016-11-08T08:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tomorrow morning at 7", "2016-11-08T07");
    //BasicTest_Luis(it, extractor, parser, referenceTime,"I'll go back Oct. 5 in the afternoon at 7", "XXXX-10-05T19");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 7:00 on next Sunday afternoon", "2016-11-20T19:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back twenty minutes past five tomorrow morning", "2016-11-08T05:20");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, Sunday", "XXXX-WXX-7T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1st Jan", "XXXX-01-01T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1 Jan", "XXXX-01-01T20");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 10pm tonight", "2016-11-07T22");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8am this morning", "2016-11-07T08");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm this evening", "2016-11-07T20");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7", "2016-11-07T07");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7am", "2016-11-07T07");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at seven", "2016-11-07T07");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7:00", "2016-11-07T07:00");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this night at 7", "2016-11-07T19");
    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tonight at 7", "2016-11-07T19");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 2016-12-16T12:23:59", "2016-12-16T12:23:59");

    BasicTest_Luis(it, extractor, parser, referenceTime, "I'll go back in 5 hours", "2016-11-07T05:00:00");
});

function BasicTest(it, extractor, parser, referenceDay, text, futureTime, pastTime) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDayTime);
        t.is(Constants.SYS_DATETIME_DATE, pr.type);
        t.deepEqual(futureDate, pr.value.futureValue);
        t.deepEqual(pastDate, pr.value.pastValue);
    });
}

function BasicTest_Luis(it, extractor, parser, referenceTime, text, luisValueStr) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_DATETIME, pr.type);
        t.is(luisValueStr, pr.value.timex);
    });
}