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

        public NumberRangeExtractor() : base(NumberExtractor.GetInstance(), OrdinalExtractor.GetInstance())
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // between...and...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.TWONUMBETWEEN
                },
                {
                    // more than ... less than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.TWONUM
                },
                {
                    // less than ... more than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.TWONUM
                },
                {
                    // from ... to/~/- ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.TWONUMTILL
                },
                {
                    // more/greater/higher than ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.MORE
                },
                {
                    // 30 and greater/higher
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.MORE
                },
                {
                    // less/smaller/lower than ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.LESS
                },
                {
                    // 30 and less/smaller/lower
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.LESS
                },
                {
                    // equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , Constants.EQUAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
