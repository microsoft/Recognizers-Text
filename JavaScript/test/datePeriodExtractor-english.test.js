var describe = require('ava-spec').describe;
var configuration = require("../compiled/dateTime/english/extractorConfiguration").EnglishDatePeriodExtractorConfiguration;
var baseExtractor = require("../compiled/dateTime/extractors").BaseDatePeriodExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

var shortMonths = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Sept", "Oct", "Nov", "Dec"];
var fullMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

describe('Date Period Extractor', it => {
    let extractor = new baseExtractor(new configuration());

    shortMonths.forEach(function (month) {
        BasicTest(it, extractor, `I'll be out in ${month}`, 15, month.length);
        BasicTest(it, extractor, `I'll be out this ${month}`, 12, 5 + month.length);
        BasicTest(it, extractor, `I was missing ${month} 2001`, 14, 5 + month.length);
        BasicTest(it, extractor, `I was missing ${month}, 2001`, 14, 6 + month.length);
    });

    fullMonths.forEach(function (month) {
        BasicTest(it, extractor, `I'll be out in ${month}`, 15, month.length);
        BasicTest(it, extractor, `I'll be out this ${month}`, 12, 5 + month.length);
        BasicTest(it, extractor, `I was missing ${month} 2001`, 14, 5 + month.length);
        BasicTest(it, extractor, `I was missing ${month}, 2001`, 14, 6 + month.length);
    });

    // Basic Cases
    BasicTest(it, extractor, "I'll be out from 4 to 22 this month", 12, 23);
    BasicTest(it, extractor, "I'll be out from 4-23 in next month", 12, 23);
    BasicTest(it, extractor, "I'll be out from 3 until 12 of Sept hahaha", 12, 23);
    BasicTest(it, extractor, "I'll be out 4 to 23 next month", 12, 18);
    BasicTest(it, extractor, "I'll be out 4 till 23 of this month", 12, 23);
    BasicTest(it, extractor, "I'll be out between 4 and 22 this month", 12, 27);
    BasicTest(it, extractor, "I'll be out between 3 and 12 of Sept hahaha", 12, 24);
    BasicTest(it, extractor, "I'll be out between september 4th through september 8th", 12, 43);
    BasicTest(it, extractor, "I'll be out between November 15th through 19th", 12, 34);
    BasicTest(it, extractor, "I'll be out between November 15th through the 19th", 12, 38);
    BasicTest(it, extractor, "I'll be out between November the 15th through 19th", 12, 38);
    BasicTest(it, extractor, "I'll be out between 4 and 22 this month", 12, 27);
    BasicTest(it, extractor, "I'll be out from 4 to 22 January, 2017", 12, 26);
    BasicTest(it, extractor, "I'll be out between 4-22 January, 2017", 12, 26);

    BasicTest(it, extractor, "I'll be out on this week", 15, 9);
    BasicTest(it, extractor, "I'll be out September", 12, 9);
    BasicTest(it, extractor, "I'll be out this September", 12, 14);
    BasicTest(it, extractor, "I'll be out last sept", 12, 9);
    BasicTest(it, extractor, "I'll be out next june", 12, 9);
    BasicTest(it, extractor, "I'll be out june 2016", 12, 9);
    BasicTest(it, extractor, "I'll be out june next year", 12, 14);
    BasicTest(it, extractor, "I'll be out this weekend", 12, 12);
    BasicTest(it, extractor, "I'll be out the third week of this month", 12, 28);
    BasicTest(it, extractor, "I'll be out the last week of july", 12, 21);

    // Duration
    BasicTest(it, extractor, "I'll be out next 3 days", 12, 11);
    BasicTest(it, extractor, "I'll be out next 3 months", 12, 13);
    BasicTest(it, extractor, "I'll be out in 3 year", 12, 9);
    BasicTest(it, extractor, "I'll be out in 3 years", 12, 10);
    BasicTest(it, extractor, "I'll be out in 3 weeks", 12, 10);
    BasicTest(it, extractor, "I'll be out in 3 months", 12, 11);
    BasicTest(it, extractor, "I'll be out past 3 weeks", 12, 12);
    BasicTest(it, extractor, "I'll be out last 3year", 12, 10);
    BasicTest(it, extractor, "I'll be out last year", 12, 9);
    BasicTest(it, extractor, "I'll be out past month", 12, 10);
    BasicTest(it, extractor, "I'll be out previous 3 weeks", 12, 16);

    BasicTest(it, extractor, "past few weeks", 0, 14);
    BasicTest(it, extractor, "past several days", 0, 17);

    // Merging two time points
    BasicTest(it, extractor, "I'll be out Oct. 2 to October 22", 12, 20);
    BasicTest(it, extractor, "I'll be out January 12, 2016 - 02/22/2016", 12, 29);
    BasicTest(it, extractor, "I'll be out 1st Jan until Wed, 22 of Jan", 12, 28);
    BasicTest(it, extractor, "I'll be out today till tomorrow", 12, 19);
    BasicTest(it, extractor, "I'll be out today to October 22", 12, 19);
    BasicTest(it, extractor, "I'll be out Oct. 2 until the day after tomorrow", 12, 35);
    BasicTest(it, extractor, "I'll be out today until next Sunday", 12, 23);
    BasicTest(it, extractor, "I'll be out this Friday until next Sunday", 12, 29);

    BasicTest(it, extractor, "I'll be out from Oct. 2 to October 22", 12, 25);
    BasicTest(it, extractor, "I'll be out from 2015/08/12 until October 22", 12, 32);
    BasicTest(it, extractor, "I'll be out from today till tomorrow", 12, 24);
    BasicTest(it, extractor, "I'll be out from this Friday until next Sunday", 12, 34);
    BasicTest(it, extractor, "I'll be out between Oct. 2 and October 22", 12, 29);

    BasicTest(it, extractor, "I'll be out November 19-20", 12, 14);
    BasicTest(it, extractor, "I'll be out November 19 to 20", 12, 17);
    BasicTest(it, extractor, "I'll be out November between 19 and 20", 12, 26);

    BasicTest(it, extractor, "I'll be out the third quarter of 2016", 12, 25);
    BasicTest(it, extractor, "I'll be out the third quarter this year", 12, 27);
    BasicTest(it, extractor, "I'll be out 2016 the third quarter", 12, 22);

    BasicTest(it, extractor, "I'll be out 2015.3", 12, 6);
    BasicTest(it, extractor, "I'll be out 2015-3", 12, 6);
    BasicTest(it, extractor, "I'll be out 2015/3", 12, 6);
    BasicTest(it, extractor, "I'll be out 3/2015", 12, 6);

    BasicTest(it, extractor, "I'll be out the third week of 2027", 12, 22);
    BasicTest(it, extractor, "I'll be out the third week next year", 12, 24);

    // Season
    BasicTest(it, extractor, "I'll leave this summer", 11, 11);
    BasicTest(it, extractor, "I'll leave next spring", 11, 11);
    BasicTest(it, extractor, "I'll leave the summer", 15, 6);
    BasicTest(it, extractor, "I'll leave summer", 11, 6);
    BasicTest(it, extractor, "I'll leave summer 2016", 11, 11);
    BasicTest(it, extractor, "I'll leave summer of 2016", 11, 14);

    // Next and upcoming
    BasicTest(it, extractor, "upcoming month holidays", 0, 14);
    BasicTest(it, extractor, "next month holidays", 0, 10);

    // Week of and month of
    BasicTest(it, extractor, "week of september.15th", 0, 22);
    BasicTest(it, extractor, "month of september.15th", 0, 23);

    // over the weekend = this weekend
    BasicTest(it, extractor, "I'll leave over the weekend", 11, 16);
});

function BasicTest(it, extractor, text, start, length, expected = 1) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(expected, results.length);
        if (expected < 1) return;
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DATEPERIOD, results[0].type);
    });
}