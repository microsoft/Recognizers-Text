package com.microsoft.recognizers.text.number;

public class LongFormatType {

    // Reference Value : 1234567.89

    // 1,234,567
    public static LongFormatType IntegerNumComma = new LongFormatType(',', '\0');

    // 1.234.567
    public static LongFormatType IntegerNumDot = new LongFormatType('.', '\0');

    // 1 234 567
    public static LongFormatType IntegerNumBlank = new LongFormatType(' ', '\0');

    // 1 234 567
    public static LongFormatType IntegerNumNoBreakSpace = new LongFormatType(Constants.NO_BREAK_SPACE, '\0');

    // 1'234'567
    public static LongFormatType IntegerNumQuote = new LongFormatType('\'', '\0');

    // 1,234,567.89
    public static LongFormatType DoubleNumCommaDot = new LongFormatType(',', '.');

    // 1,234,567·89
    public static LongFormatType DoubleNumCommaCdot = new LongFormatType(',', '·');

    // 1 234 567,89
    public static LongFormatType DoubleNumBlankComma = new LongFormatType(' ', ',');

    // 1 234 567,89
    public static LongFormatType DoubleNumNoBreakSpaceComma = new LongFormatType(Constants.NO_BREAK_SPACE, ',');

    // 1 234 567.89
    public static LongFormatType DoubleNumBlankDot = new LongFormatType(' ', '.');

    // 1 234 567.89
    public static LongFormatType DoubleNumNoBreakSpaceDot = new LongFormatType(Constants.NO_BREAK_SPACE, '.');

    // 1.234.567,89
    public static LongFormatType DoubleNumDotComma = new LongFormatType('.', ',');

    // 1'234'567,89
    public static LongFormatType DoubleNumQuoteComma = new LongFormatType('\'', ',');

    public final char decimalsMark;
    public final char thousandsMark;

    public LongFormatType(char thousandsMark, char decimalsMark) {
        this.thousandsMark = thousandsMark;
        this.decimalsMark = decimalsMark;
    }
}
