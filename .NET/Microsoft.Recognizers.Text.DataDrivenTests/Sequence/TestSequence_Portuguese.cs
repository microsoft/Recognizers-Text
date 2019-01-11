using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequence_Portuguese : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PhoneNumberModel-Portuguese.csv", "PhoneNumberModel-Portuguese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PhoneNumberModel()
        {
            TestPhoneNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "IpAddressModel-Portuguese.csv", "IpAddressModel-Portuguese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void IpAddressModel()
        {
            TestIpAddress();
        }
   }
}
