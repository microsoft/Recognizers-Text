// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.Choice.Extractors;

namespace Microsoft.Recognizers.Text.Choice.Portuguese
{
    public class PortugueseBooleanExtractorConfiguration : BaseBooleanExtractorConfiguration
    {
        public PortugueseBooleanExtractorConfiguration(bool onlyTopMatch = true)
            : base(
                  trueRegex: ChoiceDefinitions.TrueRegex,
                  falseRegex: ChoiceDefinitions.FalseRegex,
                  tokenRegex: ChoiceDefinitions.TokenizerRegex,
                  options: RegexOptions.Singleline,
                  allowPartialMatch: false,
                  maxDistance: 2,
                  onlyTopMatch)
        {
        }
    }
}