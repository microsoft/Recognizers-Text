// Enable a reference time pass to the extractor

using DateObject = System.DateTime;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    interface IDateTimeExtractor : IExtractor
    {
        List<ExtractResult> Extract(string input, DateObject reference);
    }
}
