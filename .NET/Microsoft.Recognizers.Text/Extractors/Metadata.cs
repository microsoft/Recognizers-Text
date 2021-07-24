﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text
{
    public class Metadata
    {
        // For cases like "from 2014 to 2018", the period end "2018" could be inclusive or exclusive
        // For extraction, we only mark this flag to avoid future duplicate judgment, whether to include the period end or not is not determined in the extraction step
        public bool PossiblyIncludePeriodEnd { get; set; } = false;

        // For cases like "2015年以前" (usually regards as "before 2015" in English), "5天以前" (usually regards as "5 days ago" in English) in Chinese, we need to decide whether this is a "Date with Mode" or "Duration with Before and After". We use this flag to avoid duplicate judgment both in the Extraction step and Parse step.
        // Currently, this flag is only used in Chinese DateTime as other languages don't have this ambiguity cases.
        public bool IsDurationWithAgoAndLater { get; set; } = false;

        // For Holiday cases as they are special cases of Date
        public bool IsHoliday { get; set; } = false;

        // For Ordinal.relative, a subtype of Ordinal
        public bool IsOrdinalRelative { get; set; } = false;

        public bool HasMod { get; set; } = false;

        public string Offset { get; set; } = string.Empty;

        public string RelativeTo { get; set; } = string.Empty;

        public bool IsMealtime { get; set; } = false;

        // For cases where a language has variations in handling decimal separators
        public bool TreatAsInteger { get; set; } = false;

        public Metadata Clone()
        {
            return (Metadata)MemberwiseClone();
        }
    }
}
