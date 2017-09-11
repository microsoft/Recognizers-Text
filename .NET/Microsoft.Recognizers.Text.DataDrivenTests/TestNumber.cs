using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    [TestClass]
    public class TestNumber
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"numberResults.csv", "numberResults#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel_FromDataSource()
        {
            var language = TestContext.DataRow[0].ToString();
            var fileName = TestContext.DataRow[1].ToString();
            var method = TestContext.DataRow[2].ToString();
            var model = TestContext.DataRow[3].ToString();
            var source = TestContext.DataRow[4].ToString();
            var resultCount = Convert.ToInt32(TestContext.DataRow[5]);
            var result = TestContext.DataRow[6].ToString();
            Assert.AreEqual(3, language.Length);
            Assert.IsNotNull(fileName);
            Assert.IsNotNull(method);
            Assert.IsNotNull(model);
            Assert.IsNotNull(source);
            Assert.IsTrue(resultCount >= 0);
            if (!string.IsNullOrEmpty(result))
            {
                Assert.IsTrue(result.EndsWith("}"));
            }
        }
    }
}
