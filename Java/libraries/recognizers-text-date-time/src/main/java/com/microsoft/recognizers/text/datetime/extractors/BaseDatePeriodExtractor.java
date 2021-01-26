package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.Metadata;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
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
        List<ExtractResult> ordinalExtractions = config.getOrdinalExtractor().extract(input);

        tokens.addAll(mergeTwoTimePoints(input, reference));
        tokens.addAll(matchDuration(input, reference));
        tokens.addAll(singleTimePointWithPatterns(input, ordinalExtractions, reference));
        tokens.addAll(matchComplexCases(input, simpleCasesResults, reference));
        tokens.addAll(matchYearPeriod(input, reference));
        tokens.addAll(matchOrdinalNumberWithCenturySuffix(input, ordinalExtractions));

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

                // handle single year which is surrounded by '-' at both sides, e.g., a single year falls in a GUID
                if (match.length == Constants.FourDigitsYearLength &&
                        RegExpUtility.getMatches(this.config.getYearRegex(), match.value).length > 0 &&
                        infixBoundaryCheck(match, input)) {
                    String subStr = input.substring(match.index - 1, match.index - 1 + 6);
                    if (RegExpUtility.getMatches(this.config.getIllegalYearRegex(), subStr).length > 0) {
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

        // Handle "now"
        Match[] matches = RegExpUtility.getMatches(this.config.getNowRegex(), input);
        if (matches.length != 0) {
            for (Match match : matches) {
                ers.add(new ExtractResult(match.index, match.length, match.value, Constants.SYS_DATETIME_DATE));
            }

            ers.sort(Comparator.comparingInt(arg -> arg.getStart()));
        }
        
        return mergeMultipleExtractions(input, ers);
    }

    private List<Token> mergeMultipleExtractions(String input, List<ExtractResult> extractionResults) {
        List<Token> results = new ArrayList<>();

        Metadata metadata = new Metadata() {
            {
                setPossiblyIncludePeriodEnd(true);
            }
        };

        if (extractionResults.size() <= 1) {
            return results;
        }

        int idx = 0;

        while (idx < extractionResults.size() - 1) {
            ExtractResult thisResult = extractionResults.get(idx);
            ExtractResult nextResult = extractionResults.get(idx + 1);

            int middleBegin = thisResult.getStart() + thisResult.getLength();
            int middleEnd = nextResult.getStart();
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }

            String middleStr = input.substring(middleBegin, middleEnd).trim().toLowerCase();

            if (RegexExtension.isExactMatch(config.getTillRegex(), middleStr, true)) {
                int periodBegin = thisResult.getStart();
                int periodEnd = nextResult.getStart() + nextResult.getLength();

                // handle "from/between" together with till words (till/until/through...)
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase();

                ResultIndex fromIndex = config.getFromTokenIndex(beforeStr);
                ResultIndex betweenIndex = config.getBetweenTokenIndex(beforeStr);

                if (fromIndex.getResult()) {
                    periodBegin = fromIndex.getIndex();
                } else if (betweenIndex.getResult()) {
                    periodBegin = betweenIndex.getIndex();
                }

                results.add(new Token(periodBegin, periodEnd, metadata));

                // merge two tokens here, increase the index by two
                idx += 2;
                continue;
            }

            boolean hasConnectorToken = config.hasConnectorToken(middleStr);
            if (hasConnectorToken) {
                int periodBegin = thisResult.getStart();
                int periodEnd = nextResult.getStart() + nextResult.getLength();

                // handle "between...and..." case
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase();

                ResultIndex beforeIndex = config.getBetweenTokenIndex(beforeStr);

                if (beforeIndex.getResult()) {
                    periodBegin = beforeIndex.getIndex();
                    results.add(new Token(periodBegin, periodEnd, metadata));

                    // merge two tokens here, increase the index by two
                    idx += 2;
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
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), durationExtraction.getText())).findFirst();
            if (match.isPresent()) {
                durations.add(new Token(durationExtraction.getStart(), durationExtraction.getStart() + durationExtraction.getLength()));
            }
        }

        for (Token duration : durations) {
            String beforeStr = input.substring(0, duration.getStart()).toLowerCase();
            String afterStr = input.substring(duration.getStart() + duration.getLength()).toLowerCase();

            if (StringUtility.isNullOrWhiteSpace(beforeStr) && StringUtility.isNullOrWhiteSpace(afterStr)) {
                continue;
            }

            // within "Days/Weeks/Months/Years" should be handled as dateRange here
            // if duration contains "Seconds/Minutes/Hours", it should be treated as datetimeRange
            ConditionalMatch match = RegexExtension.matchEnd(config.getWithinNextPrefixRegex(), beforeStr, true);

            if (match.getSuccess()) {
                int startToken = match.getMatch().get().index;
                String tokenString = input.substring(duration.getStart(), duration.getEnd());
                Match matchDate = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), tokenString)).findFirst().orElse(null);
                Match matchTime = Arrays.stream(RegExpUtility.getMatches(config.getTimeUnitRegex(), tokenString)).findFirst().orElse(null);

                if (matchDate != null && matchTime == null) {
                    results.add(new Token(startToken, duration.getEnd()));
                    continue;
                }
            }

            // Match prefix
            match = RegexExtension.matchEnd(config.getPastRegex(), beforeStr, true);

            int index = -1;

            if (match.getSuccess()) {
                index = match.getMatch().get().index;
            }

            if (index < 0) {
                // For cases like "next five days"
                match = RegexExtension.matchEnd(config.getFutureRegex(), beforeStr, true);

                if (match.getSuccess()) {
                    index = match.getMatch().get().index;
                }
            }

            if (index >= 0) {
                String prefix = beforeStr.substring(0, index).trim();
                String durationText = input.substring(duration.getStart(), duration.getStart() + duration.getLength());
                List<ExtractResult> numbersInPrefix = config.getCardinalExtractor().extract(prefix);
                List<ExtractResult> numbersInDuration = config.getCardinalExtractor().extract(durationText);

                // Cases like "2 upcoming days", should be supported here
                // Cases like "2 upcoming 3 days" is invalid, only extract "upcoming 3 days" by default
                if (!numbersInPrefix.isEmpty() && numbersInDuration.isEmpty()) {
                    ExtractResult lastNumber = numbersInPrefix.stream()
                            .sorted(Comparator.comparingInt(x -> x.getStart() + x.getLength()))
                            .reduce((acc, item) -> item).orElse(null);

                    // Prefix should ends with the last number
                    if (lastNumber.getStart() + lastNumber.getLength() == prefix.length()) {
                        results.add(new Token(lastNumber.getStart(), duration.getEnd()));
                    }

                } else {
                    results.add(new Token(index, duration.getEnd()));
                }

                continue;
            }

            // Match suffix
            match = RegexExtension.matchBegin(config.getPastRegex(), afterStr, true);
            if (match.getSuccess()) {
                int matchLength = match.getMatch().get().index + match.getMatch().get().length;
                results.add(new Token(duration.getStart(), duration.getEnd() + matchLength));
                continue;
            }

            match = RegexExtension.matchBegin(config.getFutureSuffixRegex(), afterStr, true);
            if (match.getSuccess()) {
                int matchLength = match.getMatch().get().index + match.getMatch().get().length;
                results.add(new Token(duration.getStart(), duration.getEnd() + matchLength));
            }
        }

        return results;
    }

    // 1. Extract the month of date, week of date to a date range
    // 2. Extract cases like within two weeks from/before today/tomorrow/yesterday
    private List<Token> singleTimePointWithPatterns(String input, List<ExtractResult> ordinalExtractions, LocalDateTime reference) {
        List<Token> results = new ArrayList<>();

        List<ExtractResult> datePoints = config.getDatePointExtractor().extract(input, reference);

        // For cases like "week of the 18th"
        datePoints.addAll(ordinalExtractions.stream().filter(o -> datePoints.stream().noneMatch(er -> er.isOverlap(o))).collect(Collectors.toList()));

        if (datePoints.size() < 1) {
            return results;
        }

        for (ExtractResult er : datePoints) {
            if (er.getStart() != null && er.getLength() != null) {
                String beforeStr = input.substring(0, er.getStart());
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
        return Arrays.stream(RegExpUtility.getMatches(config.getAgoRegex(), er.getText())).findAny().isPresent();
    }

    // Cases like "3 days from today", "2 weeks before yesterday", "3 months after
    // tomorrow"
    private boolean isRelativeDurationDate(ExtractResult er) {
        boolean isAgo = Arrays.stream(RegExpUtility.getMatches(config.getAgoRegex(), er.getText())).findAny().isPresent();
        boolean isLater = Arrays.stream(RegExpUtility.getMatches(config.getLaterRegex(), er.getText())).findAny().isPresent();

        return isAgo || isLater;
    }

    private List<Token> getTokenForRegexMatching(String source, Pattern regex, ExtractResult er) {
        List<Token> results = new ArrayList<>();
        Match match = Arrays.stream(RegExpUtility.getMatches(regex, source)).findFirst().orElse(null);
        if (match != null && source.trim().endsWith(match.value.trim())) {
            int startIndex = source.lastIndexOf(match.value);
            results.add(new Token(startIndex, er.getStart() + er.getLength()));
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

        List<ExtractResult> results = ers.stream().sorted((o1, o2) -> o1.getStart().compareTo(o2.getStart())).collect(Collectors.toList());

        return mergeMultipleExtractions(input, results);
    }

    private boolean filterErs(ExtractResult simpleDateRange, List<ExtractResult> ers) {
        return !ers.stream().anyMatch(datePoint -> compareErs(simpleDateRange, datePoint));
    }

    private boolean compareErs(ExtractResult simpleDateRange, ExtractResult datePoint) {
        return datePoint.getStart() <= simpleDateRange.getStart() && datePoint.getStart() + datePoint.getLength() >= simpleDateRange.getStart() + simpleDateRange.getLength();
    }

    private List<Token> matchYearPeriod(String input, LocalDateTime reference) {
        List<Token> results = new ArrayList<>();
        Metadata metadata = new Metadata() {
            {
                setPossiblyIncludePeriodEnd(true);
            }
        };

        Match[] matches = RegExpUtility.getMatches(config.getYearPeriodRegex(), input);
        for (Match match : matches) {
            Match matchYear = Arrays.stream(RegExpUtility.getMatches(config.getYearRegex(), match.value)).findFirst().orElse(null);
            if (matchYear != null && matchYear.length == match.value.length()) {
                int year = ((BaseDateExtractor)config.getDatePointExtractor()).getYearFromText(matchYear);
                if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                    continue;
                }
                // Possibly include period end only apply for cases like "2014-2018", which are not single year cases
                metadata.setPossiblyIncludePeriodEnd(false);
            } else {
                Match[] yearMatches = RegExpUtility.getMatches(config.getYearRegex(), match.value);
                boolean isValidYear = true;
                for (Match yearMatch : yearMatches) {
                    int year = ((BaseDateExtractor)config.getDatePointExtractor()).getYearFromText(yearMatch);
                    if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                        isValidYear = false;
                        break;
                    }
                }

                if (!isValidYear) {
                    continue;
                }

            }

            results.add(new Token(match.index, match.index + match.length, metadata));
        }

        return results;
    }

    private List<Token> matchOrdinalNumberWithCenturySuffix(String input, List<ExtractResult>  ordinalExtractions) {
        List<Token> results = new ArrayList<>();

        for (ExtractResult er : ordinalExtractions) {
            if (er.getStart() + er.getLength() >= input.length()) {
                continue;
            }

            String afterStr = input.substring(er.getStart() + er.getLength());
            String trimmedAfterStr = afterStr.trim();
            int whiteSpacesCount = afterStr.length() - trimmedAfterStr.length();
            int afterStringOffset = er.getStart() + er.getLength() + whiteSpacesCount;

            Match match = Arrays.stream(RegExpUtility.getMatches(config.getCenturySuffixRegex(), trimmedAfterStr)).findFirst().orElse(null);

            if (match != null) {
                results.add(new Token(er.getStart(), afterStringOffset + match.index + match.length));
            }
        }

        return results;
    }

    private boolean isDateRelativeToNowOrToday(ExtractResult input) {
        for (String flagWord : config.getDurationDateRestrictions()) {
            if (input.getText().contains(flagWord)) {
                return true;
            }
        }

        return false;
    }

    // check whether the match is an infix of source
    private boolean infixBoundaryCheck(Match match, String source) {
        boolean isMatchInfixOfSource = false;
        if (match.index > 0 && match.index + match.length < source.length()) {
            if (source.substring(match.index, match.index + match.length).equals(match.value)) {
                isMatchInfixOfSource = true;
            }
        }

        return isMatchInfixOfSource;
    }
}
