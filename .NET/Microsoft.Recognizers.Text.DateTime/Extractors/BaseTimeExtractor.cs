using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.InternalCache;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeExtractor : IDateTimeExtractor
    {
        public static readonly Regex HourRegex =
            new Regex(BaseDateTime.HourRegex, RegexOptions.Singleline | RegexOptions.Compiled);

        public static readonly Regex MinuteRegex =
            new Regex(BaseDateTime.MinuteRegex, RegexOptions.Singleline | RegexOptions.Compiled);

        public static readonly Regex SecondRegex =
            new Regex(BaseDateTime.SecondRegex, RegexOptions.Singleline | RegexOptions.Compiled);

        private const string ExtractorName = Constants.SYS_DATETIME_TIME; // "Time";

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly string keyPrefix;

        private readonly ITimeExtractorConfiguration config;

        public BaseTimeExtractor(ITimeExtractorConfiguration config)
        {
            this.config = config;
            keyPrefix = string.Intern(config.Options + "_" + config.LanguageMarker);
        }

        public virtual List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public virtual List<ExtractResult> Extract(string text, DateObject reference)
        {

            List<ExtractResult> results;

            if ((this.config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                results = ExtractImpl(text, reference);
            }
            else
            {
                var key = (keyPrefix, text, reference);

                results = ResultsCache.GetOrCreate(key, () => ExtractImpl(text, reference));
            }

            return results;
        }

        public virtual List<ExtractResult> ExtractImpl(string text, DateObject reference)
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

            // Remove common ambiguous cases
            timeErs = FilterAmbiguity(timeErs, text);

            return timeErs;
        }

        private List<Token> BasicRegexMatch(string text)
        {
            var results = new List<Token>();

            foreach (var regex in this.config.TimeRegexList)
            {
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    // Check that the extracted time is not part of a decimal number (e.g. 123.24)
                    if (match.Index > 1 && (text[match.Index - 1].Equals(',') || text[match.Index - 1].Equals('.')) &&
                        char.IsDigit(text[match.Index - 2]) && char.IsDigit(match.Value[0]))
                    {
                        continue;
                    }

                    // @TODO Workaround to avoid incorrect partial-only matches. Remove after time regex reviews across languages.
                    var lth = match.Groups["lth"].Value;

                    if (string.IsNullOrEmpty(lth) ||
                        (lth.Length != match.Length && !(match.Length == lth.Length + 1 && match.Value.EndsWith(" ", StringComparison.Ordinal))))
                    {
                        results.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }

            }

            return results;
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

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in this.config.AmbiguityFiltersDict)
                {
                    foreach (var extractResult in extractResults)
                    {
                        if (regex.Key.IsMatch(extractResult.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            extractResults = extractResults.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                .ToList();
                        }
                    }
                }
            }

            return extractResults;
        }
    }
}
