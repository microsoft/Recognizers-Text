// Enable a reference time pass to the extractor

using DateObject = System.DateTime;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeExtractor : IExtractor
    {
        List<ExtractResult> Extract(string input, DateObject reference);
    }
}
