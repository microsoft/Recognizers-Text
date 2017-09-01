var describe = require('ava-spec').describe;
var EnglishMergedExtractorConfiguration = require('../compiled/dateTime/english/extractorConfiguration').EnglishMergedExtractorConfiguration;
var BaseMergedExtractor = require('../compiled/dateTime/extractors').BaseMergedExtractor;
var DateTimeOptions = require('../compiled/dateTime/extractors').DateTimeOptions;
var Constants = require('../compiled/dateTime/Constants').Constants;

describe('Merged Extractor', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    BasicTest(it, extractor, "this is 2 days", 8, 6);

    BasicTest(it, extractor, "this is before 4pm", 8, 10);
    BasicTest(it, extractor, "this is before 4pm tomorrow", 8, 19);
    BasicTest(it, extractor, "this is before tomorrow 4pm ", 8, 19);

    BasicTest(it, extractor, "this is after 4pm", 8, 9);
    BasicTest(it, extractor, "this is after 4pm tomorrow", 8, 18);
    BasicTest(it, extractor, "this is after tomorrow 4pm ", 8, 18);

    BasicTest(it, extractor, "I'll be back in 5 minutes", 13, 12);
    BasicTest(it, extractor, "past week", 0, 9);
    BasicTest(it, extractor, "past Monday", 0, 11);
    BasicTest(it, extractor, "schedule a meeting in 10 hours", 19, 11);
});

describe('Merged Extractor The Duration', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    BasicTest(it, extractor, "What's this day look like?", 7, 8);
    BasicTest(it, extractor, "What's this week look like?", 7, 9);
    BasicTest(it, extractor, "What's my week look like?", 7, 7);
    BasicTest(it, extractor, "What's the week look like?", 7, 8);
    BasicTest(it, extractor, "What's my day look like?", 7, 6);
    BasicTest(it, extractor, "What's the day look like?", 7, 7);
});

describe('Merged Extractor Skip From-To', it => {
    BasicTestWithOptions(it, "Change my meeting from 9am to 11am", 2, DateTimeOptions.SkipFromToMerge);
    BasicTestWithOptions(it, "Change my meeting from Nov.19th to Nov.23th", 2, DateTimeOptions.SkipFromToMerge);

    BasicTestWithOptions(it, "Schedule a meeting from 9am to 11am", 1, DateTimeOptions.None); // Testing None.
    BasicTestWithOptions(it, "Schedule a meeting from 9am to 11am", 1);
    BasicTestWithOptions(it, "Schedule a meeting from 9am to 11am tomorrow", 1);

    BasicTestWithOptions(it, "Change July 22nd meeting in Bellevue to August 22nd", 2); // No merge.
});

describe('Merged Extractor After Before', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    // after before
    BasicTest(it, extractor, "after 7/2 ", 0, 9);
    BasicTest(it, extractor, "since 7/2 ", 0, 9);
    BasicTest(it, extractor, "before 7/2 ", 0, 10);
});

describe('Merged Extractor Date With Time', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    // date with time
    BasicTest(it, extractor, "06/06 12:15", 0, 11);
    BasicTest(it, extractor, "06/06/12 15:15", 0, 14);
    BasicTest(it, extractor, "06/06, 2015", 0, 11);
});

describe('Merged Extractor Ambiguous Word', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    BasicTest(it, extractor, "May 29th", 0, 8);
    BasicTest(it, extractor, "March 29th", 0, 10);
    BasicTest(it, extractor, "I born in March", 10, 5);
    BasicTest(it, extractor, "I born in the March", 10, 9);
    BasicTest(it, extractor, "what happend at the May", 16, 7);
});

describe('Merged Extractor Negative', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    BasicTestNone(it, extractor, "in the sun");
    BasicTestNone(it, extractor, "may i help you");
    BasicTestNone(it, extractor, "the group proceeded with a march they knew would lead to bloodshed");
    BasicTestNone(it, extractor, "which email have gotten a reply");
    BasicTestNone(it, extractor, "He is often alone");
    BasicTestNone(it, extractor, "often a bird");
    BasicTestNone(it, extractor, "michigan hours");
});

function BasicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
    });
}

function BasicTestNone(it, extractor, text) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(0, results.length);
    });
}

function BasicTestWithOptions(it, text, count, options) {
    it(text, t => {
        let extractorWithOptions = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options);
        let results = extractorWithOptions.extract(text);
        t.is(count, results.length);
    });
}
