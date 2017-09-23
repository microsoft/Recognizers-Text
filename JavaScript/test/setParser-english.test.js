var describe = require('ava-spec').describe;
var EnglishSetExtractorConfiguration = require('../compiled/dateTime/english/setConfiguration').EnglishSetExtractorConfiguration;
var BaseSetExtractor = require('../compiled/dateTime/baseSet').BaseSetExtractor;
var EnglishCommonDateTimeParserConfiguration = require('../compiled/dateTime/english/baseConfiguration').EnglishCommonDateTimeParserConfiguration;
var EnglishSetParserConfiguration = require('../compiled/dateTime/english/setConfiguration').EnglishSetParserConfiguration;
var BaseSetParser = require('../compiled/dateTime/baseSet').BaseSetParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Set Parser', it => {
    let extractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration());
    let parser = new BaseSetParser(new EnglishSetParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

    basicTest(it, extractor, parser, "I'll leave every night at 9", "Set: T21", "T21");

    basicTest(it, extractor, parser, "I'll leave weekly", "Set: P1W", "P1W");
    basicTest(it, extractor, parser, "I'll leave biweekly", "Set: P2W", "P2W");
    basicTest(it, extractor, parser, "I'll leave daily", "Set: P1D", "P1D");
    basicTest(it, extractor, parser, "I'll leave every day", "Set: P1D", "P1D");
    basicTest(it, extractor, parser, "I'll leave each month", "Set: P1M", "P1M");
    basicTest(it, extractor, parser, "I'll leave annually", "Set: P1Y", "P1Y");
    basicTest(it, extractor, parser, "I'll leave annual", "Set: P1Y", "P1Y");

    basicTest(it, extractor, parser, "I'll leave each two days", "Set: P2D", "P2D");
    basicTest(it, extractor, parser, "I'll leave every three week", "Set: P3W", "P3W");

    basicTest(it, extractor, parser, "I'll leave 3pm every day", "Set: T15", "T15");
    basicTest(it, extractor, parser, "I'll leave 3pm each day", "Set: T15", "T15");

    basicTest(it, extractor, parser, "I'll leave each 4/15", "Set: XXXX-04-15", "XXXX-04-15");
    basicTest(it, extractor, parser, "I'll leave every monday", "Set: XXXX-WXX-1", "XXXX-WXX-1");
    basicTest(it, extractor, parser, "I'll leave each monday 4pm", "Set: XXXX-WXX-1T16", "XXXX-WXX-1T16");

    basicTest(it, extractor, parser, "I'll leave every morning", "Set: TMO", "TMO");

    // SetParse_EveryOther
    basicTest(it, extractor, parser, "every other day", "Set: P2D", "P2D");
    basicTest(it, extractor, parser, "every other week", "Set: P2W", "P2W");
    basicTest(it, extractor, parser, "every other month", "Set: P2M", "P2M");

    // SetParseTimePeriod_Time
    basicTest(it, extractor, parser, "I'll leave every morning at 9am", "Set: T09", "T09");
    basicTest(it, extractor, parser, "I'll leave every afternoon at 4pm", "Set: T16", "T16");
    basicTest(it, extractor, parser, "I'll leave every night at 9pm", "Set: T21", "T21");
    basicTest(it, extractor, parser, "I'll leave every night at 9", "Set: T21", "T21");
    basicTest(it, extractor, parser, "I'll leave mornings at 9am", "Set: T09", "T09");
    basicTest(it, extractor, parser, "I'll leave on mornings at 9", "Set: T09", "T09");

    // SetExtractMergeDate_Time
    basicTest(it, extractor, parser, "I'll leave at 9am every Sunday", "Set: XXXX-WXX-7T09", "XXXX-WXX-7T09");
    basicTest(it, extractor, parser, "I'll leave at 9am on Sundays", "Set: XXXX-WXX-7T09", "XXXX-WXX-7T09");
    basicTest(it, extractor, parser, "I'll leave at 9am Sundays", "Set: XXXX-WXX-7T09", "XXXX-WXX-7T09");

    // SetExtractDate
    basicTest(it, extractor, parser, "I'll leave on Mondays", "Set: XXXX-WXX-1", "XXXX-WXX-1");
    basicTest(it, extractor, parser, "I'll leave on Sundays", "Set: XXXX-WXX-7", "XXXX-WXX-7");
    basicTest(it, extractor, parser, "I'll leave Sundays", "Set: XXXX-WXX-7", "XXXX-WXX-7");
});

function basicTest(it, extractor, parser, text, value, luisValue) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0]);
        t.is(Constants.SYS_DATETIME_SET, pr.type);
        t.is(value, pr.value.futureValue); 
        t.is(luisValue, pr.timexStr);
    });
}
