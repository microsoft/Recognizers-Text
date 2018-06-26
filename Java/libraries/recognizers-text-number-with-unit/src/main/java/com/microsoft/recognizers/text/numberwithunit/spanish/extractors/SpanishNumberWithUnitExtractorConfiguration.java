package com.microsoft.recognizers.text.numberwithunit.spanish.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.number.spanish.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class SpanishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {

    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;

    protected SpanishNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = new NumberExtractor();
        this.compoundUnitConnectorRegex = Pattern.compile(SpanishNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    public CultureInfo getCultureInfo() { return this.cultureInfo; }
    public IExtractor getUnitNumExtractor() { return this.unitNumExtractor; }
    public String getBuildPrefix() { return SpanishNumericWithUnit.BuildPrefix; }
    public String getBuildSuffix() { return SpanishNumericWithUnit.BuildSuffix; }
    public String getConnectorToken() { return SpanishNumericWithUnit.ConnectorToken; }
    public Pattern getCompoundUnitConnectorRegex() { return this.compoundUnitConnectorRegex; }

    public abstract String getExtractType();
    public abstract Map<String, String> getSuffixList();
    public abstract Map<String, String> getPrefixList();
    public abstract List<String> getAmbiguousUnitList();
}
