package com.microsoft.recognizers.text.numberwithunit.chinese.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.chinese.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.numberwithunit.parsers.BaseNumberWithUnitParserConfiguration;

public class ChineseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {

    private final IExtractor internalNumberExtractor;
    private final IParser internalNumberParser;
    private final String connectorToken;

    @Override
    public IParser getInternalNumberParser() {
        return this.internalNumberParser;
    }

    @Override
    public IExtractor getInternalNumberExtractor() {
        return this.internalNumberExtractor;
    }

    @Override
    public String getConnectorToken() {
        return this.connectorToken;
    }

    public ChineseNumberWithUnitParserConfiguration(CultureInfo ci) {
        super(ci);
        this.internalNumberExtractor = new NumberExtractor();
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        this.connectorToken = "";
    }
}
