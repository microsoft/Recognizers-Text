using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{

    [TestClass]
    public class TestSplitDateAndTime
    {
        public readonly DateTimeModel model = DateTimeRecognizer.GetSingleCultureInstance(Culture.English, DateTimeOptions.SplitDateAndTime).GetDateTimeModel();
        readonly DateObject referenceDate;

        public TestSplitDateAndTime()
        {
            referenceDate = new DateObject(2016, 11, 7);
        }

        public void BasicTest(string text, int numberOfEntities)
        {
            var results = model.Parse(text, referenceDate);
            Assert.AreEqual(results.Count, numberOfEntities);
        }

        [TestMethod]
        public void TestSplitDateAndTimeCount()
        {
            BasicTest("schedule a meeting 2 hours later", 1);
            BasicTest("schedule a meeting tomorrow morning at 7pm", 2);
        }
    }
}
