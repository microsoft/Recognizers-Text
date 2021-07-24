// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRangeModel : AbstractNumberModel
    {
        public NumberRangeModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_NUMBERRANGE;
    }
}
