using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeExtractor : IDateTimeExtractor
    {
        public static readonly Regex HourRegex =
            new Regex(BaseDateTime.HourRegex, RegexOptions.Singleline);

        public static readonly Regex MinuteRegex =
            new Regex(BaseDateTime.MinuteRegex, RegexOptions.Singleline);

        public static readonly Regex SecondRegex =
            new Regex(BaseDateTime.SecondRegex, RegexOptions.Singleline);

        private const string ExtractorName = Constants.SYS_DATETIME_TIME; // "Time";

        private readonly ITimeExtractorConfiguration config;

        public BaseTimeExtractor(ITimeExtractorConfiguration config)
        {
            this.config = config;
        }

        public virtual List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public virtual List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(AtRegexMatch(text));
            tokens.AddRange(BeforeAfterRegexMatch(text));
            tokens.AddRange(SpecialCasesRegexMatch(text, reference));

            var timeErs = Token.MergeAllTokens(tokens, text, ExtractorName);

            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                timeErs = TimeZoneUtility.MergeTimeZones(timeErs, config.TimeZoneExtractor.Extract(text, reference), text);
            }

            return timeErs;
        }

        private List<Token> BasicRegexMatch(string text)
        {
            var result = new List<Token>();

            foreach (var regex in this.config.TimeRegexList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    result.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return result;
        }

        private List<Token> AtRegexMatch(string text)
        {
            var result = new List<Token>();

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

                    result.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return result;
        }

        private List<Token> BeforeAfterRegexMatch(string text)
        {
            var result = new List<Token>();

            // only enabled in CalendarMode
            if ((this.config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                // handle "before 3", "after three"
                var beforeAfterRegex = this.config.TimeBeforeAfterRegex;
                if (beforeAfterRegex.IsMatch(text))
                {
                    var matches = beforeAfterRegex.Matches(text);
                    foreach (Match match in matches)
                    {
                        result.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }
            }

            return result;
        }

        private List<Token> SpecialCasesRegexMatch(string text, DateObject reference)
        {
            var result = new List<Token>();

            // handle "ish"
            if (this.config.IshRegex != null && this.config.IshRegex.IsMatch(text))
            {
                var matches = this.config.IshRegex.Matches(text);

                foreach (Match match in matches)
                {
                    result.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return result;
        }
    }
}
