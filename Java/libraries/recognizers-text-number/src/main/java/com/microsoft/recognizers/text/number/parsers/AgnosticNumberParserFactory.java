package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.number.Constants;

import java.util.Arrays;

public abstract class AgnosticNumberParserFactory {

    public static BaseNumberParser getParser(AgnosticNumberParserType type, INumberParserConfiguration languageConfiguration) {

        boolean isChinese = languageConfiguration.getCultureInfo().cultureCode.equalsIgnoreCase(Culture.Chinese);
        boolean isJapanese = languageConfiguration.getCultureInfo().cultureCode.equalsIgnoreCase(Culture.Japanese);

        BaseNumberParser parser;


        if (isChinese || isJapanese) {
             parser = new BaseCJKNumberParser(languageConfiguration);
        } else {
            parser = new BaseNumberParser(languageConfiguration);
        }

        switch (type) {
            case Cardinal:
                parser.setSupportedTypes(Arrays.asList(Constants.SYS_NUM_CARDINAL, Constants.SYS_NUM_INTEGER, Constants.SYS_NUM_DOUBLE));
                break;
            case Double:
                parser.setSupportedTypes(Arrays.asList(Constants.SYS_NUM_DOUBLE));
                break;
            case Fraction:
                parser.setSupportedTypes(Arrays.asList(Constants.SYS_NUM_FRACTION));
                break;
            case Integer:
                parser.setSupportedTypes(Arrays.asList(Constants.SYS_NUM_INTEGER));
                break;
            case Ordinal:
                parser.setSupportedTypes(Arrays.asList(Constants.SYS_NUM_ORDINAL));
                break;
            case Percentage:
                if (!isChinese && !isJapanese) {
                     parser = new BasePercentageParser(languageConfiguration);
                }
                break;
        }

        return parser;
    }

}
