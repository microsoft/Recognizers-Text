using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequence_Japanese : TestBase
    {
        public static TestResources TestResources { get; protected set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "URLModel-Japanese.csv", "URLModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void URLModel()
        {
            TestURL();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PhoneNumberModel-Japanese.csv", "PhoneNumberModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PhoneNumberModel()
        {
            TestPhoneNumber();
        }
    }
}
