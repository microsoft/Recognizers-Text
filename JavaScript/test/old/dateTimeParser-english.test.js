var describe = require('ava-spec').describe;
var EnglishDateTimeExtractorConfiguration = require('../compiled/dateTime/english/dateTimeConfiguration').EnglishDateTimeExtractorConfiguration;
var BaseDateTimeExtractor = require('../compiled/dateTime/baseDateTime').BaseDateTimeExtractor;
var EnglishCommonDateTimeParserConfiguration = require('../compiled/dateTime/english/baseConfiguration').EnglishCommonDateTimeParserConfiguration;
var EnglishDateTimeParserConfiguration = require('../compiled/dateTime/english/dateTimeConfiguration').EnglishDateTimeParserConfiguration;
var BaseDateTimeParser = require('../compiled/dateTime/baseDateTime').BaseDateTimeParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Time Parse', it => {
    let year = 2016, month = 11 - 1, day = 7, hour = 0, min = 0, second = 0;
    let referenceTime = new Date(2016, 11 - 1, 7, 0, 0, 0);
    let extractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
    let parser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    basicTest(it, extractor, parser, referenceTime, "I'll go back now", new Date(2016, 11 - 1, 7, 0, 0, 0));
    basicTest(it, extractor, parser, referenceTime, "I'll go back as soon as possible", new Date(year, month, day, hour, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00", new Date(year, month, 15, 8, 0, second),
        new Date(year, month - 1, 15, 8, 0, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00:20", new Date(year, month, 15, 8, 0, 20),
        new Date(year, month - 1, 15, 8, 0, 20));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back on 15, 8pm", new Date(year, month, 15, 20, min, second),
        new Date(year, month - 1, 15, 20, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back at 5th at 4 a.m.", new Date(year, month + 1, 5, 4, min, second),
        new Date(year, month, 5, 4, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00pm", new Date(2016, 4 - 1, 21, 20, 0, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00:20pm", new Date(2016, 4 - 1, 21, 20, 0, 20));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back Oct.23 at seven", new Date(year + 1, 10 - 1, 23, 7, min, second),
        new Date(year, 10 - 1, 23, 7, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14 8:00am", new Date(year + 1, 10 - 1, 14, 8, 0, second),
        new Date(year, 10 - 1, 14, 8, 0, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14 8:00:31am", new Date(year + 1, 10 - 1, 14, 8, 0, 31),
        new Date(year, 10 - 1, 14, 8, 0, 31));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14 around 8:00am", new Date(year + 1, 10 - 1, 14, 8, 0, second),
        new Date(year, 10 - 1, 14, 8, 0, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14 for 8:00:31am", new Date(year + 1, 10 - 1, 14, 8, 0, 31),
        new Date(year, 10 - 1, 14, 8, 0, 31));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00am", new Date(year + 1, 10 - 1, 14, 8, 0, second),
        new Date(year, 10 - 1, 14, 8, 0, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00:25am", new Date(year + 1, 10 - 1, 14, 8, 0, 25),
        new Date(year, 10 - 1, 14, 8, 0, 25));
    basicTest(it, extractor, parser, referenceTime, "I'll go back May 5, 2016, 20 min past eight in the evening",
        new Date(2016, 5 - 1, 5, 20, 20, second));

    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back 8pm on 15", new Date(year, month, 15, 20, min, second),
        new Date(year, month - 1, 15, 20, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back 8pm on the 15", new Date(year, month, 15, 20, min, second),
        new Date(year, month - 1, 15, 20, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back at seven on 15", new Date(year, month, 15, 7, min, second),
        new Date(year, month - 1, 15, 7, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 8pm today", new Date(year, month, day, 20, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back a quarter to seven tomorrow", new Date(year, month, 8, 6, 45, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 19:00, 2016-12-22", new Date(2016, 12 - 1, 22, 19, 0, second));

    basicTest(it, extractor, parser, referenceTime, "I'll go back tomorrow 8:00am", new Date(year, month, 8, 8, 0, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back tomorrow morning at 7", new Date(2016, 11 - 1, 8, 7, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back tonight around 7", new Date(2016, 11 - 1, 7, 19, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 7:00 on next Sunday afternoon", new Date(2016, 11 - 1, 20, 19, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back twenty minutes past five tomorrow morning", new Date(2016, 11 - 1, 8, 5, 20, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 7, this morning", new Date(year, month, day, 7, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 10, tonight", new Date(year, month, day, 22, min, second));

    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, Sunday", new Date(2016, 11 - 1, 13, 20, min, second),
        new Date(2016, 11 - 1, 6, 20, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1st Jan", new Date(2017, 1 - 1, 1, 20, min, second),
        new Date(2016, 1 - 1, 1, 20, min, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1 Jan", new Date(2017, 1 - 1, 1, 20, min, second),
        new Date(2016, 1 - 1, 1, 20, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 10pm tonight", new Date(2016, 11 - 1, 7, 22, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 8am this morning", new Date(2016, 11 - 1, 7, 8, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back 8pm this evening", new Date(2016, 11 - 1, 7, 20, min, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back the end of the day", new Date(2016, 11 - 1, 7, 23, 59, second));
    basicTest(it, extractor, parser, referenceTime, "I'll go back end of tomorrow", new Date(2016, 11 - 1, 8, 23, 59, second));
    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back end of the sunday", new Date(2016, 11 - 1, 13, 23, 59, second),
        new Date(2016, 11 - 1, 6, 23, 59, second));

    basicTest_FuturePastTime(it, extractor, parser, referenceTime, "I'll go back in 5 hours", new Date(2016, 11 - 1, 7, hour + 5, min, second),
        new Date(2016, 11 - 1, 7, hour + 5, min, second));
});

describe('Date Time Luis', it => {
    let year = 2016, month = 11 - 1, day = 7, hour = 0, min = 0, second = 0;
    let referenceTime = new Date(2016, 11 - 1, 7, 0, 0, 0);
    let extractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
    let parser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back as soon as possible", "FUTURE_REF");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00", "XXXX-XX-15T08:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15 at 8:00:24", "XXXX-XX-15T08:00:24");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back on 15, 8pm", "XXXX-XX-15T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00pm", "2016-04-21T20:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 04/21/2016, 8:00:24pm", "2016-04-21T20:00:24");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back Oct.23 at seven", "XXXX-10-23T07");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14 8:00am", "XXXX-10-14T08:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14 8:00:13am", "XXXX-10-14T08:00:13");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00am", "XXXX-10-14T08:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back October 14, 8:00:25am", "XXXX-10-14T08:00:25");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back May 5, 2016, 20 min past eight in the evening", "2016-05-05T20:20");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm on 15", "XXXX-XX-15T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back at seven on 15", "XXXX-XX-15T07");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm today", "2016-11-07T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back a quarter to seven tomorrow", "2016-11-08T06:45");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 19:00, 2016-12-22", "2016-12-22T19:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back now", "PRESENT_REF");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tomorrow 8:00am", "2016-11-08T08:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tomorrow morning at 7", "2016-11-08T07");
    //basicTest_Luis(it, extractor, parser, referenceTime,"I'll go back Oct. 5 in the afternoon at 7", "XXXX-10-05T19");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 7:00 on next Sunday afternoon", "2016-11-20T19:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back twenty minutes past five tomorrow morning", "2016-11-08T05:20");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, Sunday", "XXXX-WXX-7T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1st Jan", "XXXX-01-01T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm in the evening, 1 Jan", "XXXX-01-01T20");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 10pm tonight", "2016-11-07T22");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8am this morning", "2016-11-07T08");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 8pm this evening", "2016-11-07T20");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7", "2016-11-07T07");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7am", "2016-11-07T07");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at seven", "2016-11-07T07");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this morning at 7:00", "2016-11-07T07:00");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back this night at 7", "2016-11-07T19");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back tonight at 7", "2016-11-07T19");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back 2016-12-16T12:23:59", "2016-12-16T12:23:59");

    basicTest_Luis(it, extractor, parser, referenceTime, "I'll go back in 5 hours", "2016-11-07T05:00:00");
});

function basicTest(it, extractor, parser, referenceTime, text, date) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_DATETIME, pr.type);
        t.deepEqual(date, pr.value.futureValue);
        t.deepEqual(date, pr.value.pastValue);
    });
}

function basicTest_FuturePastTime(it, extractor, parser, referenceTime, text, futureTime, pastTime) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_DATETIME, pr.type);
        t.deepEqual(futureTime, pr.value.futureValue);
        t.deepEqual(pastTime, pr.value.pastValue);
    });
}

function basicTest_Luis(it, extractor, parser, referenceTime, text, luisValueStr) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_DATETIME, pr.type);
        t.is(luisValueStr, pr.value.timex);
    });
}