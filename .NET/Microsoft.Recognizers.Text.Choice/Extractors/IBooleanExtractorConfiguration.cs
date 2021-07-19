// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Choice
{
    public interface IBooleanExtractorConfiguration : IChoiceExtractorConfiguration
    {
        Regex TrueRegex { get; }

        Regex FalseRegex { get; }
    }
}
