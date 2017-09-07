var describe = require('ava-spec').describe;
var EnglishTimePeriodExtractorConfiguration = require('../compiled/dateTime/english/timePeriodConfiguration').EnglishTimePeriodExtractorConfiguration;
var BaseTimePeriodExtractor = require('../compiled/dateTime/baseTimePeriod').BaseTimePeriodExtractor;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Time Extractor', it => {
    let extractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());

    // basic match
    basicTest(it, extractor, "I'll be out 5 to 6pm", 12, 8);
    basicTest(it, extractor, "I'll be out 5 to 6 p.m.", 12, 11);
    basicTest(it, extractor, "I'll be out 5 to 6 in the afternoon", 12, 23);
    basicTest(it, extractor, "I'll be out 5 to seven in the morning", 12, 25);
    basicTest(it, extractor, "I'll be out from 5 to 6pm", 12, 13);
    basicTest(it, extractor, "I'll be out between 5 and 6pm", 12, 17);
    basicTest(it, extractor, "I'll be out between 5pm and 6pm", 12, 19);
    basicTest(it, extractor, "I'll be out between 5 and 6 in the afternoon", 12, 32);

    //// merge to time points
    basicTest(it, extractor, "I'll be out 4pm till 5pm", 12, 12);
    basicTest(it, extractor, "I'll be out 4:00 till 5pm", 12, 13);
    basicTest(it, extractor, "I'll be out 4:00 to 7 oclock", 12, 16);
    basicTest(it, extractor, "I'll be out 3pm to half past seven", 12, 22);
    basicTest(it, extractor, "I'll be out 4pm-5pm", 12, 7);
    basicTest(it, extractor, "I'll be out 4pm - 5pm", 12, 9);
    basicTest(it, extractor, "I'll be out 20 minutes to three to eight in the evening", 12, 43);

    basicTest(it, extractor, "I'll be out from 4pm to 5pm", 12, 15);
    basicTest(it, extractor, "I'll be out from 4pm to half past five", 12, 26);
    basicTest(it, extractor, "I'll be out from 3 in the morning until 5pm", 12, 31);
    basicTest(it, extractor, "I'll be out from 3 in the morning until five in the afternoon", 12, 49);

    basicTest(it, extractor, "I'll be out between 4pm and half past five", 12, 30);
    basicTest(it, extractor, "I'll be out between 3 in the morning and 5pm", 12, 32);

    basicTest(it, extractor, "let's meet in the morning", 11, 14);
    basicTest(it, extractor, "let's meet in the afternoon", 11, 16);
    basicTest(it, extractor, "let's meet in the night", 11, 12);
    basicTest(it, extractor, "let's meet in the evening", 11, 14);
    basicTest(it, extractor, "let's meet in the evenings", 11, 15);

    basicTest(it, extractor, "let's meet in the early-mornings", 11, 21);
    basicTest(it, extractor, "let's meet in the late-mornings", 11, 20);
    basicTest(it, extractor, "let's meet in the early-morning", 11, 20);
    basicTest(it, extractor, "let's meet in the late-morning", 11, 19);
    basicTest(it, extractor, "let's meet in the early-afternoon", 11, 22);
    basicTest(it, extractor, "let's meet in the late-afternoon", 11, 21);
    basicTest(it, extractor, "let's meet in the early-evening", 11, 20);
    basicTest(it, extractor, "let's meet in the late-evening", 11, 19);
    basicTest(it, extractor, "let's meet in the early-night", 11, 18);
    basicTest(it, extractor, "let's meet in the late-night", 11, 17);
    basicTest(it, extractor, "let's meet in the early night", 11, 18);
    basicTest(it, extractor, "let's meet in the late night", 11, 17);

    // TimePeriodExtraExtract
    basicTest(it, extractor, "set up meeting from two to five pm", 15, 19);
    basicTest(it, extractor, "Party at Jean’s from 6 to 11 pm", 16, 15);
    basicTest(it, extractor, "set up meeting from 14:00 to 16:30", 15, 19);

    // TODO fix these for next release
    basicTest(it, extractor, "set up meeting from two to five p m", 15, 20);
    // basicTest(it, extractor, "set up meeting from 14 to 16h", 15, 14);
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
        t.is(Constants.SYS_DATETIME_TIMEPERIOD, results[0].type);
    });
}