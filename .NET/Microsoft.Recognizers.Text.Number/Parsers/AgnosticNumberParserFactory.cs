using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number
{
    public enum AgnosticNumberParserType
    {
        /// <summary>
        /// Type Cardinal
        /// </summary>
        Cardinal,

        /// <summary>
        /// type Double
        /// </summary>
        Double,

        /// <summary>
        /// Type Fraction
        /// </summary>
        Fraction,

        /// <summary>
        /// Type Integer
        /// </summary>
        Integer,

        /// <summary>
        /// Type Number
        /// </summary>
        Number,

        /// <summary>
        /// Tyoe Ordinal
        /// </summary>
        Ordinal,

        /// <summary>
        /// Type Percentage
        /// </summary>
        Percentage,
    }

    public static class AgnosticNumberParserFactory
    {
        public static BaseNumberParser GetParser(AgnosticNumberParserType type, INumberParserConfiguration languageConfiguration)
        {
            var culture = languageConfiguration.CultureInfo.Name.ToLowerInvariant();
            var isChinese = culture == Culture.Chinese;
            var isJapanese = culture == Culture.Japanese;
            var isKorean = culture == Culture.Korean;

            BaseNumberParser parser;

            if (isChinese || isJapanese || isKorean)
            {
                parser = new BaseCJKNumberParser(languageConfiguration);
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
                    if ((!isChinese && !isJapanese) || isKorean)
                    {
                        parser = new BasePercentageParser(languageConfiguration);
                    }

                    break;
            }

            return parser;
        }
    }
}