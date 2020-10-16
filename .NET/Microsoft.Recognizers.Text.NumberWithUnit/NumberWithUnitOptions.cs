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
        /// EnableCompoundTypes, mode that extracts compound dimension units as single entities.
        /// </summary>
        EnableCompoundTypes = 2097152, // 2 ^21,

        /// <summary>
        /// EnablePreview
        /// </summary>
        EnablePreview = 8388608, // 2 ^23
    }
}
