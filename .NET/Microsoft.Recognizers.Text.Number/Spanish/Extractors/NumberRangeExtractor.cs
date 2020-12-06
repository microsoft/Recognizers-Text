using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseNumberParser(new SpanishNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // entre ...y ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // más que ... monos que ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // monos que ... más que ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // de ... a ...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // más/mayor que ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex1LB, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex1LB, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // 30 y/o mas/más/mayor/mayores
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // igual a ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // igual a 30 o más, más que 30 o igual ...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // igual a 30 o menos, menos que 30 o igual ...
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
