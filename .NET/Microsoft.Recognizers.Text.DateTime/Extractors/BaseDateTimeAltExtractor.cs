using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;
using System;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeAltExtractor: IDateTimeListExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEALT; // "DateTimeALT";

        private readonly IDateTimeAltExtractorConfiguration config;

        public BaseDateTimeAltExtractor(IDateTimeAltExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(List<ExtractResult> extractResult, string text)
        {
            return Extract(extractResult, text, DateObject.Now);
        }

        public List<ExtractResult> Extract(List<ExtractResult> extractResult, string text, DateObject reference)
        {
            extractResult = ExtractAlt(extractResult, text, reference);

            return extractResult;
        }

        // modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
        // modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
        public List<ExtractResult> ExtractAlt(List<ExtractResult> extractResult, string text, DateObject reference)
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
                    var data = ExtractAlt(ers[i], ers[j]);
                    if (data.Count > 0)
                    {
                        var parentTextStart = ers[i].Start;
                        var parentTextLen = ers[j].Start + ers[j].Length - ers[i].Start;
                        var parentText = text.Substring(parentTextStart ?? 0, parentTextLen ?? 0);

                        data.Add(Constants.ParentText, parentText);
                        ers[j].Type = Constants.SYS_DATETIME_DATETIMEALT;
                        ers[j].Data = data;

                        ers[i].Data = new Dictionary<string, object> {
                                { Constants.SubType, ers[i].Type },
                                { Constants.ParentText, parentText }
                            };
                        ers[i].Type = Constants.SYS_DATETIME_DATETIMEALT;

                        i = j + 1;
                        continue;
                    }
                }
                
                i = j;
            }

            return ers;
        }

        private Dictionary<string, object> ExtractAlt(ExtractResult former, ExtractResult latter)
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

            if (data.Count == 0)
            {
                data = ExtractDateTime_DateTime(former, latter);
            }

            if (data.Count == 0)
            {
                data = ExtractDateTimeRange_TimeRange(former, latter);
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

        private Dictionary<string, object> ExtractDateTimeRange_TimeRange(ExtractResult former, ExtractResult latter)
        {
            var data = new Dictionary<string, object>();
            // modify time entity to an alternative DateTimeRange expression, such as "9-10 am" in "Monday 7-8 am or 9-10 am"
            if (former.Type == Constants.SYS_DATETIME_DATETIMEPERIOD && latter.Type == Constants.SYS_DATETIME_TIMEPERIOD)
            {
                var ers = config.DateExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    data.Add(Constants.Context, ers[0]);
                    data.Add(Constants.SubType, Constants.SYS_DATETIME_TIMEPERIOD);
                }
            }
            return data;
        }

        private Dictionary<string, object> ExtractDateTime_DateTime(ExtractResult former, ExtractResult latter)
        {
            var data = new Dictionary<string, object>();
            // modify time entity to an alternative DateTime expression, such as "Tue 1 pm" in "next week Mon 9 am or Tue 1 pm"
            if (former.Type == Constants.SYS_DATETIME_DATETIME && latter.Type == Constants.SYS_DATETIME_DATETIME)
            {
                var ers = config.DatePeriodExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    data.Add(Constants.Context, ers[0]);
                    data.Add(Constants.SubType, Constants.SYS_DATETIME_DATETIME);
                }
            }
            return data;
        }
    }
}