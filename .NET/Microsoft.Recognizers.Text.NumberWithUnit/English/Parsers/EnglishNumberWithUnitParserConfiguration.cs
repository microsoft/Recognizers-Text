﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class EnglishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public EnglishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            var numConfig = new BaseNumberOptionsConfiguration(Culture.English, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new EnglishNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
