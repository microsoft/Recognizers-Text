using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(NumberOptions options = NumberOptions.None)
            : base(NumberExtractor.GetInstance(), OrdinalExtractor.GetInstance(), new BaseNumberParser(new ItalianNumberParserConfiguration()),
                   options)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                 {
                     // between...and...
                     new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                     NumberRangeConstants.TWONUMBETWEEN
                 },
                 {
                     // more than ... less than ...
                     new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                     NumberRangeConstants.TWONUM
                 },
                 {
                     // less than ... more than ...
                     new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                     NumberRangeConstants.TWONUM
                 },
                 {
                     // from ... to/~/- ...
                     new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                     NumberRangeConstants.TWONUMTILL
                 },
                 {
                     // more/greater/higher than ...
                     new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags),
                     NumberRangeConstants.MORE
                 },
                 {
                     // 30 and/or greater/higher
                     new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                     NumberRangeConstants.MORE
                 },
                 {
                     // less/smaller/lower than ...
                     new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags),
                     NumberRangeConstants.LESS
                 },
                 {
                     // 30 and/or less/smaller/lower
                     new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                     NumberRangeConstants.LESS
                 },
                 {
                     // equal to ...
                     new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                     NumberRangeConstants.EQUAL
                 },
                 {
                     // equal to 30 or more than, larger than 30 or equal to ...
                     new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags),
                     NumberRangeConstants.MORE
                 },
                 {
                     // equal to 30 or less, smaller than 30 or equal ...
                     new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags),
                     NumberRangeConstants.LESS
                 },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}