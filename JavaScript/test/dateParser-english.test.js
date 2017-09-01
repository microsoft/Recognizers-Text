var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/extractorConfiguration").EnglishDateExtractorConfiguration;
var Extractor = require("../compiled/dateTime/extractors").BaseDateExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/parserConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/parserConfiguration").EnglishDateParserConfiguration;
var Parser = require("../compiled/dateTime/parsers").BaseDateParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);
    let tYear = referenceDate.getFullYear();
    let tMonth = referenceDate.getMonth();
    let tDay = referenceDate.getDate();

    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 15", new Date(tYear, tMonth, 15), new Date(tYear, tMonth - 1, 15));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct. 2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct-2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct/2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back October. 2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back January 12, 2016", new Date(2016, 0, 12), new Date(2016, 0, 12));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back Monday January 12th, 2016", new Date(2016, 0, 12));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 02/22/2016", new Date(2016, 1, 22));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21/04/2016", new Date(2016, 3, 21));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21/04/16", new Date(2016, 3, 21));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21-04-2016", new Date(2016, 3, 21));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 4.22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 4-22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back in 4.22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back at 4-22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on    4/22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 22/04", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back     4/22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back 22/04", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", new Date(2015, 7, 12));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 08/12,2015", new Date(2015, 7, 12));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 08/12,15", new Date(2015, 7, 12));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back 1st Jan", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Jan-1", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Wed, 22 of Jan", new Date(tYear + 1, 0, 22), new Date(tYear, 0, 22));

    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Jan first", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back May twenty-first", new Date(tYear + 1, 4, 21), new Date(tYear, 4, 21));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back May twenty one", new Date(tYear + 1, 4, 21), new Date(tYear, 4, 21));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back second of Aug.", new Date(tYear + 1, 7, 2), new Date(tYear, 7, 2));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back twenty second of June", new Date(tYear + 1, 5, 22), new Date(tYear, 5, 22));

    // cases below change with reference day
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on Friday", new Date(2016, 10, 11), new Date(2016, 10, 4));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back |Friday", new Date(2016, 10, 11), new Date(2016, 10, 4));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on Fridays", new Date(2016, 10, 11), new Date(2016, 10, 4));
    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back |Fridays", new Date(2016, 10, 11), new Date(2016, 10, 4));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back today", new Date(2016, 10, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back tomorrow", new Date(2016, 10, 8));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back yesterday", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day before yesterday", new Date(2016, 10, 5));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day after tomorrow", new Date(2016, 10, 9));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "The day after tomorrow", new Date(2016, 10, 9));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the next day", new Date(2016, 10, 8));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next day", new Date(2016, 10, 8));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this Friday", new Date(2016, 10, 11));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next Sunday", new Date(2016, 10, 20));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last Sunday", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this week Friday", new Date(2016, 10, 11));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next week Sunday", new Date(2016, 10, 20));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last week Sunday", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last day", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the last day", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day", new Date(tYear, tMonth, tDay));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 15 June 2016", new Date(2016, 5, 15));

    BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back the first friday of july", new Date(2017, 6, 7), new Date(2016, 6, 1));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the first friday in this month", new Date(2016, 10, 4));

    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next week on Friday", new Date(2016, 10, 18));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back on Friday next week", new Date(2016, 10, 18));
});

describe('Date Parser The Day', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);
    
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next day", new Date(2016, 10, 8));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day", new Date(2016, 10, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back my day", new Date(2016, 10, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this day", new Date(2016, 10, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last day", new Date(2016, 10, 6));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back past day", new Date(2016, 10, 6));
});


describe('Date Parser Ago Later', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back two weeks from now", new Date(2016, 10, 21));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email a month ago", new Date(2016, 9, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email few month ago", new Date(2016, 7, 7));
    BasicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email a few day ago", new Date(2016, 10, 4));
});

describe('Date Parser LUIS', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on 15", "XXXX-XX-15");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back Oct. 2", "XXXX-10-02");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back Oct/2", "XXXX-10-02");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back January 12, 2018", "2018-01-12");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 21/04/2016", "2016-04-21");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on 4.22", "XXXX-04-22");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on 4-22", "XXXX-04-22");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on    4/22", "XXXX-04-22");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on 22/04", "XXXX-04-22");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 21/04/16", "2016-04-21");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 9-18-15", "2015-09-18");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", "2015-08-12");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", "2015-08-12");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 08/12,2015", "2015-08-12");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 1st Jan", "XXXX-01-01");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back Wed, 22 of Jan", "XXXX-01-22");

    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back Jan first", "XXXX-01-01");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back May twenty-first", "XXXX-05-21");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back May twenty one", "XXXX-05-21");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back second of Aug.", "XXXX-08-02");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back twenty second of June", "XXXX-06-22");

    // cases below change with reference day
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on Friday", "XXXX-WXX-5");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back |Friday", "XXXX-WXX-5");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back today", "2016-11-07");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back tomorrow", "2016-11-08");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back yesterday", "2016-11-06");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back the day before yesterday", "2016-11-05");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back the day after tomorrow", "2016-11-09");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "The day after tomorrow", "2016-11-09");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back the next day", "2016-11-08");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back next day", "2016-11-08");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back this Friday", "2016-11-11");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back next Sunday", "2016-11-20");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back the day", "2016-11-07");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back 15 June 2016", "2016-06-15");

    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I went back two days ago", "2016-11-05");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I went back two years ago", "2014-11-07");

    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back two weeks from now", "2016-11-21");

    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back next week on Friday", "2016-11-18");
    BasicTestWithLUIS(it, extractor, parser, referenceDate, "I'll go back on Friday next week", "2016-11-18");
});

function BasicTestWithFutureAndPast(it, extractor, parser, referenceDate, text, futureDate, pastDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.deepEqual(futureDate, result.value.futureValue);
        t.deepEqual(pastDate, result.value.pastValue);
    });
}

function BasicTestWithOneDate(it, extractor, parser, referenceDate, text, resultDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.deepEqual(resultDate, result.value.futureValue);
        t.deepEqual(resultDate, result.value.pastValue);
    });
}

function BasicTestWithLUIS(it, extractor, parser, referenceDate, text, luisDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.is(luisDate, result.value.timex);
    });
}