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

        public void BasicTest(string text, string typeName)
        {
            var results = model.Parse(text, referenceDate);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].TypeName.Replace("datetimeV2.", ""), typeName);
        }

        [TestMethod]
        public void TestSplitDateAndTimeCount()
        {
            BasicTest("schedule a meeting tomorrow from 5pm to 7pm", 3);
            BasicTest("schedule a meeting today from 5pm to 7pm", 3);
            BasicTest("schedule a meeting next Monday from 5pm to 7pm", 3);

            BasicTest("schedule a meeting from 5pm to 7pm tomorrow", 3);
            BasicTest("schedule a meeting from 5pm to 7pm today", 3);
            BasicTest("schedule a meeting from 5pm to 7pm next Monday", 3);

            //from 5 to 7pm is a time period which is extracted by a whole regex but not merge of two time points
            BasicTest("schedule a meeting tomorrow from 5 to 7pm", 2);

            BasicTest("schedule a meeting from Sep.1st to Sep.5th", 2);
            BasicTest("schedule a meeting from July the 5th to July the 8th", 2);

            BasicTest("schedule a meeting from 5:30 to 7:00", 2);
            BasicTest("schedule a meeting from 5pm to 7pm", 2);
            BasicTest("schedule a meeting from 5am to 7pm", 2);

            BasicTest("schedule a meeting 2 hours later", 1);
            BasicTest("schedule a meeting 2 days later", 1);
            BasicTest("I had 2 minutes ago", 1);

            BasicTest("schedule a meeting tomorrow at 7pm", 2);
            BasicTest("schedule a meeting tomorrow morning at 7pm", 2);
        }

        [TestMethod]
        public void TestSplitDateAndTimeTypeName()
        {
            BasicTest("I'll be out next hour", Constants.SYS_DATETIME_DURATION);
            BasicTest("I'll be out next 5 minutes", Constants.SYS_DATETIME_DURATION);
            BasicTest("I'll be out next 3 days", Constants.SYS_DATETIME_DURATION);
            BasicTest("schedule a meeting now", Constants.SYS_DATETIME_TIME);
            BasicTest("schedule a meeting tongiht at 7", Constants.SYS_DATETIME_TIME);
            BasicTest("schedule a meeting tongiht at 7pm", Constants.SYS_DATETIME_TIME);
            BasicTest("schedule a meeting 2 hours later", Constants.SYS_DATETIME_DURATION);
        }
    }
}
