// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public abstract class BaseNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        public BaseNumberWithUnitExtractorConfiguration(
            string compoundUnitConnectorRegex,
            string pmNonUnitRegex,
            string multiplierRegex,
            RegexOptions options)
        {
            this.CompoundUnitConnectorRegex = new Regex(compoundUnitConnectorRegex, options, RegexTimeOut);
            this.NonUnitRegex = new Regex(pmNonUnitRegex, options, RegexTimeOut);
            if (!string.IsNullOrEmpty(multiplierRegex))
            {
                this.MultiplierRegex = new Regex(multiplierRegex, options, RegexTimeOut);
            }
        }

        public static TimeSpan RegexTimeOut => NumberWithUnitRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

        public virtual ImmutableDictionary<string, string> SuffixList { get; set; }

        public virtual ImmutableDictionary<string, string> PrefixList { get; set; }

        public virtual ImmutableList<string> AmbiguousUnitList { get; set; }

        public virtual string ExtractType { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public IExtractor UnitNumExtractor { get; set; }

        public string BuildPrefix { get; set; }

        public string BuildSuffix { get; set; }

        public string ConnectorToken { get; set; }

        public Regex CompoundUnitConnectorRegex { get; set; }

        public Regex NonUnitRegex { get; set; }

        public Regex MultiplierRegex { get; set; }

        public virtual Regex AmbiguousUnitNumberMultiplierRegex { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; set; }

        public virtual Dictionary<Regex, Regex> TemperatureAmbiguityFiltersDict { get; set; }

        public Dictionary<Regex, Regex> DimensionAmbiguityFiltersDict { get; set; }

        public abstract void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers);
    }
}
