using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            base.TestSpecInitialize(TestResources);
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
    }
}
