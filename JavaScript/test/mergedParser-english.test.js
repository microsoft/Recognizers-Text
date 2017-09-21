var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/mergedConfiguration").EnglishMergedExtractorConfiguration;
var Extractor = require("../compiled/dateTime/baseMerged").BaseMergedExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/baseConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/mergedConfiguration").EnglishMergedParserConfiguration;
var Parser = require("../compiled/dateTime/baseMerged").BaseMergedParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('DateTime Merged Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTest(it, extractor, parser, referenceDate, "i need a reserve for 3 peeps at a pizza joint in seattle for tonight around 8 pm", Constants.SYS_DATETIME_DATETIME);
    basicTest(it, extractor, parser, referenceDate, "Set an appointment for Easter", Constants.SYS_DATETIME_DATE);
    basicTest(it, extractor, parser, referenceDate, "day after tomorrow", Constants.SYS_DATETIME_DATE);
    basicTest(it, extractor, parser, referenceDate, "day after tomorrow at 8am", Constants.SYS_DATETIME_DATETIME);
    basicTest(it, extractor, parser, referenceDate, "on Friday in the afternoon", Constants.SYS_DATETIME_DATETIMEPERIOD);
    basicTest(it, extractor, parser, referenceDate, "on Friday for 3 in the afternoon", Constants.SYS_DATETIME_DATETIME);

    basicTest(it, extractor, parser, referenceDate, "Set appointment for tomorrow morning at 9 o'clock.", Constants.SYS_DATETIME_DATETIME);
});

describe('DateTime Merged Parser In', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTest(it, extractor, parser, referenceDate, "schedule a meeting in 8 minutes", Constants.SYS_DATETIME_DATETIME);
    basicTest(it, extractor, parser, referenceDate, "schedule a meeting in 10 hours", Constants.SYS_DATETIME_DATETIME);
    basicTest(it, extractor, parser, referenceDate, "schedule a meeting in 10 days", Constants.SYS_DATETIME_DATE);
    basicTest(it, extractor, parser, referenceDate, "schedule a meeting in 3 weeks", Constants.SYS_DATETIME_DATEPERIOD);
    basicTest(it, extractor, parser, referenceDate, "schedule a meeting in 3 months", Constants.SYS_DATETIME_DATEPERIOD);
    basicTest(it, extractor, parser, referenceDate, "I'll be out in 3 year", Constants.SYS_DATETIME_DATEPERIOD);
});

describe('DateTime Merged Parser After-Before-Since', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTest(it, extractor, parser, referenceDate, "after 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
    basicTest(it, extractor, parser, referenceDate, "before 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
    basicTest(it, extractor, parser, referenceDate, "since 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
});

describe('DateTime Merged Parser Invalid Datetime', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTest(it, extractor, parser, referenceDate, "2016-2-30", Constants.SYS_DATETIME_DATE);
    //only 2015-1 is extracted
    basicTest(it, extractor, parser, referenceDate, "2015-1-32", Constants.SYS_DATETIME_DATEPERIOD);
    //only 2017 is extracted
    basicTest(it, extractor, parser, referenceDate, "2017-13-12", Constants.SYS_DATETIME_DATEPERIOD);
});

describe('DateTime Merged Parser with two results', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "add yoga to personal calendar on monday and wednesday at 3pm",
        Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_DATETIME);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "schedule a meeting at 8 am every week ",
        Constants.SYS_DATETIME_TIME, Constants.SYS_DATETIME_SET);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "schedule second saturday of each month",
        Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_SET);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "Set an appointment for Easter Sunday",
        Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_DATE);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "block 1 hour on my calendar tomorrow morning",
        Constants.SYS_DATETIME_DURATION, Constants.SYS_DATETIME_DATETIMEPERIOD);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "Change July 22nd meeting in Bellevue to August 22nd",
        Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_DATE);

    basicTestWithTwoResults(it, extractor, parser, referenceDate, "on Friday for 3 in Bellevue in the afternoon",
        Constants.SYS_DATETIME_DATE, Constants.SYS_DATETIME_TIMEPERIOD);
});

describe('DateTime Merged Parse resolution weekday and ordinary', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestResolution(it, extractor, parser, "put make cable's wedding in my calendar for wednesday the thirty first", "not resolved", new Date(2017, 8, 15));
    basicTestResolution(it, extractor, parser, "put make cable's wedding in my calendar for wednesday the thirty first", "not resolved", new Date(2017, 9, 15));
    basicTestResolution(it, extractor, parser, "put make cable's wedding in my calendar for tuesday the thirty first", "2017-10-31", new Date(2017, 9, 15));
});

function basicTest(it, extractor, parser, referenceDate, text, type) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(type, pr.type.substr('datetimeV2.'.length));
    });
}

function basicTestWithTwoResults(it, extractor, parser, referenceDate, text, startType, endType) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(2, ers.length);
        let prs = ers.map(er => { return parser.parse(er, referenceDate) });
        t.is(startType, prs[0].type.substr('datetimeV2.'.length));
        t.is(endType, prs[1].type.substr('datetimeV2.'.length));
    });
}

function basicTestResolution(it, extractor, parser, text, resolution, refDate) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], refDate)
        let prValue = pr.value.get("values");
        t.is(resolution, prValue[0].get("value"));
    });
}
