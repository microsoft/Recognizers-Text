using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeZoneExtractor : IDateTimeExtractor
    {
        List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> extractResult);
    }
}