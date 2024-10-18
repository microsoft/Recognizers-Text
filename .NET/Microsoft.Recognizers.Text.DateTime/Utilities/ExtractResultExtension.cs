// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class ExtractResultExtension
    {
        public static bool IsOverlap(this ExtractResult er1, ExtractResult er2)
        {
            return !(er1.Start >= er2.Start + er2.Length) && !(er2.Start >= er1.Start + er1.Length);
        }

        public static bool IsCover(this ExtractResult er1, ExtractResult er2)
        {
            return (er2.Start < er1.Start && er2.Start + er2.Length >= er1.Start + er1.Length) ||
                   (er2.Start <= er1.Start && er2.Start + er2.Length > er1.Start + er1.Length);
        }

        public static List<ExtractResult> MergeAllResults(List<ExtractResult> results)
        {
            var ret = new List<ExtractResult>();

            results = results.OrderBy(s => s.Start).ThenByDescending(s => s.Length).ToList();
            var mergedResults = new List<ExtractResult>();
            foreach (var result in results)
            {
                if (result != null)
                {
                    bool shouldAdd = true;
                    var resStart = result.Start;
                    var resEnd = resStart + result.Length;
                    for (var index = 0; index < mergedResults.Count && shouldAdd; index++)
                    {
                        var mergedStart = mergedResults[index].Start;
                        var mergedEnd = mergedStart + mergedResults[index].Length;

                        // It is included in one of the current results
                        if (resStart >= mergedStart && resEnd <= mergedEnd)
                        {
                            shouldAdd = false;
                        }

                        // If it contains overlaps
                        if (resStart > mergedStart && resStart < mergedEnd)
                        {
                            shouldAdd = false;
                        }

                        // It includes one of the results and should replace the included one
                        if (resStart <= mergedStart && resEnd >= mergedEnd)
                        {
                            shouldAdd = false;
                            mergedResults[index] = result;
                        }
                    }

                    if (shouldAdd)
                    {
                        mergedResults.Add(result);
                    }
                }
            }

            return mergedResults;
        }

        public static List<ExtractResult> MergeDurationWithStartingMod(List<ExtractResult> results, string text)
        {
            var ret = new List<ExtractResult>();

            results = results.OrderBy(s => s.Start).ToList();
            var mergedResults = new List<ExtractResult>();
            bool mergeFlag = false;
            int i = 0;

            if (results.Count <= 1)
            {
                return results;
            }

            for (i = 0; i < results.Count - 1; i++)
            {
                var currentRet = results[i];
                var nextRet = results[i + 1];
                mergeFlag = false;

                if (currentRet != null && nextRet != null && currentRet.Type == "duration"
                    && (nextRet.Type == "time" || nextRet.Type == "date" || nextRet.Type == "datetime")
                    && nextRet.Start >= currentRet.Start + currentRet.Length)
                {
                    string strMidStr = text.Substring((int)currentRet.Start + (int)currentRet.Length, (int)nextRet.Start - (int)currentRet.Start - (int)currentRet.Length).Trim();
                    if (strMidStr == "starting" || strMidStr == "starting from")
                    {
                        mergeFlag = true;
                        var mergedResult = new ExtractResult();
                        mergedResult.Start = currentRet.Start;
                        mergedResult.Length = nextRet.Start - currentRet.Start + nextRet.Length;
                        mergedResult.Text = text.Substring((int)mergedResult.Start, (int)mergedResult.Length);

                        if (nextRet.Type == "date")
                        {
                            mergedResult.Type = "daterange";
                        }
                        else if (nextRet.Type == "time")
                        {
                            mergedResult.Type = "timerange";
                        }
                        else
                        {
                            mergedResult.Type = "datetimerange";
                        }

                        mergedResults.Add(mergedResult);
                    }
                }

                if (!mergeFlag)
                {
                    mergedResults.Add(currentRet);
                    mergedResults.Add(nextRet);
                }

                i = i + 1;
            }

            if (mergedResults.Count == 0)
            {
                return results;
            }

            // Add last item.
            if (results.Count % 2 == 1)
            {
                mergedResults.Add(results[results.Count - 1]);
            }

            return mergedResults;
        }

        public static List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text, Dictionary<Regex, Regex> ambiguityFiltersDict)
        {
            if (ambiguityFiltersDict != null)
            {
                foreach (var regex in ambiguityFiltersDict)
                {
                    for (int i = extractResults.Count - 1; i >= 0; i--)
                    {
                        var er = extractResults[i];
                        if (regex.Key.IsMatch(er.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            if (matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                            {
                                extractResults.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            return extractResults;
        }
    }
}