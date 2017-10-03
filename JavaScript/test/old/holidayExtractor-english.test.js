var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/holidayConfiguration').EnglishHolidayExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/baseHoliday').BaseHolidayExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Holiday Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
    basicTest(it, extractor, "I'll go back on christmas", 16, 9);
    basicTest(it, extractor, "I'll go back on christmas day", 16, 13);

    basicTest(it, extractor, "I'll go back on Yuandan", 16, 7);
    basicTest(it, extractor, "I'll go back on thanks giving day", 16, 17);
    basicTest(it, extractor, "I'll go back on father's day", 16, 12);

    basicTest(it, extractor, "I'll go back on Yuandan of this year", 16, 20);
    basicTest(it, extractor, "I'll go back on Yuandan of 2016", 16, 15);
    basicTest(it, extractor, "I'll go back on Yuandan 2016", 16, 12);

    basicTest(it, extractor, "I'll go back on clean monday", 16, 12);
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DATE, results[0].type);
    });
}