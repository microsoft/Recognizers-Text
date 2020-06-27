using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequence_English : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void PhoneNumberModel(TestModel testSpec)
        {
            TestPhoneNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void IpAddressModel(TestModel testSpec)
        {
            TestIpAddress(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void MentionModel(TestModel testSpec)
        {
            TestMention(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HashtagModel(TestModel testSpec)
        {
            TestHashtag(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void EmailModel(TestModel testSpec)
        {
            TestEmail(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void URLModel(TestModel testSpec)
        {
            TestURL(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void GUIDModel(TestModel testSpec)
        {
            TestGUID(testSpec);
        }
    }
}
