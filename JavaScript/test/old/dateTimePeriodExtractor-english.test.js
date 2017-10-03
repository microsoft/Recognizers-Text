var describe = require('ava-spec').describe;
var configuration = require("../compiled/dateTime/english/dateTimePeriodConfiguration").EnglishDateTimePeriodExtractorConfiguration;
var baseExtractor = require("../compiled/dateTime/baseDateTimePeriod").BaseDateTimePeriodExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('DateTimePeriod Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
    // basic match
    basicTest(it, extractor, "I'll be out five to seven today", 12, 19);
    basicTest(it, extractor, "I'll be out five to seven of tomorrow", 12, 25);
    basicTest(it, extractor, "I'll be out from 5 to 6 next sunday", 12, 23);
    basicTest(it, extractor, "I'll be out from 5 to 6pm next sunday", 12, 25);

    basicTest(it, extractor, "I'll be out from 4pm to 5pm today", 12, 21);
    basicTest(it, extractor, "I'll be out from 4pm today to 5pm tomorrow", 12, 30);
    basicTest(it, extractor, "I'll be out from 4pm to 5pm of tomorrow", 12, 27);
    basicTest(it, extractor, "I'll be out from 4pm to 5pm of 2017-6-6", 12, 27);
    basicTest(it, extractor, "I'll be out from 4pm to 5pm May 5, 2018", 12, 27);
    basicTest(it, extractor, "I'll be out from 4:00 to 5pm May 5, 2018", 12, 28);
    basicTest(it, extractor, "I'll be out from 4pm on Jan 1, 2016 to 5pm today", 12, 36);
    basicTest(it, extractor, "I'll be out from 2:00pm, 2016-2-21 to 3:32, 04/23/2016", 12, 42);
    basicTest(it, extractor, "I'll be out from today at 4 to next Wedn at 5", 12, 33);

    basicTest(it, extractor, "I'll be out between 4pm and 5pm today", 12, 25);
    basicTest(it, extractor, "I'll be out between 4pm on Jan 1, 2016 and 5pm today", 12, 40);

    basicTest(it, extractor, "I'll go back tonight", 13, 7);
    basicTest(it, extractor, "I'll go back this night", 13, 10);
    basicTest(it, extractor, "I'll go back this evening", 13, 12);
    basicTest(it, extractor, "I'll go back this morning", 13, 12);
    basicTest(it, extractor, "I'll go back this afternoon", 13, 14);
    basicTest(it, extractor, "I'll go back next night", 13, 10);
    basicTest(it, extractor, "I'll go back last night", 13, 10);
    basicTest(it, extractor, "I'll go back tomorrow night", 13, 14);
    basicTest(it, extractor, "I'll go back next monday afternoon", 13, 21);
    basicTest(it, extractor, "I'll go back May 5th night", 13, 13);

    basicTest(it, extractor, "I'll go back last 3 minute", 13, 13);
    basicTest(it, extractor, "I'll go back past 3 minute", 13, 13);
    basicTest(it, extractor, "I'll go back previous 3 minute", 13, 17);
    basicTest(it, extractor, "I'll go back previous 3mins", 13, 14);
    basicTest(it, extractor, "I'll go back next 5 hrs", 13, 10);
    basicTest(it, extractor, "I'll go back last minute", 13, 11);
    basicTest(it, extractor, "I'll go back next hour", 13, 9);
    basicTest(it, extractor, "I'll go back last few minutes", 13, 16);
    basicTest(it, extractor, "I'll go back past several minutes", 13, 20);

    basicTest(it, extractor, "I'll go back tuesday in the morning", 13, 22);
    basicTest(it, extractor, "I'll go back tuesday in the afternoon", 13, 24);
    basicTest(it, extractor, "I'll go back tuesday in the evening", 13, 22);

    // early/late date time
    basicTest(it, extractor, "let's meet in the early-morning Tuesday", 11, 28);
    basicTest(it, extractor, "let's meet in the late-morning Tuesday", 11, 27);
    basicTest(it, extractor, "let's meet in the early-afternoon Tuesday", 11, 30);
    basicTest(it, extractor, "let's meet in the late-afternoon Tuesday", 11, 29);
    basicTest(it, extractor, "let's meet in the early-evening Tuesday", 11, 28);
    basicTest(it, extractor, "let's meet in the late-evening Tuesday", 11, 27);
    basicTest(it, extractor, "let's meet in the early-night Tuesday", 11, 26);
    basicTest(it, extractor, "let's meet in the late-night Tuesday", 11, 25);
    basicTest(it, extractor, "let's meet in the early night Tuesday", 11, 26);
    basicTest(it, extractor, "let's meet in the late night Tuesday", 11, 25);

    basicTest(it, extractor, "let's meet in the early-morning on Tuesday", 11, 31);
    basicTest(it, extractor, "let's meet in the late-morning on Tuesday", 11, 30);
    basicTest(it, extractor, "let's meet in the early-afternoon on Tuesday", 11, 33);
    basicTest(it, extractor, "let's meet in the late-afternoon on Tuesday", 11, 32);
    basicTest(it, extractor, "let's meet in the early-evening on Tuesday", 11, 31);
    basicTest(it, extractor, "let's meet in the late-evening on Tuesday", 11, 30);
    basicTest(it, extractor, "let's meet in the early-night on Tuesday", 11, 29);
    basicTest(it, extractor, "let's meet in the late-night on Tuesday", 11, 28);
    basicTest(it, extractor, "let's meet in the early night on Tuesday", 11, 29);
    basicTest(it, extractor, "let's meet in the late night on Tuesday", 11, 28);

    basicTest(it, extractor, "let's meet on Tuesday early-morning", 14, 21);
    basicTest(it, extractor, "let's meet on Tuesday late-morning", 14, 20);
    basicTest(it, extractor, "let's meet on Tuesday early-afternoon", 14, 23);
    basicTest(it, extractor, "let's meet on Tuesday late-afternoon", 14, 22);
    basicTest(it, extractor, "let's meet on Tuesday early-evening", 14, 21);
    basicTest(it, extractor, "let's meet on Tuesday late-evening", 14, 20);
    basicTest(it, extractor, "let's meet on Tuesday early-night", 14, 19);
    basicTest(it, extractor, "let's meet on Tuesday late-night", 14, 18);
    basicTest(it, extractor, "let's meet on Tuesday early night", 14, 19);
    basicTest(it, extractor, "let's meet on Tuesday late night", 14, 18);

    // rest of
    basicTest(it, extractor, "I'll be out rest of the day", 12, 15);
    basicTest(it, extractor, "I'll be out rest of this day", 12, 16);
    basicTest(it, extractor, "I'll be out rest of current day", 12, 19);
    basicTest(it, extractor, "I'll be out rest the day", 12, 12);
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DATETIMEPERIOD, results[0].type);
    });
}