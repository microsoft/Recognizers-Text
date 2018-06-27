package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.number.portuguese.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class PortugueseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {

    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;

    protected PortugueseNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = new NumberExtractor();
        this.compoundUnitConnectorRegex = Pattern.compile(PortugueseNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    public CultureInfo getCultureInfo() { return this.cultureInfo; }
    public IExtractor getUnitNumExtractor() { return this.unitNumExtractor; }
    public String getBuildPrefix() { return PortugueseNumericWithUnit.BuildPrefix; }
    public String getBuildSuffix() { return PortugueseNumericWithUnit.BuildSuffix; }
    public String getConnectorToken() { return PortugueseNumericWithUnit.ConnectorToken; }
    public Pattern getCompoundUnitConnectorRegex() { return this.compoundUnitConnectorRegex; }

    public abstract String getExtractType();
    public abstract Map<String, String> getSuffixList();
    public abstract Map<String, String> getPrefixList();
    public abstract List<String> getAmbiguousUnitList();
}
