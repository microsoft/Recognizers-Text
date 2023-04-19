// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public abstract class FrenchNumberWithUnitExtractorConfiguration : BaseNumberWithUnitExtractorConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        protected FrenchNumberWithUnitExtractorConfiguration(CultureInfo ci)
            : base(
                  NumbersWithUnitDefinitions.CompoundUnitConnectorRegex,
                  BaseUnits.PmNonUnitRegex,
                  string.Empty,
                  RegexFlags)
        {
            this.CultureInfo = ci;

            var unitNumConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None, NumberMode.Unit);
            this.UnitNumExtractor = NumberExtractor.GetInstance(unitNumConfig);

            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.AmbiguityFiltersDict);
            TemperatureAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.TemperatureAmbiguityFiltersDict);
            DimensionAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.DimensionAmbiguityFiltersDict);
        }

        public override void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
        }
    }
}