package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.number.spanish.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.parsers.BaseNumberWithUnitParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;

public class SpanishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {

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
        return SpanishNumericWithUnit.ConnectorToken;
    }

    public SpanishNumberWithUnitParserConfiguration(CultureInfo ci) {
        super(ci);
        this.internalNumberExtractor = new NumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
    }
}
