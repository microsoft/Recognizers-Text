package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import org.junit.Assert;
import org.junit.Test;

public class DecimalAndThousandsSeparatorsTest {

    public void parseTest(LongFormatType type, String query, String value) {
        char decimalSep = type.decimalsMark, nonDecimalSep = type.thousandsMark;

        IParser parser = AgnosticNumberParserFactory.getParser(
                AgnosticNumberParserType.Double,
                new LongFormTestConfiguration(decimalSep, nonDecimalSep));
        ParseResult resultJson = parser.parse(new ExtractResult(0, query.length(), query, "builtin.num.double", "Num"));
        Assert.assertEquals(value, resultJson.resolutionStr);
    }

    @Test
    public void arabicParse() {
        parseTest(LongFormatType.DoubleNumBlankComma, "123 456 789,123", "123456789.123");
        parseTest(LongFormatType.DoubleNumBlankDot, "123 456 789.123", "123456789.123");
        parseTest(LongFormatType.DoubleNumCommaCdot, "123,456,789·123", "123456789.123");
        parseTest(LongFormatType.DoubleNumCommaDot, "123,456,789.123", "123456789.123");
        parseTest(LongFormatType.DoubleNumDotComma, "123.456.789,123", "123456789.123");
        parseTest(LongFormatType.DoubleNumQuoteComma, "123'456'789,123", "123456789.123");
        parseTest(LongFormatType.IntegerNumBlank, "123 456 789", "123456789");
        parseTest(LongFormatType.IntegerNumComma, "123,456,789", "123456789");
        parseTest(LongFormatType.IntegerNumDot, "123.456.789", "123456789");
        parseTest(LongFormatType.IntegerNumQuote, "123'456'789", "123456789");
    }
}
