var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/setConfiguration').EnglishSetExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/baseSet').BaseSetExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Set Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
    basicTest(it, extractor, "I'll leave weekly", 11, 6);
    basicTest(it, extractor, "I'll leave daily", 11, 5);
    basicTest(it, extractor, "I'll leave every day", 11, 9);
    basicTest(it, extractor, "I'll leave each month", 11, 10);
    basicTest(it, extractor, "I'll leave annually", 11, 8);
    basicTest(it, extractor, "I'll leave annual", 11, 6);

    basicTest(it, extractor, "I'll leave each two days", 11, 13);
    basicTest(it, extractor, "I'll leave every three week", 11, 16);

    basicTest(it, extractor, "I'll leave 3pm every day", 11, 13);
    basicTest(it, extractor, "I'll leave 3pm each day", 11, 12);

    basicTest(it, extractor, "I'll leave each 4/15", 11, 9);
    basicTest(it, extractor, "I'll leave every monday", 11, 12);
    basicTest(it, extractor, "I'll leave each monday 4pm", 11, 15);

    basicTest(it, extractor, "I'll leave every morning", 11, 13);


    // SetExtractMergeTimePeriod_Time
    basicTest(it, extractor, "I'll leave every morning at 9am", 11, 20);
    basicTest(it, extractor, "I'll leave every afternoon at 4pm", 11, 22);
    basicTest(it, extractor, "I'll leave every night at 9pm", 11, 18);
    basicTest(it, extractor, "I'll leave every night at 9", 11, 16);
    basicTest(it, extractor, "I'll leave mornings at 9am", 11, 15);
    basicTest(it, extractor, "I'll leave on mornings at 9", 11, 16);

    // SetExtractMergeDate_Time
    basicTest(it, extractor, "I'll leave at 9am every Sunday", 14, 16);
    basicTest(it, extractor, "I'll leave at 9am on Mondays", 14, 14);
    basicTest(it, extractor, "I'll leave at 9am Mondays", 14, 11);

    // SetExtractDate
    basicTest(it, extractor, "I'll leave on Mondays", 11, 10);
    basicTest(it, extractor, "I'll leave on Sundays", 11, 10);
    basicTest(it, extractor, "I'll leave Sundays", 11, 7);
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_SET, results[0].type);
    });
}