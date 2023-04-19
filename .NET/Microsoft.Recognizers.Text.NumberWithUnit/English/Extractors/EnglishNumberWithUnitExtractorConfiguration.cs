// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public abstract class EnglishNumberWithUnitExtractorConfiguration : BaseNumberWithUnitExtractorConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        protected EnglishNumberWithUnitExtractorConfiguration(CultureInfo ci)
            : base(
                  NumbersWithUnitDefinitions.CompoundUnitConnectorRegex,
                  BaseUnits.PmNonUnitRegex,
                  NumbersWithUnitDefinitions.MultiplierRegex,
                  RegexFlags)
        {
            this.CultureInfo = ci;

            // PlaceHolderMixed allows to extract numbers from expressions like 'USD15', '15USD' where there is no space between
            // alphabetic and numeric characters (PlaeHolderDefault does not extract numbers from expressions like 'USD15').
            var unitNumConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None, NumberMode.Unit, BaseNumbers.PlaceHolderMixed);
            this.UnitNumExtractor = NumberExtractor.GetInstance(unitNumConfig);

            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = string.Empty;

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.AmbiguityFiltersDict);
            TemperatureAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.TemperatureAmbiguityFiltersDict);
            DimensionAmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.DimensionAmbiguityFiltersDict);
        }

        public override void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
        }
    }
}