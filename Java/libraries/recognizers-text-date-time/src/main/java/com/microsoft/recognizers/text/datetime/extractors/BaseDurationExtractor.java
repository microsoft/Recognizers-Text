package com.microsoft.recognizers.text.datetime.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DurationParsingUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class BaseDurationExtractor implements IDateTimeExtractor {

    private final IDurationExtractorConfiguration config;
    private final boolean merge;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DURATION;
    }

    public BaseDurationExtractor(IDurationExtractorConfiguration config) {
        this(config, true);
    }

    public BaseDurationExtractor(IDurationExtractorConfiguration config, boolean merge) {
        this.config = config;
        this.merge = merge;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        List<Token> numberWithUnitTokens = numberWithUnit(input);

        tokens.addAll(numberWithUnitTokens);
        tokens.addAll(numberWithUnitAndSuffix(input, numberWithUnitTokens));
        tokens.addAll(implicitDuration(input));

        List<ExtractResult> result = Token.mergeAllTokens(tokens, input, getExtractorName());

        // First MergeMultipleDuration then ResolveMoreThanOrLessThanPrefix so cases like "more than 4 days and less than 1 week" will not be merged into one "multipleDuration"
        if (merge) {
            result = mergeMultipleDuration(input, result);
        }

        result = tagInequalityPrefix(input, result);

        return result;
    }

    private List<ExtractResult> tagInequalityPrefix(String input, List<ExtractResult> result) {
        Stream<ExtractResult> resultStream = result.stream().map(er -> {
            String beforeString = input.substring(0, er.getStart());
            boolean isInequalityPrefixMatched = false;

            ConditionalMatch match = RegexExtension.matchEnd(this.config.getMoreThanRegex(), beforeString, true);

            // The second condition is necessary so for "1 week" in "more than 4 days and less than 1 week", it will not be tagged incorrectly as "more than"
            if (match.getSuccess()) {
                er.setData(Constants.MORE_THAN_MOD);
                isInequalityPrefixMatched = true;
            }

            if (!isInequalityPrefixMatched) {
                match = RegexExtension.matchEnd(this.config.getLessThanRegex(), beforeString, true);

                if (match.getSuccess()) {
                    er.setData(Constants.LESS_THAN_MOD);
                    isInequalityPrefixMatched = true;
                }
            }

            if (isInequalityPrefixMatched) {
                int length = er.getLength() + er.getStart() - match.getMatch().get().index;
                int start = match.getMatch().get().index;
                String text = input.substring(start, start + length);
                er.setStart(start);
                er.setLength(length);
                er.setText(text);
            }

            return er;
        });
        return resultStream.collect(Collectors.toList());
    }

    private List<ExtractResult> mergeMultipleDuration(String input, List<ExtractResult> extractResults) {
        if (extractResults.size() <= 1) {
            return extractResults;
        }

        ImmutableMap<String, String> unitMap = config.getUnitMap();
        ImmutableMap<String, Long> unitValueMap = config.getUnitValueMap();
        Pattern unitRegex = config.getDurationUnitRegex();

        List<ExtractResult> result = new ArrayList<>();

        int firstExtractionIndex = 0;
        int timeUnit = 0;
        int totalUnit = 0;

        while (firstExtractionIndex < extractResults.size()) {
            String currentUnit = null;
            Optional<Match> unitMatch = Arrays.stream(RegExpUtility.getMatches(unitRegex, extractResults.get(firstExtractionIndex).getText())).findFirst();

            if (unitMatch.isPresent() && unitMap.containsKey(unitMatch.get().getGroup("unit").value)) {
                currentUnit = unitMatch.get().getGroup("unit").value;
                totalUnit++;
                if (DurationParsingUtil.isTimeDurationUnit(unitMap.get(currentUnit))) {
                    timeUnit++;
                }
            }

            if (StringUtility.isNullOrEmpty(currentUnit)) {
                firstExtractionIndex++;
                continue;
            }

            int secondExtractionIndex = firstExtractionIndex + 1;
            while (secondExtractionIndex < extractResults.size()) {
                boolean valid = false;
                int midStrBegin = extractResults.get(secondExtractionIndex - 1).getStart() + extractResults.get(secondExtractionIndex - 1).getLength();
                int midStrEnd = extractResults.get(secondExtractionIndex).getStart();
                String midStr = input.substring(midStrBegin, midStrEnd);
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getDurationConnectorRegex(), midStr)).findFirst();

                if (match.isPresent()) {
                    unitMatch = Arrays.stream(RegExpUtility.getMatches(unitRegex, extractResults.get(secondExtractionIndex).getText())).findFirst();

                    if (unitMatch.isPresent() && unitMap.containsKey(unitMatch.get().getGroup("unit").value)) {
                        String nextUnitStr = unitMatch.get().getGroup("unit").value;

                        if (unitValueMap.get(nextUnitStr) != unitValueMap.get(currentUnit)) {
                            valid = true;

                            if (unitValueMap.get(nextUnitStr) < unitValueMap.get(currentUnit)) {
                                currentUnit = nextUnitStr;
                            }
                        }

                        totalUnit++;

                        if (DurationParsingUtil.isTimeDurationUnit(unitMap.get(nextUnitStr))) {
                            timeUnit++;
                        }
                    }
                }

                if (!valid) {
                    break;
                }

                secondExtractionIndex++;
            }

            if (secondExtractionIndex - 1 > firstExtractionIndex) {
                int start = extractResults.get(firstExtractionIndex).getStart();
                int length = extractResults.get(secondExtractionIndex - 1).getStart() + extractResults.get(secondExtractionIndex - 1).getLength() - start;
                String text = input.substring(start, start + length);
                String rType = extractResults.get(firstExtractionIndex).getType();
                ExtractResult node = new ExtractResult(start, length, text, rType, null);

                // add multiple duration type to extract result
                String type = null;

                if (timeUnit == totalUnit) {
                    type = Constants.MultipleDuration_Time;
                } else if (timeUnit == 0) {
                    type = Constants.MultipleDuration_Date;
                } else {
                    type = Constants.MultipleDuration_DateTime;
                }

                node.setData(type);
                result.add(node);

                timeUnit = 0;
                totalUnit = 0;

            } else {
                result.add(extractResults.get(firstExtractionIndex));
            }

            firstExtractionIndex = secondExtractionIndex;
        }

        return result;
    }

    // handle cases that don't contain nubmer
    private Collection<Token> implicitDuration(String text) {
        Collection<Token> result = new ArrayList<>();

        // handle "all day", "all year"
        result.addAll(getTokenFromRegex(config.getAllRegex(), text));

        // handle "half day", "half year"
        result.addAll(getTokenFromRegex(config.getHalfRegex(), text));

        // handle "next day", "last year"
        result.addAll(getTokenFromRegex(config.getRelativeDurationUnitRegex(), text));

        // handle "during/for the day/week/month/year"
        if (config.getOptions().match(DateTimeOptions.CalendarMode)) {
            result.addAll(getTokenFromRegex(config.getDuringRegex(), text));
        }

        return result;
    }

    // simple cases made by a number followed an unit
    private List<Token> numberWithUnit(String text) {
        List<Token> result = new ArrayList<>();
        List<ExtractResult> ers = this.config.getCardinalExtractor().extract(text);
        for (ExtractResult er : ers) {
            String afterStr = text.substring(er.getStart() + er.getLength());
            ConditionalMatch match = RegexExtension.matchBegin(this.config.getFollowedUnit(), afterStr, true);
            if (match.getSuccess() && match.getMatch().get().index == 0) {
                result.add(new Token(er.getStart(), er.getStart() + er.getLength() + match.getMatch().get().length));
            }
        }

        // handle "3hrs"
        result.addAll(this.getTokenFromRegex(this.config.getNumberCombinedWithUnit(), text));

        // handle "an hour"
        result.addAll(this.getTokenFromRegex(this.config.getAnUnitRegex(), text));

        // handle "few" related cases
        result.addAll(this.getTokenFromRegex(this.config.getInexactNumberUnitRegex(), text));

        return result;
    }

    private Collection<Token> getTokenFromRegex(Pattern pattern, String text) {
        Collection<Token> result = new ArrayList<>();

        for (Match match : RegExpUtility.getMatches(pattern, text)) {
            result.add(new Token(match.index, match.index + match.length));
        }

        return result;
    }

    // handle cases look like: {number} {unit}? and {an|a} {half|quarter} {unit}?
    // define the part "and {an|a} {half|quarter}" as Suffix
    private Collection<Token> numberWithUnitAndSuffix(String text, Collection<Token> tokens) {
        Collection<Token> result = new ArrayList<>();

        for (Token token : tokens) {
            String afterStr = text.substring(token.getStart() + token.getLength());
            ConditionalMatch match = RegexExtension.matchBegin(this.config.getSuffixAndRegex(), afterStr, true);
            if (match.getSuccess() && match.getMatch().get().index == 0) {
                result.add(new Token(token.getStart(), token.getStart() + token.getLength() + match.getMatch().get().length));
            }
        }

        return result;
    }
}
