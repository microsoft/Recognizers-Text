var describe = require('ava-spec').describe;
var EnglishMergedExtractorConfiguration = require('../compiled/dateTime/english/mergedConfiguration').EnglishMergedExtractorConfiguration;
var BaseMergedExtractor = require('../compiled/dateTime/baseMerged').BaseMergedExtractor;
var DateTimeOptions = require('../compiled/dateTime/baseMerged').DateTimeOptions;
var Constants = require('../compiled/dateTime/constants').Constants;

describe('Merged Extractor', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    basicTest(it, extractor, "this is 2 days", 8, 6);

    basicTest(it, extractor, "this is before 4pm", 8, 10);
    basicTest(it, extractor, "this is before 4pm tomorrow", 8, 19);
    basicTest(it, extractor, "this is before tomorrow 4pm ", 8, 19);

    basicTest(it, extractor, "this is after 4pm", 8, 9);
    basicTest(it, extractor, "this is after 4pm tomorrow", 8, 18);
    basicTest(it, extractor, "this is after tomorrow 4pm ", 8, 18);

    basicTest(it, extractor, "I'll be back in 5 minutes", 13, 12);
    basicTest(it, extractor, "past week", 0, 9);
    basicTest(it, extractor, "past Monday", 0, 11);
    basicTest(it, extractor, "schedule a meeting in 10 hours", 19, 11);
});

describe('Merged Extractor The Duration', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    basicTest(it, extractor, "What's this day look like?", 7, 8);
    basicTest(it, extractor, "What's this week look like?", 7, 9);
    basicTest(it, extractor, "What's my week look like?", 7, 7);
    basicTest(it, extractor, "What's the week look like?", 7, 8);
    basicTest(it, extractor, "What's my day look like?", 7, 6);
    basicTest(it, extractor, "What's the day look like?", 7, 7);
});

describe('Merged Extractor Skip From-To', it => {
    basicTestWithOptions(it, "Change my meeting from 9am to 11am", 2, DateTimeOptions.SkipFromToMerge);
    basicTestWithOptions(it, "Change my meeting from Nov.19th to Nov.23th", 2, DateTimeOptions.SkipFromToMerge);

    basicTestWithOptions(it, "Schedule a meeting from 9am to 11am", 1, DateTimeOptions.None); // Testing None.
    basicTestWithOptions(it, "Schedule a meeting from 9am to 11am", 1);
    basicTestWithOptions(it, "Schedule a meeting from 9am to 11am tomorrow", 1);

    basicTestWithOptions(it, "Change July 22nd meeting in Bellevue to August 22nd", 2); // No merge.
});

describe('Merged Extractor After Before Since', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    // after before
    basicTest(it, extractor, "after 7/2 ", 0, 9);
    basicTest(it, extractor, "since 7/2 ", 0, 9);
    basicTest(it, extractor, "before 7/2 ", 0, 10);
});

describe('Merged Extractor Date With Time', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    // date with time
    basicTest(it, extractor, "06/06 12:15", 0, 11);
    basicTest(it, extractor, "06/06/12 15:15", 0, 14);
    basicTest(it, extractor, "06/06, 2015", 0, 11);
});

describe('Merged Extractor Ambiguous Word', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    basicTest(it, extractor, "May 29th", 0, 8);
    basicTest(it, extractor, "March 29th", 0, 10);
    basicTest(it, extractor, "I born in March", 10, 5);
    basicTest(it, extractor, "I born in the March", 10, 9);
    basicTest(it, extractor, "what happend at the May", 16, 7);
});

describe('Merged Extractor Negative', it => {
    let extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

    basicTestNone(it, extractor, "What are the hours of Palomino? ");
    basicTestNone(it, extractor, "in the sun");

    basicTestNone(it, extractor, "which email have gotten a reply");
    basicTestNone(it, extractor, "He is often alone");
    basicTestNone(it, extractor, "often a bird");
    basicTestNone(it, extractor, "michigan hours");
});

function basicTest(it, extractor, text, start, length) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(1, results.length);
        t.is(start, results[0].start);
        t.is(length, results[0].length);
    });
}

function basicTestNone(it, extractor, text) {
    it(text, t => {
        let results = extractor.extract(text);
        t.is(0, results.length);
    });
}

function basicTestWithOptions(it, text, count, options) {
    it(text, t => {
        let extractorWithOptions = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options);
        let results = extractorWithOptions.extract(text);
        t.is(count, results.length);
    });
}
