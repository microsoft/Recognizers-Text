// A type of Extractors receiving ExtractResult List as its input

using DateObject = System.DateTime;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeListExtractor
    {
        List<ExtractResult> Extract(List<ExtractResult> extractResult, string text, DateObject reference);
    }
}