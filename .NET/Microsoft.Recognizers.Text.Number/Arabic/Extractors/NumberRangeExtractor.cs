using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.Number.Arabic
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft;

        private readonly BaseNumberExtractor numberExtractor;

        private readonly BaseNumberExtractor ordinalExtractor;

        private readonly BaseNumberParser numberParser;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(
                   NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseNumberParser(new ArabicNumberParserConfiguration(config)),
                   config)
        {

            this.numberExtractor = NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options));
            this.ordinalExtractor = OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options));
            this.numberParser = new BaseNumberParser(new ArabicNumberParserConfiguration(config));

            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // between...and...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // more than ... less than ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // less than ... more than ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // from ... to/~/- ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // more/greater/higher than ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // فيه خمس مائة وأكثر منتجات
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // 30 and/or less/smaller/lower
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // equal to ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // equal to 30 or more than, larger than 30 or equal to ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // equal to 30 or less, smaller than 30 or equal ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                RegexCache.Get(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}