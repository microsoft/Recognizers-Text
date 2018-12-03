package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class BaseDatePeriodExtractor implements IDateTimeExtractor {

    private final IDatePeriodExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DATEPERIOD;
    }

    public BaseDatePeriodExtractor(IDatePeriodExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        tokens.addAll(matchSimpleCases(input));
        List<ExtractResult> simpleCasesResults = Token.mergeAllTokens(tokens, input, getExtractorName());
        tokens.addAll(mergeTwoTimePoints(input, reference));
        tokens.addAll(matchDuration(input, reference));
        tokens.addAll(singleTimePointWithPatterns(input, reference));
        tokens.addAll(matchComplexCases(input, simpleCasesResults, reference));
        tokens.addAll(matchYearPeriod(input, reference));
        tokens.addAll(matchOrdinalNumberWithCenturySuffix(input, reference));

        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    private List<Token> matchSimpleCases(String input) {
        List<Token> results = new ArrayList<>();

        for (Pattern regex : config.getSimpleCasesRegexes()) {
            Match[] matches = RegExpUtility.getMatches(regex, input);

            for (Match match : matches) {
                Optional<Match> matchYear = Arrays.stream(RegExpUtility.getMatches(config.getYearRegex(), match.value)).findFirst();

                if (matchYear.isPresent() && matchYear.get().length == match.length) {
                    int year = ((BaseDateExtractor)config.getDatePointExtractor()).getYearFromText(matchYear.get());
                    if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                        continue;
                    }
                }

                results.add(new Token(match.index, match.index + match.length));
            }

        }

        return results;
    }

    private List<Token> mergeTwoTimePoints(String input, LocalDateTime reference) {
        List<ExtractResult> ers = config.getDatePointExtractor().extract(input, reference);

        return mergeMultipleExtractions(input, ers);
    }

    private List<Token> mergeMultipleExtractions(String input, List<ExtractResult> extractionResults) {
        List<Token> results = new ArrayList<>();

        if (extractionResults.size() <= 1) {
            return results;
        }

        int idx = 0;

        while (idx < extractionResults.size() - 1) {
            ExtractResult thisResult = extractionResults.get(idx);
            ExtractResult nextResult = extractionResults.get(idx + 1);

            int middleBegin = thisResult.start + thisResult.length;
            int middleEnd = nextResult.start;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }

            String middleStr = input.substring(middleBegin, middleEnd).trim().toLowerCase();
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getTillRegex(), middleStr)).findFirst();
            if (match.isPresent() && match.get().index == 0 && match.get().length == middleStr.length()) {
                int periodBegin = thisResult.start;
                int periodEnd = nextResult.start + nextResult.length;

                // handle "from/between" together with till words (till/until/through...)
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase();

                ResultIndex fromIndex = config.getFromTokenIndex(beforeStr);
                ResultIndex betweenIndex = config.getBetweenTokenIndex(beforeStr);

                if (fromIndex.result) {
                    periodBegin = fromIndex.index;
                } else if (betweenIndex.result) {
                    periodBegin = betweenIndex.index;
                }

                results.add(new Token(periodBegin, periodEnd));

                // merge two tokens here, increase the index by two
                idx = +2;
                continue;
            }

            boolean hasConnectorToken = config.hasConnectorToken(middleStr);
            if (hasConnectorToken) {
                int periodBegin = thisResult.start;
                int periodEnd = nextResult.start + nextResult.length;

                // handle "between...and..." case
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase();

                ResultIndex beforeIndex = config.getBetweenTokenIndex(beforeStr);

                if (beforeIndex.result) {
                    periodBegin = beforeIndex.index;
                    results.add(new Token(periodBegin, periodEnd));

                    // merge two tokens here, increase the index by two
                    idx = +2;
                    continue;
                }
            }
            idx++;
        }

        return results;
    }

    private List<Token> matchDuration(String input, LocalDateTime reference) {

        List<Token> results = new ArrayList<>();

        List<Token> durations = new ArrayList<>();
        Iterable<ExtractResult> durationExtractions = config.getDurationExtractor().extract(input, reference);

        for (ExtractResult durationExtraction : durationExtractions) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), durationExtraction.text)).findFirst();
            if (match.isPresent()) {
                durations.add(new Token(durationExtraction.start, durationExtraction.start + durationExtraction.length));
            }
        }

        for (Token duration : durations) {
            String beforeStr = input.substring(0, duration.getStart()).toLowerCase();
            String afterStr = input.substring(duration.getStart() + duration.getLength()).toLowerCase();

            if (StringUtility.isNullOrWhiteSpace(beforeStr) && StringUtility.isNullOrWhiteSpace(afterStr)) {
                continue;
            }

            // Match prefix
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), beforeStr)).findFirst();

            if (matchPrefixRegexInSegment(beforeStr, match)) {
                int start = match.get().index;
                int end = duration.getEnd();
                results.add(new Token(start, end));
                continue;
            }


            // within "Days/Weeks/Months/Years" should be handled as dateRange here
            // if duration contains "Seconds/Minutes/Hours", it should be treated as datetimeRange
            match = Arrays.stream(RegExpUtility.getMatches(config.getWithinNextPrefixRegex(), beforeStr)).reduce((f, s) -> s);
            if (matchPrefixRegexInSegment(beforeStr, match)) {
                int startToken = match.get().index;
                String tokenString = input.substring(duration.getStart(), duration.getEnd());
                Match matchDate = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), tokenString)).findFirst().orElse(null);
                Match matchTime = Arrays.stream(RegExpUtility.getMatches(config.getTimeUnitRegex(), tokenString)).findFirst().orElse(null);

                if (matchDate != null && matchTime == null) {
                    results.add(new Token(startToken, duration.getEnd()));
                    continue;
                }
            }

            // For cases like "next five days"
            match = Arrays.stream(RegExpUtility.getMatches(config.getFutureRegex(), beforeStr)).reduce((f, s) -> s);
            if (matchPrefixRegexInSegment(beforeStr, match)) {
                results.add(new Token(match.get().index, duration.getEnd()));
                continue;
            }

            // Match suffix
            match = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), afterStr)).findFirst();
            if (matchSuffixRegexInSegment(afterStr, match)) {
                results.add(new Token(duration.getStart(), duration.getEnd() + match.get().index + match.get().length));
                continue;
            }

            match = Arrays.stream(RegExpUtility.getMatches(config.getFutureRegex(), afterStr)).findFirst();
            if (matchSuffixRegexInSegment(afterStr, match)) {
                results.add(new Token(duration.getStart(), duration.getEnd() + match.get().index + match.get().length));
                continue;
            }

            match = Arrays.stream(RegExpUtility.getMatches(config.getFutureSuffixRegex(), afterStr)).findFirst();
            if (matchSuffixRegexInSegment(afterStr, match)) {
                results.add(new Token(duration.getStart(), duration.getEnd() + match.get().index + match.get().length));
                continue;
            }
        }

        return results;
    }

    // 1. Extract the month of date, week of date to a date range
    // 2. Extract cases like within two weeks from/before today/tomorrow/yesterday
    private List<Token> singleTimePointWithPatterns(String input, LocalDateTime reference) {
        List<Token> results = new ArrayList<>();

        List<ExtractResult> ers = config.getDatePointExtractor().extract(input, reference);
        if (ers.size() < 1) {
            return results;
        }

        for (ExtractResult er : ers) {
            if (er.start != null && er.length != null) {
                String beforeStr = input.substring(0, er.start);
                results.addAll(getTokenForRegexMatching(beforeStr, config.getWeekOfRegex(), er));
                results.addAll(getTokenForRegexMatching(beforeStr, config.getMonthOfRegex(), er));

                // Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
                if (isRelativeDurationDate(er)) {
                    results.addAll(getTokenForRegexMatching(beforeStr, config.getLessThanRegex(), er));
                    results.addAll(getTokenForRegexMatching(beforeStr, config.getMoreThanRegex(), er));

                    // For "within" case, only duration with relative to "today" or "now" makes sense
                    // Cases like "within 3 days from yesterday/tomorrow" does not make any sense
                    if (isDateRelativeToNowOrToday(er)) {
                        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getWithinNextPrefixRegex(), beforeStr)).findFirst();
                        if (match.isPresent()) {
                            boolean isNext = !StringUtility.isNullOrEmpty(match.get().getGroup(Constants.NextGroupName).value);

                            // For "within" case
                            // Cases like "within the next 5 days before today" is not acceptable
                            if (!(isNext && isAgoRelativeDurationDate(er))) {
                                results.addAll(getTokenForRegexMatching(beforeStr, config.getWithinNextPrefixRegex(), er));
                            }
                        }
                    }
                }
            }
        }

        return results;
    }

    private boolean isAgoRelativeDurationDate(ExtractResult er) {
        return Arrays.stream(RegExpUtility.getMatches(config.getAgoRegex(), er.text)).findAny().isPresent();
    }

    // Cases like "3 days from today", "2 weeks before yesterday", "3 months after
    // tomorrow"
    private boolean isRelativeDurationDate(ExtractResult er) {
        boolean isAgo = Arrays.stream(RegExpUtility.getMatches(config.getAgoRegex(), er.text)).findAny().isPresent();
        boolean isLater = Arrays.stream(RegExpUtility.getMatches(config.getLaterRegex(), er.text)).findAny().isPresent();

        return isAgo || isLater;
    }

    private List<Token> getTokenForRegexMatching(String source, Pattern regex, ExtractResult er) {
        List<Token> results = new ArrayList<>();
        Match match = Arrays.stream(RegExpUtility.getMatches(regex, source)).findFirst().orElse(null);
        if (match != null && source.trim().endsWith(match.value.trim())) {
            int startIndex = source.lastIndexOf(match.value);
            results.add(new Token(startIndex, er.start + er.length));
        }

        return results;
    }

    // Complex cases refer to the combination of daterange and datepoint
    // For Example: from|between {DateRange|DatePoint} to|till|and {DateRange|DatePoint}
    private List<Token> matchComplexCases(String input, List<ExtractResult> simpleCasesResults, LocalDateTime reference) {
        List<ExtractResult> ers = config.getDatePointExtractor().extract(input, reference);

        // Filter out DateRange results that are part of DatePoint results
        // For example, "Feb 1st 2018" => "Feb" and "2018" should be filtered out here
        List<ExtractResult> simpleErs = simpleCasesResults.stream().filter(simpleDateRange -> filterErs(simpleDateRange, ers)).collect(Collectors.toList());
        ers.addAll(simpleErs);

        List<ExtractResult> results = ers.stream().sorted((o1, o2) -> o1.start.compareTo(o2.start)).collect(Collectors.toList());

        return mergeMultipleExtractions(input, results);
    }

    private boolean filterErs(ExtractResult simpleDateRange, List<ExtractResult> ers) {
        return !ers.stream().anyMatch(datePoint -> compareErs(simpleDateRange, datePoint));
    }

    private boolean compareErs(ExtractResult simpleDateRange, ExtractResult datePoint) {
        return datePoint.start <= simpleDateRange.start && datePoint.start + datePoint.length >= simpleDateRange.start + simpleDateRange.length;
    }

    private List<Token> matchYearPeriod(String input, LocalDateTime reference) {
        List<Token> results = new ArrayList<>();
        Match[] matches = RegExpUtility.getMatches(config.getYearPeriodRegex(), input);
        for (Match match : matches) {
            Match matchYear = Arrays.stream(RegExpUtility.getMatches(config.getYearRegex(), match.value)).findFirst().orElse(null);
            if (matchYear != null && matchYear.length == match.value.length()) {
                int year = ((BaseDateExtractor)config.getDatePointExtractor()).getYearFromText(matchYear);
                if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                    continue;
                }
            }

            results.add(new Token(match.index, match.index + match.length));
        }

        return results;
    }

    private List<Token> matchOrdinalNumberWithCenturySuffix(String input, LocalDateTime reference) {
        List<Token> results = new ArrayList<>();
        List<ExtractResult> ers = config.getOrdinalExtractor().extract(input);

        for (ExtractResult er : ers) {
            if (er.start + er.length >= input.length()) {
                continue;
            }

            String afterStr = input.substring(er.start + er.length);
            String trimmedAfterStr = afterStr.trim();
            int whiteSpacesCount = afterStr.length() - trimmedAfterStr.length();
            int afterStringOffset = er.start + er.length + whiteSpacesCount;

            Match match = Arrays.stream(RegExpUtility.getMatches(config.getCenturySuffixRegex(), trimmedAfterStr)).findFirst().orElse(null);

            if (match != null) {
                results.add(new Token(er.start, afterStringOffset + match.index + match.length));
            }
        }

        return results;
    }

    private boolean matchSuffixRegexInSegment(String input, Optional<Match> match) {
        return match.isPresent() && StringUtility.isNullOrWhiteSpace(input.substring(0, match.get().index));
    }

    private boolean matchPrefixRegexInSegment(String input, Optional<Match> match) {
        return match.isPresent() && StringUtility.isNullOrWhiteSpace(input.substring(match.get().index + match.get().length));
    }

    private boolean isDateRelativeToNowOrToday(ExtractResult input) {
        for (String flagWord : config.getDurationDateRestrictions()) {
            if (input.text.contains(flagWord)) {
                return true;
            }
        }

        return false;
    }
}
