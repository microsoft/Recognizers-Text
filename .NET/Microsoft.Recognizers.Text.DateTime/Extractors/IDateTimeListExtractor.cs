// A type of Extractors receiving ExtractResult List as its input

using System.Collections.Generic;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeListExtractor
    {
        List<ExtractResult> Extract(List<ExtractResult> extractResult, string text, DateObject reference);
    }
}