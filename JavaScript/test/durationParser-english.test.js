var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/durationConfiguration").EnglishDurationExtractorConfiguration;
var Extractor = require("../compiled/dateTime/baseDuration").BaseDurationExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/baseConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/durationConfiguration").EnglishDurationParserConfiguration;
var Parser = require("../compiled/dateTime/baseDuration").BaseDurationParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Duration Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));

    basicTest(it, extractor, parser, "I'll leave for 3h", 10800, "PT3H");
    basicTest(it, extractor, parser, "I'll leave for 3day", 259200, "P3D");
    basicTest(it, extractor, parser, "I'll leave for 3.5years", 110376000, "P3.5Y");

    basicTest(it, extractor, parser, "I'll leave for 3 h", 10800, "PT3H");
    basicTest(it, extractor, parser, "I'll leave for 3 hours", 10800, "PT3H");
    basicTest(it, extractor, parser, "I'll leave for 3 hrs", 10800, "PT3H");
    basicTest(it, extractor, parser, "I'll leave for 3 hr", 10800, "PT3H");
    basicTest(it, extractor, parser, "I'll leave for 3 day", 259200, "P3D");
    basicTest(it, extractor, parser, "I'll leave for 3 months", 7776000, "P3M");
    basicTest(it, extractor, parser, "I'll leave for 3 minutes", 180, "PT3M");
    basicTest(it, extractor, parser, "I'll leave for 3 min", 180, "PT3M");
    basicTest(it, extractor, parser, "I'll leave for 3.5 second ", 3.5, "PT3.5S");
    basicTest(it, extractor, parser, "I'll leave for 123.45 sec", 123.45, "PT123.45S");
    basicTest(it, extractor, parser, "I'll leave for two weeks", 1209600, "P2W");
    basicTest(it, extractor, parser, "I'll leave for twenty min", 1200, "PT20M");
    basicTest(it, extractor, parser, "I'll leave for twenty and four hours", 86400, "PT24H");

    basicTest(it, extractor, parser, "I'll leave for all day", 86400, "P1D");
    basicTest(it, extractor, parser, "I'll leave for all week", 604800, "P1W");
    basicTest(it, extractor, parser, "I'll leave for all month", 2592000, "P1M");
    basicTest(it, extractor, parser, "I'll leave for all year", 31536000, "P1Y");

    basicTest(it, extractor, parser, "I'll leave for an hour", 3600, "PT1H");

    basicTest(it, extractor, parser, "half year", 15768000, "P0.5Y");
    basicTest(it, extractor, parser, "half an year", 15768000, "P0.5Y");

    basicTest(it, extractor, parser, "I'll leave for 3-min", 180, "PT3M");
    basicTest(it, extractor, parser, "I'll leave for 30-minutes", 1800, "PT30M");

    basicTest(it, extractor, parser, "I'll leave for an hour and a half", 5400, "PT1.5H");
    basicTest(it, extractor, parser, "I'll leave for an hour and half", 5400, "PT1.5H");
    basicTest(it, extractor, parser, "I'll leave for half hour", 1800, "PT0.5H");

    basicTest(it, extractor, parser, "I'll leave for two hour", 7200, "PT2H");
    basicTest(it, extractor, parser, "I'll leave for two and a half hours", 9000, "PT2.5H");
});

function basicTest(it, extractor, parser, text, dateValue, luisValue) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], new Date(2016, 10, 7))
        t.is(dateValue, pr.value.futureValue);
        t.is(luisValue, pr.value.timex);
        t.is(Constants.SYS_DATETIME_DURATION, pr.type);
    });
}