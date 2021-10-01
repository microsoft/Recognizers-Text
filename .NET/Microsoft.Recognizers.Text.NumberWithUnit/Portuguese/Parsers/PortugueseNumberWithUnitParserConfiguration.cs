﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class PortugueseNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public PortugueseNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.Portuguese, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new PortugueseNumberParserConfiguration(numConfig));
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
