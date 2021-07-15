// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceParseDataResult
    {
        public double Score { get; set; }

        public IEnumerable<OtherMatchParseResult> OtherMatches { get; set; }
    }
}
