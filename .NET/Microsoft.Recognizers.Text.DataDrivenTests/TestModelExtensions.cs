using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public static class TestModelExtensions
    {
        public static bool IsNotSupported(this TestModel testSpec)
        {
            return testSpec.NotSupported.HasFlag(Platform.DotNet);
        }

        public static bool IsNotSupportedByDesign(this TestModel testSpec)
        {
            return testSpec.NotSupportedByDesign.HasFlag(Platform.DotNet);
        }

        public static DateObject GetReferenceDateTime(this TestModel testSpec)
        {
            if (testSpec.Context.TryGetValue("ReferenceDateTime", out object dateTimeObject))
            {
                return (DateObject)dateTimeObject;
            }

            return DateObject.Now;
        }
    }
}