// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class DimensionExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {

    public DimensionExtractorConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
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
        return Stream.of(PortugueseNumericWithUnit.AmbiguousDimensionUnitList,
                         PortugueseNumericWithUnit.AmbiguousDimensionUnitList,
                         PortugueseNumericWithUnit.AmbiguousAngleUnitList,
                         PortugueseNumericWithUnit.AmbiguousLengthUnitList,
                         PortugueseNumericWithUnit.AmbiguousSpeedUnitList,
                         PortugueseNumericWithUnit.AmbiguousWeightUnitList
                        ).flatMap(Collection::stream).collect(Collectors.toList());
    }

    public static Map<String, String> DimensionSuffixList = new ImmutableMap.Builder<String, String>()
            .putAll(PortugueseNumericWithUnit.InformationSuffixList)
            .putAll(AreaExtractorConfiguration.AreaSuffixList)
            .putAll(LengthExtractorConfiguration.LengthSuffixList)
            .putAll(SpeedExtractorConfiguration.SpeedSuffixList)
            .putAll(VolumeExtractorConfiguration.VolumeSuffixList)
            .putAll(WeightExtractorConfiguration.WeightSuffixList)
            .build();
}
