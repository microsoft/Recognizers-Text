using System;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    [Flags]
    public enum NumberWithUnitOptions
    {
        /// <summary>
        /// Represents None
        /// </summary>
        None = 0,

        /// <summary>
        /// NoProtoCache
        /// </summary>
        NoProtoCache = 16,

        /// <summary>
        /// EnablePreview
        /// </summary>
        EnablePreview = 8388608, // 2 ^23
    }
}
