using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceParseDataResult
    {
        public double Score { get; set; }

        public IEnumerable<OtherMatchParseResult> OtherMatches { get; set; }
    }
}
