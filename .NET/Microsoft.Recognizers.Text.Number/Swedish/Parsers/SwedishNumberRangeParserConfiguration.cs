using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public class SwedishNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {
        public SwedishNumberRangeParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public SwedishNumberRangeParserConfiguration(CultureInfo ci)
        {
            CultureInfo = ci;

            NumberExtractor = English.NumberExtractor.GetInstance();
            OrdinalExtractor = English.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new SwedishNumberParserConfiguration());
            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexOptions.Singleline);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexOptions.Singleline);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexOptions.Singleline);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexOptions.Singleline);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexOptions.Singleline);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexOptions.Singleline);
        }

        public CultureInfo CultureInfo { get; private set; }

        public IExtractor NumberExtractor { get; private set; }

        public IExtractor OrdinalExtractor { get; private set; }

        public IParser NumberParser { get; private set; }

        public Regex MoreOrEqual { get; private set; }

        public Regex LessOrEqual { get; private set; }

        public Regex MoreOrEqualSuffix { get; private set; }

        public Regex LessOrEqualSuffix { get; private set; }

        public Regex MoreOrEqualSeparate { get; private set; }

        public Regex LessOrEqualSeparate { get; private set; }
    }
}
