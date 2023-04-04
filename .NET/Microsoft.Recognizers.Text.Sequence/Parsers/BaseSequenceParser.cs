// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseSequenceParser : IParser
    {
        protected static TimeSpan RegexTimeOut => SequenceRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

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
