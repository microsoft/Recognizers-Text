using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseEmailExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_EMAIL;

        private static Regex EmailRegex2 { get; } = new Regex(BaseEmail.EmailRegex2,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public BaseEmailExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseEmail.EmailRegex), Constants.EMAIL_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        public override List<ExtractResult> Extract(string text)
        {
            var ret = base.Extract(text);

            if (!ret.Any())
            {
                ret.AddRange(ExtractEmail(text, EmailRegex2));
            }

            return ret;
        }

        private List<ExtractResult> ExtractEmail(string text, Regex emailRegex)
        {
            var ret = new List<ExtractResult>();
            var emailMatches = emailRegex.Matches(text);

            foreach (Match emailMatch in emailMatches)
            {
                ret.Add(new ExtractResult
                {
                    Start = emailMatch.Index,
                    Length = emailMatch.Length,
                    Text = emailMatch.Value,
                    Type = ExtractType,
                    Data = Constants.EMAIL_REGEX
                });
            }

            return ret;
        }
    }
}
