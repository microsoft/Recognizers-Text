using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.Number.Turkish
{
    public class TurkishNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public TurkishNumberRangeParserConfiguration()
            : this(new CultureInfo(Culture.Turkish))
        {
        }

        public TurkishNumberRangeParserConfiguration(CultureInfo ci)
        {
            CultureInfo = ci;

            NumberExtractor = Turkish.NumberExtractor.GetInstance();
            OrdinalExtractor = Turkish.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new TurkishNumberParserConfiguration());
            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexFlags);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexFlags);
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
