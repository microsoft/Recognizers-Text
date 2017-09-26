using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestTimePeriodExtractor
    {
        private readonly BaseTimePeriodExtractor extractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.English, typeof(BaseTimePeriodExtractor));
        }

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, results[0].Type);
            TestWriter.Write(TestCulture.English, extractor, text, results);
        }

        [TestMethod]
        public void TestTimePeriodBasicExtract()
        {

            // basic match
            BasicTest("I'll be out 5 to 6pm", 12, 8);
            BasicTest("I'll be out 5 to 6 p.m.", 12, 11);
            BasicTest("I'll be out 5 to 6 in the afternoon", 12, 23);
            BasicTest("I'll be out 5 to seven in the morning", 12, 25);
            BasicTest("I'll be out from 5 to 6pm", 12, 13);
            BasicTest("I'll be out between 5 and 6pm", 12, 17);
            BasicTest("I'll be out between 5pm and 6pm", 12, 19);
            BasicTest("I'll be out between 5 and 6 in the afternoon", 12, 32);
        }

        [TestMethod]
        public void TestTimePeriodMergeExtract()
        {

            // merge to time points
            BasicTest("I'll be out 4pm till 5pm", 12, 12);
            BasicTest("I'll be out 4 til 5pm", 12, 9);
            BasicTest("I'll be out 4:00 till 5pm", 12, 13);
            BasicTest("I'll be out 4:00 til 5pm", 12, 12);
            BasicTest("I'll be out 4:00 to 7 oclock", 12, 16);
            BasicTest("I'll be out 3pm to half past seven", 12, 22);
            BasicTest("I'll be out 4pm-5pm", 12, 7);
            BasicTest("I'll be out 4pm - 5pm", 12, 9);
            BasicTest("I'll be out 20 minutes to three to eight in the evening", 12, 43);

            BasicTest("I'll be out from 4pm to 5pm", 12, 15);
            BasicTest("I'll be out from 4pm to half past five", 12, 26);
            BasicTest("I'll be out from 3 in the morning until 5pm", 12, 31);
            BasicTest("I'll be out from 3 in the morning until five in the afternoon", 12, 49);

            BasicTest("I'll be out between 4pm and half past five", 12, 30);
            BasicTest("I'll be out between 3 in the morning and 5pm", 12, 32);
        }

        [TestMethod]
        public void TestTimePeriodTimeOfDayExtract()
        {

            BasicTest("let's meet in the morning", 11, 14);
            BasicTest("let's meet in the afternoon", 11, 16);
            BasicTest("let's meet in the night", 11, 12);
            BasicTest("let's meet in the evening", 11, 14);
            BasicTest("let's meet in the evenings", 11, 15);

        }

        [TestMethod]
        public void TestTimePeriodEarlyLateExtract()
        {

            BasicTest("let's meet in the early-mornings", 11, 21);
            BasicTest("let's meet in the late-mornings", 11, 20);
            BasicTest("let's meet in the early-morning", 11, 20);
            BasicTest("let's meet in the late-morning", 11, 19);
            BasicTest("let's meet in the early-afternoon", 11, 22);
            BasicTest("let's meet in the late-afternoon", 11, 21);
            BasicTest("let's meet in the early-evening", 11, 20);
            BasicTest("let's meet in the late-evening", 11, 19);
            BasicTest("let's meet in the early-night", 11, 18);
            BasicTest("let's meet in the late-night", 11, 17);
            BasicTest("let's meet in the early night", 11, 18);
            BasicTest("let's meet in the late night", 11, 17);
        }

        [TestMethod]
        public void TestTimePeriodExtraExtract()
        {
            BasicTest("set up meeting from two to five pm", 15, 19);
            BasicTest("Party at Jean’s from 6 to 11 pm", 16, 15);
            BasicTest("set up meeting from 14:00 to 16:30", 15, 19);

            //TODO fix these for next release
            BasicTest("set up meeting from two to five p m", 15, 20);
            //BasicTest("set up meeting from 14 to 16h", 15, 14);
        }

    }
}