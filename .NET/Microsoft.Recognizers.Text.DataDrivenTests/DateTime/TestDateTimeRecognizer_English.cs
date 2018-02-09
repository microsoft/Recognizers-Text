using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Tests
{
    [TestClass]
    public class TestDateTimeRecognizer_English
    {
        [TestMethod]
        public void RecognizeDateTime()
        {
            var input = "I'll go back Oct/2";
            var refTime = System.DateTime.Parse("2016-11-07T00:00:00");

            var actual = DateTimeRecognizer.RecognizeDateTime(input, Culture.English,refTime: refTime);

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("datetimeV2.date", actual[0].TypeName);
            Assert.AreEqual("oct/2", actual[0].Text);
            var values = actual[0].Resolution["values"] as IList<Dictionary<string, string>>;

            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("XXXX-10-02", values[0]["timex"]);
            Assert.AreEqual("date", values[0]["type"]);
            Assert.AreEqual("2016-10-02", values[0]["value"]);

            Assert.AreEqual("XXXX-10-02", values[1]["timex"]);
            Assert.AreEqual("date", values[1]["type"]);
            Assert.AreEqual("2017-10-02", values[1]["value"]);
        }
    }
}
