﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class TurkishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public TurkishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.Turkish, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new TurkishNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
