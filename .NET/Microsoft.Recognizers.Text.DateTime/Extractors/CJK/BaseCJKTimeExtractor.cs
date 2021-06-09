using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.InternalCache;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKTimeExtractor : IDateTimeExtractor
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

        private readonly ICJKTimeExtractorConfiguration config;

        public BaseCJKTimeExtractor(ICJKTimeExtractorConfiguration config)
        {
            this.config = config;
            keyPrefix = string.Intern(config.Options + "_" + config.LanguageMarker);
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public virtual List<ExtractResult> Extract(string source, DateObject referenceTime)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var result = new List<ExtractResult>();
            var matchSource = new Dictionary<Match, TimeType>();
            var matched = new bool[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                matched[i] = false;
            }

            foreach (var collection in this.config.Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value))
            {
                foreach (Match m in collection.Key)
                {
                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.Add(m, collection.Value);
                }
            }

            var last = -1;
            for (var i = 0; i < source.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == source.Length || !matched[i + 1])
                    {
                        var start = last + 1;
                        var length = i - last;
                        var substr = source.Substring(start, length);

                        if (matchSource.Keys.Any(o => o.Index == start && o.Length == length))
                        {
                            var srcMatch = matchSource.Keys.First(o => o.Index == start && o.Length == length);
                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractorName,
                                Data = matchSource.ContainsKey(srcMatch) ?
                                    new DateTimeExtra<TimeType>
                                    {
                                        NamedEntity = srcMatch.Groups,
                                        Type = matchSource[srcMatch],
                                    }
                                    : null,
                            };
                            result.Add(er);
                        }
                    }
                }
                else
                {
                    last = i;
                }
            }

            return result;
        }
    }
}
