var describe = require('ava-spec').describe;
var configuration = require('../compiled/dateTime/english/extractorConfiguration').EnglishMergedExtractorConfiguration;
var baseExtractor = require('../compiled/dateTime/extractors').BaseMergedExtractor;
var constants = require('../compiled/dateTime/constants').Constants;

describe('Merged Extractor', it => {
    let extractor = new baseExtractor(new configuration());
    
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

            // TODO: implement options
            /*
            BasicTestWithOptions("Change my meeting from 9am to 11am", 2, DateTimeOptions.SkipFromToMerge);
            BasicTestWithOptions("Change my meeting from Nov.19th to Nov.23th", 2, DateTimeOptions.SkipFromToMerge);

            BasicTestWithOptions("Schedule a meeting from 9am to 11am", 1, DateTimeOptions.None); // Testing None.
            BasicTestWithOptions("Schedule a meeting from 9am to 11am", 1);
            BasicTestWithOptions("Schedule a meeting from 9am to 11am tomorrow", 1);

            BasicTestWithOptions("Change July 22nd meeting in Bellevue to August 22nd", 2); // No merge.
            */

            // after before
            BasicTest(it, extractor, "after 7/2 ", 0, 9);
            BasicTest(it, extractor, "since 7/2 ", 0, 9);
            BasicTest(it, extractor, "before 7/2 ", 0, 10);

            // date with time
            BasicTest(it, extractor, "06/06 12:15", 0, 11);
            BasicTest(it, extractor, "06/06/12 15:15", 0, 14);
            BasicTest(it, extractor, "06/06, 2015", 0, 11);
            
            //Unit tests for text should not extract datetime
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