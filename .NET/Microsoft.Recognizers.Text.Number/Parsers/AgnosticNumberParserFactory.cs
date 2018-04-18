using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.Japanese;

namespace Microsoft.Recognizers.Text.Number
{
    public enum AgnosticNumberParserType
    {
        Cardinal,
        Double,
        Fraction,
        Integer,
        Number,
        Ordinal,
        Percentage
    }

    public static class AgnosticNumberParserFactory
    {
        public static BaseNumberParser GetParser(AgnosticNumberParserType type, INumberParserConfiguration languageConfiguration)
        {
            var isChinese = languageConfiguration.CultureInfo.Name.ToLowerInvariant() == Culture.Chinese;
            var isJapanese = languageConfiguration.CultureInfo.Name.ToLowerInvariant() == Culture.Japanese;

            BaseNumberParser parser;

            if (isChinese)
            {
                parser = new ChineseNumberParser(languageConfiguration as ChineseNumberParserConfiguration);
            }
            else if (isJapanese)
            {
                parser = new JapaneseNumberParser(languageConfiguration as JapaneseNumberParserConfiguration);
            }
            else
            {
                parser = new BaseNumberParser(languageConfiguration);
            }

            switch (type)
            {
                case AgnosticNumberParserType.Cardinal:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_CARDINAL, Constants.SYS_NUM_INTEGER, Constants.SYS_NUM_DOUBLE };
                    break;
                case AgnosticNumberParserType.Double:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_DOUBLE };
                    break;
                case AgnosticNumberParserType.Fraction:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_FRACTION };
                    break;
                case AgnosticNumberParserType.Integer:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_INTEGER };
                    break;
                case AgnosticNumberParserType.Ordinal:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_ORDINAL };
                    break;
                case AgnosticNumberParserType.Percentage:
                    if (!isChinese && !isJapanese)
                    {
                        parser = new BasePercentageParser(languageConfiguration);
                    }
                    break;
            }

            return parser;
        }
    }
}