using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequence_English : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PhoneNumberModel-English.csv", "PhoneNumberModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PhoneNumberModel()
        {
            TestPhoneNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "IpAddressModel-English.csv", "IpAddressModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void IpAddressModel()
        {
            TestIpAddress();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MentionModel-English.csv", "MentionModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MentionModel()
        {
            TestMention();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HashtagModel-English.csv", "HashtagModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HashtagModel()
        {
            TestHashtag();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "EmailModel-English.csv", "EmailModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void EmailModel()
        {
            TestEmail();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "URLModel-English.csv", "URLModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void URLModel()
        {
            TestURL();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "GUIDModel-English.csv", "GUIDModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void GUIDModel()
        {
            TestGUID();
        }
    }
}
