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
    }
}