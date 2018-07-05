using DateObject = System.DateTime;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeZoneExtractor : IDateTimeExtractor
    {
        List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> extractResult);
    }
}