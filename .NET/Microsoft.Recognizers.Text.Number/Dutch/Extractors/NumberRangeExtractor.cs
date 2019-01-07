using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        public NumberRangeExtractor(NumberOptions options = NumberOptions.None)
            : base(NumberExtractor.GetInstance(), OrdinalExtractor.GetInstance(), new BaseNumberParser(new DutchNumberParserConfiguration()), options)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // between...and...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // more than ... less than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // less than ... more than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // from ... to/~/- ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // more/greater/higher than ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // 30 and/or less/smaller/lower
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.Singleline),
                    NumberRangeConstants.EQUAL
                },
                {
                    // equal to 30 or more than, larger than 30 or equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // equal to 30 or less, smaller than 30 or equal ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexOptions.Singleline);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}
