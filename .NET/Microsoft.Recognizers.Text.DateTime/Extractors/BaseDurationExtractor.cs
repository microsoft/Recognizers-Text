using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDurationExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DURATION;

        private readonly IDurationExtractorConfiguration config;

        public BaseDurationExtractor(IDurationExtractorConfiguration config)
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
            tokens.AddRange(NumberWithUnit(text));
            tokens.AddRange(NumberWithUnitAndSuffix(text, NumberWithUnit(text)));
            tokens.AddRange(ImplicitDuration(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }
        
        // handle cases look like: {number} {unit}? and {an|a} {half|quarter} {unit}?
        // define the part "and {an|a} {half|quarter}" as Suffix
        private List<Token> NumberWithUnitAndSuffix(string text, List<Token> ers)
        {
            var ret = new List<Token>();
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length);
                var match = this.config.SuffixAndRegex.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    ret.Add(new Token(er.Start, (er.Start + er.Length) + match.Length));
                }
            }
            return ret;
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
            ret.AddRange(GetTokenFromRegex(config.NumberCombinedWithUnit, text));

            // handle "an hour"
            ret.AddRange(GetTokenFromRegex(config.AnUnitRegex, text));

            // handle "few" related cases
            ret.AddRange(GetTokenFromRegex(config.InExactNumberUnitRegex, text));

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

            // handle "next day", "last year"
            ret.AddRange(GetTokenFromRegex(config.RelativeDurationUnitRegex, text));

            return ret;
        }

        private static List<Token> GetTokenFromRegex(Regex regex, string text)
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
