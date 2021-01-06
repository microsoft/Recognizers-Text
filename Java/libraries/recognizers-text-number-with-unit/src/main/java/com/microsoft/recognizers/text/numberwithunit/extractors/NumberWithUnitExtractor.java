package com.microsoft.recognizers.text.numberwithunit.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.models.PrefixUnitResult;
import com.microsoft.recognizers.text.numberwithunit.resources.BaseUnits;
import com.microsoft.recognizers.text.numberwithunit.utilities.StringComparer;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.QueryProcessor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.Set;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class NumberWithUnitExtractor implements IExtractor {

    private final INumberWithUnitExtractorConfiguration config;

    private final Set<Pattern> suffixRegexes;
    private final Set<Pattern> prefixRegexes;

    private final Pattern separateRegex;
    private final Pattern singleCharUnitRegex = Pattern.compile(BaseUnits.SingleCharUnitRegex, Pattern.UNICODE_CHARACTER_CLASS);

    private final int maxPrefixMatchLen;

    private final List<String> separators = Arrays.asList("|");

    public NumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config) {
        this.config = config;

        if (config.getSuffixList() != null && config.getSuffixList().size() > 0) {
            suffixRegexes = buildRegexFromSet(this.config.getSuffixList().values());
        } else {
            suffixRegexes = new HashSet<>();
        }

        int tempMaxPrefixMatchLen = 0;
        if (this.config.getPrefixList() != null && this.config.getPrefixList().size() > 0) {
            for (String preMatch : this.config.getPrefixList().values()) {
                List<String> matchList = QueryProcessor.split(preMatch, separators);
                for (String match : matchList) {
                    tempMaxPrefixMatchLen = tempMaxPrefixMatchLen >= match.length() ? tempMaxPrefixMatchLen : match.length();
                }
            }

            // 2 is the maximum length of spaces.
            tempMaxPrefixMatchLen += 2;
            maxPrefixMatchLen = tempMaxPrefixMatchLen;
            prefixRegexes = buildRegexFromSet(this.config.getPrefixList().values());
        } else {
            maxPrefixMatchLen = 0;
            prefixRegexes = new HashSet<>();
        }

        separateRegex = buildSeparateRegexFromSet();
    }

    @Override
    public List<ExtractResult> extract(String source) {
        List<ExtractResult> result = new ArrayList<>();

        if (!preCheckStr(source)) {
            return result;
        }

        Map<Integer, PrefixUnitResult> mappingPrefix = new HashMap<Integer, PrefixUnitResult>();
        boolean[] matched = new boolean[source.length()];
        Arrays.fill(matched, false);
        List<ExtractResult> numbers = this.config.getUnitNumExtractor().extract(source);
        int sourceLen = source.length();

        List<Matcher> prefixMatch = new ArrayList<Matcher>();
        List<Matcher> suffixMatch = new ArrayList<Matcher>();

        for (Pattern regex : prefixRegexes) {
            Matcher match = regex.matcher(source);
            if (match.find()) {
                prefixMatch.add(match);
            }
        }

        for (Pattern regex : suffixRegexes) {
            Matcher match = regex.matcher(source);
            if (match.find()) {
                suffixMatch.add(match);
            }
        }

        if (numbers.size() > 0  && this.config.getExtractType() == Constants.SYS_UNIT_CURRENCY && prefixMatch.size() > 0 && suffixMatch.size() > 0) {

            for (ExtractResult number : numbers) {
                int start = number.getStart();
                int length = number.getLength();
                Boolean numberPrefix = false;
                Boolean numberSuffix = false;

                for (Matcher match : prefixMatch) {
                    if (match.end() == start) {
                        numberPrefix = true;
                    }
                }

                for (Matcher match : suffixMatch) {
                    if (start + length == match.start()) {
                        numberSuffix = true;
                    }
                }

                if (numberPrefix && numberSuffix && number.getText().contains(",")) {
                    int commaIndex = start + number.getText().indexOf(",");
                    source = source.substring(0, commaIndex) + " " + source.substring(commaIndex + 1);
                }
            }
            numbers = this.config.getUnitNumExtractor().extract(source);
        }

        /* Special case for cases where number multipliers clash with unit */
        Pattern ambiguousMultiplierRegex = this.config.getAmbiguousUnitNumberMultiplierRegex();
        if (ambiguousMultiplierRegex != null) {
            for (int i = 0; i < numbers.size(); i++) {
                ExtractResult number = numbers.get(i);

                Match[] matches = RegExpUtility.getMatches(ambiguousMultiplierRegex, number.getText());
                if (matches.length == 1) {
                    int newLength = number.getLength() - matches[0].length;
                    numbers.set(i, new ExtractResult(number.getStart(), newLength, number.getText().substring(0, newLength),
                            number.getType(), number.getData()));
                }
            }
        }

        /* Mix prefix and numbers, make up a prefix-number combination */
        if (maxPrefixMatchLen != 0) {
            for (ExtractResult number : numbers) {
                if (number.getStart() == null || number.getLength() == null) {
                    continue;
                }

                int maxFindPref = Math.min(maxPrefixMatchLen, number.getStart());
                if (maxFindPref == 0) {
                    continue;
                }

                /* Scan from left to right , find the longest match */
                String leftStr = source.substring(number.getStart() - maxFindPref, number.getStart());
                int lastIndex = leftStr.length();

                MatchResult bestMatch = null;
                for (Pattern regex : prefixRegexes) {
                    Matcher match = regex.matcher(leftStr);
                    while (match.find()) {
                        if (leftStr.substring(match.start(), lastIndex).trim().equals(match.group())) {
                            if (bestMatch == null || bestMatch.start() >= match.start()) {
                                bestMatch = match.toMatchResult();
                            }
                        }
                    }
                }

                if (bestMatch != null) {
                    int offset = lastIndex - bestMatch.start();
                    String unitStr = leftStr.substring(bestMatch.start(), lastIndex);
                    mappingPrefix.put(number.getStart(), new PrefixUnitResult(offset, unitStr));
                }
            }
        }

        for (ExtractResult number : numbers) {
            if (number.getStart() == null || number.getLength() == null) {
                continue;
            }

            int start = number.getStart();
            int length = number.getLength();
            int maxFindLen = sourceLen - start - length;

            PrefixUnitResult prefixUnit = null;
            if (mappingPrefix.containsKey(start)) {
                prefixUnit = mappingPrefix.get(start);
            }

            if (maxFindLen > 0) {
                String rightSub = source.substring(start + length, start + length + maxFindLen);
                List<Matcher> unitMatch = suffixRegexes.stream().map(p -> p.matcher(rightSub)).collect(Collectors.toList());

                int maxlen = 0;
                for (int i = 0; i < unitMatch.size(); i++) {
                    Matcher m = unitMatch.get(i);
                    while (m.find()) {
                        int endpos = m.end();
                        if (m.start() >= 0) {
                            String midStr = rightSub.substring(0, Math.min(m.start(), rightSub.length()));
                            if (maxlen < endpos && (midStr.trim().isEmpty() || midStr.trim().equalsIgnoreCase(this.config.getConnectorToken()))) {
                                maxlen = endpos;
                            }
                        }
                    }
                }

                if (maxlen != 0) {
                    for (int i = 0; i < length + maxlen; i++) {
                        matched[i + start] = true;
                    }

                    String substr = source.substring(start, start + length + maxlen);
                    ExtractResult er = new ExtractResult(start, length + maxlen, substr, this.config.getExtractType(), null);

                    if (prefixUnit != null) {
                        er.setStart(er.getStart() - prefixUnit.offset);
                        er.setLength(er.getLength() + prefixUnit.offset);
                        er.setText(prefixUnit.unitStr + er.getText());
                    }

                    /* Relative position will be used in Parser */
                    number.setStart(start - er.getStart());
                    er.setData(number);
                    result.add(er);

                    continue;
                }
            }

            if (prefixUnit != null) {
                ExtractResult er = new ExtractResult(
                        number.getStart() - prefixUnit.offset,
                        number.getLength() + prefixUnit.offset,
                        prefixUnit.unitStr + number.getText(),
                        this.config.getExtractType(),
                        null);

                /* Relative position will be used in Parser */
                number.setStart(start - er.getStart());
                er.setData(number);
                result.add(er);
            }
        }

        // Extract Separate unit
        if (separateRegex != null) {
            extractSeparateUnits(source, result);
        }

        // Remove common ambiguous cases
        result = filterAmbiguity(result, source);

        // Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
        result = this.config.expandHalfSuffix(source, result, numbers);

        return result;
    }

    private List<ExtractResult> filterAmbiguity(List<ExtractResult> extractResults, String input) {

        if (this.config.getAmbiguityFiltersDict() != null) {

            for (Map.Entry<Pattern, Pattern> pair : this.config.getAmbiguityFiltersDict().entrySet()) {

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

        // Filter single-char units if not exact match
        extractResults = extractResults.stream().filter(er -> !(er.getLength() != input.length() && Pattern.matches(singleCharUnitRegex.toString(), er.getText())))
                .collect(Collectors.toCollection(ArrayList::new));

        return extractResults;
    }

    public void extractSeparateUnits(String source, List<ExtractResult> numDependResults) {
        //Default is false
        boolean[] matchResult = new boolean[source.length()];
        Arrays.fill(matchResult, false);

        for (ExtractResult numDependResult : numDependResults) {
            int start = numDependResult.getStart();
            int i = 0;
            do {
                matchResult[start + i++] = true;
            } while (i < numDependResult.getLength());
        }

        //Extract all SeparateUnits, then merge it with numDependResults
        Matcher matcher = separateRegex.matcher(source);
        while (matcher.find()) {


            int start = matcher.start();
            int end = matcher.end();
            int length = end - start;

            int i = 0;
            while (i < length && !matchResult[start + i]) {
                i++;
            }

            if (i == length) {
                //Mark as extracted
                for (int j = 0; j < i; j++) {
                    matchResult[j] = true;
                }

                numDependResults.add(new ExtractResult(
                        start,
                        length,
                        matcher.group(),
                        this.config.getExtractType(),
                        null));
            }
        }
    }

    protected boolean preCheckStr(String str) {
        return str != null && !str.isEmpty();
    }

    protected Set<Pattern> buildRegexFromSet(Collection<String> values) {
        return buildRegexFromSet(values, false);
    }

    protected Set<Pattern> buildRegexFromSet(Collection<String> collection, boolean ignoreCase) {

        Set<Pattern> regexes = new HashSet<>();
        for (String regexString : collection) {
            List<String> regexTokens = new ArrayList<>();
            for (String token : QueryProcessor.split(regexString, Arrays.asList("|"))) {
                regexTokens.add(Pattern.quote(token));
            }

            String pattern = String.format(
                    "%s(%s)%s",
                    this.config.getBuildPrefix(),
                    String.join("|", regexTokens),
                    this.config.getBuildSuffix());

            int options = Pattern.UNICODE_CHARACTER_CLASS | (ignoreCase ? Pattern.CASE_INSENSITIVE : 0);

            Pattern regex = Pattern.compile(pattern, options);
            regexes.add(regex);
        }

        return regexes;
    }

    protected Pattern buildSeparateRegexFromSet() {
        return buildSeparateRegexFromSet(false);
    }

    protected Pattern buildSeparateRegexFromSet(boolean ignoreCase) {

        Set<String> separateWords = new HashSet<>();
        if (config.getPrefixList() != null && config.getPrefixList().size() > 0) {
            for (String addWord : config.getPrefixList().values()) {

                for (String word : QueryProcessor.split(addWord, separators)) {
                    if (validateUnit(word)) {
                        separateWords.add(word);
                    }
                }
            }
        }

        if (config.getSuffixList() != null && config.getSuffixList().size() > 0) {
            for (String addWord : config.getSuffixList().values()) {
                for (String word : QueryProcessor.split(addWord, separators)) {
                    if (validateUnit(word)) {
                        separateWords.add(word);
                    }
                }
            }
        }

        if (config.getAmbiguousUnitList() != null && config.getAmbiguousUnitList().size() > 0) {
            List<String> abandonWords = config.getAmbiguousUnitList();
            for (String abandonWord : abandonWords) {
                if (separateWords.contains(abandonWord)) {
                    separateWords.remove(abandonWord);
                }
            }
        }

        //Sort separateWords using descending length.
        List<String> regexTokens = separateWords.stream().map(s -> Pattern.quote(s)).collect(Collectors.toList());
        if (regexTokens.size() == 0) {
            return null;
        }

        Collections.sort(regexTokens, new StringComparer());
        String pattern = String.format(
                "%s(%s)%s",
                this.config.getBuildPrefix(),
                String.join("|", regexTokens),
                this.config.getBuildSuffix());
        int options = Pattern.UNICODE_CHARACTER_CLASS | (ignoreCase ? Pattern.CASE_INSENSITIVE : 0);

        Pattern regex = Pattern.compile(pattern, options);
        return regex;
    }

    public boolean validateUnit(String source) {
        return !source.startsWith("-");
    }
}
