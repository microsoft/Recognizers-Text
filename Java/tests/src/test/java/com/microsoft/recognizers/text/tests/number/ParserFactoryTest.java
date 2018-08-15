package com.microsoft.recognizers.text.tests.number;


import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.french.parsers.FrenchNumberParserConfiguration;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.parsers.BasePercentageParser;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;
import org.junit.Assert;
import org.junit.Test;

public class ParserFactoryTest {
    @Test
    public void englishParser() {
        IParser parserNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
        IParser parserCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new EnglishNumberParserConfiguration());
        IParser parserPercentaje = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration());

        Assert.assertTrue(parserNumber instanceof BaseNumberParser);
        Assert.assertTrue(parserCardinal instanceof BaseNumberParser);
        Assert.assertTrue(parserPercentaje instanceof BasePercentageParser);
    }

    @Test
    public void spanishParser() {
        IParser parserNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
        IParser parserCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new SpanishNumberParserConfiguration());
        IParser parserPercentaje = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration());

        Assert.assertTrue(parserNumber instanceof BaseNumberParser);
        Assert.assertTrue(parserCardinal instanceof BaseNumberParser);
        Assert.assertTrue(parserPercentaje instanceof BasePercentageParser);
    }

    @Test
    public void frenchParser() {
        IParser parseNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration());
        IParser parseCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new FrenchNumberParserConfiguration());
        IParser parsePercentage = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration());

        Assert.assertTrue(parseNumber instanceof BaseNumberParser);
        Assert.assertTrue(parseCardinal instanceof BaseNumberParser);
        Assert.assertTrue(parsePercentage instanceof BasePercentageParser);
    }

    @Test
    public void grmanParser() {
        IParser parseNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration());
        IParser parseCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new GermanNumberParserConfiguration());
        IParser parsePercentage = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration());

        Assert.assertTrue(parseNumber instanceof BaseNumberParser);
        Assert.assertTrue(parseCardinal instanceof BaseNumberParser);
        Assert.assertTrue(parsePercentage instanceof BasePercentageParser);
    }

    /*
    @Test
    public void chineseParser() {
        IParser parserNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        IParser parserCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new ChineseNumberParserConfiguration());
        IParser parserPercentaje = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration());

        Assert.assertTrue(parserNumber instanceof BaseCJKNumberParser);
        Assert.assertTrue(parserCardinal instanceof BaseCJKNumberParser);
        Assert.assertTrue(parserPercentaje instanceof BaseCJKNumberParser);
    }

    @Test
    public void japaneseParser() {
        IParser parserNumber = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration());
        IParser parserCardinal = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new JapaneseNumberParserConfiguration());
        IParser parserPercentaje = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration());

        Assert.assertTrue(parserNumber instanceof BaseCJKNumberParser);
        Assert.assertTrue(parserCardinal instanceof BaseCJKNumberParser);
        Assert.assertTrue(parserPercentaje instanceof BaseCJKNumberParser);
    }
    */
}
