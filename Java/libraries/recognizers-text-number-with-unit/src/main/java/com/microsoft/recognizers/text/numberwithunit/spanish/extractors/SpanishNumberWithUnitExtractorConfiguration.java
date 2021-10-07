// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.spanish.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.spanish.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;
import com.microsoft.recognizers.text.utilities.DefinitionLoader;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class SpanishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {

    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;
    private Map<Pattern, Pattern> ambiguityFiltersDict;

    protected SpanishNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = NumberExtractor.getInstance(NumberMode.Unit);;
        this.compoundUnitConnectorRegex =
                Pattern.compile(SpanishNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

        this.ambiguityFiltersDict = DefinitionLoader.loadAmbiguityFilters(SpanishNumericWithUnit.AmbiguityFiltersDict);
    }

    public CultureInfo getCultureInfo() {
        return this.cultureInfo;
    }

    public IExtractor getUnitNumExtractor() {
        return this.unitNumExtractor;
    }

    public String getBuildPrefix() {
        return SpanishNumericWithUnit.BuildPrefix;
    }

    public String getBuildSuffix() {
        return SpanishNumericWithUnit.BuildSuffix;
    }

    public String getConnectorToken() {
        return SpanishNumericWithUnit.ConnectorToken;
    }

    public Pattern getCompoundUnitConnectorRegex() {
        return this.compoundUnitConnectorRegex;
    }

    public Pattern getAmbiguousUnitNumberMultiplierRegex() {
        return null;
    }

    public abstract String getExtractType();
    
    public abstract Map<String, String> getSuffixList();
    
    public abstract Map<String, String> getPrefixList();
    
    public abstract List<String> getAmbiguousUnitList();

    public Map<Pattern, Pattern> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }

    public List<ExtractResult> expandHalfSuffix(String source, List<ExtractResult> result, List<ExtractResult> numbers) {
        return result;
    }
}
