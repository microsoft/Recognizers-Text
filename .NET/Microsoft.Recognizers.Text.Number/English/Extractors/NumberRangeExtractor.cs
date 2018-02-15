using Microsoft.Recognizers.Definitions.English;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;

        public NumberRangeExtractor() : base(NumberExtractor.GetInstance(), OrdinalExtractor.GetInstance(), new BaseNumberParser(new EnglishNumberParserConfiguration()))
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // between...and...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // more than ... less than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // less than ... more than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // from ... to/~/- ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMTILL
                },
                {
                    // more/greater/higher than ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // 30 and/or less/smaller/lower
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.EQUAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
