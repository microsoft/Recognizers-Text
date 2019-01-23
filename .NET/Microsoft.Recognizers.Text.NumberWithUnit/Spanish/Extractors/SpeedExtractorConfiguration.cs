﻿using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class SpeedExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> SpeedSuffixList = NumbersWithUnitDefinitions.SpeedSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousSpeedUnitList.ToImmutableList();

        public SpeedExtractorConfiguration()
               : base(new CultureInfo(Culture.Spanish))
        {
        }

        public SpeedExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;
    }
}
