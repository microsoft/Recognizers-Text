package com.microsoft.recognizers.text.numberwithunit.chinese.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.numberwithunit.extractors.INumberWithUnitExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.extractors.NumberWithUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.resources.ChineseNumericWithUnit;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class ChineseNumberWithUnitExtractor extends NumberWithUnitExtractor {

    private final Pattern halfUnitRegex = Pattern.compile(ChineseNumericWithUnit.HalfUnitRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

    public ChineseNumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config) {
        super(config);
    }

    @Override
    public List<ExtractResult> extract(String source) {
        List<ExtractResult> result = extractV1(source);
        List<ExtractResult> numbers = this.config.getUnitNumExtractor().extract(source);


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
