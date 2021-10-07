﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class SpanishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SpanishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.Spanish, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new SpanishNumberParserConfiguration(numConfig));
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
