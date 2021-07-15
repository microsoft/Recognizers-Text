// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseSequenceParser : IParser
    {
        public virtual ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                ResolutionStr = extResult.Text,
            };

            return result;
        }
    }
}
