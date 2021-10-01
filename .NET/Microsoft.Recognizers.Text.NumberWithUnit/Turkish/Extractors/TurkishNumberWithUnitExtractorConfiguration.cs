﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public abstract class TurkishNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        private static readonly Regex CompoundUnitConnRegex =
            new Regex(NumbersWithUnitDefinitions.CompoundUnitConnectorRegex, RegexOptions.None);

        private static readonly Regex NonUnitsRegex =
            new Regex(BaseUnits.PmNonUnitRegex, RegexOptions.None);

        protected TurkishNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = NumberExtractor.GetInstance(NumberMode.Unit);
            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = string.Empty;

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.AmbiguityFiltersDict);
        }

        public abstract string ExtractType { get; }

        public CultureInfo CultureInfo { get; }

        public IExtractor UnitNumExtractor { get; }

        public string BuildPrefix { get; }

        public string BuildSuffix { get; }

        public string ConnectorToken { get; }

        public Regex CompoundUnitConnectorRegex => CompoundUnitConnRegex;

        public Regex NonUnitRegex => NonUnitsRegex;

        public virtual Regex AmbiguousUnitNumberMultiplierRegex => null;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        public abstract ImmutableDictionary<string, string> SuffixList { get; }

        public abstract ImmutableDictionary<string, string> PrefixList { get; }

        public abstract ImmutableList<string> AmbiguousUnitList { get; }

        public void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
        }
    }
}