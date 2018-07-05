package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.numberwithunit.Constants;

public class BaseMergedUnitParser implements IParser {

    protected final BaseNumberWithUnitParserConfiguration config;
    private final NumberWithUnitParser numberWithUnitParser;

    public BaseMergedUnitParser(BaseNumberWithUnitParserConfiguration config) {
        this.config = config;
        this.numberWithUnitParser = new NumberWithUnitParser(config);
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {
        // For now only currency model recognizes compound units.
        if (extResult.type.equals(Constants.SYS_UNIT_CURRENCY))
        {
            return new BaseCurrencyParser(config).parse(extResult);
        }
        else
        {
            return numberWithUnitParser.parse(extResult);
        }
    }
}