// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class DimensionModel : AbstractNumberWithUnitModel
    {
        public DimensionModel(Dictionary<IExtractor, IParser> extractorParserDic)
            : base(extractorParserDic)
        {
        }

        public override string ModelTypeName => "dimension";
    }
}