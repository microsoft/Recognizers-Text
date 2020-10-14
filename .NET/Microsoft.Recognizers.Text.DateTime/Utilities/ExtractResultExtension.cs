using System.Collections.Generic;
using System.Linq;

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
    }
}