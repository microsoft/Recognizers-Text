// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class TemperatureExtractorConfiguration : JapaneseNumberWithUnitExtractorConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex AmbiguousUnitMultiplierRegex =
            new Regex(BaseUnits.AmbiguousUnitNumberMultiplierRegex, RegexFlags);

        public TemperatureExtractorConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public TemperatureExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList =>
            NumbersWithUnitDefinitions.TemperatureSuffixList.ToImmutableDictionary();

        public override ImmutableDictionary<string, string> PrefixList =>
            NumbersWithUnitDefinitions.TemperaturePrefixList.ToImmutableDictionary();

        public override ImmutableList<string> AmbiguousUnitList =>
            NumbersWithUnitDefinitions.TemperatureAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public override Regex AmbiguousUnitNumberMultiplierRegex => AmbiguousUnitMultiplierRegex;
    }
}