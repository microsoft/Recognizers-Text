using System;

namespace Microsoft.Recognizers.Text.DateTime
{
    [Flags]
    public enum DateTimeOptions
    {
        None = 0,
        SkipFromToMerge = 1,
        SplitDateAndTime = 2,
        Calendar = 4,
    }
}
