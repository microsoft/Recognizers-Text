var describe = require('ava-spec').describe;
import { EnglishCommonDateTimeParserConfiguration, EnglishDateParserConfiguration } from "../compiled/dateTime/english/parserConfiguration";
import { BaseDateParser } from "../compiled/dateTime/parsers";
import { EnglishDateExtractorConfiguration } from "../compiled/dateTime/english/extractorConfiguration";
import { BaseDateExtractor } from "../compiled/dateTime/extractors";
import { Constants } from "../constants";

var refrenceDay = new Date(2016, 11, 7);
var parser = new BaseDateParser(new EnglishDateParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
var extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

describe('Date Parse', it => {
    let tYear = 2016, tMonth = 11, tDay = 7;
    BasicTest_FutureAndPastDates("I'll go back on 15", new Date(tYear, tMonth, 15), new Date(tYear, tMonth - 1, 15));
    BasicTest_FutureAndPastDates("I'll go back Oct. 2", new Date(tYear + 1, 10, 2), new Date(tYear, 10, 2));
    BasicTest_FutureAndPastDates("I'll go back Oct-2", new Date(tYear + 1, 10, 2), new Date(tYear, 10, 2));
    BasicTest_FutureAndPastDates("I'll go back Oct/2", new Date(tYear + 1, 10, 2), new Date(tYear, 10, 2));
    BasicTest_FutureAndPastDates("I'll go back October. 2", new Date(tYear + 1, 10, 2), new Date(tYear, 10, 2));
    BasicTest_FutureAndPastDates("I'll go back January 12, 2016", new Date(2016, 1, 12), new Date(2016, 1, 12));
    BasicTest("I'll go back Monday January 12th, 2016", new Date(2016, 1, 12));
    BasicTest("I'll go back 02/22/2016", new Date(2016, 2, 22));
    BasicTest("I'll go back 21/04/2016", new Date(2016, 4, 21));
    BasicTest("I'll go back 21/04/16", new Date(2016, 4, 21));
    BasicTest("I'll go back 21-04-2016", new Date(2016, 4, 21));
    BasicTest_FutureAndPastDates("I'll go back on 4.22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back on 4-22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back in 4.22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back at 4-22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back on    4/22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back on 22/04", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back     4/22", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest_FutureAndPastDates("I'll go back 22/04", new Date(tYear + 1, 4, 22), new Date(tYear, 4, 22));
    BasicTest("I'll go back 2015/08/12", new Date(2015, 8, 12));
    BasicTest("I'll go back 08/12,2015", new Date(2015, 8, 12));
    BasicTest("I'll go back 08/12,15", new Date(2015, 8, 12));
    BasicTest_FutureAndPastDates("I'll go back 1st Jan", new Date(tYear + 1, 1, 1), new Date(tYear, 1, 1));
    BasicTest_FutureAndPastDates("I'll go back Jan-1", new Date(tYear + 1, 1, 1), new Date(tYear, 1, 1));
    BasicTest_FutureAndPastDates("I'll go back Wed, 22 of Jan", new Date(tYear + 1, 1, 22), new Date(tYear, 1, 22));

    BasicTest_FutureAndPastDates("I'll go back Jan first", new Date(tYear + 1, 1, 1), new Date(tYear, 1, 1));
    BasicTest_FutureAndPastDates("I'll go back May twenty-first", new Date(tYear + 1, 5, 21), new Date(tYear, 5, 21));
    BasicTest_FutureAndPastDates("I'll go back May twenty one", new Date(tYear + 1, 5, 21), new Date(tYear, 5, 21));
    BasicTest_FutureAndPastDates("I'll go back second of Aug.", new Date(tYear + 1, 8, 2), new Date(tYear, 8, 2));
    BasicTest("I'll go back twenty second of June", new Date(tYear + 1, 6, 22),
        new Date(tYear, 6, 22));

    // cases below change with reference day
    BasicTest_FutureAndPastDates("I'll go back on Friday", new Date(2016, 11, 11), new Date(2016, 11, 4));
    BasicTest_FutureAndPastDates("I'll go back |Friday", new Date(2016, 11, 11), new Date(2016, 11, 4));
    BasicTest_FutureAndPastDates("I'll go back on Fridays", new Date(2016, 11, 11), new Date(2016, 11, 4));
    BasicTest_FutureAndPastDates("I'll go back |Fridays", new Date(2016, 11, 11), new Date(2016, 11, 4));
    BasicTest("I'll go back today", new Date(2016, 11, 7));
    BasicTest("I'll go back tomorrow", new Date(2016, 11, 8));
    BasicTest("I'll go back yesterday", new Date(2016, 11, 6));
    BasicTest("I'll go back the day before yesterday", new Date(2016, 11, 5));
    BasicTest("I'll go back the day after tomorrow", new Date(2016, 11, 9));
    BasicTest("The day after tomorrow", new Date(2016, 11, 9));
    BasicTest("I'll go back the next day", new Date(2016, 11, 8));
    BasicTest("I'll go back next day", new Date(2016, 11, 8));
    BasicTest("I'll go back this Friday", new Date(2016, 11, 11));
    BasicTest("I'll go back next Sunday", new Date(2016, 11, 20));
    BasicTest("I'll go back last Sunday", new Date(2016, 11, 6));
    BasicTest("I'll go back this week Friday", new Date(2016, 11, 11));
    BasicTest("I'll go back nextweek Sunday", new Date(2016, 11, 20));
    BasicTest("I'll go back last week Sunday", new Date(2016, 11, 6));
    BasicTest("I'll go back last day", new Date(2016, 11, 6));
    BasicTest("I'll go back the last day", new Date(2016, 11, 6));
    BasicTest("I'll go back the day", new Date(tYear, tMonth, tDay));
    BasicTest("I'll go back 15 June 2016", new Date(2016, 6, 15));

    BasicTest_FutureAndPastDates("I'll go back the first friday of july", new Date(2017, 7, 7), new Date(2016, 7, 1));
    BasicTest("I'll go back the first friday in this month", new Date(2016, 11, 4));

    BasicTest("I'll go back in two weeks", new Date(2016, 11, 21));
    BasicTest("I'll go back two weeks from now", new Date(2016, 11, 21));
});

describe('Date Parse Luis', it => {
    BasicTest_Luis("I'll go back on 15", "XXXX-XX-15");
    BasicTest_Luis("I'll go back Oct. 2", "XXXX-10-02");
    BasicTest_Luis("I'll go back Oct/2", "XXXX-10-02");
    BasicTest_Luis("I'll go back January 12, 2018", "2018-01-12");
    BasicTest_Luis("I'll go back 21/04/2016", "2016-04-21");
    BasicTest_Luis("I'll go back on 4.22", "XXXX-04-22");
    BasicTest_Luis("I'll go back on 4-22", "XXXX-04-22");
    BasicTest_Luis("I'll go back on    4/22", "XXXX-04-22");
    BasicTest_Luis("I'll go back on 22/04", "XXXX-04-22");
    BasicTest_Luis("I'll go back 21/04/16", "2016-04-21");
    BasicTest_Luis("I'll go back 9-18-15", "2015-09-18");
    BasicTest_Luis("I'll go back 2015/08/12", "2015-08-12");
    BasicTest_Luis("I'll go back 2015/08/12", "2015-08-12");
    BasicTest_Luis("I'll go back 08/12,2015", "2015-08-12");
    BasicTest_Luis("I'll go back 1st Jan", "XXXX-01-01");
    BasicTest_Luis("I'll go back Wed, 22 of Jan", "XXXX-01-22");

    BasicTest_Luis("I'll go back Jan first", "XXXX-01-01");
    BasicTest_Luis("I'll go back May twenty-first", "XXXX-05-21");
    BasicTest_Luis("I'll go back May twenty one", "XXXX-05-21");
    BasicTest_Luis("I'll go back second of Aug.", "XXXX-08-02");
    BasicTest_Luis("I'll go back twenty second of June", "XXXX-06-22");

    // cases below change with reference day
    BasicTest_Luis("I'll go back on Friday", "XXXX-WXX-5");
    BasicTest_Luis("I'll go back |Friday", "XXXX-WXX-5");
    BasicTest_Luis("I'll go back today", "2016-11-07");
    BasicTest_Luis("I'll go back tomorrow", "2016-11-08");
    BasicTest_Luis("I'll go back yesterday", "2016-11-06");
    BasicTest_Luis("I'll go back the day before yesterday", "2016-11-05");
    BasicTest_Luis("I'll go back the day after tomorrow", "2016-11-09");
    BasicTest_Luis("The day after tomorrow", "2016-11-09");
    BasicTest_Luis("I'll go back the next day", "2016-11-08");
    BasicTest_Luis("I'll go back next day", "2016-11-08");
    BasicTest_Luis("I'll go back this Friday", "2016-11-11");
    BasicTest_Luis("I'll go back next Sunday", "2016-11-20");
    BasicTest_Luis("I'll go back the day", "2016-11-07");
    BasicTest_Luis("I'll go back 15 June 2016", "2016-06-15");
    BasicTest_Luis("I went back two days ago", "2016-11-05");
    BasicTest_Luis("I went back two years ago", "2014-11-07");

    BasicTest_Luis("I'll go back in two weeks", "2016-11-21");
    BasicTest_Luis("I'll go back two weeks from now", "2016-11-21");
});

function BasicTest(it, extractor, text, date) {
    it(text, t => {
        let er = extractor.Extract(text);
        t.is(1, er.Count);
        let pr = parser.Parse(er[0], refrenceDay);
        t.is(Constants.SYS_DATETIME_DATE, pr.Type);
        t.is(date, pr.Value.FutureValue);
        t.is(date, pr.Value.PastValue);
    });
}

function BasicTest_FutureAndPastDates(it, extractor, text, futureDate, pastDate) {
    it(text, t => {
        let er = extractor.Extract(text);
        t.is(1, er.Count);
        let pr = parser.Parse(er[0], refrenceDay);
        t.is(Constants.SYS_DATETIME_DATE, pr.Type);
        t.is(futureDate, pr.Value.FutureValue);
        t.is(pastDate, pr.Value.PastValue);
    });
}

function BasicTest_Luis(it, extractor, text, luisValueStr) {
    it(text, t => {
        let er = extractor.Extract(text);
        t.is(1, er.Count);
        let pr = parser.Parse(er[0], refrenceDay);
        t.is(Constants.SYS_DATETIME_DATE, pr.Type);
        t.is(luisValueStr, pr.Value.Timex);
    });
}