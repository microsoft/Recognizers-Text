using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDurationExtractor : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DURATION;

        private readonly IDurationExtractorConfiguration config;

        public BaseDurationExtractor(IDurationExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(NumberWithUnit(text));
            tokens.AddRange(ImplicitDuration(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // simple cases made by a number followed an unit
        private List<Token> NumberWithUnit(string text)
        {
            var ret = new List<Token>();
            var ers = this.config.CardinalExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            // handle "3hrs"
            var matches = this.config.NumberCombinedWithUnit.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // handle "an hour"
            matches = this.config.AnUnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // handle cases that don't contain nubmer
        private List<Token> ImplicitDuration(string text)
        {
            var ret = new List<Token>();
            // handle "all day", "all year"
            ret.AddRange(GetTokenFromRegex(config.AllRegex, text));

            // handle "half day", "half year"
            ret.AddRange(GetTokenFromRegex(config.HalfRegex, text));

            return ret;
        }

        private List<Token> GetTokenFromRegex(Regex regex, string text)
        {
            var ret = new List<Token>();
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }
            return ret;
        } 
    }
}
