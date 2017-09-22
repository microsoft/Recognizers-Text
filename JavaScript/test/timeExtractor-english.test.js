var describe = require('ava-spec').describe;
var EnglishTimeExtractorConfiguration = require('../compiled/dateTime/english/timeConfiguration').EnglishTimeExtractorConfiguration;
var BaseTimeExtractor = require('../compiled/dateTime/baseTime').BaseTimeExtractor;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Time Extractor', it => {
    let extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());

    basicTest(it, extractor, "I'll be back at 7", 16, 1);
    basicTest(it, extractor, "I'll be back at seven", 16, 5);
    basicTest(it, extractor, "I'll be back 7pm", 13, 3);
    basicTest(it, extractor, "I'll be back 7p.m.", 13, 5);
    basicTest(it, extractor, "I'll be back 7:56pm", 13, 6);
    basicTest(it, extractor, "I'll be back 7:56:35pm", 13, 9);
    basicTest(it, extractor, "I'll be back 7:56:35 pm", 13, 10);
    basicTest(it, extractor, "I'll be back 12:34", 13, 5);
    basicTest(it, extractor, "I'll be back 12:34:20", 13, 8);
    basicTest(it, extractor, "I'll be back T12:34:20", 13, 9);
    basicTest(it, extractor, "I'll be back 00:00", 13, 5);
    basicTest(it, extractor, "I'll be back 00:00:30", 13, 8);

    basicTest(it, extractor, "It's 7 o'clock", 5, 9);
    basicTest(it, extractor, "It's seven o'clock", 5, 13);
    basicTest(it, extractor, "It's 8 in the morning", 5, 16);
    basicTest(it, extractor, "It's 8 in the night", 5, 14);

    basicTest(it, extractor, "It's half past eight", 5, 15);
    basicTest(it, extractor, "It's half past 8pm", 5, 13);
    basicTest(it, extractor, "It's 30 mins past eight", 5, 18);
    basicTest(it, extractor, "It's a quarter past eight", 5, 20);
    basicTest(it, extractor, "It's quarter past eight", 5, 18);
    basicTest(it, extractor, "It's three quarters past 9pm", 5, 23);
    basicTest(it, extractor, "It's three minutes to eight", 5, 22);

    basicTest(it, extractor, "It's half past seven o'clock", 5, 23);
    basicTest(it, extractor, "It's half past seven afternoon", 5, 25);
    basicTest(it, extractor, "It's half past seven in the morning", 5, 30);
    basicTest(it, extractor, "It's a quarter to 8 in the morning", 5, 29);
    basicTest(it, extractor, "It's 20 min past eight in the evening", 5, 32);

    basicTest(it, extractor, "I'll be back in the afternoon at 7", 13, 21);
    basicTest(it, extractor, "I'll be back afternoon at 7", 13, 14);
    basicTest(it, extractor, "I'll be back afternoon 7:00", 13, 14);
    basicTest(it, extractor, "I'll be back afternoon 7:00:14", 13, 17);
    basicTest(it, extractor, "I'll be back afternoon seven pm", 13, 18);

    basicTest(it, extractor, "I'll go back seven thirty pm", 13, 15);
    basicTest(it, extractor, "I'll go back seven thirty five pm", 13, 20);
    basicTest(it, extractor, "I'll go back at eleven five", 16, 11);
    basicTest(it, extractor, "I'll go back three mins to five thirty ", 13, 25);
    basicTest(it, extractor, "I'll go back five thirty in the night", 13, 24);
    basicTest(it, extractor, "I'll go back in the night five thirty", 13, 24);

    basicTest(it, extractor, "I'll be back noonish", 13, 7);
    basicTest(it, extractor, "I'll be back noon", 13, 4);
    basicTest(it, extractor, "I'll be back 12 noon", 13, 7);
    basicTest(it, extractor, "I'll be back 11ish", 13, 5);
    basicTest(it, extractor, "I'll be back 11-ish", 13, 6);

    basicTest(it, extractor, "I'll be back 340pm", 13, 5);
    basicTest(it, extractor, "I'll be back 1140 a.m.", 13, 9);

    basicTest(it, extractor, "midnight", 0, 8);
    basicTest(it, extractor, "mid-night", 0, 9);
    basicTest(it, extractor, "mid night", 0, 9);
    basicTest(it, extractor, "midmorning", 0, 10);
    basicTest(it, extractor, "mid-morning", 0, 11);
    basicTest(it, extractor, "mid morning", 0, 11);
    basicTest(it, extractor, "midafternoon", 0, 12);
    basicTest(it, extractor, "mid-afternoon", 0, 13);
    basicTest(it, extractor, "mid afternoon", 0, 13);
    basicTest(it, extractor, "midday", 0, 6);
    basicTest(it, extractor, "mid-day", 0, 7);
    basicTest(it, extractor, "mid day", 0, 7);
    basicTest(it, extractor, "noon", 0, 4);

    // Desc extractor
    basicTest(it, extractor, "I'll be back 7pm", 13, 3);
    basicTest(it, extractor, "I'll be back 7 p m", 13, 5);
    basicTest(it, extractor, "I'll be back 7 p. m", 13, 6);
    basicTest(it, extractor, "I'll be back 7 p. m.", 13, 7);
    basicTest(it, extractor, "I'll be back 7p.m.", 13, 5);
    basicTest(it, extractor, "I'll be back 7 p.m.", 13, 6);
    basicTest(it, extractor, "I'll be back 7:56 a m", 13, 8);
    basicTest(it, extractor, "I'll be back 7:56:35 a. m", 13, 12);
    basicTest(it, extractor, "I'll be back 7:56:35 am", 13, 10);
    basicTest(it, extractor, "I'll be back 7:56:35 a. m.", 13, 13);

    basicTest(it, extractor, "I'll go back seven thirty pm", 13, 15);
    basicTest(it, extractor, "I'll go back seven thirty p.m.", 13, 17);
    basicTest(it, extractor, "I'll go back seven thirty p m", 13, 16);
    basicTest(it, extractor, "I'll go back seven thirty p. m", 13, 17);
    basicTest(it, extractor, "I'll go back seven thirty p. m.", 13, 18);

    basicTest(it, extractor, "I'll be back 340pm", 13, 5);
    basicTest(it, extractor, "I'll be back 340 pm", 13, 6);
    basicTest(it, extractor, "I'll be back 1140 a.m.", 13, 9);
    basicTest(it, extractor, "I'll be back 1140 a m", 13, 8);

    // negative case
    basicNegativeTest(it, extractor, "which emails have gotten p as subject");
    basicNegativeTest(it, extractor, "which emails have gotten a reply");

    //Meal time
    basicTest(it, extractor, "I'll be back 12 o'clock breakfast", 13, 20);
    basicTest(it, extractor, "I'll be back 12 o'clock dinner", 13, 17);
    basicTest(it, extractor, "I'll be back 12 o'clock lunch", 13, 16);
    basicTest(it, extractor, "I'll be back 12 o'clock lunchtime", 13, 20);
    basicTest(it, extractor, "I'll be back lunchtime 12 o'clock", 13, 20);
    basicTest(it, extractor, "I'll be back at lunchtime 12 o'clock", 13, 23);
});

function basicTest(it, extractor, text, start, length, expected = 1) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(expected, results.length);
        if (expected < 1) return;
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(Constants.SYS_DATETIME_TIME, results[0].type);
    });
}

function basicNegativeTest(it, extractor, text) {
    basicTest(it, extractor, text, -1, -1, 0);
}