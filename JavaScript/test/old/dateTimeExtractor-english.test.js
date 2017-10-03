var describe = require('ava-spec').describe;
var configuration = require("../compiled/dateTime/english/dateTimeConfiguration").EnglishDateTimeExtractorConfiguration;
var baseExtractor = require("../compiled/dateTime/baseDateTime").BaseDateTimeExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('DateTime Extractor', it => {
    let extractor = new baseExtractor(new configuration());

    basicTest(it, extractor, "I'll go back now", 13, 3);
    basicTest(it, extractor, "I'll go back as soon as possible", 13, 19);
    basicTest(it, extractor, "I'll go back right now", 13, 9);
    basicTest(it, extractor, "I'll go back on 15 at 8:00", 16, 10);
    basicTest(it, extractor, "I'll go back on 15 at 8:00:30", 16, 13);
    basicTest(it, extractor, "I'll go back on 15, 8pm", 16, 7);
    basicTest(it, extractor, "I'll go back 04/21/2016, 8:00pm", 13, 18);
    basicTest(it, extractor, "I'll go back 04/21/2016, 8:00:13pm", 13, 21);
    basicTest(it, extractor, "I'll go back Oct. 23 at seven", 13, 16);
    basicTest(it, extractor, "I'll go back October 14 8:00am", 13, 17);
    basicTest(it, extractor, "I'll go back October 14 8:00:00am", 13, 20);
    basicTest(it, extractor, "I'll go back October 14, 8:00am", 13, 18);
    basicTest(it, extractor, "I'll go back October 14, 8:00:01am", 13, 21);
    basicTest(it, extractor, "I'll go back tomorrow 8:00am", 13, 15);
    basicTest(it, extractor, "I'll go back tomorrow around 8:00am", 13, 22);
    basicTest(it, extractor, "I'll go back tomorrow for 8:00am", 13, 19);
    basicTest(it, extractor, "I'll go back tomorrow 8:00:05am", 13, 18);
    basicTest(it, extractor, "I'll go back next friday at half past 3 ", 13, 26);
    basicTest(it, extractor, "I'll go back May 5, 2016, 20 min past eight in the evening", 13, 45);

    basicTest(it, extractor, "I'll go back 8pm on 15", 13, 9);
    basicTest(it, extractor, "I'll go back at seven on 15", 16, 11);
    basicTest(it, extractor, "I'll go back 8pm next sunday", 13, 15);
    basicTest(it, extractor, "I'll go back 8pm today", 13, 9);
    basicTest(it, extractor, "I'll go back a quarter to seven tomorrow", 13, 27);
    basicTest(it, extractor, "I'll go back 19:00, 2016-12-22", 13, 17);
    basicTest(it, extractor, "I'll go back seven o'clock tomorrow", 13, 22);

    basicTest(it, extractor, "I'll go back tomorrow morning at 7", 13, 21);
    basicTest(it, extractor, "I'll go back 7:00 on Sunday afternoon", 13, 24);
    basicTest(it, extractor, "I'll go back twenty minutes past five tomorrow morning", 13, 41);
    basicTest(it, extractor, "I'll go back October 14 8:00, October 14", 13, 15);
    basicTest(it, extractor, "I'll go back 7, this morning", 13, 15);

    basicTest(it, extractor, "I'll go back 8pm in the evening, Monday", 13, 26);
    basicTest(it, extractor, "I'll go back 8pm in the evening, 1st Jan", 13, 27);
    basicTest(it, extractor, "I'll go back 8pm in the evening, 1 Jan", 13, 25);
    basicTest(it, extractor, "I'll go back 10pm tonight", 13, 12);
    basicTest(it, extractor, "I'll go back 8am this morning", 13, 16);
    basicTest(it, extractor, "I'll go back 8pm this evening", 13, 16);
    basicTest(it, extractor, "I'll go back tonight around 7", 13, 16);

    basicTest(it, extractor, "I'll go back this morning at 7", 13, 17);
    basicTest(it, extractor, "I'll go back this morning at 7pm", 13, 19);
    basicTest(it, extractor, "I'll go back this morning at seven", 13, 21);
    basicTest(it, extractor, "I'll go back this morning at 7:00", 13, 20);
    basicTest(it, extractor, "I'll go back this night at 7", 13, 15);
    basicTest(it, extractor, "I'll go back tonight at 7", 13, 12);
    basicTest(it, extractor, "for 2 people tonight at 9:30 pm", 13, 18);
    basicTest(it, extractor, "for 2 people tonight at 9:30:31 pm", 13, 21);

    basicTest(it, extractor, "I'll go back the end of the day", 13, 18);
    basicTest(it, extractor, "I'll go back end of tomorrow", 13, 15);
    basicTest(it, extractor, "I'll go back end of the sunday", 13, 17);

    basicTest(it, extractor, "I'll go back at 5th at 4 a.m.", 16, 13);

    basicTest(it, extractor, "I'll go back 2016-12-16T12:23:59", 13, 19);

    basicTest(it, extractor, "I'll go back in 5 hours", 13, 10);
});

describe('DateTime Extractor With Ambiguous', it => {
    let extractor = new baseExtractor(new configuration());

    basicTest(it, extractor, "see if I am available for 3pm on Sun", 26, 10);
    
    // TODO: triage if we will support the following
    // basicTest(it, extractor, "five tomorrow", 0, 13);
    // basicTest(it, extractor, "dinner 5 tomorrow", 0, 13);
});

describe("DateTime Extractor o'clock", it => {
    let extractor = new baseExtractor(new configuration());

    basicTest(it, extractor, "Set appointment for tomorrow morning at 9 o'clock.", 20, 29);
    basicTest(it, extractor, "I'll go back tomorrow morning at 9 o'clock", 13, 29);
    basicTest(it, extractor, "I'll go back tomorrow morning at 9 oclock", 13, 28);
    basicTest(it, extractor, "I'll go back tomorrow at 9 o'clock", 13, 21);
    basicTest(it, extractor, "I'll go back tomorrow at 9 oclock", 13, 20);
    basicTest(it, extractor, "this friday at one o'clock pm", 0, 29);

    //TODO: need a pattern to support this
    //basicTest(it, extractor, "Set an appointment for the 30th at 5:30 PM for language sessions.", 23, 19);
});

describe('DateTime Extractor Date With Time', it => {
    let extractor = new baseExtractor(new configuration());

    basicTest(it, extractor, "I'll go back August 1st 11 AM", 13, 16);
    basicTest(it, extractor, "I'll go back August 1st 11 pm", 13, 16);
    basicTest(it, extractor, "I'll go back August 1st 11 p.m.", 13, 18);
    basicTest(it, extractor, "I'll go back 25/02 11 am", 13, 11);
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(constants.SYS_DATETIME_DATETIME, results[0].type);
    });
}