// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.Choice.Extractors;

namespace Microsoft.Recognizers.Text.Choice.Hindi
{
    public class HindiBooleanExtractorConfiguration : BaseBooleanExtractorConfiguration
    {
        public HindiBooleanExtractorConfiguration(bool onlyTopMatch = true)
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