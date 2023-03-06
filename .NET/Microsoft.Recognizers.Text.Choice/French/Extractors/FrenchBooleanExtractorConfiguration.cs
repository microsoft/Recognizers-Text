// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Choice.Extractors;

namespace Microsoft.Recognizers.Text.Choice.French
{
    public class FrenchBooleanExtractorConfiguration : BaseBooleanExtractorConfiguration
    {
        public FrenchBooleanExtractorConfiguration(bool onlyTopMatch = true)
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
