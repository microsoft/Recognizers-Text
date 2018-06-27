package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.number.french.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.french.parsers.FrenchNumberParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.parsers.BaseNumberWithUnitParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

public class FrenchNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {

    private final IParser internalNumberParser;
    private final IExtractor internalNumberExtractor;

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
        return FrenchNumericWithUnit.ConnectorToken;
    }

    public FrenchNumberWithUnitParserConfiguration(CultureInfo ci) {
        super(ci);
        this.internalNumberExtractor = NumberExtractor.getInstance();
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration());
    }
}
