using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoice_Spanish : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void BooleanModel(TestModel testSpec)
        {
            TestChoice(testSpec);
        }
    }
}
