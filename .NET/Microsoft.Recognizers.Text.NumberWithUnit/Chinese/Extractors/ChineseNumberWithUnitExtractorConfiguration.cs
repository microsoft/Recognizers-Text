// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.Config;
using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public abstract class ChineseNumberWithUnitExtractorConfiguration : BaseNumberWithUnitExtractorConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex HalfUnitRegex = new Regex(NumbersWithUnitDefinitions.HalfUnitRegex, RegexFlags, RegexTimeOut);

        protected ChineseNumberWithUnitExtractorConfiguration(CultureInfo ci)
            : base(
                  NumbersWithUnitDefinitions.CompoundUnitConnectorRegex,
                  BaseUnits.PmNonUnitRegex,
                  string.Empty,
                  RegexFlags)
        {
            this.CultureInfo = ci;

            var numConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None);

            this.UnitNumExtractor = new NumberExtractor(numConfig, CJKNumberExtractorMode.ExtractAll);

            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.AmbiguityFiltersDict);
        }

        public IExtractor IntegerExtractor { get; }

        public override void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
            // Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
            CommonUtils.ExpandHalfSuffix(source, ref result, numbers, HalfUnitRegex);
        }
    }
}
