using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;
using System;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeALTExtractor: IDateTimeListExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEALT; // "DateTimeALT";

        private readonly IDateTimeALTExtractorConfiguration config;

        public BaseDateTimeALTExtractor(IDateTimeALTExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(List<ExtractResult> extractResult, string text)
        {
            return Extract(extractResult, text, DateObject.Now);
        }

        public List<ExtractResult> Extract(List<ExtractResult> extractResult, string text, DateObject reference)
        {
            extractResult = ExtractALT(extractResult, text, reference);

            return extractResult;
        }

        // modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
        // modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
        public List<ExtractResult> ExtractALT(List<ExtractResult> extractResult, string text, DateObject reference)
        {
            var ers = extractResult;
            ers = ers.OrderBy(o => o.Start).ToList();

            var i = 0;
            while (i < ers.Count - 1)
            {
                var j = i + 1;

                if (j >= ers.Count)
                {
                    break;
                }

                // check whether middle string is a connector
                var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                var middleEnd = ers[j].Start ?? 0;
                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                var matches = this.config.OrRegex.Matches(middleStr);
                if (matches.Count == 1)
                {
                    // extract different data accordingly
                    var data = ExtractALT(ers[i], ers[j]);
                    if (data.Count > 0)
                    {
                        ers[j].Type = Constants.SYS_DATETIME_DATETIMEALT;
                        ers[j].Data = data;

                        i = j + 1;
                        continue;
                    }
                }
                
                i = j;
            }

            return ers;
        }

        private Dictionary<string, object> ExtractALT(ExtractResult former, ExtractResult latter)
        {
            var data = ExtractDateTime_Time(former, latter);

            if (data.Count == 0)
            {
                data = ExtractDate_Date(former, latter);
            }
            if (data.Count == 0)
            {
                data = ExtractTime_Time(former, latter);
            }

            return data;
        }

        private Dictionary<string, object> ExtractDateTime_Time(ExtractResult former, ExtractResult latter)
        {
            var data = new Dictionary<string, object>();
            // modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
            if (former.Type == Constants.SYS_DATETIME_DATETIME && latter.Type == Constants.SYS_DATETIME_TIME)
            {
                var ers = config.DateExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    data.Add(Constants.Context, ers[0]);
                    data.Add(Constants.SubType, Constants.SYS_DATETIME_TIME);
                }
            }
            return data;
        }

        private Dictionary<string, object> ExtractDate_Date(ExtractResult former, ExtractResult latter)
        {
            var data = new Dictionary<string, object>();
            // modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
            if (former.Type == Constants.SYS_DATETIME_DATE && latter.Type == Constants.SYS_DATETIME_DATE)
            {
                var ers = config.DatePeriodExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    data.Add(Constants.Context, ers[0]);
                    data.Add(Constants.SubType, Constants.SYS_DATETIME_DATE);
                }
                else
                {
                    // "Thursday" in "next/last/this Tuesday or Thursday"
                    foreach (var regex in config.RelativePrefixList)
                    {
                        var match = regex.Match(former.Text);
                        if (match.Success)
                        {
                            var contextErs = new ExtractResult();
                            contextErs.Text = match.Value;
                            contextErs.Start = match.Index;
                            contextErs.Length = match.Length;
                            contextErs.Type = TimeTypeConstants.relativePrefixMod;
                            data.Add(Constants.Context, contextErs);
                            data.Add(Constants.SubType, Constants.SYS_DATETIME_DATE);
                        }
                    }
                }
            }
            return data;
        }

        private Dictionary<string, object> ExtractTime_Time(ExtractResult former, ExtractResult latter)
        {
            var data = new Dictionary<string, object>();
            if (former.Type == Constants.SYS_DATETIME_TIME && latter.Type == Constants.SYS_DATETIME_TIME)
            {
                // "8 oclock" in "in the morning at 7 oclock or 8 oclock"
                foreach (var regex in config.AmPmRegexList)
                {
                    var match = regex.Match(former.Text);
                    if (match.Success)
                    {
                        var contextErs = new ExtractResult();
                        contextErs.Text = match.Value;
                        contextErs.Start = match.Index;
                        contextErs.Length = match.Length;
                        contextErs.Type = TimeTypeConstants.AmPmMod;
                        data.Add(Constants.Context, contextErs);
                        data.Add(Constants.SubType, Constants.SYS_DATETIME_TIME);
                    }
                }
            }
            return data;
        }
    }
}