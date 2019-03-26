using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseHolidayExtractor : IDateTimeExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        private readonly IHolidayExtractorConfiguration config;

        public BaseHolidayExtractor(IHolidayExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(HolidayMatch(text));
            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        private List<Token> HolidayMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.HolidayRegexes)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }
    }
}
