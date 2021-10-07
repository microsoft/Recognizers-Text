// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.french.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class DimensionExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {

    public DimensionExtractorConfiguration() {
        this(new CultureInfo(Culture.French));
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
        return FrenchNumericWithUnit.AmbiguousDimensionUnitList;
    }

    public static Map<String, String> DimensionSuffixList = new ImmutableMap.Builder<String, String>()
            .putAll(FrenchNumericWithUnit.InformationSuffixList)
            .putAll(AreaExtractorConfiguration.AreaSuffixList)
            .putAll(LengthExtractorConfiguration.LengthSuffixList)
            .putAll(SpeedExtractorConfiguration.SpeedSuffixList)
            .putAll(VolumeExtractorConfiguration.VolumeSuffixList)
            .putAll(WeightExtractorConfiguration.WeightSuffixList)
            .build();
}
