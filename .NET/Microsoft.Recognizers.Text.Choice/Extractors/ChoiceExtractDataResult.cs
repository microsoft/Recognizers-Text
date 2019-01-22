using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Choice.Utilities;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceExtractDataResult
    {
        public IEnumerable<ExtractResult> OtherMatches { get; set; }

        public string Source { get; set; }

        public double Score { get; set; }
    }
}