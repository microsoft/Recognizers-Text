package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.german.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class GermanNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;

    protected GermanNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = NumberExtractor.getInstance();
        this.compoundUnitConnectorRegex = Pattern.compile(GermanNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    public CultureInfo getCultureInfo() { return this.cultureInfo; }
    public IExtractor getUnitNumExtractor() { return this.unitNumExtractor; }
    public String getBuildPrefix() { return GermanNumericWithUnit.BuildPrefix; }
    public String getBuildSuffix() { return GermanNumericWithUnit.BuildSuffix; }
    public String getConnectorToken() { return ""; }
    public Pattern getCompoundUnitConnectorRegex() { return this.compoundUnitConnectorRegex; }

    public abstract String getExtractType();
    public abstract Map<String, String> getSuffixList();
    public abstract Map<String, String> getPrefixList();
    public abstract List<String> getAmbiguousUnitList();
}
