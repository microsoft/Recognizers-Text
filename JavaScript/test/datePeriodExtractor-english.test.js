var describe = require('ava-spec').describe;
var configuration = require("../compiled/dateTime/english/datePeriodConfiguration").EnglishDatePeriodExtractorConfiguration;
var baseExtractor = require("../compiled/dateTime/baseDatePeriod").BaseDatePeriodExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

var shortMonths = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Sept", "Oct", "Nov", "Dec"];
var fullMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

describe('Date Period Extractor', it => {
    let extractor = new baseExtractor(new configuration());

    shortMonths.forEach(function (month) {
        basicTest(it, extractor, `I'll be out in ${month}`, 15, month.length);
        basicTest(it, extractor, `I'll be out this ${month}`, 12, 5 + month.length);
        basicTest(it, extractor, `I'll be out month of ${month}`, 12, 9 + month.length);
        basicTest(it, extractor, `I'll be out the month of ${month}`, 12, 13 + month.length);
        basicTest(it, extractor, `I was missing ${month} 2001`, 14, 5 + month.length);
        basicTest(it, extractor, `I was missing ${month}, 2001`, 14, 6 + month.length);
    });

    fullMonths.forEach(function (month) {
        basicTest(it, extractor, `I'll be out in ${month}`, 15, month.length);
        basicTest(it, extractor, `I'll be out this ${month}`, 12, 5 + month.length);
        basicTest(it, extractor, `I'll be out month of ${month}`, 12, 9 + month.length);
        basicTest(it, extractor, `I'll be out the month of ${month}`, 12, 13 + month.length);
        basicTest(it, extractor, `I was missing ${month} 2001`, 14, 5 + month.length);
        basicTest(it, extractor, `I was missing ${month}, 2001`, 14, 6 + month.length);
    });

    basicTest(it, extractor, "Calendar for the month of September.", 13, 22);

    // Basic Cases
    basicTest(it, extractor, "I'll be out from 4 to 22 this month", 12, 23);
    basicTest(it, extractor, "I'll be out from 4-23 in next month", 12, 23);
    basicTest(it, extractor, "I'll be out from 3 until 12 of Sept hahaha", 12, 23);
    basicTest(it, extractor, "I'll be out 4 to 23 next month", 12, 18);
    basicTest(it, extractor, "I'll be out 4 till 23 of this month", 12, 23);
    basicTest(it, extractor, "I'll be out between 4 and 22 this month", 12, 27);
    basicTest(it, extractor, "I'll be out between 3 and 12 of Sept hahaha", 12, 24);
    basicTest(it, extractor, "I'll be out between september 4th through september 8th", 12, 43);
    basicTest(it, extractor, "I'll be out between November 15th through 19th", 12, 34);
    basicTest(it, extractor, "I'll be out between November 15th through the 19th", 12, 38);
    basicTest(it, extractor, "I'll be out between November the 15th through 19th", 12, 38);
    basicTest(it, extractor, "I'll be out between 4 and 22 this month", 12, 27);
    basicTest(it, extractor, "I'll be out from 4 to 22 January, 2017", 12, 26);
    basicTest(it, extractor, "I'll be out between 4-22 January, 2017", 12, 26);

    basicTest(it, extractor, "I'll be out on this week", 15, 9);
    basicTest(it, extractor, "I'll be out September", 12, 9);
    basicTest(it, extractor, "I'll be out this September", 12, 14);
    basicTest(it, extractor, "I'll be out last sept", 12, 9);
    basicTest(it, extractor, "I'll be out next june", 12, 9);
    basicTest(it, extractor, "I'll be out june 2016", 12, 9);
    basicTest(it, extractor, "I'll be out june next year", 12, 14);
    basicTest(it, extractor, "I'll be out this weekend", 12, 12);
    basicTest(it, extractor, "I'll be out the third week of this month", 12, 28);
    basicTest(it, extractor, "I'll be out the last week of july", 12, 21);

    basicTest(it, extractor, "schedule camping for Friday through Sunday", 21, 21);

    // Duration
    basicTest(it, extractor, "I'll be out next 3 days", 12, 11);
    basicTest(it, extractor, "I'll be out next 3 months", 12, 13);
    basicTest(it, extractor, "I'll be out in 3 year", 12, 9);
    basicTest(it, extractor, "I'll be out in 3 years", 12, 10);
    basicTest(it, extractor, "I'll be out in 3 weeks", 12, 10);
    basicTest(it, extractor, "I'll be out in 3 months", 12, 11);
    basicTest(it, extractor, "I'll be out past 3 weeks", 12, 12);
    basicTest(it, extractor, "I'll be out last 3year", 12, 10);
    basicTest(it, extractor, "I'll be out last year", 12, 9);
    basicTest(it, extractor, "I'll be out past month", 12, 10);
    basicTest(it, extractor, "I'll be out previous 3 weeks", 12, 16);

    basicTest(it, extractor, "past few weeks", 0, 14);
    basicTest(it, extractor, "past several days", 0, 17);

    // Merging two time points
    basicTest(it, extractor, "I'll be out Oct. 2 to October 22", 12, 20);
    basicTest(it, extractor, "I'll be out January 12, 2016 - 02/22/2016", 12, 29);
    basicTest(it, extractor, "I'll be out 1st Jan until Wed, 22 of Jan", 12, 28);
    basicTest(it, extractor, "I'll be out today till tomorrow", 12, 19);
    basicTest(it, extractor, "I'll be out today to October 22", 12, 19);
    basicTest(it, extractor, "I'll be out Oct. 2 until the day after tomorrow", 12, 35);
    basicTest(it, extractor, "I'll be out today until next Sunday", 12, 23);
    basicTest(it, extractor, "I'll be out this Friday until next Sunday", 12, 29);

    basicTest(it, extractor, "I'll be out from Oct. 2 to October 22", 12, 25);
    basicTest(it, extractor, "I'll be out from 2015/08/12 until October 22", 12, 32);
    basicTest(it, extractor, "I'll be out from today till tomorrow", 12, 24);
    basicTest(it, extractor, "I'll be out from this Friday until next Sunday", 12, 34);
    basicTest(it, extractor, "I'll be out between Oct. 2 and October 22", 12, 29);

    basicTest(it, extractor, "I'll be out November 19-20", 12, 14);
    basicTest(it, extractor, "I'll be out November 19 to 20", 12, 17);
    basicTest(it, extractor, "I'll be out November between 19 and 20", 12, 26);

    basicTest(it, extractor, "I'll be out the third quarter of 2016", 12, 25);
    basicTest(it, extractor, "I'll be out the third quarter this year", 12, 27);
    basicTest(it, extractor, "I'll be out 2016 the third quarter", 12, 22);

    basicTest(it, extractor, "I'll be out 2015.3", 12, 6);
    basicTest(it, extractor, "I'll be out 2015-3", 12, 6);
    basicTest(it, extractor, "I'll be out 2015/3", 12, 6);
    basicTest(it, extractor, "I'll be out 3/2015", 12, 6);

    basicTest(it, extractor, "I'll be out the third week of 2027", 12, 22);
    basicTest(it, extractor, "I'll be out the third week next year", 12, 24);

    // Season
    basicTest(it, extractor, "I'll leave this summer", 11, 11);
    basicTest(it, extractor, "I'll leave next spring", 11, 11);
    basicTest(it, extractor, "I'll leave the summer", 11, 10);
    basicTest(it, extractor, "I'll leave summer", 11, 6);
    basicTest(it, extractor, "I'll leave summer 2016", 11, 11);
    basicTest(it, extractor, "I'll leave summer of 2016", 11, 14);

    // Next and upcoming
    basicTest(it, extractor, "upcoming month holidays", 0, 14);
    basicTest(it, extractor, "next month holidays", 0, 10);

    // Week of and month of
    basicTest(it, extractor, "week of september.15th", 0, 22);
    basicTest(it, extractor, "month of september.15th", 0, 23);

    // the weekend = this weekend
    basicTest(it, extractor, "I'll leave over the weekend", 16, 11);

    // Rest of
    basicTest(it, extractor, "I'll leave rest of the week", 11, 16);
    basicTest(it, extractor, "I'll leave rest of my week", 11, 15);
    basicTest(it, extractor, "I'll leave rest of week", 11, 12);
    basicTest(it, extractor, "I'll leave rest the week", 11, 13);
    basicTest(it, extractor, "I'll leave rest of this week", 11, 17);
    basicTest(it, extractor, "I'll leave rest current week", 11, 17);
    basicTest(it, extractor, "I'll leave rest of the month", 11, 17);
    basicTest(it, extractor, "I'll leave rest of the year", 11, 16);
});

function basicTest(it, extractor, text, start, length, expected = 1) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(expected, results.length);
        if (expected < 1) return;
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DATEPERIOD, results[0].type);
    });
}