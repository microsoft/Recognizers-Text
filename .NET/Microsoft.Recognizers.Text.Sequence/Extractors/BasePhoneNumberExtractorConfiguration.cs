// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Utilities;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public BasePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex;
            NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex;
            EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex;
            ColonPrefixCheckRegex = new Regex(BasePhoneNumbers.ColonPrefixCheckRegex);
            ColonMarkers = (List<char>)BasePhoneNumbers.ColonMarkers;
            ForbiddenPrefixMarkers = (List<char>)BasePhoneNumbers.ForbiddenPrefixMarkers;
            ForbiddenSuffixMarkers = (List<char>)BasePhoneNumbers.ForbiddenSuffixMarkers;
            AmbiguityFiltersDict = AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(BasePhoneNumbers.AmbiguityFiltersDict);
        }
    }
}