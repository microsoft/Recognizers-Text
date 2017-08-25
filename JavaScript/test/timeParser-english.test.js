var describe = require('ava-spec').describe;
var EnglishTimeExtractorConfiguration = require('../compiled/dateTime/english/extractorConfiguration').EnglishTimeExtractorConfiguration;
var BaseTimeExtractor = require('../compiled/dateTime/extractors').BaseTimeExtractor;
var EnglishCommonDateTimeParserConfiguration = require('../compiled/dateTime/english/parserConfiguration').EnglishCommonDateTimeParserConfiguration;
var EnglishTimeParserConfiguration = require('../compiled/dateTime/english/parserConfiguration').EnglishTimeParserConfiguration;
var EnglishTimeParser = require('../compiled/dateTime/english/parserConfiguration').EnglishTimeParser;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Time Parse', it => {
    let today = new Date();
    let year = today.getFullYear(), month = today.getMonth(), day = today.getDate(), min = 0, second = 0;
    let extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
    let parser = new EnglishTimeParser(new EnglishTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    BasicTestFuture(it, extractor, parser, "I'll be back at 7ampm", new Date(year, month, day, 7, min, second));
    BasicTestFuture(it, extractor, parser, "I'll be back at 7", new Date(year, month, day, 7, min, second));
    BasicTestFuture(it, extractor, parser, "I'll be back at seven", new Date(year, month, day, 7, min, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 7pm", new Date(year, month, day, 19, min, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 7:56pm", new Date(year, month, day, 19, 56, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 7:56:30pm", new Date(year, month, day, 19, 56, 30));
    BasicTestFuture(it, extractor, parser, "I'll be back 7:56:30 pm", new Date(year, month, day, 19, 56, 30));
    BasicTestFuture(it, extractor, parser, "I'll be back 12:34", new Date(year, month, day, 12, 34, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 12:34:25 ", new Date(year, month, day, 12, 34, 25));

    BasicTestFuture(it, extractor, parser, "It's 7 o'clock", new Date(year, month, day, 7, min, second));
    BasicTestFuture(it, extractor, parser, "It's seven o'clock", new Date(year, month, day, 7, min, second));
    BasicTestFuture(it, extractor, parser, "It's 8 in the morning", new Date(year, month, day, 8, min, second));
    BasicTestFuture(it, extractor, parser, "It's 8 in the night", new Date(year, month, day, 20, min, second));

    BasicTestFuture(it, extractor, parser, "It's half past eight", new Date(year, month, day, 8, 30, second));
    BasicTestFuture(it, extractor, parser, "It's half past 8pm", new Date(year, month, day, 20, 30, second));
    BasicTestFuture(it, extractor, parser, "It's 30 mins past eight", new Date(year, month, day, 8, 30, second));
    BasicTestFuture(it, extractor, parser, "It's a quarter past eight", new Date(year, month, day, 8, 15, second));
    BasicTestFuture(it, extractor, parser, "It's quarter past eight", new Date(year, month, day, 8, 15, second));
    BasicTestFuture(it, extractor, parser, "It's three quarters past 9pm", new Date(year, month, day, 21, 45, second));
    BasicTestFuture(it, extractor, parser, "It's three minutes to eight", new Date(year, month, day, 7, 57, second));

    BasicTestFuture(it, extractor, parser, "It's half past seven o'clock", new Date(year, month, day, 7, 30, second));
    BasicTestFuture(it, extractor, parser, "It's half past seven afternoon", new Date(year, month, day, 19, 30, second));
    BasicTestFuture(it, extractor, parser, "It's half past seven in the morning", new Date(year, month, day, 7, 30, second));
    BasicTestFuture(it, extractor, parser, "It's a quarter to 8 in the morning", new Date(year, month, day, 7, 45, second));
    BasicTestFuture(it, extractor, parser, "It's 20 min past eight in the evening", new Date(year, month, day, 20, 20, second));

    BasicTestFuture(it, extractor, parser, "I'll be back in the afternoon at 7", new Date(year, month, day, 19, 0, second));
    BasicTestFuture(it, extractor, parser, "I'll be back afternoon at 7", new Date(year, month, day, 19, 0, second));
    BasicTestFuture(it, extractor, parser, "I'll be back afternoon 7:00", new Date(year, month, day, 19, 0, second));
    BasicTestFuture(it, extractor, parser, "I'll be back afternoon 7:00:05", new Date(year, month, day, 19, 0, 05));
    BasicTestFuture(it, extractor, parser, "I'll be back afternoon seven pm", new Date(year, month, day, 19, 0, second));

    BasicTestFuture(it, extractor, parser, "I'll go back seven thirty pm", new Date(year, month, day, 19, 30, second));
    BasicTestFuture(it, extractor, parser, "I'll go back seven thirty five pm", new Date(year, month, day, 19, 35, second));
    BasicTestFuture(it, extractor, parser, "I'll go back eleven twenty pm", new Date(year, month, day, 23, 20, second));


    BasicTestFuture(it, extractor, parser, "I'll be back noonish", new Date(year, month, day, 12, 0, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 11ish", new Date(year, month, day, 11, 0, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 11-ish", new Date(year, month, day, 11, 0, second));

    BasicTestFuture(it, extractor, parser, "I'll be back 340pm", new Date(year, month, day, 15, 40, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 1140 a.m.", new Date(year, month, day, 11, 40, second));
    BasicTestFuture(it, extractor, parser, "I'll be back 1140 a.m.", new Date(year, month, day, 11, 40, second));

    BasicTestFuture(it, extractor, parser, "midnight", new Date(year, month, day, 0, 0, second));
    BasicTestFuture(it, extractor, parser, "mid-night", new Date(year, month, day, 0, 0, second));
    BasicTestFuture(it, extractor, parser, "mid night", new Date(year, month, day, 0, 0, second));
    BasicTestFuture(it, extractor, parser, "midmorning", new Date(year, month, day, 10, 0, second));
    BasicTestFuture(it, extractor, parser, "mid-morning", new Date(year, month, day, 10, 0, second));
    BasicTestFuture(it, extractor, parser, "mid morning", new Date(year, month, day, 10, 0, second));
    BasicTestFuture(it, extractor, parser, "midafternoon", new Date(year, month, day, 14, 0, second));
    BasicTestFuture(it, extractor, parser, "mid-afternoon", new Date(year, month, day, 14, 0, second));
    BasicTestFuture(it, extractor, parser, "mid afternoon", new Date(year, month, day, 14, 0, second));
    BasicTestFuture(it, extractor, parser, "midday", new Date(year, month, day, 12, 0, second));
    BasicTestFuture(it, extractor, parser, "mid-day", new Date(year, month, day, 12, 0, second));
    BasicTestFuture(it, extractor, parser, "mid day", new Date(year, month, day, 12, 0, second));
    BasicTestFuture(it, extractor, parser, "noon", new Date(year, month, day, 12, 0, second));

    // parse with two numbers
    BasicTestFuture(it, extractor, parser, "set an alarm for eight forty", new Date(year, month, day, 8, 40, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for eight forty am", new Date(year, month, day, 8, 40, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for eight forty pm", new Date(year, month, day, 20, 40, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for ten forty five", new Date(year, month, day, 10, 45, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for fifteen fifteen p m", new Date(year, month, day, 15 , 15, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for fifteen thirty p m", new Date(year, month, day, 15, 30, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for ten ten", new Date(year, month, day, 10, 10, second));
    BasicTestFuture(it, extractor, parser, "set an alarm for ten fifty five p. m.", new Date(year, month, day, 22, 55, second));
});

describe('Time Parse Luis', it => {
    let extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
    let parser = new EnglishTimeParser(new EnglishTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    BasicTest_Luis(it, extractor, parser, "I'll be back at 7", "T07");
    BasicTest_Luis(it, extractor, parser, "I'll be back at seven", "T07");
    BasicTest_Luis(it, extractor, parser, "I'll be back 7pm", "T19");
    BasicTest_Luis(it, extractor, parser, "I'll be back 7:56pm", "T19:56");
    BasicTest_Luis(it, extractor, parser, "I'll be back 7:56:30pm", "T19:56:30");
    BasicTest_Luis(it, extractor, parser, "I'll be back 7:56:13 pm", "T19:56:13");
    BasicTest_Luis(it, extractor, parser, "I'll be back 12:34", "T12:34");
    BasicTest_Luis(it, extractor, parser, "I'll be back 12:34:45 ", "T12:34:45");

    BasicTest_Luis(it, extractor, parser, "It's 7 o'clock", "T07");
    BasicTest_Luis(it, extractor, parser, "It's seven o'clock", "T07");
    BasicTest_Luis(it, extractor, parser, "It's 8 in the morning", "T08");
    BasicTest_Luis(it, extractor, parser, "It's 8 in the night", "T20");

    BasicTest_Luis(it, extractor, parser, "It's half past eight", "T08:30");
    BasicTest_Luis(it, extractor, parser, "It's half past 8pm", "T20:30");
    BasicTest_Luis(it, extractor, parser, "It's 30 mins past eight", "T08:30");
    BasicTest_Luis(it, extractor, parser, "It's a quarter past eight", "T08:15");
    BasicTest_Luis(it, extractor, parser, "It's three quarters past 9pm", "T21:45");
    BasicTest_Luis(it, extractor, parser, "It's three minutes to eight", "T07:57");

    BasicTest_Luis(it, extractor, parser, "It's half past seven o'clock", "T07:30");
    BasicTest_Luis(it, extractor, parser, "It's half past seven afternoon", "T19:30");
    BasicTest_Luis(it, extractor, parser, "It's half past seven in the morning", "T07:30");
    BasicTest_Luis(it, extractor, parser, "It's a quarter to 8 in the morning", "T07:45");
    BasicTest_Luis(it, extractor, parser, "It's 20 min past eight in the evening", "T20:20");

    BasicTest_Luis(it, extractor, parser, "I'll be back in the afternoon at 7", "T19");
    BasicTest_Luis(it, extractor, parser, "I'll be back afternoon at 7", "T19");
    BasicTest_Luis(it, extractor, parser, "I'll be back afternoon 7:00", "T19:00");
    BasicTest_Luis(it, extractor, parser, "I'll be back afternoon 7:00:25", "T19:00:25");
    BasicTest_Luis(it, extractor, parser, "I'll be back afternoon seven pm", "T19");

    BasicTest_Luis(it, extractor, parser, "I'll go back seven thirty am", "T07:30");
    BasicTest_Luis(it, extractor, parser, "I'll go back seven thirty five pm", "T19:35");
    BasicTest_Luis(it, extractor, parser, "I'll go back at eleven five", "T11:05");
    BasicTest_Luis(it, extractor, parser, "I'll go back three mins to five thirty ", "T05:27");
    BasicTest_Luis(it, extractor, parser, "I'll go back five thirty in the night", "T17:30");
    BasicTest_Luis(it, extractor, parser, "I'll go back in the night five thirty", "T17:30");

    BasicTest_Luis(it, extractor, parser, "I'll be back noonish", "T12");
    BasicTest_Luis(it, extractor, parser, "I'll be back noon", "T12");
    BasicTest_Luis(it, extractor, parser, "I'll be back 11ish", "T11");
    BasicTest_Luis(it, extractor, parser, "I'll be back 11-ish", "T11");

    //TODO: discussion on the definition
    //Default time period definition for now
    //LUIS Time Resolution Spec address: https://microsoft.sharepoint.com/teams/luis_core/_layouts/15/WopiFrame2.aspx?sourcedoc=%7B852DBAAF-911B-4CCC-9401-996505EC9B67%7D&file=LUIS%20Time%20Resolution%20Spec.docx&action=default
    //a.Morning: 08:00:00 - 12:00:00
    //b.Afternoon: 12:00:00 – 16:00:00
    //c.Evening: 16:00:00 – 20:00:00
    //d.Night: 20:00:00 – 23:59:59
    //e.Daytime: 08:00:00 – 16:00:00(morning + afternoon)
    BasicTest_Luis(it, extractor, parser, "midnight", "T00");
    BasicTest_Luis(it, extractor, parser, "mid-night", "T00");
    BasicTest_Luis(it, extractor, parser, "mid night", "T00");
    BasicTest_Luis(it, extractor, parser, "midmorning", "T10");
    BasicTest_Luis(it, extractor, parser, "mid-morning", "T10");
    BasicTest_Luis(it, extractor, parser, "mid morning", "T10");
    BasicTest_Luis(it, extractor, parser, "midafternoon", "T14");
    BasicTest_Luis(it, extractor, parser, "mid-afternoon", "T14");
    BasicTest_Luis(it, extractor, parser, "mid afternoon", "T14");
    BasicTest_Luis(it, extractor, parser, "midday", "T12");
    BasicTest_Luis(it, extractor, parser, "mid-day", "T12");
    BasicTest_Luis(it, extractor, parser, "mid day", "T12");
    BasicTest_Luis(it, extractor, parser, "noon", "T12");
});

function BasicTestFuture(it, extractor, parser, text, date) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0]);
        t.is(constants.SYS_DATETIME_TIME, pr.type);
        t.deepEqual(date, pr.value.futureValue);
    });
}

function BasicTest_Luis(it, extractor, parser, text, luisValueStr) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0]);
        t.is(constants.SYS_DATETIME_TIME, pr.type);
        t.is(luisValueStr, pr.value.timex);
    });
}