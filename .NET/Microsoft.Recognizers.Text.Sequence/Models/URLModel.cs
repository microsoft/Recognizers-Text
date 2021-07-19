// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence
{
    public class URLModel : AbstractSequenceModel
    {
        public URLModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_URL;
    }
}