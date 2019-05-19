using System;

namespace Microsoft.Recognizers.Text.DateTime
{
    [Flags]
    public enum DateTimeOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// SkipFromToMerge
        /// </summary>
        SkipFromToMerge = 1,

        /// <summary>
        /// SplitDateAndTime
        /// </summary>
        SplitDateAndTime = 2,

        /// <summary>
        /// CalendarMode
        /// </summary>
        CalendarMode = 4,

        /// <summary>
        /// ExtendedTypes
        /// </summary>
        ExtendedTypes = 8,

        /// <summary>
        /// FailFast, mode that aborts extraction/tagging quickly for non-entity cases. May be removed later.
        /// </summary>
        FailFast = 2097152, // 2 ^21

        /// <summary>
        /// ExperimentalMode
        /// </summary>
        ExperimentalMode = 4194304, // 2 ^22

        /// <summary>
        /// EnablePreview
        /// </summary>
        EnablePreview = 8388608, // 2 ^23
    }
}
