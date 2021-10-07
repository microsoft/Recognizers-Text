﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor(BaseNumberOptionsConfiguration config)
            : base(NumberExtractor.GetInstance(config))
        {
            Options = config.Options;
            Regexes = InitRegexes();
        }

        protected override NumberOptions Options { get; }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            HashSet<string> regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage,
                NumbersDefinitions.NumberWithPrefixPercentage,
            };

            if ((Options & NumberOptions.PercentageMode) != 0)
            {
                regexStrs.Add(NumbersDefinitions.FractionNumberWithSuffixPercentage);
                regexStrs.Add(NumbersDefinitions.NumberWithPrepositionPercentage);
            }

            return BuildRegexes(regexStrs);
        }
    }
}