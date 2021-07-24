// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceExtractDataResult
    {
        public IEnumerable<ExtractResult> OtherMatches { get; set; }

        public string Source { get; set; }

        public double Score { get; set; }
    }
}