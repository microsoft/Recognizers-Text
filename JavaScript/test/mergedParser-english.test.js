var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/extractorConfiguration").EnglishMergedExtractorConfiguration;
var Extractor = require("../compiled/dateTime/extractors").BaseMergedExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/parserConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/parserConfiguration").EnglishMergedParserConfiguration;
var Parser = require("../compiled/dateTime/parsers").BaseMergedParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('DateTime Merged Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTest(it, extractor, parser, referenceDate, "day after tomorrow", Constants.SYS_DATETIME_DATE);
    BasicTest(it, extractor, parser, referenceDate, "day after tomorrow at 8am", Constants.SYS_DATETIME_DATETIME);
    BasicTest(it, extractor, parser, referenceDate, "on Friday in the afternoon", Constants.SYS_DATETIME_DATETIMEPERIOD);
    BasicTest(it, extractor, parser, referenceDate, "on Friday for 3 in the afternoon", Constants.SYS_DATETIME_DATETIME);
});

describe('DateTime Merged Parser In', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTest(it, extractor, parser, referenceDate, "schedule a meeting in 8 minutes", Constants.SYS_DATETIME_DATETIME);
    BasicTest(it, extractor, parser, referenceDate, "schedule a meeting in 10 hours", Constants.SYS_DATETIME_DATETIME);
    BasicTest(it, extractor, parser, referenceDate, "schedule a meeting in 10 days", Constants.SYS_DATETIME_DATE);
    BasicTest(it, extractor, parser, referenceDate, "schedule a meeting in 3 weeks", Constants.SYS_DATETIME_DATEPERIOD);
    BasicTest(it, extractor, parser, referenceDate, "schedule a meeting in 3 months", Constants.SYS_DATETIME_DATEPERIOD);
    BasicTest(it, extractor, parser, referenceDate, "I'll be out in 3 year", Constants.SYS_DATETIME_DATEPERIOD);
});

describe('DateTime Merged Parser After-Before', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTest(it, extractor, parser, referenceDate, "after 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
    BasicTest(it, extractor, parser, referenceDate, "before 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
    BasicTest(it, extractor, parser, referenceDate, "since 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
});

describe('DateTime Merged Parser Invalid Datetime', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTest(it, extractor, parser, referenceDate, "2016-2-30", Constants.SYS_DATETIME_DATE);
    //only 2015-1 is extracted
    BasicTest(it, extractor, parser, referenceDate, "2015-1-32", Constants.SYS_DATETIME_DATEPERIOD);
    //only 2017 is extracted
    BasicTest(it, extractor, parser, referenceDate, "2017-13-12", Constants.SYS_DATETIME_DATEPERIOD);
});

describe('DateTime Merged Parser Invalid Datetime', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    BasicTestWithTwoResults(it, extractor, parser, referenceDate, "block 1 hour on my calendar tomorrow morning", Constants.SYS_DATETIME_DURATION, Constants.SYS_DATETIME_DATETIMEPERIOD);
    BasicTestWithTwoResults(it, extractor, parser, referenceDate, "Change July 22nd meeting in Bellevue to August 22nd", Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_DATE);
    BasicTestWithTwoResults(it, extractor, parser, referenceDate, "on Friday for 3 in Bellevue in the afternoon", Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_TIMEPERIOD);
});

function BasicTest(it, extractor, parser, referenceDate, text, type) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(type, pr.type.substr('datetimeV2.'.length));
    });
}

function BasicTestWithTwoResults(it, extractor, parser, referenceDate, text, startType, endType) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(2, ers.length);
        let prs = ers.map(er => { return parser.parse(er, referenceDate) });
        t.is(startType, prs[0].type.substr('datetimeV2.'.length));
        t.is(endType, prs[1].type.substr('datetimeV2.'.length));
    });
}