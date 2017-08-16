var describe = require('ava-spec').describe;
var EnglishTimeExtractorConfiguration = require('../compiled/dateTime/english/extractorConfiguration').EnglishTimeExtractorConfiguration;
var BaseTimeExtractor = require('../compiled/dateTime/extractors').BaseTimeExtractor;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Time Extractor', it => {
    let extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());

    BasicTest(it, extractor, "I'll be back at 7", 16, 1);
    BasicTest(it, extractor, "I'll be back at seven", 16, 5);
    BasicTest(it, extractor, "I'll be back 7pm", 13, 3);
    BasicTest(it, extractor, "I'll be back 7p.m.", 13, 5);
    BasicTest(it, extractor, "I'll be back 7:56pm", 13, 6);
    BasicTest(it, extractor, "I'll be back 7:56:35pm", 13, 9);
    BasicTest(it, extractor, "I'll be back 7:56:35 pm", 13, 10);
    BasicTest(it, extractor, "I'll be back 12:34", 13, 5);
    BasicTest(it, extractor, "I'll be back 12:34:20", 13, 8);
    BasicTest(it, extractor, "I'll be back T12:34:20", 13, 9);
    BasicTest(it, extractor, "I'll be back 00:00", 13, 5);
    BasicTest(it, extractor, "I'll be back 00:00:30", 13, 8);

    BasicTest(it, extractor, "It's 7 o'clock", 5, 9);
    BasicTest(it, extractor, "It's seven o'clock", 5, 13);
    BasicTest(it, extractor, "It's 8 in the morning", 5, 16);
    BasicTest(it, extractor, "It's 8 in the night", 5, 14);


    BasicTest(it, extractor, "It's half past eight", 5, 15);
    BasicTest(it, extractor, "It's half past 8pm", 5, 13);
    BasicTest(it, extractor, "It's 30 mins past eight", 5, 18);
    BasicTest(it, extractor, "It's a quarter past eight", 5, 20);
    BasicTest(it, extractor, "It's quarter past eight", 5, 18);
    BasicTest(it, extractor, "It's three quarters past 9pm", 5, 23);
    BasicTest(it, extractor, "It's three minutes to eight", 5, 22);

    BasicTest(it, extractor, "It's half past seven o'clock", 5, 23);
    BasicTest(it, extractor, "It's half past seven afternoon", 5, 25);
    BasicTest(it, extractor, "It's half past seven in the morning", 5, 30);
    BasicTest(it, extractor, "It's a quarter to 8 in the morning", 5, 29);
    BasicTest(it, extractor, "It's 20 min past eight in the evening", 5, 32);

    BasicTest(it, extractor, "I'll be back in the afternoon at 7", 13, 21);
    BasicTest(it, extractor, "I'll be back afternoon at 7", 13, 14);
    BasicTest(it, extractor, "I'll be back afternoon 7:00", 13, 14);
    BasicTest(it, extractor, "I'll be back afternoon 7:00:14", 13, 17);
    BasicTest(it, extractor, "I'll be back afternoon seven pm", 13, 18);

    BasicTest(it, extractor, "I'll go back seven thirty pm", 13, 15);
    BasicTest(it, extractor, "I'll go back seven thirty five pm", 13, 20);
    BasicTest(it, extractor, "I'll go back at eleven five", 16, 11);
    BasicTest(it, extractor, "I'll go back three mins to five thirty ", 13, 25);
    BasicTest(it, extractor, "I'll go back five thirty in the night", 13, 24);
    BasicTest(it, extractor, "I'll go back in the night five thirty", 13, 24);

    BasicTest(it, extractor, "I'll be back noonish", 13, 7);
    BasicTest(it, extractor, "I'll be back noon", 13, 4);
    BasicTest(it, extractor, "I'll be back 11ish", 13, 5);
    BasicTest(it, extractor, "I'll be back 11-ish", 13, 6);

    BasicTest(it, extractor, "I'll be back 340pm", 13, 5);
    BasicTest(it, extractor, "I'll be back 1140 a.m.", 13, 9);
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(Constants.SYS_DATETIME_TIME, results[0].type);
    });
}