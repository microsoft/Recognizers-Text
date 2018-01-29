using Microsoft.Recognizers.Definitions.English;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class EnglishNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {
        public CultureInfo CultureInfo { get; private set; }

        public IExtractor NumberExtractor { get; private set; }

        public IExtractor OrdinalExtractor { get; private set; }

        public IParser NumberParser { get; private set; }

        public Regex MoreOrEqual { get; private set; }

        public Regex LessOrEqual { get; private set; }

        public EnglishNumberRangeParserConfiguration() : this(new CultureInfo(Culture.English))
        {
        }

        public EnglishNumberRangeParserConfiguration(CultureInfo ci)
        {
            CultureInfo = ci;

            NumberExtractor = English.NumberExtractor.GetInstance();
            OrdinalExtractor = English.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
    }
}
