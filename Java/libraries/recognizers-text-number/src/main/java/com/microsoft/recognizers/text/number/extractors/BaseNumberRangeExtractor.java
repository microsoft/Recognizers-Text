package com.microsoft.recognizers.text.number.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberRangeConstants;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import org.javatuples.Pair;
import org.javatuples.Triplet;

import java.util.*;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public abstract class BaseNumberRangeExtractor implements IExtractor {

    private final BaseNumberExtractor numberExtractor;

    private final BaseNumberExtractor ordinalExtractor;

    private final BaseNumberParser numberParser;

    protected abstract Map<Pattern, String> getRegexes();

    protected String getExtractType() {
        return "";
    }

    protected BaseNumberRangeExtractor(BaseNumberExtractor numberExtractor, BaseNumberExtractor ordinalExtractor, BaseNumberParser numberParser) {
        this.numberExtractor = numberExtractor;
        this.ordinalExtractor = ordinalExtractor;
        this.numberParser = numberParser;
    }

    @Override
    public List<ExtractResult> extract(String source) {
        if (source == null || source.isEmpty()) {
            return Collections.emptyList();
        }

        List<ExtractResult> result = new ArrayList<>();
        Map<Pair<Integer, Integer>, String> matchSource = new HashMap<>();
        boolean[] matched = new boolean[source.length()];
        Arrays.fill(matched, false);

        List<Pair<Matcher, String>> matches = new ArrayList<>();
        getRegexes().forEach((k, value) -> {
            Matcher matcher = k.matcher(source);
            if(matcher.find()) {
                matcher.reset();
                matches.add(Pair.with(matcher, value));
            }
        });

        for(Pair<Matcher, String> pair : matches) {
            Matcher matcher = pair.getValue0();
            String value = pair.getValue1();
            while (matcher.find()) {
                int start = NumberRangeConstants.INVALID_NUM;
                int length = NumberRangeConstants.INVALID_NUM;
                Pair<Integer, Integer> startAndLength = getMatchedStartAndLength(matcher, value, source, start, length);
                start = startAndLength.getValue0();
                length = startAndLength.getValue1();

                if (start >= 0 && length > 0) {
                    for (int j = 0; j < length; j++) {
                        matched[start + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.put(Pair.with(start, length), value);
                }
            }
        }

        int last = -1;
        for (int i = 0; i < source.length(); i++) {
            if (matched[i]) {
                if (i + 1 == source.length() || !matched[i + 1]) {
                    int start = last + 1;
                    int length = i - last;
                    String substr = source.substring(start, start + length);

                    Optional<Pair<Integer, Integer>> srcMatches = matchSource.keySet().stream().filter(o -> o.getValue0() == start && o.getValue1() == length).findFirst();
                    if (srcMatches.isPresent()) {
                        Pair<Integer, Integer> srcMatch = srcMatches.get();
                        ExtractResult er = new ExtractResult(start, length, substr, getExtractType(), matchSource.containsKey(srcMatch) ? matchSource.get(srcMatch) : null);
                        result.add(er);
                    }
                }
            } else {
                last = i;
            }
        }

        return result;
    }

    private Pair<Integer, Integer> getMatchedStartAndLength(Matcher match, String type, String source, int start, int length) {

        Map<String, String> groupValues = RegExpUtility.getNamedGroups(match);
        String numberStr1 = groupValues.containsKey("number1") ? groupValues.get("number1") : "";
        String numberStr2 = groupValues.containsKey("number2") ? groupValues.get("number2") : "";

        if (type.contains(NumberRangeConstants.TWONUM)) {
            List<ExtractResult> extractNumList1 = extractNumberAndOrdinalFromStr(numberStr1);
            List<ExtractResult> extractNumList2 = extractNumberAndOrdinalFromStr(numberStr2);

            if (extractNumList1 != null && extractNumList2 != null) {
                if (type.contains(NumberRangeConstants.TWONUMTILL)) {
                    // num1 must have same type with num2
                    if (!extractNumList1.get(0).type.equals(extractNumList2.get(0).type)) {
                        return Pair.with(start, length);
                    }

                    // num1 must less than num2
                    ParseResult numExt1 = numberParser.parse(extractNumList1.get(0));
                    ParseResult numExt2 = numberParser.parse(extractNumList2.get(0));
                    double num1 = numExt1.value != null ? (double) numExt1.value : 0;
                    double num2 = numExt1.value != null ? (double) numExt2.value : 0;

                    if (num1 > num2) {
                        return Pair.with(start, length);
                    }

                    extractNumList1.subList(1, extractNumList1.size()).clear();
                    extractNumList2.subList(1, extractNumList2.size()).clear();
                }

                start = match.start();
                length = match.end() - start;

                Triplet<Boolean, Integer, Integer> num1 = validateMatchAndGetStartAndLength(extractNumList1, numberStr1, match, source, start, length);
                start = num1.getValue1();
                length = num1.getValue2();
                Triplet<Boolean, Integer, Integer> num2 = validateMatchAndGetStartAndLength(extractNumList2, numberStr2, match, source, start, length);
                start = num2.getValue1();
                length = num2.getValue2();

                if (!num1.getValue0() || !num2.getValue0()) {
                    start = NumberRangeConstants.INVALID_NUM;
                    length = NumberRangeConstants.INVALID_NUM;
                }
            }
        } else {
            String numberStr = numberStr1 == null || numberStr1.isEmpty() ? numberStr2 : numberStr1;

            List<ExtractResult> extractNumList = extractNumberAndOrdinalFromStr(numberStr);

            if (extractNumList != null) {
                start = match.start();
                length = match.end() - start;

                Triplet<Boolean, Integer, Integer> num = validateMatchAndGetStartAndLength(extractNumList, numberStr, match, source, start, length);
                start = num.getValue1();
                length = num.getValue2();
                if (!num.getValue0()) {
                    start = NumberRangeConstants.INVALID_NUM;
                    length = NumberRangeConstants.INVALID_NUM;
                }
            }
        }

        return Pair.with(start, length);
    }

    private Triplet<Boolean, Integer, Integer> validateMatchAndGetStartAndLength(List<ExtractResult> extractNumList, String numberStr, MatchResult match, String source, int start, int length) {

        boolean validNum = false;

        for(ExtractResult extractNum : extractNumList)
        {
            if (numberStr.trim().endsWith(extractNum.text) && match.group().startsWith(numberStr))
            {
                start = source.indexOf(numberStr) + (extractNum.start != null ? extractNum.start : 0);
                length = length - (extractNum.start != null ? extractNum.start : 0);
                validNum = true;
            }
            else if (extractNum.start == 0 && match.group().endsWith(numberStr))
            {
                length = length - numberStr.length() + (extractNum.length != null ? extractNum.length : 0);
                validNum = true;
            }
            else if (extractNum.start == 0 && extractNum.length == numberStr.trim().length())
            {
                validNum = true;
            }

            if (validNum)
            {
                break;
            }
        }

        return Triplet.with(validNum, start, length);
    }

    private List<ExtractResult> extractNumberAndOrdinalFromStr(String numberStr) {
        List<ExtractResult> extractNumber = numberExtractor.extract(numberStr);
        List<ExtractResult> extractOrdinal = ordinalExtractor.extract(numberStr);

        if (extractNumber.size() == 0) {
            return extractOrdinal.size() == 0 ? null : extractOrdinal;
        }

        if (extractOrdinal.size() == 0) {
            return extractNumber;
        }

        extractNumber.addAll(extractOrdinal);

        //        extractNumber = extractNumber.OrderByDescending(num => num.Length).ThenByDescending(num => num.Start).ToList();
        Collections.sort(extractNumber, (Comparator) (o1, o2) -> {
            Integer x1 = ((ExtractResult) o1).length;
            Integer x2 = ((ExtractResult) o2).length;
            int sComp = x2.compareTo(x1);

            if (sComp != 0) {
                return sComp;
            }

            x1 = ((ExtractResult) o1).start;
            x2 = ((ExtractResult) o2).start;
            return x2.compareTo(x1);
        });

        return extractNumber;
    }
}
