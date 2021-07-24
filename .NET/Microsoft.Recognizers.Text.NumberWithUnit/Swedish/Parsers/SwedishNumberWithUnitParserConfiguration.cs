﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class SwedishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SwedishNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var config = new BaseNumberOptionsConfiguration(Culture.Swedish, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(config);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new SwedishNumberParserConfiguration(config));
            this.ConnectorToken = string.Empty;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
