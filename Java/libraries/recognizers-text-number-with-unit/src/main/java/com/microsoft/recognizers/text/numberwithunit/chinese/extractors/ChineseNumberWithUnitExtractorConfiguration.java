package com.microsoft.recognizers.text.numberwithunit.chinese.extractors;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.chinese.extractors.NumberExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.ChineseNumericWithUnit;
import com.microsoft.recognizers.text.utilities.DefinitionLoader;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class ChineseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
    private final Pattern halfUnitRegex = Pattern.compile(ChineseNumericWithUnit.HalfUnitRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    private final CultureInfo cultureInfo;
    private final IExtractor unitNumExtractor;
    private final Pattern compoundUnitConnectorRegex;
    private Map<Pattern, Pattern> ambiguityFiltersDict;

    protected ChineseNumberWithUnitExtractorConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.unitNumExtractor = new NumberExtractor(ChineseNumberExtractorMode.ExtractAll);
        this.compoundUnitConnectorRegex =
                Pattern.compile(ChineseNumericWithUnit.CompoundUnitConnectorRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

        this.ambiguityFiltersDict = DefinitionLoader.loadAmbiguityFilters(ChineseNumericWithUnit.AmbiguityFiltersDict);
    }

    public CultureInfo getCultureInfo() {
        return this.cultureInfo;
    }

    public IExtractor getUnitNumExtractor() {
        return this.unitNumExtractor;
    }

    public String getBuildPrefix() {
        return ChineseNumericWithUnit.BuildPrefix;
    }

    public String getBuildSuffix() {
        return ChineseNumericWithUnit.BuildSuffix;
    }

    public String getConnectorToken() {
        return ChineseNumericWithUnit.ConnectorToken;
    }

    public Pattern getCompoundUnitConnectorRegex() {
        return this.compoundUnitConnectorRegex;
    }
    
    public Pattern getAmbiguousUnitNumberMultiplierRegex() {
        return null;
    }

    public Pattern getHalfUnitRegex() {
        return Pattern.compile(ChineseNumericWithUnit.HalfUnitRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    public abstract String getExtractType();

    public abstract Map<String, String> getSuffixList();

    public abstract Map<String, String> getPrefixList();
    
    public abstract List<String> getAmbiguousUnitList();

    public Map<Pattern, Pattern> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }

    public List<ExtractResult> expandHalfSuffix(String source, List<ExtractResult> result, List<ExtractResult> numbers) {
        // Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
        if (halfUnitRegex != null) {
            Match[] match = RegExpUtility.getMatches(halfUnitRegex, source);
            if (match.length > 0) {
                List<ExtractResult> res = new ArrayList<>();
                for (ExtractResult er : result) {
                    int start = er.getStart();
                    int length = er.getLength();
                    List<ExtractResult> matchSuffix = new ArrayList<>();
                    for (Match mr : match) {
                        if (mr.index == (start + length)) {
                            ExtractResult m = new ExtractResult(mr.index, mr.length, mr.value, numbers.get(0).getType(), numbers.get(0).getData());
                            matchSuffix.add(m);
                        }
                    }
                    if (matchSuffix.size() == 1) {
                        ExtractResult mr = matchSuffix.get(0);
                        er.setStart(er.getLength() + mr.getLength());
                        er.setText(er.getText() + mr.getText());
                        List<ExtractResult> tmp = new ArrayList<>();
                        tmp.add((ExtractResult)er.getData());
                        tmp.add(mr);
                        er.setData(tmp);
                    }
                    res.add(er);
                }
                result = res;
            }
        }
        return result;
    }
}
