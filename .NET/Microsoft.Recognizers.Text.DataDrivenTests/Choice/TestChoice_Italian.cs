using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoice_Italian : TestBase
    {
        public static TestResources TestResources { get; protected set; }

        [NetCoreTestDataSource]
        [TestMethod]
        public void BooleanModel(TestModel testSpec)
        {
            TestChoice(testSpec);
        }
    }
}
