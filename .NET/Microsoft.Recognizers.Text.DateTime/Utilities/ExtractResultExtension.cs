using Microsoft.Recognizers.Text.Number;

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
            string[] splitedGroupEr1 = er1.Text.ToString().Split(' ');
            string[] splitedGroupEr2 = er2.Text.ToString().Split(' ');
            string lastWordEr1 = splitedGroupEr1[0];
            string firstWordEr2 = splitedGroupEr2[splitedGroupEr2.Length - 1];

            return lastWordEr1.Equals(firstWordEr2);
        }
    }
}