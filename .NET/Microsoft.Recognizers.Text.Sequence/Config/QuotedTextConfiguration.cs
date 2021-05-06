using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class QuotedTextConfiguration : ISequenceConfiguration
    {
        public QuotedTextConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public Regex QuotedTextRegex1 { get; set; }

        public Regex QuotedTextRegex2 { get; set; }

        public Regex QuotedTextRegex3 { get; set; }

        public Regex QuotedTextRegex4 { get; set; }

        public Regex QuotedTextRegex5 { get; set; }

        public Regex QuotedTextRegex6 { get; set; }

        public Regex QuotedTextRegex7 { get; set; }

        public Regex QuotedTextRegex8 { get; set; }

        public Regex QuotedTextRegex9 { get; set; }

    }
}
