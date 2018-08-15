using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeExtractor : IDateTimeExtractor
    {
        private static readonly string ExtractorName = Constants.SYS_DATETIME_TIME; // "Time";

        public static readonly Regex HourRegex = new Regex(BaseDateTime.HourRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteRegex = new Regex(BaseDateTime.MinuteRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SecondRegex = new Regex(BaseDateTime.SecondRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

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
                timeErs = MergeTimeZones(timeErs, config.TimeZoneExtractor.Extract(text, reference), text);
            }

            return timeErs;
        }

        private List<ExtractResult> MergeTimeZones(List<ExtractResult> timeErs, List<ExtractResult> timeZoneErs, string text)
        {
            foreach (var er in timeErs)
            {
                foreach (var timeZoneEr in timeZoneErs)
                {
                    var begin = er.Start + er.Length;
                    var end = timeZoneEr.Start;

                    if (begin < end)
                    {
                        var gapText = text.Substring((int) begin, (int) (end - begin));

                        if (string.IsNullOrWhiteSpace(gapText))
                        {
                            var newLength = (int) (timeZoneEr.Start + timeZoneEr.Length - er.Start);

                            er.Text = text.Substring((int) er.Start, newLength);
                            er.Length = newLength;
                            er.Data = new KeyValuePair<string, ExtractResult>(Constants.SYS_DATETIME_TIMEZONE,
                                timeZoneEr);
                        }
                    }
                }
            }

            return timeErs;
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

        private List<Token> BeforeAfterRegexMatch(string text)
        {
            var ret = new List<Token>();
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
                        ret.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }
            }
            return ret;
        }

        private List<Token> SpecialCasesRegexMatch(string text, DateObject reference)
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
