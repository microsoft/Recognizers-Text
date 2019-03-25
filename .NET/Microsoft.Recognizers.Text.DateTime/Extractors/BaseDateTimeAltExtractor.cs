using System;
using System.Collections.Generic;
using System.Linq;
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
                    i++;
                }
            }

            ResolveImplicitRelativeDatePeriod(ers, text);
            PruneInvalidImplicitDate(ers);

            return ers;
        }

        private static void PruneInvalidImplicitDate(List<ExtractResult> ers)
        {
            ers.RemoveAll(er => er.Data != null && er.Type.Equals(Constants.SYS_DATETIME_DATE) && er.Data.Equals(ExtractorName));
        }

        private static bool IsSupportedAltEntitySequence(List<ExtractResult> altEntities)
        {
            var subSeq = altEntities.Skip(1);
            var entityTypes = subSeq.Select(t => t.Type).Distinct();

            return entityTypes.Count() == 1;
        }

        private static ExtractResult ExtractContext(ExtractResult er, List<Func<string, List<ExtractResult>>> extractMethods, Action<ExtractResult, ExtractResult> postProcessMethod)
        {
            ExtractResult contextEr = null;

            foreach (var extractMethod in extractMethods)
            {
                var contextErCandidates = extractMethod(er.Text);

                if (contextErCandidates.Count == 1)
                {
                    contextEr = contextErCandidates.Single();
                    break;
                }
            }

            if (contextEr != null)
            {
                postProcessMethod?.Invoke(contextEr, er);
            }

            if (contextEr != null && string.IsNullOrEmpty(contextEr.Text))
            {
                contextEr = null;
            }

            return contextEr;
        }

        private static bool ShouldCreateMetadata(List<ExtractResult> originalErs, ExtractResult contextEr)
        {
            // For alternative entities sequence which are all DatePeriod, we should create metadata even if context is null
            return contextEr != null ||
                   (originalErs.First().Type == Constants.SYS_DATETIME_DATEPERIOD && originalErs.Last().Type == Constants.SYS_DATETIME_DATEPERIOD);
        }

        private static Dictionary<string, object> MergeMetadata(object originalMetadata, Dictionary<string, object> newMetadata)
        {
            var result = new Dictionary<string, object>();

            if (originalMetadata is Dictionary<string, object>)
            {
                result = originalMetadata as Dictionary<string, object>;
            }

            if (originalMetadata == null)
            {
                result = newMetadata;
            }
            else
            {
                foreach (var data in newMetadata)
                {
                    result.Add(data.Key, data.Value);
                }
            }

            return result;
        }

        private static Dictionary<string, object> CreateMetadata(string subType, string parentText, ExtractResult contextEr = null)
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

        private static Action<ExtractResult, ExtractResult> GetPostProcessMethod(string firstEntityType, string lastEntityType)
        {
            if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.Equals(Constants.SYS_DATETIME_DATE))
            {
                return (contextEr, originalEr) =>
                {
                    contextEr.Text = originalEr.Text.Substring(0, (int)contextEr.Start) +
                                  originalEr.Text.Substring((int)(contextEr.Start + contextEr.Length));
                    contextEr.Type = Constants.ContextType_RelativeSuffix;
                };
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATE) && lastEntityType.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                return (contextEr, originalEr) =>
                {
                    contextEr.Text = originalEr.Text.Substring(0, (int)contextEr.Start);
                };
            }

            return null;
        }

        private static bool ShouldApplyParentText(List<ExtractResult> extractResults)
        {
            var shouldApply = false;

            if (IsSupportedAltEntitySequence(extractResults))
            {
                var firstEntityType = extractResults.First().Type;
                var lastEntityType = extractResults.Last().Type;

                if (firstEntityType.Equals(Constants.SYS_DATETIME_DATE) && lastEntityType.Equals(Constants.SYS_DATETIME_DATE))
                {
                    // "11/20 or 11/22"
                    shouldApply = true;
                }
                else if (firstEntityType.Equals(Constants.SYS_DATETIME_TIME) && lastEntityType.Equals(Constants.SYS_DATETIME_TIME))
                {
                    // "7 oclock or 8 oclock"
                    shouldApply = true;
                }
                else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.Equals(Constants.SYS_DATETIME_DATETIME))
                {
                    // "Monday 1pm or Tuesday 2pm"
                    shouldApply = true;
                }
            }

            return shouldApply;
        }

        private static bool ApplyParentTextMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var success = false;

            if (IsSupportedAltEntitySequence(extractResults))
            {
                foreach (var extractResult in extractResults)
                {
                    var metadata = CreateMetadata(extractResult.Type, parentText, contextEr: null);
                    extractResult.Data = MergeMetadata(extractResult.Data, metadata);
                    extractResult.Type = ExtractorName;
                }

                success = true;
            }

            return success;
        }

        private static void ApplyMetadata(List<ExtractResult> ers, Dictionary<string, object> metadata, string parentText)
        {
            // The first extract results don't need any context
            var metadataWithoutConext = CreateMetadata(ers[0].Type, parentText, contextEr: null);
            ers[0].Data = MergeMetadata(ers[0].Data, metadataWithoutConext);
            ers[0].Type = ExtractorName;

            for (var i = 1; i < ers.Count; i++)
            {
                ers[i].Data = MergeMetadata(ers[i].Data, metadata);
                ers[i].Type = ExtractorName;
            }
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

                var prefixEnd = ers[pivot - 1].Start ?? 0;
                var prefixStr = text.Substring(0, prefixEnd);

                if (IsEndsWithRangePrefix(prefixStr))
                {
                    break;
                }

                if (IsSupportedAltEntitySequence(ers.GetRange(startIndex, pivot - startIndex + 1)))
                {
                    types.Add(ers[pivot].Type);
                    pivot++;
                }
                else
                {
                    break;
                }
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
                    Data = ExtractorName,
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
                                        { Constants.SubType, result.Type },
                                        { ExtendedModelResult.ParentTextKey, parentText },
                                        { Constants.Context, contextErs },
                                    },
                                });

                                result.Data = new Dictionary<string, object>
                                {
                                    { Constants.SubType, result.Type },
                                    { ExtendedModelResult.ParentTextKey, parentText },
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

        private bool IsEndsWithRangePrefix(string prefixText)
        {
            return config.RangePrefixRegex.MatchEnd(prefixText, trim: true).Success;
        }

        private bool ExtractAndApplyMetadata(List<ExtractResult> extractResults, string parentText)
        {
            var success = ExtractAndApplyMetadata(extractResults, parentText, reverse: false);

            if (!success)
            {
                success = ExtractAndApplyMetadata(extractResults, parentText, reverse: true);
            }

            if (!success && ShouldApplyParentText(extractResults))
            {
                success = ApplyParentTextMetadata(extractResults, parentText);
            }

            return success;
        }

        private bool ExtractAndApplyMetadata(List<ExtractResult> extractResults, string parentText, bool reverse)
        {
            if (reverse)
            {
                extractResults.Reverse();
            }

            var success = false;

            // Currently, we support alt entity sequence only when the second alt entity to the last alt entity share the same type
            if (IsSupportedAltEntitySequence(extractResults))
            {
                var metadata = ExtractMetadata(extractResults.First(), parentText, extractResults);
                Dictionary<string, object> metadataCandidate = null;

                int i = 0;
                while (i < extractResults.Count)
                {
                    if (metadata == null)
                    {
                        break;
                    }

                    int j = i + 1;

                    while (j < extractResults.Count)
                    {
                        metadataCandidate = ExtractMetadata(extractResults[j], parentText, extractResults);

                        // No context extracted, the context would follow the previous one
                        // Such as "Wednesday" in "next Tuesday or Wednesday"
                        if (metadataCandidate == null)
                        {
                            j++;
                        }
                        else
                        {
                            // Current extraction has context, the context would not follow the previous ones
                            // Such as "Wednesday" in "next Monday or Tuesday or previous Wednesday"
                            break;
                        }
                    }

                    var ersShareContext = extractResults.GetRange(i, j - i);
                    ApplyMetadata(ersShareContext, metadata, parentText);
                    metadata = metadataCandidate;

                    i = j;
                    success = true;
                }
            }

            return success;
        }

        // This method is to extract metadata from the targeted ExtractResult
        // For cases like "next week Monday or Tuesday or previous Wednesday", ExtractMethods can be more than one
        private Dictionary<string, object> ExtractMetadata(ExtractResult targetEr, string parentText, List<ExtractResult> ers)
        {
            Dictionary<string, object> metadata = null;
            var extractMethods = GetExtractMethods(targetEr.Type, ers.Last().Type);
            var postProcessMethod = GetPostProcessMethod(targetEr.Type, ers.Last().Type);
            var contextEr = ExtractContext(targetEr, extractMethods, postProcessMethod);

            if (ShouldCreateMetadata(ers, contextEr))
            {
                metadata = CreateMetadata(targetEr.Type, parentText, contextEr);
            }

            return metadata;
        }

        private List<ExtractResult> ExtractRelativePrefixContext(string entityText)
        {
            List<ExtractResult> results = new List<ExtractResult>();

            foreach (var regex in config.RelativePrefixList)
            {
                var match = regex.Match(entityText);

                if (match.Success)
                {
                    results.Add(new ExtractResult
                    {
                        Text = match.Value,
                        Start = match.Index,
                        Length = match.Length,
                        Type = Constants.ContextType_RelativePrefix,
                    });
                }
            }

            return results;
        }

        private List<ExtractResult> ExtractAmPmContext(string entityText)
        {
            List<ExtractResult> results = new List<ExtractResult>();

            foreach (var regex in config.AmPmRegexList)
            {
                var match = regex.Match(entityText);

                if (match.Success)
                {
                    results.Add(new ExtractResult
                    {
                        Text = match.Value,
                        Start = match.Index,
                        Length = match.Length,
                        Type = Constants.ContextType_AmPm,
                    });
                }
            }

            return results;
        }

        private List<Func<string, List<ExtractResult>>> GetExtractMethods(string firstEntityType, string lastEntityType)
        {
            var methods = new List<Func<string, List<ExtractResult>>>();

            if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.Equals(Constants.SYS_DATETIME_TIME))
            {
                // "Monday 7pm or 8pm"
                methods.Add(config.DateExtractor.Extract);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATE) && lastEntityType.Equals(Constants.SYS_DATETIME_DATE))
            {
                // "next week Monday or Tuesday", "previous Monday or Wednesday"
                methods.Add(config.DatePeriodExtractor.Extract);
                methods.Add(ExtractRelativePrefixContext);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_TIME) && lastEntityType.Equals(Constants.SYS_DATETIME_TIME))
            {
                // "in the morning at 7 oclock or 8 oclock"
                methods.Add(ExtractAmPmContext);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                // "next week Mon 9am or Tue 1pm"
                methods.Add(config.DatePeriodExtractor.Extract);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                // "Monday 7-8 am or 9-10am"
                methods.Add(config.DateExtractor.Extract);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATEPERIOD) && lastEntityType.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                // For alt entities that are all DatePeriod, no need to share context
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.Equals(Constants.SYS_DATETIME_DATE))
            {
                // "Tuesday or Wednesday morning"
                methods.Add(config.DateExtractor.Extract);
            }
            else if (firstEntityType.Equals(Constants.SYS_DATETIME_DATE) && lastEntityType.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                // "Monday this week or next week"
                methods.Add(config.DatePeriodExtractor.Extract);
            }

            return methods;
        }
    }
}
