﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.Number.Hindi
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<string, IntegerExtractor> Instances =
            new ConcurrentDictionary<string, IntegerExtractor>();

        private IntegerExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.NumbersWithPlaceHolder(placeholder), RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.IndianNumberingSystemRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithSuffix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithDozenSuffix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithLocks, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.HINDI)
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithDozenSuffixLocks, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.HINDI)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumComma, placeholder, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NegativeHinglishRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.HINDI)
                },
                {
                    new Regex(NumbersDefinitions.CompoundEnglishNumberRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.HINDI)
                },
                {
                    new Regex(NumbersDefinitions.DecimalUnitsWithRoundNumberRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.HINDI)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER; // "Integer";

        public static IntegerExtractor GetInstance(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new IntegerExtractor(placeholder);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }
    }
}