using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestTimeExtractor
    {
        private readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, results[0].Type);
        }

        [TestMethod]
        public void TestTimeExtract()
        {
            BasicTest("I'll be back at 7", 16, 1);
            BasicTest("I'll be back at seven", 16, 5);
            BasicTest("I'll be back 7pm", 13, 3);
            BasicTest("I'll be back 7p.m.", 13, 5);
            BasicTest("I'll be back 7:56pm", 13, 6);
            BasicTest("I'll be back 7:56:35pm", 13, 9);
            BasicTest("I'll be back 7:56:35 pm", 13, 10);
            BasicTest("I'll be back 12:34", 13, 5);
            BasicTest("I'll be back 12:34:20", 13, 8);
            BasicTest("I'll be back T12:34:20", 13, 9);
            BasicTest("I'll be back 00:00", 13, 5);
            BasicTest("I'll be back 00:00:30", 13, 8);

            BasicTest("It's 7 o'clock", 5, 9);
            BasicTest("It's seven o'clock", 5, 13);
            BasicTest("It's 8 in the morning", 5, 16);
            BasicTest("It's 8 in the night", 5, 14);


            BasicTest("It's half past eight", 5, 15);
            BasicTest("It's half past 8pm", 5, 13);
            BasicTest("It's 30 mins past eight", 5, 18);
            BasicTest("It's a quarter past eight", 5, 20);
            BasicTest("It's quarter past eight", 5, 18);
            BasicTest("It's three quarters past 9pm", 5, 23);
            BasicTest("It's three minutes to eight", 5, 22);

            BasicTest("It's half past seven o'clock", 5, 23);
            BasicTest("It's half past seven afternoon", 5, 25);
            BasicTest("It's half past seven in the morning", 5, 30);
            BasicTest("It's a quarter to 8 in the morning", 5, 29);
            BasicTest("It's 20 min past eight in the evening", 5, 32);

            BasicTest("I'll be back in the afternoon at 7", 13, 21);
            BasicTest("I'll be back afternoon at 7", 13, 14);
            BasicTest("I'll be back afternoon 7:00", 13, 14);
            BasicTest("I'll be back afternoon 7:00:14", 13, 17);
            BasicTest("I'll be back afternoon seven pm", 13, 18);

            BasicTest("I'll go back seven thirty pm", 13, 15);
            BasicTest("I'll go back seven thirty five pm", 13, 20);
            BasicTest("I'll go back at eleven five", 16, 11);
            BasicTest("I'll go back three mins to five thirty ", 13, 25);
            BasicTest("I'll go back five thirty in the night", 13, 24);
            BasicTest("I'll go back in the night five thirty", 13, 24);

            BasicTest("I'll be back noonish", 13, 7);
            BasicTest("I'll be back noon", 13, 4);
            BasicTest("I'll be back 11ish", 13, 5);
            BasicTest("I'll be back 11-ish", 13, 6);

            BasicTest("I'll be back 340pm", 13, 5);
            BasicTest("I'll be back 1140 a.m.", 13, 9);

            BasicTest("midnight", 0, 8);
            BasicTest("mid-night", 0, 9);
            BasicTest("mid night", 0, 9);
            BasicTest("midmorning", 0, 10);
            BasicTest("mid-morning", 0, 11);
            BasicTest("mid morning", 0, 11);
            BasicTest("midafternoon", 0, 12);
            BasicTest("mid-afternoon", 0, 13);
            BasicTest("mid afternoon", 0, 13);
            BasicTest("midday", 0, 6);
            BasicTest("mid-day", 0, 7);
            BasicTest("mid day", 0, 7);
            BasicTest("noon", 0, 4);
        }
    }
}