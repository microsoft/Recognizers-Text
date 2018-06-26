package com.microsoft.recognizers.text.numberwithunit.french.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.number.french.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class FrenchNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {

    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;

    protected FrenchNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = NumberExtractor.getInstance();
        this.compoundUnitConnectorRegex = Pattern.compile(FrenchNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    public CultureInfo getCultureInfo() { return this.cultureInfo; }
    public IExtractor getUnitNumExtractor() { return this.unitNumExtractor; }
    public String getBuildPrefix() { return FrenchNumericWithUnit.BuildPrefix; }
    public String getBuildSuffix() { return FrenchNumericWithUnit.BuildSuffix; }
    public String getConnectorToken() { return FrenchNumericWithUnit.ConnectorToken; }
    public Pattern getCompoundUnitConnectorRegex() { return this.compoundUnitConnectorRegex; }

    public abstract String getExtractType();
    public abstract Map<String, String> getSuffixList();
    public abstract Map<String, String> getPrefixList();
    public abstract List<String> getAmbiguousUnitList();
}
