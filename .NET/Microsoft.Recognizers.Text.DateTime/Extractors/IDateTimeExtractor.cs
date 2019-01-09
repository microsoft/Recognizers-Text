// Enable a reference time pass to the extractor

using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeExtractor : IExtractor
    {
        List<ExtractResult> Extract(string input, DateObject reference);
    }
}
