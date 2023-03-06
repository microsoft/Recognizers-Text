// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public abstract class SpanishNumberWithUnitExtractorConfiguration : BaseNumberWithUnitExtractorConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        protected SpanishNumberWithUnitExtractorConfiguration(CultureInfo ci)
            : base(
                  NumbersWithUnitDefinitions.CompoundUnitConnectorRegex,
                  BaseUnits.PmNonUnitRegex,
                  NumbersWithUnitDefinitions.MultiplierRegex,
                  RegexFlags)
        {
            this.CultureInfo = ci;

            var unitNumConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None, NumberMode.Unit);
            this.UnitNumExtractor = NumberExtractor.GetInstance(unitNumConfig);

            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            TemperatureAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.TemperatureAmbiguityFiltersDict);
            DimensionAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.DimensionAmbiguityFiltersDict);
        }

        public override void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
        }
    }
}
