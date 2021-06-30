using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(new NumberExtractor(new BaseNumberOptionsConfiguration(config)),
                   new OrdinalExtractor(new BaseNumberOptionsConfiguration(config)),
                   new BaseCJKNumberParser(new KoreanNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>
            {
                {
                    // ...과...사이
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // 이상...이하...

                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // 이하...이상...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // 이십보다 크고 삼십오보다 작다
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex7, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // ...에서..., 20~30
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex5, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex6, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // ...이상|초과|많|높|크|더많|더높|더크|>
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                     // ...이상|초과|많|높|크|더많|더높|더크|>
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex5, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                   // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegexFraction, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // 까지최소|<|≤...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex2, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex4, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // 700에 달하는
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex4, RegexFlags),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}