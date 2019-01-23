using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        public NumberRangeExtractor(NumberOptions options = NumberOptions.None)
            : base(NumberExtractor.GetInstance(), OrdinalExtractor.GetInstance(), new BaseNumberParser(new SpanishNumberParserConfiguration()), options)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // entre ...y ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // más que ... monos que ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // monos que ... más que ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // de ... a ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // más/mayor que ...
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
                    // 30 y/o mas/más/mayor/mayores
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // igual a ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.Singleline),
                    NumberRangeConstants.EQUAL
                },
                {
                    // igual a 30 o más, más que 30 o igual ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // igual a 30 o menos, menos que 30 o igual ...
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
