namespace Microsoft.Recognizers.Text.Number
{
    public class LongFormatType
    {
        private LongFormatType(char thousandsMark, char decimalsMark)
        {
            ThousandsMark = thousandsMark;
            DecimalsMark = decimalsMark;
        }

        // Reference Value : 1234567.89

        // 1,234,567
        public static LongFormatType IntegerNumComma { get; set; } = new LongFormatType(',', '\0');

        // 1.234.567
        public static LongFormatType IntegerNumDot { get; set; } = new LongFormatType('.', '\0');

        // 1 234 567
        public static LongFormatType IntegerNumBlank { get; set; } = new LongFormatType(' ', '\0');

        // 1 234 567
        public static LongFormatType IntegerNumNoBreakSpace { get; set; } = new LongFormatType(Constants.NO_BREAK_SPACE, '\0');

        // 1'234'567
        public static LongFormatType IntegerNumQuote { get; set; } = new LongFormatType('\'', '\0');

        // 1,234,567.89
        public static LongFormatType DoubleNumCommaDot { get; set; } = new LongFormatType(',', '.');

        // 1,234,567·89
        public static LongFormatType DoubleNumCommaCdot { get; set; } = new LongFormatType(',', '·');

        // 1 234 567,89
        public static LongFormatType DoubleNumBlankComma { get; set; } = new LongFormatType(' ', ',');

        // 1 234 567,89
        public static LongFormatType DoubleNumNoBreakSpaceComma { get; set; } = new LongFormatType(Constants.NO_BREAK_SPACE, ',');

        // 1 234 567.89
        public static LongFormatType DoubleNumBlankDot { get; set; } = new LongFormatType(' ', '.');

        // 1 234 567.89
        public static LongFormatType DoubleNumNoBreakSpaceDot { get; set; } = new LongFormatType(Constants.NO_BREAK_SPACE, '.');

        // 1.234.567,89
        public static LongFormatType DoubleNumDotComma { get; set; } = new LongFormatType('.', ',');

        // 1'234'567,89
        public static LongFormatType DoubleNumQuoteComma { get; set; } = new LongFormatType('\'', ',');

        public char DecimalsMark { get; }

        public char ThousandsMark { get; }
    }
}