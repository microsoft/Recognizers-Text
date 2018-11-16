using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeAltExtractor : IDateTimeListExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_DATETIMEALT; // "DateTimeALT";

        private readonly IDateTimeAltExtractorConfiguration config;

        public BaseDateTimeAltExtractor(IDateTimeAltExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(List<ExtractResult> extractResult, string text, DateObject reference)
        {
            extractResult = ExtractAlt(extractResult, text, reference);

            return extractResult;
        }

        // Modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
        // or "Thursday" in "next week on Tuesday or Thursday"
        public List<ExtractResult> ExtractAlt(List<ExtractResult> extractResult, string text, DateObject reference)
        {
            var ers = AddImplicitDates(extractResult, text);

            // Sort the extracted results for the further sequential process.
            ers.Sort((x, y) => x.Start - y.Start ?? 0);

            var i = 0;
            while (i < ers.Count - 1)
            {
                var altErs = GetAltErsWithSameParentText(ers, i, text);

                if (altErs.Count == 0)
                {
                    i++;
                    continue;
                }

                var j = i + altErs.Count - 1;

                var parentTextStart = ers[i].Start;
                var parentTextLen = ers[j].Start + ers[j].Length - ers[i].Start;
                var parentText = text.Substring(parentTextStart ?? 0, parentTextLen ?? 0);

                var success = ExtractAndApplyMetadata(altErs, parentText);

                if (success)
                {
                    i = j + 1;
                }
                else
                {
                    i = j;
                }
            }

            ResolveImplicitRelativeDatePeriod(ers, text);
            PruneInvalidImplicitDate(ers);

            return ers;
        }

        private List<ExtractResult> GetAltErsWithSameParentText(List<ExtractResult> ers, int startIndex, string text)
        {
            var pivot = startIndex + 1;
            var types = new HashSet<string> { ers[startIndex].Type };

            while (pivot < ers.Count)
            {
                // Currently only support merge two kinds of types
                if (!types.Contains(ers[pivot].Type) && types.Count > 1)
                {
                    break;
                }

                // Check whether middle string is a connector
                var middleBegin = ers[pivot - 1].Start + ers[pivot - 1].Length ?? 0;
                var middleEnd = ers[pivot].Start ?? 0;

                if (!IsConnectorOrWhiteSpace(middleBegin, middleEnd, text))
                {
                    break;
                }

                types.Add(ers[pivot].Type);
                pivot++;
            }

            pivot--;

            if (startIndex == pivot)
            {
                startIndex++;
            }

            return ers.GetRange(startIndex, pivot - startIndex + 1);
        }

        private List<ExtractResult> AddImplicitDates(List<ExtractResult> originalErs, string text)
        {
            var ret = new List<ExtractResult>();
            var i = 0;

            originalErs = originalErs.OrderBy(o => o.Start).ToList();

            var implicitDateMatches = config.DayRegex.Matches(text);

            foreach (Match dateMatch in implicitDateMatches)
            {
                var notIncluded = true;
                while (i < originalErs.Count)
                {
                    if (originalErs[i].Start <= dateMatch.Index && originalErs[i].Start + originalErs[i].Length >=
                        dateMatch.Index + dateMatch.Length)
                    {
                        notIncluded = false;
                        break;
                    }

                    if (originalErs[i].Start + originalErs[i].Length < dateMatch.Index + dateMatch.Length)
                    {
                        i++;
                    }
                    else if (originalErs[i].Start + originalErs[i].Length >= dateMatch.Index + dateMatch.Length)
                    {
                        break;
                    }
                }

                var dateEr = new ExtractResult
                {
                    Start = dateMatch.Index,
                    Length = dateMatch.Length,
                    Text = dateMatch.Value,
                    Type = Constants.SYS_DATETIME_DATE,
                    Data = ExtractorName
                };

                if (notIncluded)
                {
                    ret.Add(dateEr);
                }
                else if (i + 1 < originalErs.Count)
                {
                    // For cases like "I am looking at 18 and 19 June"
                    // in which "18" is wrongly recognized as time without context.
                    var nextEr = originalErs[i + 1];
                    if (nextEr.Type.Equals(Constants.SYS_DATETIME_DATE) && originalErs[i].Text.Equals(dateEr.Text) &&
                        IsConnectorOrWhiteSpace((int)(dateEr.Start + dateEr.Length), (int)nextEr.Start, text))
                    {
                        ret.Add(dateEr);
                        originalErs.RemoveAt(i);
                    }
                }
            }

            ret.AddRange(originalErs);
            ret = ret.OrderBy(o => o.Start).ToList();
            return ret;
        }

        private void PruneInvalidImplicitDate(List<ExtractResult> ers)
        {
            ers.RemoveAll(er => er.Data != null && er.Type.Equals(Constants.SYS_DATETIME_DATE) && er.Data.Equals(ExtractorName));
        }

        // Resolve cases like "this week or next".
        private void ResolveImplicitRelativeDatePeriod(List<ExtractResult> ers, string text)
        {
            var relativeTermsMatches = new List<Match>();
            foreach (var regex in config.RelativePrefixList)
            {
                relativeTermsMatches.AddRange(regex.Matches(text).Cast<Match>());
            }

            // Remove overlapping matches
            relativeTermsMatches.RemoveAll(m =>
                ers.Any(e => e.Start <= m.Index && e.Start + e.Length >= m.Index + m.Length));

            var relativeDatePeriodErs = new List<ExtractResult>();
            foreach (var result in ers)
            {
                if (!result.Type.Equals(ExtractorName))
                {
                    var resultEnd = result.Start + result.Length;
                    foreach (var relativeTermsMatch in relativeTermsMatches)
                    {
                        if (relativeTermsMatch.Index > resultEnd)
                        {
                            // Check whether middle string is a connector
                            var middleBegin = resultEnd ?? 0;
                            var middleEnd = relativeTermsMatch.Index;

                            if (IsConnectorOrWhiteSpace(middleBegin, middleEnd, text))
                            {
                                var parentTextStart = result.Start;
                                var parentTextLen = relativeTermsMatch.Index + relativeTermsMatch.Length - result.Start;
                                var parentText = text.Substring(parentTextStart ?? 0, parentTextLen ?? 0);

                                var contextErs = new ExtractResult();
                                foreach (var regex in config.RelativePrefixList)
                                {
                                    var match = regex.Match(result.Text);
                                    if (match.Success)
                                    {
                                        var matchEnd = match.Index + match.Length;
                                        contextErs.Start = matchEnd;
                                        contextErs.Length = result.Length - matchEnd;
                                        contextErs.Text = result.Text.Substring((int)contextErs.Start, (int)contextErs.Length);
                                        contextErs.Type = Constants.ContextType_RelativeSuffix;
                                        break;
                                    }
                                }

                                relativeDatePeriodErs.Add(new ExtractResult
                                {
                                    Text = relativeTermsMatch.Value,
                                    Start = relativeTermsMatch.Index,
                                    Length = relativeTermsMatch.Length,
                                    Type = ExtractorName,
                                    Data = new Dictionary<string, object>
                                    {
                                        {Constants.SubType, result.Type},
                                        {ExtendedModelResult.ParentTextKey, parentText},
                                        {Constants.Context, contextErs}
                                    }
                                });

                                result.Data = new Dictionary<string, object>
                                {
                                    {Constants.SubType, result.Type},
                                    {ExtendedModelResult.ParentTextKey, parentText}
                                };
                                result.Type = ExtractorName;
                            }
                        }
                    }
                }
            }

            ers.AddRange(relativeDatePeriodErs);
            ers.Sort((a, b) => a.Start - b.Start ?? 0);
        }

        private bool IsConnectorOrWhiteSpace(int start, int end, string text)
        {
            if (end <= start)
            {
                return false;
            }

            var middleStr = text.Substring(start, end - start).Trim().ToLower();

            if (string.IsNullOrEmpty(middleStr))
            {
                return true;
            }

            var orTermMatches = config.OrRegex.Matches(middleStr);

            return orTermMatches.Count == 1 && orTermMatches[0].Index == 0 &&
                   orTermMatches[0].Length == middleStr.Length;
        }

        private bool ExtractAndApplyMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var success = ExtractAndApplyUniqueMetadata(extractResults, parentText);

            if (!success)
            {
                success = ExtractAndApplyMultiMetadata(extractResults, parentText);
            }

            // Reverse the DateTimeAlt entities and try extract and apply metadata again
            if (!success)
            {
                extractResults.Reverse();
                success = ExtractAndApplyUniqueMetadata(extractResults, parentText);
            }

            if (!success)
            {
                success = ExtractAndApplyMultiMetadata(extractResults, parentText);
            }

            return success;
        }

        // In this method, we assume that all extract results with same parentText
        // 1. Doesn't share context: cases like "next week or previous week", "next Monday 1pm or previous Monday 1pm"
        // 2. Share the same context: cases like "next week Monday or Tuesday", the context is "next week"
        private bool ExtractAndApplyUniqueMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var metadata = ExtractUniqueMetadata(extractResults, parentText);
            var success = false;

            // Apply metadata to extract results
            if (metadata.Count > 0)
            {
                // The first extract results don't need any context
                extractResults[0].Data = CreateMetadata(extractResults[0].Type, parentText, contextEr: null);
                extractResults[0].Type = ExtractorName;

                for (var k = 1; k < extractResults.Count; k++)
                {
                    extractResults[k].Data = metadata;
                    extractResults[k].Type = ExtractorName;
                }

                success = true;
            }

            return success;
        }

        private Dictionary<string, object> ExtractUniqueMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var former = extractResults.First();
            var latter = extractResults.Last();
            var metadata = ExtractDateTime_Time_Metadata(former, latter, parentText);

            if (metadata.Count == 0)
            {
                metadata = ExtractTime_Time_Metadata(former, latter, parentText);
            }

            if (metadata.Count == 0)
            {
                metadata = ExtractDateTime_DateTime_Metadata(former, latter, parentText);
            }

            if (metadata.Count == 0)
            {
                metadata = ExtractDateTimeRange_TimeRange_Metadata(former, latter, parentText);
            }

            if (metadata.Count == 0)
            {
                metadata = ExtractDateRange_DateRange_Metadata(former, latter, parentText);
            }

            if (metadata.Count == 0)
            {
                metadata = ExtractDateTimeRange_Date_Metadata(former, latter, parentText);
            }

            if (metadata.Count == 0)
            {
                metadata = ExtractDate_DateRange_Metadata(former, latter, parentText);
            }

            return metadata;
        }

        private Dictionary<string, object> ExtractDateTime_Time_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();
            // Modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
            if (former.Type == Constants.SYS_DATETIME_DATETIME && latter.Type == Constants.SYS_DATETIME_TIME)
            {
                var ers = config.DateExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    data = CreateMetadata(Constants.SYS_DATETIME_TIME, parentText, ers[0]);
                }
            }

            return data;
        }

        // Handle cases like "next Monday or previous Tuesday", "next Monday or Tuesday or previous Wednesday"
        private bool ExtractAndApplyMultiMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var allAreDates = true;
            var isValidContext = false;
            foreach (var er in extractResults)
            {
                if (!er.Type.Equals(Constants.SYS_DATETIME_DATE))
                {
                    allAreDates = false;
                    break;
                }
            }

            // Modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
            if (allAreDates)
            {
                var datePeriodErs = config.DatePeriodExtractor.Extract(extractResults[0].Text);

                // DatePeriod as Context: such as "next week on Tuesday or Thursday"
                if (datePeriodErs.Count == 1)
                {
                    extractResults[0].Data = CreateMetadata(extractResults[0].Type, parentText, contextEr: null);
                    extractResults[0].Type = ExtractorName;

                    for (int i = 1; i < extractResults.Count; i++)
                    {
                        extractResults[i].Data = CreateMetadata(extractResults[i].Type, parentText, datePeriodErs[0]);
                        extractResults[i].Type = ExtractorName;
                    }

                    isValidContext = true;
                }
                else
                {
                    // RelativePrefix as Context: such as "next Tuesday or Wednesday", "next Monday or Tuesday or previous Wednesday"
                    int i = 0;
                    ExtractResult contextEr = ExtractRelativePrefixContext(extractResults[0].Text);

                    while (i < extractResults.Count)
                    {
                        if (contextEr == null)
                        {
                            break;
                        }
                        else
                        {
                            extractResults[i].Data = CreateMetadata(extractResults[i].Type, parentText, contextEr: null);
                            extractResults[i].Type = ExtractorName;

                            isValidContext = true;
                        }

                        int j = i + 1;

                        while (j < extractResults.Count)
                        {
                            var relativePrefixContext = ExtractRelativePrefixContext(extractResults[j].Text);

                            // No relativePrefix extracted, the context would follow the previous relativePrefix
                            // Such as "Wednesday" in "next Tuesday or Wednesday"
                            if (relativePrefixContext == null)
                            {
                                extractResults[j].Data = CreateMetadata(extractResults[j].Type, parentText, contextEr);
                                extractResults[j].Type = ExtractorName;
                                j++;
                            }
                            else
                            {
                                // Current extraction has relativePrefix, the context would not follow the previous relativePrefix
                                // Such as "Wednesday" in "next Monday or Tuesday or previous Wednesday"
                                contextEr = relativePrefixContext;
                                break;
                            }
                        }

                        i = j;
                    }
                }
            }

            return isValidContext;
        }

        private Dictionary<string, object> CreateMetadata(string subType, string parentText, ExtractResult contextEr = null)
        {
            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(subType))
            {
                data.Add(Constants.SubType, subType);
            }

            if (!string.IsNullOrEmpty(parentText))
            {
                data.Add(ExtendedModelResult.ParentTextKey, parentText);
            }

            if (contextEr != null)
            {
                data.Add(Constants.Context, contextEr);
            }

            return data;
        }


        private ExtractResult ExtractRelativePrefixContext(string text)
        {
            ExtractResult contextEr = null;

            foreach (var regex in config.RelativePrefixList)
            {
                var match = regex.Match(text);

                if (match.Success)
                {
                    contextEr = new ExtractResult
                    {
                        Text = match.Value,
                        Start = match.Index,
                        Length = match.Length,
                        Type = Constants.ContextType_RelativePrefix
                    };

                    break;
                }
            }

            return contextEr;
        }

        private Dictionary<string, object> ExtractTime_Time_Metadata(ExtractResult former, ExtractResult latter, string parentText)
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
                        var contextErs = new ExtractResult
                        {
                            Text = match.Value,
                            Start = match.Index,
                            Length = match.Length,
                            Type = Constants.ContextType_AmPm
                        };

                        data = CreateMetadata(Constants.SYS_DATETIME_TIME, parentText, contextErs);
                    }
                }
            }

            return data;
        }

        private Dictionary<string, object> ExtractDateTimeRange_TimeRange_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();

            // Modify time entity to an alternative DateTimeRange expression, such as "9-10 am" in "Monday 7-8 am or 9-10 am"
            if (former.Type == Constants.SYS_DATETIME_DATETIMEPERIOD && latter.Type == Constants.SYS_DATETIME_TIMEPERIOD)
            {
                var ers = config.DateExtractor.Extract(former.Text);

                if (ers.Count == 1)
                {
                    data = CreateMetadata(Constants.SYS_DATETIME_TIMEPERIOD, parentText, ers[0]);
                }
            }

            return data;
        }

        private Dictionary<string, object> ExtractDateRange_DateRange_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();

            // For DateRange-DateRange DateTimeAlts, no need to share context
            if (former.Type == Constants.SYS_DATETIME_DATEPERIOD && latter.Type == Constants.SYS_DATETIME_DATEPERIOD)
            {
                data = CreateMetadata(Constants.SYS_DATETIME_DATEPERIOD, parentText, contextEr: null);
            }

            return data;
        }

        private Dictionary<string, object> ExtractDateTime_DateTime_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();

            // Modify time entity to an alternative DateTime expression, such as "Tue 1 pm" in "next week Mon 9 am or Tue 1 pm"
            if (former.Type == Constants.SYS_DATETIME_DATETIME && latter.Type == Constants.SYS_DATETIME_DATETIME)
            {
                var ers = config.DatePeriodExtractor.Extract(former.Text);

                if (ers.Count == 1)
                {
                    data = CreateMetadata(Constants.SYS_DATETIME_DATETIME, parentText, ers[0]);
                }
            }

            return data;
        }

        private Dictionary<string, object> ExtractDateTimeRange_Date_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();

            // Modify time entity to an alternative DateTimeRange expression, such as "9-10 am" in "Monday 7-8 am or 9-10 am"
            if (former.Type == Constants.SYS_DATETIME_DATETIMEPERIOD &&
                latter.Type == Constants.SYS_DATETIME_DATE)
            {
                var ers = config.DateExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    ers[0].Text = former.Text.Substring(0, (int)ers[0].Start) +
                                  former.Text.Substring((int)(ers[0].Start + ers[0].Length));
                    ers[0].Type = Constants.ContextType_RelativeSuffix;

                    data = CreateMetadata(Constants.SYS_DATETIME_DATETIMEPERIOD, parentText, ers[0]);
                }
            }

            return data;
        }

        private Dictionary<string, object> ExtractDate_DateRange_Metadata(ExtractResult former, ExtractResult latter, string parentText)
        {
            var data = new Dictionary<string, object>();

            // For cases like "monday this week or next week"
            if (former.Type == Constants.SYS_DATETIME_DATE &&
                latter.Type == Constants.SYS_DATETIME_DATEPERIOD)
            {
                var ers = config.DatePeriodExtractor.Extract(former.Text);
                if (ers.Count == 1)
                {
                    ers[0].Text = former.Text.Substring(0, (int)ers[0].Start);

                    data = CreateMetadata(Constants.SYS_DATETIME_DATE, parentText, ers[0]);
                }
            }

            return data;
        }
    }
}
