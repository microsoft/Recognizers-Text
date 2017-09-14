using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestMergedExtractor
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), DateTimeOptions.None);

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        public void BasicTestNone(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
        }

        public void BasicTestWithOptions(string text, int count, DateTimeOptions options = DateTimeOptions.None)
        {
            IExtractor extractorWithOptions = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options);
            var results = extractorWithOptions.Extract(text);
            Assert.AreEqual(count, results.Count);
        }

        [TestMethod]
        public void TestMergedExtract()
        {
            BasicTest("this is 2 days", 8, 6);

            BasicTest("this is before 4pm", 8, 10);
            BasicTest("this is before 4pm tomorrow", 8, 19);
            BasicTest("this is before tomorrow 4pm ", 8, 19);

            BasicTest("this is after 4pm", 8, 9);
            BasicTest("this is after 4pm tomorrow", 8, 18);
            BasicTest("this is after tomorrow 4pm ", 8, 18);

            BasicTest("I'll be back in 5 minutes", 13, 12);
            BasicTest("past week", 0, 9);
            BasicTest("past Monday", 0, 11);
            BasicTest("schedule a meeting in 10 hours", 19, 11);

        }

        [TestMethod]
        public void TestMergedExtract_TheDuration()
        {
            BasicTest("What's this day look like?", 7, 8);
            BasicTest("What's this week look like?", 7, 9);
            BasicTest("What's my week look like?", 7, 7);
            BasicTest("What's the week look like?", 7, 8);
            BasicTest("What's my day look like?", 7, 6);
            BasicTest("What's the day look like?", 7, 7);
        }

        [TestMethod]
        public void TestMergedSkipFromTo()
        {
            BasicTestWithOptions("Change my meeting from 9am to 11am", 2, DateTimeOptions.SkipFromToMerge);
            BasicTestWithOptions("Change my meeting from Nov.19th to Nov.23th", 2, DateTimeOptions.SkipFromToMerge);

            BasicTestWithOptions("Schedule a meeting from 9am to 11am", 1, DateTimeOptions.None); // Testing None.
            BasicTestWithOptions("Schedule a meeting from 9am to 11am", 1);
            BasicTestWithOptions("Schedule a meeting from 9am to 11am tomorrow", 1);

            BasicTestWithOptions("Change July 22nd meeting in Bellevue to August 22nd", 2); // No merge.
        }

        [TestMethod]
        public void TestAfterBeforeSince()
        {
            BasicTest("after 7/2 ", 0, 9);
            BasicTest("since 7/2 ", 0, 9);
            BasicTest("before 7/2 ", 0, 10);
        }

        [TestMethod]
        public void TestDateWithTime()
        {
            BasicTest("06/06 12:15", 0, 11);
            BasicTest("06/06/12 15:15", 0, 14);
            BasicTest("06/06, 2015", 0, 11);
        }

        [TestMethod]
        public void TestAmbiguousWordExtract()
        {
            BasicTest("May 29th", 0, 8);
            BasicTest("March 29th", 0, 10);
            BasicTest("I born in March", 10, 5);
            BasicTest("I born in the March", 10, 9);
            BasicTest("what happend at the May", 16, 7);
        }

        [TestMethod]
        public void TestNegativeExtract()
        {
            //Unit tests for text should not extract datetime
            BasicTestNone("What are the hours of Palomino? ");

            BasicTestNone("in the sun");

            //commentted the following two for the ambiguous we met
            //BasicTestNone("may i help you");
            //BasicTestNone("the group proceeded with a march they knew would lead to bloodshed");

            BasicTestNone("which email have gotten a reply");
            BasicTestNone("He is often alone");
            BasicTestNone("often a bird");
            BasicTestNone("michigan hours");
        }
    }
}