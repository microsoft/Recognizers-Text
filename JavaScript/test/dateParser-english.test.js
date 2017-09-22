var describe = require('ava-spec').describe;
var ExtractorConfig = require("../compiled/dateTime/english/dateConfiguration").EnglishDateExtractorConfiguration;
var Extractor = require("../compiled/dateTime/baseDate").BaseDateExtractor;
var CommonParserConfig = require("../compiled/dateTime/english/baseConfiguration").EnglishCommonDateTimeParserConfiguration;
var ParserConfig = require("../compiled/dateTime/english/dateConfiguration").EnglishDateParserConfiguration;
var Parser = require("../compiled/dateTime/baseDate").BaseDateParser;
var Constants = require('../compiled/dateTime/constants').Constants;
var DateUtils = require('../compiled/dateTime/utilities').DateUtils;

describe('Date Parser', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);
    let tYear = referenceDate.getFullYear();
    let tMonth = referenceDate.getMonth();
    let tDay = referenceDate.getDate();

    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 15", new Date(tYear, tMonth, 15), new Date(tYear, tMonth - 1, 15));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct. 2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct-2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Oct/2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back October. 2", new Date(tYear + 1, 9, 2), new Date(tYear, 9, 2));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back January 12, 2016", new Date(2016, 0, 12), new Date(2016, 0, 12));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back Monday January 12th, 2016", new Date(2016, 0, 12));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 02/22/2016", new Date(2016, 1, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21/04/2016", new Date(2016, 3, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21/04/16", new Date(2016, 3, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 21-04-2016", new Date(2016, 3, 21));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 4.22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 4-22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back in 4.22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back at 4-22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on    4/22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on 22/04", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back     4/22", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back 22/04", new Date(tYear + 1, 3, 22), new Date(tYear, 3, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", new Date(2015, 7, 12));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 08/12,2015", new Date(2015, 7, 12));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 08/12,15", new Date(2015, 7, 12));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back 1st Jan", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Jan-1", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Wed, 22 of Jan", new Date(tYear + 1, 0, 22), new Date(tYear, 0, 22));

    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back Jan first", new Date(tYear + 1, 0, 1), new Date(tYear, 0, 1));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back May twenty-first", new Date(tYear + 1, 4, 21), new Date(tYear, 4, 21));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back May twenty one", new Date(tYear + 1, 4, 21), new Date(tYear, 4, 21));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back second of Aug.", new Date(tYear + 1, 7, 2), new Date(tYear, 7, 2));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back twenty second of June", new Date(tYear + 1, 5, 22), new Date(tYear, 5, 22));

    // cases below change with reference day
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back on Friday", new Date(2016, 10, 11), new Date(2016, 10, 4));
    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back |Friday", new Date(2016, 10, 11), new Date(2016, 10, 4));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back today", new Date(2016, 10, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back tomorrow", new Date(2016, 10, 8));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back yesterday", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day before yesterday", new Date(2016, 10, 5));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day after tomorrow", new Date(2016, 10, 9));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "The day after tomorrow", new Date(2016, 10, 9));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the next day", new Date(2016, 10, 8));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next day", new Date(2016, 10, 8));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this Friday", new Date(2016, 10, 11));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next Sunday", new Date(2016, 10, 20));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last Sunday", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this week Friday", new Date(2016, 10, 11));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next week Sunday", new Date(2016, 10, 20));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last week Sunday", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last day", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the last day", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day", new Date(tYear, tMonth, tDay));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back 15 June 2016", new Date(2016, 5, 15));

    basicTestWithFutureAndPast(it, extractor, parser, referenceDate, "I'll go back the first friday of july", new Date(2017, 6, 7), new Date(2016, 6, 1));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the first friday in this month", new Date(2016, 10, 4));

    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next week on Friday", new Date(2016, 10, 18));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back on Friday next week", new Date(2016, 10, 18));
});

describe('Date Parser The Day', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);
    
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back next day", new Date(2016, 10, 8));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back the day", new Date(2016, 10, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back my day", new Date(2016, 10, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back this day", new Date(2016, 10, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back last day", new Date(2016, 10, 6));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back past day", new Date(2016, 10, 6));
});

describe('Date Parser Ago Later', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back two weeks from now", new Date(2016, 10, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email a month ago", new Date(2016, 9, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email few month ago", new Date(2016, 7, 7));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "who did I email a few day ago", new Date(2016, 10, 4));
});

describe('Date Parser For The', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 27", new Date(2016, 10, 27));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 27th", new Date(2016, 10, 27));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 27.", new Date(2016, 10, 27));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 27!", new Date(2016, 10, 27));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 27 .", new Date(2016, 10, 27));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 21st", new Date(2016, 10, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the 22nd", new Date(2016, 10, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the second", new Date(2016, 10, 2));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the twenty second", new Date(2016, 10, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back for the thirty", new Date(2016, 10, 30));
});

describe('Date Parser Weekday And Day of Month', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date();
    let year = referenceDate.getFullYear();
    let month = referenceDate.getMonth();

    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(21) + " the 21st", new Date(year, month, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(22) + " the 22nd", new Date(year, month, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(23) + " the 23rd", new Date(year, month, 23));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(15) + " the 15th", new Date(year, month, 15));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(21) + " the twenty first", new Date(year, month, 21));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(22) + " the twenty second", new Date(year, month, 22));
    basicTestWithOneDate(it, extractor, parser, referenceDate, getIWentBack(15) + " the fifteen", new Date(year, month, 15));
});

describe('Date Parser Relative Day of Week', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date();

    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back second Sunday", getRelativeWeekDay(2, 0, referenceDate));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back first Sunday", getRelativeWeekDay(1, 0, referenceDate));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back third Tuesday", getRelativeWeekDay(3, 2, referenceDate));
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back third Tuesday", getRelativeWeekDay(3, 2, referenceDate));
    // Negative case
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I'll go back fifth Sunday", getRelativeWeekDay(5, 0, referenceDate));
});

describe('Date Parser OdNum Relative Month', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back 20th of next month", new Date(2016, 12 - 1, 20));
    // Negative cases
    basicTestWithOneDate(it, extractor, parser, referenceDate, "I went back 31st of this month", DateUtils.minValue());
});

describe('Date Parser LUIS', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on 15", "XXXX-XX-15");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back Oct. 2", "XXXX-10-02");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back Oct/2", "XXXX-10-02");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back January 12, 2018", "2018-01-12");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 21/04/2016", "2016-04-21");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on 4.22", "XXXX-04-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on 4-22", "XXXX-04-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on    4/22", "XXXX-04-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on 22/04", "XXXX-04-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 21/04/16", "2016-04-21");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 9-18-15", "2015-09-18");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", "2015-08-12");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 2015/08/12", "2015-08-12");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 08/12,2015", "2015-08-12");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 1st Jan", "XXXX-01-01");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back Wed, 22 of Jan", "XXXX-01-22");

    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back Jan first", "XXXX-01-01");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back May twenty-first", "XXXX-05-21");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back May twenty one", "XXXX-05-21");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back second of Aug.", "XXXX-08-02");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back twenty second of June", "XXXX-06-22");

    // cases below change with reference day
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on Friday", "XXXX-WXX-5");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back |Friday", "XXXX-WXX-5");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back today", "2016-11-07");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back tomorrow", "2016-11-08");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back yesterday", "2016-11-06");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back the day before yesterday", "2016-11-05");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back the day after tomorrow", "2016-11-09");
    basicTestWithLuis(it, extractor, parser, referenceDate, "The day after tomorrow", "2016-11-09");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back the next day", "2016-11-08");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back next day", "2016-11-08");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back this Friday", "2016-11-11");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back next Sunday", "2016-11-20");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back the day", "2016-11-07");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back 15 June 2016", "2016-06-15");

    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back two days ago", "2016-11-05");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back two years ago", "2014-11-07");

    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back two weeks from now", "2016-11-21");

    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back next week on Friday", "2016-11-18");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back on Friday next week", "2016-11-18");
});

describe('Date Parser LUIS For the', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 27", "XXXX-XX-27");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 27th", "XXXX-XX-27");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 27.", "XXXX-XX-27");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 27!", "XXXX-XX-27");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 27 .", "XXXX-XX-27");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 21st", "XXXX-XX-21");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the 22nd", "XXXX-XX-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the second", "XXXX-XX-02");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the twenty second", "XXXX-XX-22");
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back for the thirty", "XXXX-XX-30");
});

describe('Date Parser LUIS weekday', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date();

    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(21) + " the 21st", getLuisDateFor(21));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(22) + " the 22nd", getLuisDateFor(22));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(23) + " the 23rd", getLuisDateFor(23));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(15) + " the 15th", getLuisDateFor(15));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(21) + " the twenty first", getLuisDateFor(21));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(22) + " the twenty second", getLuisDateFor(22));
    basicTestWithLuis(it, extractor, parser, referenceDate, getIWentBack(15) + " the fifteen", getLuisDateFor(15));
});

describe('Date Parser Relative Day of Week LUIS', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date();

    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back second Sunday", getRelativeLUISWeekDay(2, 0, referenceDate));
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back first Sunday", getRelativeLUISWeekDay(1, 0, referenceDate));
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back third Tuesday", getRelativeLUISWeekDay(3, 2, referenceDate));
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back third Tuesday", getRelativeLUISWeekDay(3, 2, referenceDate));
    // Negative case
    basicTestWithLuis(it, extractor, parser, referenceDate, "I'll go back fifth Sunday", getRelativeLUISWeekDay(5, 0, referenceDate));
});

describe('Date Parser OdNum Relative Month LUIS', it => {
    let extractor = new Extractor(new ExtractorConfig());
    let parser = new Parser(new ParserConfig(new CommonParserConfig()));
    let referenceDate = new Date(2016, 10, 7);

    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back 20th of next month", "2016-12-20");
    // Negative cases
    basicTestWithLuis(it, extractor, parser, referenceDate, "I went back 31st of this month", "2016-11-31");
});

function getWeekDay(dayOfMonth) {
    let weekDays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
    let weekDay = 'None';
    if (dayOfMonth >= 1 && dayOfMonth <= 31) {
        let referenceDate = new Date();
        let dummyDate = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), dayOfMonth);
        weekDay = weekDays[dummyDate.getDay()];
    }
    return weekDay;
}

function getIWentBack(dayOfMonth) {
    return 'I went back ' + getWeekDay(dayOfMonth);
}

function getLuisDateFor(dayOfMonth) {
    let referenceDate = new Date();
    let yearStr = '0000' + referenceDate.getFullYear();
    let monthStr = '00' + (referenceDate.getMonth() + 1);
    let dayOfMonthStr = '00' + dayOfMonth;
    return `${yearStr.substr(yearStr.length - 4)}-${monthStr.substr(monthStr.length - 2)}-${dayOfMonthStr.substr(dayOfMonthStr.length - 2)}`;
}

function getRelativeWeekDay(ordinalNum, wantedWeekDay, refDate) {
    let firstDate = new Date(refDate.getFullYear(), refDate.getMonth(), 1);
    let firstWeekDay = firstDate.getDay();
    let firstWantedWeekDate = new Date(firstDate);
    firstWantedWeekDate.setDate(firstDate.getDate() + ((wantedWeekDay > firstWeekDay) ? wantedWeekDay - firstWeekDay : wantedWeekDay- firstWeekDay + 7));
    let wantedDay = firstWantedWeekDate.getDate() + ((ordinalNum - 1) * 7);
    let answerDate = DateUtils.safeCreateFromMinValue(refDate.getFullYear(), refDate.getMonth(), wantedDay);
    return answerDate;
}

function getRelativeLUISWeekDay(ordinalNum, wantedWeekDay, refDate) {
    let firstDate = new Date(refDate.getFullYear(), refDate.getMonth(), 1);
    let firstWeekDay = firstDate.getDay();
    let firstWantedWeekDate = new Date(firstDate);
    firstWantedWeekDate.setDate(firstDate.getDate() + ((wantedWeekDay > firstWeekDay) ? wantedWeekDay - firstWeekDay : wantedWeekDay- firstWeekDay + 7));
    let wantedDay = firstWantedWeekDate.getDate() + ((ordinalNum - 1) * 7);
    let yearStr = '0000' + refDate.getFullYear();
    let monthStr = '00' + (refDate.getMonth() + 1);
    let dayOfMonthStr = '00' + wantedDay;
    return `${yearStr.substr(yearStr.length - 4)}-${monthStr.substr(monthStr.length - 2)}-${dayOfMonthStr.substr(dayOfMonthStr.length - 2)}`;
}

function basicTestWithFutureAndPast(it, extractor, parser, referenceDate, text, futureDate, pastDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.deepEqual(futureDate, result.value.futureValue);
        t.deepEqual(pastDate, result.value.pastValue);
    });
}

function basicTestWithOneDate(it, extractor, parser, referenceDate, text, resultDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.deepEqual(resultDate, result.value.futureValue);
        t.deepEqual(resultDate, result.value.pastValue);
    });
}

function basicTestWithLuis(it, extractor, parser, referenceDate, text, luisDate) {
    it(text, t => {
        let ers = extractor.extract(text);
        t.is(1, ers.length);
        let result = parser.parse(ers[0], referenceDate);
        t.is(Constants.SYS_DATETIME_DATE, result.type);
        t.is(luisDate, result.value.timex);
    });
}