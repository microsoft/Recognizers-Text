package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class DimensionExtractorConfiguration extends GermanNumberWithUnitExtractorConfiguration {

    public DimensionExtractorConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public DimensionExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_DIMENSION;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return DimensionSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return GermanNumericWithUnit.AmbiguousDimensionUnitList;
    }

    public static Map<String, String> DimensionSuffixList = new ImmutableMap.Builder<String, String>()
            .putAll(GermanNumericWithUnit.InformationSuffixList)
            .putAll(AreaExtractorConfiguration.AreaSuffixList)
            .putAll(LengthExtractorConfiguration.LenghtSuffixList)
            .putAll(SpeedExtractorConfiguration.SpeedSuffixList)
            .putAll(VolumeExtractorConfiguration.VolumeSuffixList)
            .putAll(WeightExtractorConfiguration.WeightSuffixList)
            .build();
}
