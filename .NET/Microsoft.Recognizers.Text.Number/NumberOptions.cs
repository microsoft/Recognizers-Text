using System;

namespace Microsoft.Recognizers.Text.Number
{
    [Flags]
    public enum NumberOptions
    {
        /// <summary>
        /// Represents None
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents PercentageMode
        /// </summary>
        PercentageMode = 1,

        /// <summary>
        /// Represents ExperimentalMode
        /// </summary>
        ExperimentalMode = 2,

        /// <summary>
        /// Represents Ordinal EnablePreview
        /// </summary>
        EnablePreview = 4,
    }
}
