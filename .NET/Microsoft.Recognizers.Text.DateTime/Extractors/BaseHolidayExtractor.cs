// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseHolidayExtractor : IDateTimeExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";
        private const string RangeExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "Daterange";

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
            var ers = Token.MergeAllTokens(tokens, text, ExtractorName);
            foreach (var er in ers)
            {
                // If this is a daterange that contains a holiday, we should change its
                // type to indicate that.

                if (er.Metadata?.IsHolidayRange ?? false)
                {
                    er.Type = RangeExtractorName;
                }
            }

            return ers;
        }

        private List<Token> HolidayMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.HolidayRegexes)
            {
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    var metaData = new Metadata();

                    // The objective here is to not lose the information of the holiday name
                    // and year (if captured) when choosing. The data is extracted from the match
                    // groups.

                    if (match.Groups[Constants.HolidayWeekend].Success)
                    {
                        metaData.IsHolidayRange = metaData.IsHolidayWeekend = true;
                        metaData.HolidayName = match.Groups["holiday"].Value;
                        if (match.Groups["year"].Success)
                        {
                            metaData.HolidayName = metaData.HolidayName + " " + match.Groups["year"].Value;
                        }
                    }

                    metaData.IsHoliday = true;

                    ret.Add(new Token(match.Index, match.Index + match.Length, metaData));
                }
            }

            return ret;
        }
    }
}
