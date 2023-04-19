// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Text.Choice.Extractors;

namespace Microsoft.Recognizers.Text.Choice.Turkish
{
    public class TurkishBooleanExtractorConfiguration : BaseBooleanExtractorConfiguration
    {
        public TurkishBooleanExtractorConfiguration(bool onlyTopMatch = true)
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