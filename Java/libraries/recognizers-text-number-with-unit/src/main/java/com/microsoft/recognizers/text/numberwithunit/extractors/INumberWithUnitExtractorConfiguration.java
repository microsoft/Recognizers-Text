// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public interface INumberWithUnitExtractorConfiguration {

    Map<String, String> getSuffixList();
    
    Map<String, String> getPrefixList();
    
    List<String> getAmbiguousUnitList();
    
    String getExtractType();
    
    CultureInfo getCultureInfo();
    
    IExtractor getUnitNumExtractor();
    
    String getBuildPrefix();
    
    String getBuildSuffix();
    
    String getConnectorToken();
    
    Pattern getCompoundUnitConnectorRegex();
    
    Pattern getAmbiguousUnitNumberMultiplierRegex();

    Map<Pattern, Pattern> getAmbiguityFiltersDict();

    List<ExtractResult> expandHalfSuffix(String source, List<ExtractResult> result, List<ExtractResult> numbers);
}
