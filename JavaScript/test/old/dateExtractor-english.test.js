var describe = require('ava-spec').describe;
var EnglishDateExtractorConfiguration = require('../compiled/dateTime/english/dateConfiguration').EnglishDateExtractorConfiguration;
var BaseDateExtractor = require('../compiled/dateTime/baseDate').BaseDateExtractor;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Date Extract', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I'll go back on 15", 16, 2);
    basicTest(it, extractor, "I'll go back April 22", 13, 8);
    basicTest(it, extractor, "I'll go back Jan-1", 13, 5);
    basicTest(it, extractor, "I'll go back Jan/1", 13, 5);
    basicTest(it, extractor, "I'll go back October. 2", 13, 10);
    basicTest(it, extractor, "I'll go back January 12, 2016", 13, 16);
    basicTest(it, extractor, "I'll go back January 12 of 2016", 13, 18);
    basicTest(it, extractor, "I'll go back Monday January 12th, 2016", 13, 25);
    basicTest(it, extractor, "I'll go back 02/22/2016", 13, 10);
    basicTest(it, extractor, "I'll go back 21/04/2016", 13, 10);
    basicTest(it, extractor, "I'll go back 21/04/16", 13, 8);
    basicTest(it, extractor, "I'll go back 9-18-15", 13, 7);
    basicTest(it, extractor, "I'll go back on 4.22", 16, 4);
    basicTest(it, extractor, "I'll go back on 4-22", 16, 4);
    basicTest(it, extractor, "I'll go back at 4.22", 16, 4);
    basicTest(it, extractor, "I'll go back in 4-22", 16, 4);
    basicTest(it, extractor, "I'll go back on    4/22", 19, 4);
    basicTest(it, extractor, "I'll go back on 22/04", 16, 5);
    basicTest(it, extractor, "I'll go back       4/22", 19, 4);
    basicTest(it, extractor, "I'll go back 22/04", 13, 5);
    basicTest(it, extractor, "I'll go back 2015/08/12", 13, 10);
    basicTest(it, extractor, "I'll go back 11/12,2016", 13, 10);
    basicTest(it, extractor, "I'll go back 11/12,16", 13, 8);
    basicTest(it, extractor, "I'll go back 1st Jan", 13, 7);
    basicTest(it, extractor, "I'll go back 1-Jan", 13, 5);
    basicTest(it, extractor, "I'll go back 28-Nov", 13, 6);
    basicTest(it, extractor, "I'll go back Wed, 22 of Jan", 13, 14);

    basicTest(it, extractor, "I'll go back the first friday of july", 13, 24);
    basicTest(it, extractor, "I'll go back the first friday in this month", 13, 30);

    basicTest(it, extractor, "I'll go back two weeks from now", 13, 18);

    basicTest(it, extractor, "I'll go back next week on Friday", 13, 19);
    basicTest(it, extractor, "I'll go back on Friday next week", 13, 19);

    basicTest(it, extractor, "past Monday", 0, 11);
});

describe('Date Extract Day Of Week', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I'll go back on Tues.", 16, 4);
    basicTest(it, extractor, "I'll go back on Tues. good news.", 16, 4);
    basicTest(it, extractor, "I'll go back on Tues", 16, 4);
    basicTest(it, extractor, "I'll go back on Friday", 16, 6);
    basicTest(it, extractor, "I'll go back Friday", 13, 6);
    basicTest(it, extractor, "I'll go back today", 13, 5);
    basicTest(it, extractor, "I'll go back tomorrow", 13, 8);
    basicTest(it, extractor, "I'll go back yesterday", 13, 9);
    basicTest(it, extractor, "I'll go back the day before yesterday", 13, 24);
    basicTest(it, extractor, "I'll go back the day after tomorrow", 13, 22);
    basicTest(it, extractor, "I'll go back the next day", 13, 12);
    basicTest(it, extractor, "I'll go back next day", 13, 8);
    basicTest(it, extractor, "I'll go back this Friday", 13, 11);
    basicTest(it, extractor, "I'll go back next Sunday", 13, 11);
    basicTest(it, extractor, "I'll go back last Sunday", 13, 11);
    basicTest(it, extractor, "I'll go back last day", 13, 8);
    basicTest(it, extractor, "I'll go back the last day", 13, 12);
    basicTest(it, extractor, "I'll go back the day", 13, 7);
    basicTest(it, extractor, "I'll go back this week Friday", 13, 16);
    basicTest(it, extractor, "I'll go back next week Sunday", 13, 16);
    basicTest(it, extractor, "I'll go back last week Sunday", 13, 16);
    basicTest(it, extractor, "I'll go back 15 June 2016", 13, 12);
    basicTest(it, extractor, "a baseball on may the eleventh", 14, 16);
});

describe('Date Extract Month Date', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I'll go back fourth of may", 13, 13);
    basicTest(it, extractor, "I'll go back 4th of march", 13, 12);
    basicTest(it, extractor, "I'll go back Jan first", 13, 9);
    basicTest(it, extractor, "I'll go back May twenty-first", 13, 16);
    basicTest(it, extractor, "I'll go back May twenty one", 13, 14);
    basicTest(it, extractor, "I'll go back second of Aug", 13, 13);
    basicTest(it, extractor, "I'll go back twenty second of June", 13, 21);
});

describe('Date Extract Ago Later', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I went back two months ago", 12, 14);
    basicTest(it, extractor, "I'll go back two days later", 13, 14);
    basicTest(it, extractor, "who did i email a month ago", 16, 11);
});

describe('Date Extract For The', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I went back for the 27", 12, 10);
    basicTest(it, extractor, "I went back for the 27th", 12, 12);
    basicTest(it, extractor, "I went back for the 27.", 12, 10);
    basicTest(it, extractor, "I went back for the 27!", 12, 10);
    basicTest(it, extractor, "I went back for the 27 .", 12, 10);
    basicTest(it, extractor, "I went back for the 21st", 12, 12);
    basicTest(it, extractor, "I went back for the 22nd", 12, 12);
    basicTest(it, extractor, "I went back for the second", 12, 14);
    basicTest(it, extractor, "I went back for the twenty second", 12, 21);
    basicTest(it, extractor, "I went back for the thirty first", 12, 20);
});

describe('Date Extract On', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I went back on the 27th", 12, 11);
    basicTest(it, extractor, "I went back on the 21st", 12, 11);
    basicTest(it, extractor, "I went back on 22nd", 12, 7);
    basicTest(it, extractor, "I went back on the second!", 12, 13);
    basicTest(it, extractor, "I went back on twenty second?", 12, 16);
});

describe('Date Extract For The Negative', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTestNone(it, extractor, "the first prize");
    basicTestNone(it, extractor, "I'll go to the 27th floor");
    basicTestNone(it, extractor, "Commemorative Events for the 25th Anniversary of Diplomatic Relations between Singapore and China");
    basicTestNone(it, extractor, "Get tickets for the 17th Door Haunted Experience");
});

describe('Date Extract Week Day And Day Of Month Merge', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    // Need to calculate the DayOfWeek by the date
    // Example: What do I have on Wednesday the second?
    basicTestOneOutput(it, extractor, "What do I have on " + getWeekDay(2) + " the second", getWeekDay(2) + " the second");
    basicTestOneOutput(it, extractor, "A meeting for " + getWeekDay(27) + " the 27th with Joe Smith", getWeekDay(27) + " the 27th");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(21) + " the 21st", getWeekDay(21) + " the 21st");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(22) + " the 22nd", getWeekDay(22) + " the 22nd");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(23) + " the 23rd", getWeekDay(23) + " the 23rd");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(15) + " the 15th", getWeekDay(15) + " the 15th");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(21) + " the twenty first", getWeekDay(21) + " the twenty first");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(22) + " the twenty second", getWeekDay(22) + " the twenty second");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(15) + " the fifteen", getWeekDay(15) + " the fifteen");
    basicTestOneOutput(it, extractor, "I'll go back " + getWeekDay(7) + " the seventh", getWeekDay(7) + " the seventh");
});

describe('Date Extract Relative Day of week', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I'll go back second Sunday", 13, 13);
    basicTest(it, extractor, "I'll go back first Sunday", 13, 12);
    basicTest(it, extractor, "I'll go back third Tuesday", 13, 13);
    basicTest(it, extractor, "I'll go back fifth Sunday", 13, 12);
});

describe('Date Extract Relative Day of week Single', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    // For ordinary number>5, only the DayOfWeek should be extracted
    basicTest(it, extractor, "I'll go back sixth Sunday", 19, 6);
    basicTest(it, extractor, "I'll go back tenth Monday", 19, 6);
});

describe('Date Extract OdNum Relative month', it => {
    let extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

    basicTest(it, extractor, "I'll go back 20th of next month", 13, 18);
    basicTest(it, extractor, "I'll go back 31st of this month", 13, 18);
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

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(Constants.SYS_DATETIME_DATE, results[0].type);
    });
}

function basicTestOneOutput(it, extractor, text, expected) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(expected, results[0].text);
        t.is(Constants.SYS_DATETIME_DATE, results[0].type);
    });
}

function basicTestTwoOutputs(it, extractor, text, expectedA, expectedB) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(2, results.length);
        t.is(expectedA, results[0].text);
        t.is(expectedB, results[1].text);
        t.is(Constants.SYS_DATETIME_DATE, results[0].type);
        t.is(Constants.SYS_DATETIME_DATE, results[1].type);
    });
}

function basicTestNone(it, extractor, text) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(0, results.length);
    });
}