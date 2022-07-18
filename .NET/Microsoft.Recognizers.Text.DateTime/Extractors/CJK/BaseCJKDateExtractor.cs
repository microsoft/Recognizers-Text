// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.InternalCache;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly ICJKDateExtractorConfiguration config;

        public BaseCJKDateExtractor(ICJKDateExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            var result = new List<ExtractResult>();

            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(DurationWithAgoAndLater(text, referenceTime));
            result = Token.MergeAllTokens(tokens, text, ExtractorName);

            result = ExtractResultExtension.FilterAmbiguity(result, text, this.config.AmbiguityDateFiltersDict);

            return result;
        }

        // Match basic patterns in DateRegexList
        private List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.DateRegexList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    // some match might be part of the date range entity, and might be split in a wrong way
                    if (DateContext.ValidateMatch(match, text, this.config.DateRegexList, this.config.RangeConnectorSymbolRegex))
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length));

                    }
                }
            }

            return ret;
        }

        // Match several other implicit cases
        private List<Token> ImplicitDate(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.ImplicitDateList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        // process case like "三天前" "两个月前"
        private List<Token> DurationWithAgoAndLater(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();

            var durationEr = this.config.DurationExtractor.Extract(text, referenceTime);

            foreach (var er in durationEr)
            {
                // Only handles date durations here
                // Cases with dateTime durations will be handled in DateTime Extractor
                if (this.config.DateTimePeriodUnitRegex.Match(er.Text).Success)
                {
                    continue;
                }

                var pos = (int)er.Start + (int)er.Length;

                if (pos < text.Length)
                {
                    var suffix = text.Substring(pos);
                    var match = this.config.BeforeRegex.Match(suffix);
                    if (!match.Success)
                    {
                        match = this.config.AfterRegex.Match(suffix);
                    }

                    if (match.Success && suffix.Trim().StartsWith(match.Value, StringComparison.Ordinal))
                    {
                        var metadata = new Metadata() { IsDurationWithAgoAndLater = true };
                        ret.Add(new Token((int)er.Start, (int)(er.Start + er.Length) + match.Index + match.Length, metadata));
                    }
                }
            }

            // Extend extraction with weekdays like in "Friday two weeks from now", "in 3 weeks on Monday"
            ret.AddRange(ExtendWithWeekDay(ret, text));

            return ret;
        }

        private List<Token> ExtendWithWeekDay(List<Token> ret, string text)
        {
            var newRet = new List<Token>();
            foreach (var er in ret)
            {
                var beforeStr = text.Substring(0, er.Start);
                var afterStr = text.Substring(er.End);
                var beforeMatch = this.config.WeekDayStartEnd.Match(beforeStr);
                var afterMatch = this.config.WeekDayStartEnd.Match(afterStr);
                if (beforeMatch.Success || afterMatch.Success)
                {
                    var start = beforeMatch.Success ? beforeMatch.Index : er.Start;
                    var end = beforeMatch.Success ? er.End : er.End + afterMatch.Index + afterMatch.Length;
                    Metadata metadata = new Metadata { IsDurationDateWithWeekday = true };
                    Token tok = new Token(start, end, metadata);
                    newRet.Add(tok);
                }
            }

            return newRet;
        }
    }
}
