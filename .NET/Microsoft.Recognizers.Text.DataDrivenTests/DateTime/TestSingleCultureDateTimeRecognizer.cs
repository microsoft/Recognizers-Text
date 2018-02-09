using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestSingleCultureDateTimeRecognizer
    {
        private readonly IModel model = new DateTimeRecognizer(Culture.English).GetDateTimeModel();

        public void BasicTest(string text, string expected)
        {
            var pr = model.Parse(text);
            Assert.AreEqual(1, pr.Count);
            Assert.AreEqual(expected, pr[0].Text);
            
            var values = pr[0].Resolution["values"] as IEnumerable<Dictionary<string, string>>;
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, values.First()["type"]);
        }

        [TestMethod]
        public void TestSingleCultureDateExtract()
        {
            BasicTest("I'll go back now", "now");
        }
    }
}
