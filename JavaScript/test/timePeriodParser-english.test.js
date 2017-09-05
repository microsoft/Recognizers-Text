var describe = require('ava-spec').describe;
var EnglishTimePeriodExtractorConfiguration = require('../compiled/dateTime/english/timePeriodConfiguration').EnglishTimePeriodExtractorConfiguration;
var BaseTimePeriodExtractor = require('../compiled/dateTime/baseTimePeriod').BaseTimePeriodExtractor;
var EnglishCommonDateTimeParserConfiguration = require('../compiled/dateTime/english/baseConfiguration').EnglishCommonDateTimeParserConfiguration;
var EnglishTimePeriodParserConfiguration = require('../compiled/dateTime/english/timePeriodConfiguration').EnglishTimePeriodParserConfiguration;
var BaseTimePeriodParser = require('../compiled/dateTime/baseTimePeriod').BaseTimePeriodParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Time Period Parse', it => {
    let year = 2016, month = 11, day = 7, min = 0, second = 0;
    let referenceTime = new Date(2016, 11, 7, 16, 12, 0);
    let extractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
    let parser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    // basic match
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 5 to 6pm",
        new Date(year, month, day, 17, min, second),
        new Date(year, month, day, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 5 to 6 p.m",
        new Date(year, month, day, 17, min, second),
        new Date(year, month, day, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 5 to seven in the morning",
        new Date(year, month, day, 5, min, second),
        new Date(year, month, day, 7, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out from 5 to 6 pm",
        new Date(year, month, day, 17, min, second),
        new Date(year, month, day, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out between 5 and 6pm",
        new Date(year, 11, 7, 17, min, second),
        new Date(year, 11, 7, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out between 5pm and 6pm",
        new Date(year, 11, 7, 17, min, second),
        new Date(year, 11, 7, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out between 5 and 6 in the afternoon",
        new Date(year, 11, 7, 17, min, second),
        new Date(year, 11, 7, 18, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out from 1am to 5pm",
        new Date(year, month, day, 1, min, second),
        new Date(year, month, day, 17, min, second));

    // merge two time points
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 4pm till 5pm",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));
    
    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 4pm til 5pm",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 4:00 to 7 oclock",
        new Date(year, month, day, 4, min, second),
        new Date(year, month, day, 7, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 4pm-5pm",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out 4pm - 5pm",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out from 3 in the morning until 5pm",
        new Date(year, month, day, 3, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out between 3 in the morning and 5pm",
        new Date(year, month, day, 3, min, second),
        new Date(year, month, day, 17, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "I'll be out between 4pm and 5pm today",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 17, min, second));


    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the morning",
        new Date(year, month, day, 8, min, second),
        new Date(year, month, day, 12, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the afternoon",
        new Date(year, month, day, 12, min, second),
        new Date(year, month, day, 16, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the night",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the evening",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 20, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the evenings",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 20, min, second));

    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early-mornings",
        new Date(year, month, day, 8, min, second),
        new Date(year, month, day, 10, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late-mornings",
        new Date(year, month, day, 10, min, second),
        new Date(year, month, day, 12, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early-morning",
        new Date(year, month, day, 8, min, second),
        new Date(year, month, day, 10, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late-morning",
        new Date(year, month, day, 10, min, second),
        new Date(year, month, day, 12, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early-afternoon",
        new Date(year, month, day, 12, min, second),
        new Date(year, month, day, 14, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late-afternoon",
        new Date(year, month, day, 14, min, second),
        new Date(year, month, day, 16, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early-evening",
        new Date(year, month, day, 16, min, second),
        new Date(year, month, day, 18, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late-evening",
        new Date(year, month, day, 18, min, second),
        new Date(year, month, day, 20, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early-night",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 22, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late-night",
        new Date(year, month, day, 22, min, second),
        new Date(year, month, day, 23, 59, 59));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the early night",
        new Date(year, month, day, 20, min, second),
        new Date(year, month, day, 22, min, second));
    basicTestBeginEnd(it, extractor, parser, referenceTime, "let's meet in the late night",
        new Date(year, month, day, 22, min, second),
        new Date(year, month, day, 23, 59, 59));
});

describe('Time Period Parse Luis', it => {
    let referenceTime = new Date(2016, 11, 7, 16, 12, 0);
    let extractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
    let parser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    // basic match
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 5 to 6pm", "(T17,T18,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 5 to 6 p.m", "(T17,T18,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 5 to seven in the morning", "(T05,T07,PT2H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out from 5 to 6 pm", "(T17,T18,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out from 1am to 5pm", "(T01,T17,PT16H)");


    // merge two time points
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 4pm till 5pm", "(T16,T17,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 4:00 to 7 oclock", "(T04:00,T07,PT3H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 4pm-5pm", "(T16,T17,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out 4pm - 5pm", "(T16,T17,PT1H)");
    basicTest_Luis(it, extractor, parser, referenceTime, "I'll be out from 3 in the morning until 5pm", "(T03,T17,PT14H)");

    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the morning", "TMO");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the afternoon", "TAF");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the night", "TNI");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the evening", "TEV");

    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early-mornings", "TMO");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late-mornings", "TMO");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early-morning", "TMO");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late-morning", "TMO");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early-afternoon", "TAF");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late-afternoon", "TAF");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early-evening", "TEV");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late-evening", "TEV");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early-night", "TNI");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late-night", "TNI");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the early night", "TNI");
    basicTest_Luis(it, extractor, parser, referenceTime, "let's meet in the late night", "TNI");
});

function basicTestBeginEnd(it, extractor, parser, referenceTime, text, beginDate, endDate) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_TIMEPERIOD, pr.type);
        t.deepEqual(beginDate, pr.value.futureValue.item1);
        t.deepEqual(endDate, pr.value.futureValue.item2);
    });
}

function basicTest_Luis(it, extractor, parser, referenceTime, text, luisValueStr) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceTime);
        t.is(Constants.SYS_DATETIME_TIMEPERIOD, pr.type);
        t.is(luisValueStr, pr.value.timex);
    });
}