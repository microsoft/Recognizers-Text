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
                while (j < ers.Count && ers[i].IsOverlap(ers[j]))
                {
                    j++;
                }

                if (j >= ers.Count)
                {
                    break;
                }

                var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                var middleEnd = ers[j].Start ?? 0;
                if (middleBegin > middleEnd)
                {
                    i = j + 1;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                if (this.config.IsConnector(middleStr))
                {
                    if (isALT(ers[i], ers[j], out var data))
                    {
                        ers[j].Type = Constants.SYS_DATETIME_DATETIMEALT;
                        //var data = new Dictionary<string, object>();
                        //data.Add(Constants.Context, contextErs);
                        ers[j].Data = data;

                        i = j + 1;
                        continue;
                    }
                }
                
                i = j;
            }

            return ers;
        }

        private bool isALT(ExtractResult former, ExtractResult latter, out Dictionary<string, object> data)
        {
            var alt = false;
            data = new Dictionary<string, object>();
            // modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
            if (former.Type == Constants.SYS_DATETIME_DATETIME && latter.Type == Constants.SYS_DATETIME_TIME)
            {
                var ers = config.DateExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    alt = true;
                    data.Add(Constants.Context, ers[0]);
                    data.Add(Constants.SubType, Constants.SYS_DATETIME_TIME);
                }
            }
            // modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
            else if (former.Type == Constants.SYS_DATETIME_DATE && latter.Type == Constants.SYS_DATETIME_DATE)
            {
                var ers = config.DatePeriodExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    alt = true;
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
                            alt = true;
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
            else if (former.Type == Constants.SYS_DATETIME_TIME && latter.Type == Constants.SYS_DATETIME_TIME)
            {
                // "8 oclock" in "in the morning at 7 oclock or 8 oclock"
                foreach (var regex in config.AmPmRegexList)
                {
                    var match = regex.Match(former.Text);
                    if (match.Success)
                    {
                        alt = true;
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

                return alt;
        }
    }
}