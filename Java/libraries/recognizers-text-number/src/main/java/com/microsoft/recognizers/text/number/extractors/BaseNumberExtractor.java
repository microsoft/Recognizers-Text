package com.microsoft.recognizers.text.number.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.resources.BaseNumbers;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public abstract class BaseNumberExtractor implements IExtractor {

    protected abstract Map<Pattern, String> getRegexes();

    protected Map<Pattern, Pattern> getAmbiguityFiltersDict() {
        return null;
    }

    protected abstract String getExtractType();

    protected NumberOptions getOptions() {
        return NumberOptions.None;
    }

    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    public List<ExtractResult> extract(String source) {

        if (source == null || source.isEmpty()) {
            return Collections.emptyList();
        }

        ArrayList<ExtractResult> result = new ArrayList<>();

        Boolean[] matched = new Boolean[source.length()];
        Arrays.fill(matched, false);

        HashMap<Match, String> matchSource = new HashMap<>();

        getRegexes().forEach((k, value) -> {

            Match[] matches = RegExpUtility.getMatches(k, source);

            for (Match m : matches) {
                int start = m.index;
                int length = m.length;
                for (int j = 0; j < length; j++) {
                    matched[start + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.put(m, value);
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

                    Optional<Match> srcMatches = matchSource.keySet().stream().filter(o -> o.index == finalStart && o.length == finalLength).findFirst();

                    if (srcMatches.isPresent()) {
                        Match srcMatch = srcMatches.get();

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

        result = filterAmbiguity(result, source);
        
        return result;
    }

    private ArrayList<ExtractResult> filterAmbiguity(ArrayList<ExtractResult> extractResults, String input) {
        if (getAmbiguityFiltersDict() != null) {
            for (Map.Entry<Pattern, Pattern> pair : getAmbiguityFiltersDict().entrySet()) {
                final Pattern key = pair.getKey();
                final Pattern value = pair.getValue();

                for (ExtractResult extractResult : extractResults) {
                    Optional<Match> keyMatch = Arrays.stream(RegExpUtility.getMatches(key, extractResult.getText())).findFirst();
                    if (keyMatch.isPresent()) {
                        final Match[] matches = RegExpUtility.getMatches(value, input);
                        extractResults = extractResults.stream()
                            .filter(er -> Arrays.stream(matches).noneMatch(m -> m.index < er.getStart() + er.getLength() && m.index + m.length > er.getStart()))
                            .collect(Collectors.toCollection(ArrayList::new));
                    }
                }
            }
        }

        return extractResults;
    }

    protected Pattern generateLongFormatNumberRegexes(LongFormatType type) {
        return generateLongFormatNumberRegexes(type, BaseNumbers.PlaceHolderDefault);
    }

    protected Pattern generateLongFormatNumberRegexes(LongFormatType type, String placeholder) {

        String thousandsMark = Pattern.quote(String.valueOf(type.thousandsMark));
        String decimalsMark = Pattern.quote(String.valueOf(type.decimalsMark));

        String regexDefinition = type.decimalsMark == '\0' ?
                BaseNumbers.IntegerRegexDefinition(placeholder, thousandsMark) :
                BaseNumbers.DoubleRegexDefinition(placeholder, thousandsMark, decimalsMark);

        return RegExpUtility.getSafeLookbehindRegExp(regexDefinition, Pattern.UNICODE_CHARACTER_CLASS);
    }
}
