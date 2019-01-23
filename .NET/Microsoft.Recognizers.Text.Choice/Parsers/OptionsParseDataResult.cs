using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public class OptionsParseDataResult
    {
        public double Score { get; set; }

        public IEnumerable<OptionsOtherMatchParseResult> OtherMatches { get; set; }
    }
}
