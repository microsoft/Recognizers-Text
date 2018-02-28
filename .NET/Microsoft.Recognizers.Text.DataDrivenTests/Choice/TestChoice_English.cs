using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoice_English : TestBase
    {
        public static TestResources TestResources { get; protected set; }
        public static IDictionary<string, IModel> Models { get; protected set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Models = new Dictionary<string, IModel>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestSpecInitialize(TestResources);
            base.ModelInitialize(Models);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BooleanModel-English.csv", "BooleanModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BooleanModel()
        {
            TestChoice();
        }
    }
}
