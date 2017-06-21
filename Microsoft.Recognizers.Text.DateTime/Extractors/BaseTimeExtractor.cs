using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeExtractor : IExtractor
    {
        private static readonly string ExtractorName = Constants.SYS_DATETIME_TIME; // "Time";

        public static readonly Regex HourRegex =
            new Regex(
                @"(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteRegex =
            new Regex(
                @"(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|0|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SecondRegex =
            new Regex(
                @"(?<sec>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|0|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly ITimeExtractorConfiguration config;

        public BaseTimeExtractor(ITimeExtractorConfiguration config)
        {
            this.config = config;
        }

        public virtual List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(AtRegexMatch(text));
            tokens.AddRange(SpecialsRegexMatch(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        private List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.TimeRegexList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }

        private List<Token> AtRegexMatch(string text)
        {
            var ret = new List<Token>();
            // handle "at 5", "at seven"
            if (this.config.AtRegex.IsMatch(text))
            {
                var matches = this.config.AtRegex.Matches(text);
                foreach (Match match in matches)
                {
                    if (match.Index + match.Length < text.Length &&
                        text[match.Index + match.Length].Equals('%'))
                    {
                        continue;
                    }
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }

        private List<Token> SpecialsRegexMatch(string text)
        {
            var ret = new List<Token>();
            // handle "ish"
            if (this.config.IshRegex != null && this.config.IshRegex.IsMatch(text))
            {
                var matches = this.config.IshRegex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }
    }
}
