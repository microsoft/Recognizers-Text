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
            extractResult = DateTimeAndTimeALT(extractResult, text, reference);

            return extractResult;
        }

        // modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
        public List<ExtractResult> DateTimeAndTimeALT(List<ExtractResult> extractResult, string text, DateObject reference)
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

                if (ers[i].Type.Equals(Constants.SYS_DATETIME_DATETIME) && ers[j].Type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                    if (middleStr == "or")
                    {
                        var dateErs = this.config.DateExtractor.Extract(ers[i].Text);
                        if (dateErs.Count == 1)
                        {
                            ers[j].Type = Constants.SYS_DATETIME_DATETIMEALT;
                            var data = new Dictionary<string, object>();
                            data.Add(Constants.Context, dateErs[0]);
                            ers[j].Data = data;
                        }
                    }

                    i = j + 1;
                    continue;
                }
                i = j;
            }

            return ers;
        }
    }
}