var describe = require('ava-spec').describe;
var EnglishHolidayExtractorConfiguration = require('../compiled/dateTime/english/extractorConfiguration').EnglishHolidayExtractorConfiguration;
var BaseHolidayExtractor = require('../compiled/dateTime/extractors').BaseHolidayExtractor;
var EnglishHolidayParserConfiguration = require('../compiled/dateTime/english/parserConfiguration').EnglishHolidayParserConfiguration;
var BaseHolidayParser = require('../compiled/dateTime/parsers').BaseHolidayParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Holiday Parse', it => {
    let refrenceDay = new Date(2016, 11 - 1, 7);
    let extractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
    let parser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());

    // All date's month are zero-based (-1)
    BasicTest(it, extractor, parser, referenceDay, "I'll go back on new year eve",
        new Date(2016, 12 - 1, 31),
        new Date(2015, 12 - 1, 31));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on new year's eve",
        new Date(2016, 12 - 1, 31),
        new Date(2015, 12 - 1, 31));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on christmas",
        new Date(2016, 12 - 1, 25),
        new Date(2015, 12 - 1, 25));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on Yuandan",
        new Date(2017, 1 - 1, 1),
        new Date(2016, 1 - 1, 1));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanks giving day",
        new Date(2016, 11 - 1, 24),
        new Date(2015, 11 - 1, 26));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanksgiving",
        new Date(2016, 11 - 1, 24),
        new Date(2015, 11 - 1, 26));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on father's day",
        new Date(2017, 6 - 1, 18),
        new Date(2016, 6 - 1, 19));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on Yuandan of next year",
        new Date(2017, 1 - 1, 1),
        new Date(2017, 1 - 1, 1));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on thanks giving day 2010",
        new Date(2010, 11 - 1, 25),
        new Date(2010, 11 - 1, 25));

    BasicTest(it, extractor, parser, referenceDay, "I'll go back on father's day of 2015",
        new Date(2015, 6 - 1, 21),
        new Date(2015, 6 - 1, 21));
});

describe('Holiday Luis', it => {
    let refrenceDay = new Date(2016, 11 - 1, 7);
    let extractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
    let parser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());

    BasicTest("I'll go back on Yuandan", "XXXX-01-01");
    BasicTest("I'll go back on thanks giving day", "XXXX-11-WXX-4-4");
    BasicTest("I'll go back on father's day", "XXXX-06-WXX-6-3");

    BasicTest("I'll go back on Yuandan of next year", "2017-01-01");
    BasicTest("I'll go back on thanks giving day 2010", "2010-11-WXX-4-4");
    BasicTest("I'll go back on father's day of 2015", "2015-06-WXX-6-3");
});

function BasicTest(it, extractor, parser, referenceDay, text, futureTime, pastTime) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDay);
        t.is(Constants.SYS_DATETIME_DATE, pr.type);
        t.deepEqual(futureDate, pr.value.futureValue);
        t.deepEqual(pastDate, pr.value.pastValue);
    });
}

function BasicTest_Luis(it, extractor, parser, referenceDay, text, luisValueStr) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDay);
        t.is(Constants.SYS_DATETIME_DATE, pr.type);
        t.is(luisValueStr, pr.value.timex);
    });
}