var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/dateTimePeriodConfiguration").EnglishDateTimePeriodExtractorConfiguration;
var Extractor = require("../compiled/dateTime/baseDateTimePeriod").BaseDateTimePeriodExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/baseConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/dateTimePeriodConfiguration").EnglishDateTimePeriodParserConfiguration;
var Parser = require("../compiled/dateTime/baseDateTimePeriod").BaseDateTimePeriodParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('DateTime Period Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7, 16, 12, 0);
    let year = referenceDate.getFullYear();
    let month = referenceDate.getMonth();
    let day = referenceDate.getDate();
    let hour = referenceDate.getHours();
    let min = 0;
    let second = 0;

    // basic match
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out five to seven today",
        new Date(year, month, day, 5, min, second),
        new Date(year, month, day, 7, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 of 4/22/2016",
        new Date(2016, 3, 22, 5, min, second),
        new Date(2016, 3, 22, 6, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 of April 22",
        new Date(year + 1, 3, 22, 5, min, second),
        new Date(year + 1, 3, 22, 6, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 5 to 6pm of April 22",
        new Date(year + 1, 3, 22, 17, min, second),
        new Date(year + 1, 3, 22, 18, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 on 1st Jan",
        new Date(year + 1, 0, 1, 5, min, second),
        new Date(year + 1, 0, 1, 6, min, second));

    // merge two time points
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out 3pm to 4pm tomorrow",
        new Date(year, month, 8, 15, min, second),
        new Date(year, month, 8, 16, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out 3:00 to 4:00 tomorrow",
        new Date(year, month, 8, 3, min, second),
        new Date(year, month, 8, 4, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out half past seven to 4pm tomorrow",
        new Date(year, month, 8, 7, 30, second),
        new Date(year, month, 8, 16, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 4pm today to 5pm tomorrow",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, 8, 17, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out from 2:00pm, 2016-2-21 to 3:32, 04/23/2016",
        new Date(2016, 1, 21, 14, min, second),
        new Date(2016, 3, 23, 3, 32, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out between 4pm and 5pm today",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll be out between 4pm on Jan 1, 2016 and 5pm today",
        new Date(2016, 0, 1, 16, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tonight",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tonight for 8",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back this night",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back this evening",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 20, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back this morning",
        new Date(year, month, day, 8, min, second),
        new Date(year, month, day, 12, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back this afternoon",
        new Date(year, month, day, 12, min, second),
        new Date(year, month, day, 16, min, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back next night",
        new Date(year, month, day + 1, 20, min, second),
        new Date(year, month, day + 1, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back last night",
        new Date(year, month, day - 1, 20, min, second),
        new Date(year, month, day - 1, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tomorrow night",
        new Date(year, month, day + 1, 20, min, second),
        new Date(year, month, day + 1, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back next monday afternoon",
        new Date(year, month, 14, 12, min, second),
        new Date(year, month, 14, 16, min, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back last 3 minute",
        new Date(year, month, day, 16, 9, second),
        new Date(year, month, day, 16, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back past 3 minute",
        new Date(year, month, day, 16, 9, second),
        new Date(year, month, day, 16, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back previous 3 minute",
        new Date(year, month, day, 16, 9, second),
        new Date(year, month, day, 16, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back previous 3mins",
        new Date(year, month, day, 16, 9, second),
        new Date(year, month, day, 16, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back next 5 hrs",
        new Date(year, month, day, 16, 12, second),
        new Date(year, month, day, 21, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back last minute",
        new Date(year, month, day, 16, 11, second),
        new Date(year, month, day, 16, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back next hour",
        new Date(year, month, day, 16, 12, second),
        new Date(year, month, day, 17, 12, second));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back next few hours",
        new Date(year, month, day, 16, 12, second),
        new Date(year, month, day, 19, 12, second));

    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tuesday in the morning",
        new Date(year, month, day + 1, 8, 0, 0),
        new Date(year, month, day + 1, 12, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tuesday in the afternoon",
        new Date(year, month, day + 1, 12, 0, 0),
        new Date(year, month, day + 1, 16, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "I'll go back tuesday in the evening",
        new Date(year, month, day + 1, 16, 0, 0),
        new Date(year, month, day + 1, 20, 0, 0));

    // late/early
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early-morning Tuesday",
        new Date(year, month, day + 1, 8, 0, 0),
        new Date(year, month, day + 1, 10, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early-morning on Tuesday",
        new Date(year, month, day + 1, 8, 0, 0),
        new Date(year, month, day + 1, 10, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the late-morning Tuesday",
        new Date(year, month, day + 1, 10, 0, 0),
        new Date(year, month, day + 1, 12, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early-afternoon Tuesday",
        new Date(year, month, day + 1, 12, 0, 0),
        new Date(year, month, day + 1, 14, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the late-afternoon Tuesday",
        new Date(year, month, day + 1, 14, 0, 0),
        new Date(year, month, day + 1, 16, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early-evening Tuesday",
        new Date(year, month, day + 1, 16, 0, 0),
        new Date(year, month, day + 1, 18, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the late-evening Tuesday",
        new Date(year, month, day + 1, 18, 0, 0),
        new Date(year, month, day + 1, 20, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early-night Tuesday",
        new Date(year, month, day + 1, 20, 0, 0),
        new Date(year, month, day + 1, 22, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the late-night Tuesday",
        new Date(year, month, day + 1, 22, 0, 0),
        new Date(year, month, day + 1, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the early night Tuesday",
        new Date(year, month, day + 1, 20, 0, 0),
        new Date(year, month, day + 1, 22, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet in the late night Tuesday",
        new Date(year, month, day + 1, 22, 0, 0),
        new Date(year, month, day + 1, 23, 59, 59));

    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday early-morning",
        new Date(year, month, day + 1, 8, 0, 0),
        new Date(year, month, day + 1, 10, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday late-morning",
        new Date(year, month, day + 1, 10, 0, 0),
        new Date(year, month, day + 1, 12, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday early-afternoon",
        new Date(year, month, day + 1, 12, 0, 0),
        new Date(year, month, day + 1, 14, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday late-afternoon",
        new Date(year, month, day + 1, 14, 0, 0),
        new Date(year, month, day + 1, 16, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday early-evening",
        new Date(year, month, day + 1, 16, 0, 0),
        new Date(year, month, day + 1, 18, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday late-evening",
        new Date(year, month, day + 1, 18, 0, 0),
        new Date(year, month, day + 1, 20, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday early-night",
        new Date(year, month, day + 1, 20, 0, 0),
        new Date(year, month, day + 1, 22, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday late-night",
        new Date(year, month, day + 1, 22, 0, 0),
        new Date(year, month, day + 1, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday early night",
        new Date(year, month, day + 1, 20, 0, 0),
        new Date(year, month, day + 1, 22, 0, 0));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet on Tuesday late night",
        new Date(year, month, day + 1, 22, 0, 0),
        new Date(year, month, day + 1, 23, 59, 59));

    // Rest of
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet rest of the day",
        new Date(year, month, day, hour, 12, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet rest of current day",
        new Date(year, month, day, hour, 12, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet rest of my day",
        new Date(year, month, day, hour, 12, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet rest of this day",
        new Date(year, month, day, hour, 12, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestFuture(it, extractor, parser, referenceDate, "let's meet rest the day",
        new Date(year, month, day, hour, 12, second),
        new Date(year, month, day, 23, 59, 59));
});

describe('DateTime Period Parser Luis', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7, 16, 12, 0);

    // basic match
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out five to seven today", "(2016-11-07T05,2016-11-07T07,PT2H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 of 4/22/2016", "(2016-04-22T05,2016-04-22T06,PT1H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 of April 22", "(XXXX-04-22T05,XXXX-04-22T06,PT1H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 5 to 6 on 1st Jan", "(XXXX-01-01T05,XXXX-01-01T06,PT1H)");

    // merge two time points
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out 3pm to 4pm tomorrow", "(2016-11-08T15,2016-11-08T16,PT1H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out 3pm to 4pm tomorrow", "(2016-11-08T15,2016-11-08T16,PT1H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll be out I'll be out from 2:00pm, 2016-2-21 to 3:32, 04/23/2016", "(2016-02-21T14:00,2016-04-23T03:32,PT1478H)");

    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back tonight", "2016-11-07TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back this night", "2016-11-07TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back this evening", "2016-11-07TEV");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back this morning", "2016-11-07TMO");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back this afternoon", "2016-11-07TAF");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back next night", "2016-11-08TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back last night", "2016-11-06TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back tomorrow night", "2016-11-08TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back next monday afternoon", "2016-11-14TAF");

    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back last 3 minute", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back past 3 minute", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back previous 3 minute", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back previous 3mins", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back next 5 hrs", "(2016-11-07T16:12:00,2016-11-07T21:12:00,PT5H)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back last minute", "(2016-11-07T16:11:00,2016-11-07T16:12:00,PT1M)");
    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back next hour", "(2016-11-07T16:12:00,2016-11-07T17:12:00,PT1H)");

    basicTestLuis(it, extractor, parser, referenceDate, "I'll go back tuesday in the morning", "XXXX-WXX-2TMO");

    // early/late date time
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the early-morning Tuesday", "XXXX-WXX-2TMO");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late-morning Tuesday", "XXXX-WXX-2TMO");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the early-afternoon Tuesday", "XXXX-WXX-2TAF");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late-afternoon Tuesday", "XXXX-WXX-2TAF");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the early-evening Tuesday", "XXXX-WXX-2TEV");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late-evening Tuesday", "XXXX-WXX-2TEV");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the early-night Tuesday", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late-night Tuesday", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the early night Tuesday", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late night Tuesday", "XXXX-WXX-2TNI");

    basicTestLuis(it, extractor, parser, referenceDate, "let's meet in the late night on Tuesday", "XXXX-WXX-2TNI");

    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday early-morning", "XXXX-WXX-2TMO");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday late-morning", "XXXX-WXX-2TMO");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday early-afternoon", "XXXX-WXX-2TAF");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday late-afternoon", "XXXX-WXX-2TAF");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday early-evening", "XXXX-WXX-2TEV");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday late-evening", "XXXX-WXX-2TEV");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday early-night", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday late-night", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday early night", "XXXX-WXX-2TNI");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet on Tuesday late night", "XXXX-WXX-2TNI");

    // Rest of
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet rest of the day", "(2016-11-07T16:12:00,2016-11-07T23:59:59,PT28079S)");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet rest of this day", "(2016-11-07T16:12:00,2016-11-07T23:59:59,PT28079S)");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet rest of my day", "(2016-11-07T16:12:00,2016-11-07T23:59:59,PT28079S)");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet rest of current day", "(2016-11-07T16:12:00,2016-11-07T23:59:59,PT28079S)");
    basicTestLuis(it, extractor, parser, referenceDate, "let's meet rest the day", "(2016-11-07T16:12:00,2016-11-07T23:59:59,PT28079S)");
});

function basicTestFuture(it, extractor, parser, referenceDate, text, beginDate, endDate) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.type);
        t.deepEqual(beginDate, pr.value.futureValue[0]);
        t.deepEqual(endDate, pr.value.futureValue[1]);
    });
}

function basicTestLuis(it, extractor, parser, referenceDate, text, luisDate) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.type);
        t.is(luisDate, pr.value.timex);
    });
}