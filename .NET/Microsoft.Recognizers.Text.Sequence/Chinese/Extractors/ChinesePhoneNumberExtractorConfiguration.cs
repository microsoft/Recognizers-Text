// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Definitions.Utilities;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChinesePhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public ChinesePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            WordBoundariesRegex = PhoneNumbersDefinitions.WordBoundariesRegex;
            NonWordBoundariesRegex = PhoneNumbersDefinitions.NonWordBoundariesRegex;
            EndWordBoundariesRegex = PhoneNumbersDefinitions.EndWordBoundariesRegex;
            ColonPrefixCheckRegex = new Regex(PhoneNumbersDefinitions.ColonPrefixCheckRegex);
            ForbiddenPrefixMarkers = (List<char>)PhoneNumbersDefinitions.ForbiddenPrefixMarkers;
            AmbiguityFiltersDict = AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(BasePhoneNumbers.AmbiguityFiltersDict);
        }
    }
}