package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;

public class BaseCJKNumberParser extends BaseNumberParser {

    protected final ICJKNumberParserConfiguration cjkConfig;

    public BaseCJKNumberParser(INumberParserConfiguration config) {
        super(config);
        this.cjkConfig = (ICJKNumberParserConfiguration)config;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return super.parse(extractResult);
    }
}
