var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/datePeriodConfiguration").EnglishDatePeriodExtractorConfiguration;
var Extractor = require("../compiled/dateTime/baseDatePeriod").BaseDatePeriodExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/baseConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/datePeriodConfiguration").EnglishDatePeriodParserConfiguration;
var Parser = require("../compiled/dateTime/baseDatePeriod").BaseDatePeriodParser;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Period Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);
    let year = referenceDate.getFullYear();
    let month = referenceDate.getMonth();
    let day = referenceDate.getDate();
    let inclusiveEnd = false;

    // test basic cases
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out from 4 to 22 this month", 4, 22, month, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out from 4-23 in next month", 4, 23, 11, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out from 3 until 12 of Sept hahaha", 3, 12, 8, year + 1);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out 4 to 23 next month", 4, 23, 11, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out 4 till 23 of this month", 4, 23, month, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out between 4 and 22 this month", 4, 22, month, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out between 3 and 12 of Sept hahaha", 3, 12, 8, year + 1);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out from 4 to 22 January, 1995", 4, 22, 0, 1995);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out between 4-22 January, 1995", 4, 22, 0, 1995);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out between september 4th through september 8th", 4, 8, 8, year+1);

    if (inclusiveEnd) {
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out on this week", 7, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out on current week", 7, 13, month, year);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out February", year + 1, 1, 1, year + 1, 1, 28);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out this September", year, 8, 1, year, 8, 30);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out last sept", year - 1, 8, 1, year - 1, 8, 30);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out next june", year + 1, 5, 1, year + 1, 5, 30);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out the third week of this month", 21, 27, month, year);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out the last week of july", year + 1, 6, 24, year + 1, 6, 30);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "week of september.16th", 11, 17, 8, year + 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "month of september.16th", year, 8, 1, year, 8, 30);
    } else {
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out on this week", 7, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out on current week", 7, 14, month, year);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out February", year + 1, 1, 1, year + 1, 2, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out this September", year, 8, 1, year, 9, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out last sept", year - 1, 8, 1, year - 1, 9, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out next june", year + 1, 5, 1, year + 1, 6, 1);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out the third week of this month", 21, 28, month, year);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out the last week of july", year + 1, 6, 24, year + 1, 6, 31);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "week of september.16th", 11, 18, 8, year + 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "month of september.16th", year + 1, 8, 1, year + 1, 9, 1);
    }

    if (inclusiveEnd)
    {
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015.3", 2015, 2, 1, 2015, 2, 31);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015-3", 2015, 2, 1, 2015, 2, 31);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015/3", 2015, 2, 1, 2015, 2, 31);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 3/2015", 2015, 2, 1, 2015, 2, 31);
    }
    else
    {
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015.3", 2015, 2, 1, 2015, 3, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015-3", 2015, 2, 1, 2015, 3, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 2015/3", 2015, 2, 1, 2015, 3, 1);
        BasicTestFutureFullDate(it, extractor, parser, referenceDate, "I'll be out 3/2015", 2015, 2, 1, 2015, 3, 1);
    }

    // Duration
    if (inclusiveEnd) {
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "scheduel a meeting in two weeks", 15, 21, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "next 2 days", 8, 9, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "past few days", 4, 6, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "the week", 7, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "this week", 7, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "my week", 7, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "the weekend", 12, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "this weekend", 12, 13, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "my weekend", 12, 13, month, year);
    } else {
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "scheduel a meeting in two weeks", 15, 22, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "next 2 days", 8, 10, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "past few days", 4, 7, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "the week", 7, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "this week", 7, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "my week", 7, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "the weekend", 12, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "this weekend", 12, 14, month, year);
        BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "my weekend", 12, 14, month, year);
    }

    // test merging two time points
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out Oct. 2 to October 22", 2, 22, 9, year + 1);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out January 12, 2016 - 01/22/2016", 12, 22, 0, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out 1st Jan until Wed, 22 of Jan", 1, 22, 0, year + 1);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out today till tomorrow", 7, 8, month, year);

    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out from Oct. 2 to October 22", 2, 22, 9, year + 1);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out between Oct. 2 and October 22", 2, 22, 9, year + 1);

    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out November 19-20", 19, 20, 10, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out November 19 to 20", 19, 20, 10, year);
    BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, "I'll be out November between 19 and 20", 19, 20, 10, year);
});

describe('Date Period Parser Luis', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    // test basic cases
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 4 to 22 this month", "(2016-11-04,2016-11-22,P18D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 4-23 in next month", "(2016-12-04,2016-12-23,P19D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out from 3 until 12 of Sept hahaha", "(XXXX-09-03,XXXX-09-12,P9D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 4 to 23 next month", "(2016-12-04,2016-12-23,P19D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 4 till 23 of this month", "(2016-11-04,2016-11-23,P19D)");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out on this week", "2016-W45");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out on weekend", "2016-W45-WE");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out on this weekend", "2016-W45-WE");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out February", "XXXX-02");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out this September", "2016-09");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out last sept", "2015-09");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out next june", "2017-06");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out june 2016", "2016-06");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out june next year", "2017-06");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out next year", "2017");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out next 3 days", "(2016-11-08,2016-11-11,P3D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out next 3 months", "(2016-11-08,2017-02-08,P3M)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out in 3 year", "(2018-11-08,2019-11-08,P1Y)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out past 3 weeks", "(2016-10-17,2016-11-07,P3W)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out last 3year", "(2013-11-07,2016-11-07,P3Y)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out previous 3 weeks", "(2016-10-17,2016-11-07,P3W)");

    // test merging two time points
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out Oct. 2 to October 22", "(XXXX-10-02,XXXX-10-22,P20D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out January 12, 2016 - 01/22/2016", "(2016-01-12,2016-01-22,P10D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out today till tomorrow", "(2016-11-07,2016-11-08,P1D)");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out from Oct. 2 to October 22", "(XXXX-10-02,XXXX-10-22,P20D)");

    BasicTestLuis(it, extractor, parser, referenceDate, "the first week of Oct", "XXXX-10-W01");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out the third week of 2027", "2027-01-W03");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out the third week next year", "2017-01-W03");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out November 19-20", "(XXXX-11-19,XXXX-11-20,P1D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out November 19 to 20", "(XXXX-11-19,XXXX-11-20,P1D)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out November between 19 and 20", "(XXXX-11-19,XXXX-11-20,P1D)");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out the third quarter of 2016", "(2016-07-01,2016-10-01,P3M)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out the third quarter this year", "(2016-07-01,2016-10-01,P3M)");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 2016 the third quarter", "(2016-07-01,2016-10-01,P3M)");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 2015.3", "2015-03");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 2015-3", "2015-03");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 2015/3", "2015-03");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll be out 3/2015", "2015-03");

    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave this summer", "2016-SU");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave next spring", "2017-SP");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave the summer", "SU");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave summer", "SU");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave summer 2016", "2016-SU");
    BasicTestLuis(it, extractor, parser, referenceDate, "I'll leave summer of 2016", "2016-SU");

    //next and upcoming
    BasicTestLuis(it, extractor, parser, referenceDate, "upcoming month holidays", "2016-12");
    BasicTestLuis(it, extractor, parser, referenceDate, "next month holidays", "2016-12");
});

function BasicTestFutureOnlyDay(it, extractor, parser, referenceDate, text, beginDay, endDay, month, year) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(Constants.SYS_DATETIME_DATEPERIOD, pr.type);
        let beginDate = new Date(year, month, beginDay);
        let endDate = new Date(year, month, endDay);
        t.deepEqual(beginDate, pr.value.futureValue[0]);
        t.deepEqual(endDate, pr.value.futureValue[1]);
    });
}

function BasicTestFutureFullDate(it, extractor, parser, referenceDate, text, beginYear, beginMonth, beginDay, endYear, endMonth, endDay) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(Constants.SYS_DATETIME_DATEPERIOD, pr.type);
        let beginDate = new Date(beginYear, beginMonth, beginDay);
        let endDate = new Date(endYear, endMonth, endDay);
        t.deepEqual(beginDate, pr.value.futureValue[0]);
        t.deepEqual(endDate, pr.value.futureValue[1]);
    });
}

function BasicTestLuis(it, extractor, parser, referenceDate, text, luisDate) {
    it(text, t => {
        let er = extractor.extract(text);
        t.is(1, er.length);
        let pr = parser.parse(er[0], referenceDate)
        t.is(Constants.SYS_DATETIME_DATEPERIOD, pr.type);
        t.is(luisDate, pr.value.timex);
    });
}