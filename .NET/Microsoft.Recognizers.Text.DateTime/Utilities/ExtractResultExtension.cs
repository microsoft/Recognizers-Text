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