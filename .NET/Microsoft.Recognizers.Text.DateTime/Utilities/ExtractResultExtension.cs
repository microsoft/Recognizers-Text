using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class ExtractResultExtension
    {
        public static bool IsOverlap(this ExtractResult er1, ExtractResult er2)
        {
            return !( er1.Start >= er2.Start + er2.Length ) && !( er2.Start >= er1.Start + er1.Length );
        }

        public static bool IsCover(this ExtractResult er1, ExtractResult er2)
        {
            return
                ((er2.Start < er1.Start) && ((er2.Start + er2.Length) >= (er1.Start + er1.Length)))
                || ((er2.Start <= er1.Start) && ((er2.Start + er2.Length) > (er1.Start + er1.Length)));
        }

        public static bool IsCrossOverlap(this ExtractResult er1, ExtractResult er2)
        {
            // Get first word of er2 and last word of er1
            string[] splitedGroupEr1 = er1.Text.ToString().Split(' ');
            string[] splitedGroupEr2 = er2.Text.ToString().Split(' ');
            string lastWordEr1 = splitedGroupEr1[0];
            string firstWordEr2 = splitedGroupEr2[splitedGroupEr2.Length - 1];

            // If lastWordEr1 equals firstWordEr2 and equal string is included in NextPrefixRegex(next|coming|upcoming) and PastPrefixRegex(last|past|previous),
            // not merge. Otherwise, prepare to merge.
            var isCrossOverlap = false;
            if (lastWordEr1.Equals(firstWordEr2))
            {
                string nextPrefixRegex = DateTimeDefinitions.NextPrefixRegex;
                string pastPrefixRegex = DateTimeDefinitions.PastPrefixRegex;
                var matchedNextEr1 = Regex.IsMatch(lastWordEr1, nextPrefixRegex);
                var matchedNextEr2 = Regex.IsMatch(firstWordEr2, nextPrefixRegex);
                var matchedPastEr1 = Regex.IsMatch(lastWordEr1, pastPrefixRegex);
                var matchedPastEr2 = Regex.IsMatch(firstWordEr2, pastPrefixRegex);
                if ((matchedNextEr1 && matchedNextEr2) || (matchedPastEr1 && matchedPastEr2))
                {
                    isCrossOverlap = true;
                }
            }

            return isCrossOverlap;
        }
    }
}