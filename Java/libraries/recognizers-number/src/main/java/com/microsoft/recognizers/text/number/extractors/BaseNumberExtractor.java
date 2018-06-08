package com.microsoft.recognizers.text.number.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.resources.BaseNumbers;

import java.util.*;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public abstract class BaseNumberExtractor implements IExtractor {

    protected abstract Map<Pattern, String> getRegexes();

    protected abstract String getExtractType();

    protected NumberOptions getOptions() { return NumberOptions.None; }

    protected Optional<Pattern> getNegativeNumberTermsRegex() { return Optional.empty(); }

    public List<ExtractResult> extract(String source) {
        if (source == null || source.isEmpty()) {
            return Collections.emptyList();
        }

        ArrayList<ExtractResult> result = new ArrayList<>();

        Boolean[] matched = new Boolean[source.length()];
        Arrays.fill(matched, false);

        HashMap<MatchResult, String> matchSource = new HashMap<>();
        getRegexes().forEach((k, value) -> {
            Matcher matcher = k.matcher(source);
            while (matcher.find()) {
                MatchResult r = matcher.toMatchResult();
                int start = r.start();
                int end = r.end();
                int length = end - start;
                for (int j = 0; j < length; j++) {
                    matched[start + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.put(r, value);
            }
        });

        int last = -1;
        for (int i = 0; i < source.length(); i++) {
            if (matched[i]) {
                if (i + 1 == source.length() || !matched[i + 1]) {
                    int start = last + 1;
                    int length = i - last;
                    String subStr = source.substring(start, start + length);

                    int finalStart = start;
                    int finalLength = length;
                    Optional<MatchResult> srcMatches = matchSource.keySet().stream().filter(o -> o.start() == finalStart && o.group().length() == finalLength).findFirst();
                    if (srcMatches.isPresent()) {
                        MatchResult srcMatch = srcMatches.get();

                        // Extract negative numbers
                        if (getNegativeNumberTermsRegex().isPresent()) {
                            Matcher match = getNegativeNumberTermsRegex().get().matcher(source.substring(0, start));
                            if (match.find()) {
                                start = match.start();
                                length = length + (match.end() - match.start());
                                subStr = match.group() + subStr;
                            }
                        }

                        ExtractResult er = new ExtractResult(
                                start,
                                length,
                                subStr,
                                getExtractType(),
                                matchSource.containsKey(srcMatch) ? matchSource.get(srcMatch) : null);

                        result.add(er);
                    }
                }
            } else {
                last = i;
            }
        }

        return result;
    }

    protected Pattern generateLongFormatNumberRegexes(LongFormatType type) {
        return generateLongFormatNumberRegexes(type, BaseNumbers.PlaceHolderDefault);
    }

    protected Pattern generateLongFormatNumberRegexes(LongFormatType type, String placeholder) {
        String thousandsMark = Pattern.quote(String.valueOf(type.thousandsMark));
        String decimalsMark = Pattern.quote(String.valueOf(type.decimalsMark));

        String regexDefinition = type.decimalsMark == '\0'
                ? BaseNumbers.IntegerRegexDefinition(placeholder, thousandsMark)
                : BaseNumbers.DoubleRegexDefinition(placeholder, thousandsMark, decimalsMark);

        return Pattern.compile(regexDefinition, Pattern.CASE_INSENSITIVE);
    }
}
