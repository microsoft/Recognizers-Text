﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Italian;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class ItalianNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public ItalianNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.Italian, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new ItalianNumberParserConfiguration(numConfig));
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            this.TypeList = DimensionExtractorConfiguration.DimensionTypeList;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; }
    }
}
