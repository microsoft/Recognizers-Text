// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.german.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.numberwithunit.parsers.BaseNumberWithUnitParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

public abstract class GermanNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {

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
        return GermanNumericWithUnit.ConnectorToken;
    }

    public GermanNumberWithUnitParserConfiguration(CultureInfo ci) {
        super(ci);
        this.internalNumberExtractor = NumberExtractor.getInstance();
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration());
    }
}
