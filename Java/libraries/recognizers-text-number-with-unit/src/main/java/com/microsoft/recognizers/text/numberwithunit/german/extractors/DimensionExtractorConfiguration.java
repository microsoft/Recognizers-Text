// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.Stream;

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
        return Stream.of(GermanNumericWithUnit.AmbiguousDimensionUnitList,
                         GermanNumericWithUnit.AmbiguousDimensionUnitList,
                         GermanNumericWithUnit.AmbiguousAngleUnitList,
                         GermanNumericWithUnit.AmbiguousLengthUnitList,
                         GermanNumericWithUnit.AmbiguousVolumeUnitList,
                         GermanNumericWithUnit.AmbiguousWeightUnitList
                        ).flatMap(Collection::stream).collect(Collectors.toList());
    }

    public static Map<String, String> DimensionSuffixList = new ImmutableMap.Builder<String, String>()
            .putAll(GermanNumericWithUnit.InformationSuffixList)
            .putAll(AreaExtractorConfiguration.AreaSuffixList)
            .putAll(LengthExtractorConfiguration.LengthSuffixList)
            .putAll(SpeedExtractorConfiguration.SpeedSuffixList)
            .putAll(VolumeExtractorConfiguration.VolumeSuffixList)
            .putAll(WeightExtractorConfiguration.WeightSuffixList)
            .build();
}
