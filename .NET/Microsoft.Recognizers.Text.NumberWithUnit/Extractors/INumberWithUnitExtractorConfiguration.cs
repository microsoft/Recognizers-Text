// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public interface INumberWithUnitExtractorConfiguration
    {
        ImmutableDictionary<string, string> SuffixList { get; }

        ImmutableDictionary<string, string> PrefixList { get; }

        ImmutableList<string> AmbiguousUnitList { get; }

        string ExtractType { get; }

        CultureInfo CultureInfo { get; }

        IExtractor UnitNumExtractor { get; }

        string BuildPrefix { get; }

        string BuildSuffix { get; }

        string ConnectorToken { get; }

        Regex CompoundUnitConnectorRegex { get; }

        Regex NonUnitRegex { get; }

        Regex MultiplierRegex { get; }

        Regex AmbiguousUnitNumberMultiplierRegex { get; }

        Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        Dictionary<Regex, Regex> TemperatureAmbiguityFiltersDict { get; }

        Dictionary<Regex, Regex> DimensionAmbiguityFiltersDict { get; }

        void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers);
    }
}
