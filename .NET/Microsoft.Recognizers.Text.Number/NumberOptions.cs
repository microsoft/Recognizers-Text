using System;

namespace Microsoft.Recognizers.Text.Number
{
    [Flags]
    public enum NumberOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// PercentageMode
        /// </summary>
        PercentageMode = 1,

        /// <summary>
        /// NoProtoCache
        /// </summary>
        NoProtoCache = 16,

        /// <summary>
        /// SuppressExtendedTypes, mode that skips extraction of extra types not in v1. May be removed later.
        /// </summary>
        SuppressExtendedTypes = 2097152, // 2 ^21

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
