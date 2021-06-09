package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.AgoLaterUtil;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

import org.javatuples.Pair;

public class BaseDateExtractor extends AbstractYearExtractor implements IDateTimeExtractor {

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DATE;
    }

    public BaseDateExtractor(IDateExtractorConfiguration config) {
        super(config);
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        tokens.addAll(basicRegexMatch(input));
        tokens.addAll(implicitDate(input));
        tokens.addAll(numberWithMonth(input, reference));
        tokens.addAll(extractRelativeDurationDate(input, reference));

        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    // match basic patterns in DateRegexList
    private Collection<Token> basicRegexMatch(String text) {
        List<Token> result = new ArrayList<>();

        for (Pattern regex : config.getDateRegexList()) {
            Match[] matches = RegExpUtility.getMatches(regex, text);

            for (Match match : matches) {
                // some match might be part of the date range entity, and might be splitted in a wrong way

                if (validateMatch(match, text)) {
                    // Cases that the relative term is before the detected date entity, like "this 5/12", "next friday 5/12"
                    String preText = text.substring(0, match.index);
                    ConditionalMatch relativeRegex = RegexExtension.matchEnd(config.getStrictRelativeRegex(), preText, true);
                    if (relativeRegex.getSuccess()) {
                        result.add(new Token(relativeRegex.getMatch().get().index, match.index + match.length));
                    } else {
                        result.add(new Token(match.index, match.index + match.length));
                    }
                }
            }
        }

        return result;
    }

    // this method is to validate whether the match is part of date range and is a correct split
    // For example: in case "10-1 - 11-7", "10-1 - 11" can be matched by some of the Regexes,
    // but the full text is a date range, so "10-1 - 11" is not a correct split
    private boolean validateMatch(Match match, String text) {
        // If the match doesn't contains "year" part, it will not be ambiguous and it's a valid match
        boolean isValidMatch = StringUtility.isNullOrEmpty(match.getGroup("year").value);

        if (!isValidMatch) {
            MatchGroup yearGroup = match.getGroup("year");

            // If the "year" part is not at the end of the match, it's a valid match
            if (yearGroup.index + yearGroup.length != match.index + match.length) {
                isValidMatch = true;
            } else {
                String subText = text.substring(yearGroup.index);

                // If the following text (include the "year" part) doesn't start with a Date entity, it's a valid match
                if (!startsWithBasicDate(subText)) {
                    isValidMatch = true;
                } else {
                    // If the following text (include the "year" part) starts with a Date entity,
                    // but the following text (doesn't include the "year" part) also starts with a valid Date entity,
                    // the current match is still valid
                    // For example, "10-1-2018-10-2-2018". Match "10-1-2018" is valid because though "2018-10-2" a valid match
                    // (indicates the first year "2018" might belongs to the second Date entity), but "10-2-2018" is also a valid match.
                    subText = text.substring(yearGroup.index + yearGroup.length).trim();
                    subText = trimStartRangeConnectorSymbols(subText);
                    isValidMatch = startsWithBasicDate(subText);
                }
            }
            
            // Expressions with mixed separators are not considered valid dates e.g. "30/4.85" (unless one is a comma "30/4, 2016")
            MatchGroup dayGroup = match.getGroup("day");
            MatchGroup monthGroup = match.getGroup("month");
            if (!StringUtility.isNullOrEmpty(dayGroup.value) && !StringUtility.isNullOrEmpty(monthGroup.value)) {
                String noDateText = match.value.replace(yearGroup.value, "")
                    .replace(monthGroup.value, "").replace(dayGroup.value, "");
                String[] separators = {"/", "\\", "-", "."};
                int separatorCount = 0;
                for (String separator : separators) {
                    if (noDateText.contains(separator)) {
                        separatorCount++;
                    }
                    if (separatorCount > 1) {
                        isValidMatch = false;
                        break;
                    }
                }
            }
        }

        return isValidMatch;
    }

    // TODO: Simplify this method to improve the performance
    private String trimStartRangeConnectorSymbols(String text) {
        Match[] rangeConnectorSymbolMatches = RegExpUtility.getMatches(config.getRangeConnectorSymbolRegex(), text);

        for (Match symbolMatch : rangeConnectorSymbolMatches) {
            int startSymbolLength = -1;

            if (symbolMatch.value != "" && symbolMatch.index == 0 && symbolMatch.length > startSymbolLength) {
                startSymbolLength = symbolMatch.length;
            }

            if (startSymbolLength > 0) {
                text = text.substring(startSymbolLength);
            }
        }

        return text.trim();
    }

    // TODO: Simplify this method to improve the performance
    private boolean startsWithBasicDate(String text) {
        for (Pattern regex : config.getDateRegexList()) {
            ConditionalMatch match = RegexExtension.matchBegin(regex, text, true);

            if (match.getSuccess()) {
                return true;
            }
        }

        return false;
    }

    // match several other cases
    // including 'today', 'the day after tomorrow', 'on 13'
    private Collection<Token> implicitDate(String text) {
        List<Token> result = new ArrayList<>();

        for (Pattern regex : config.getImplicitDateList()) {
            Match[] matches = RegExpUtility.getMatches(regex, text);

            for (Match match : matches) {
                result.add(new Token(match.index, match.index + match.length));
            }
        }

        return result;
    }

    // Check every integers and ordinal number for date
    private Collection<Token> numberWithMonth(String text, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        List<ExtractResult> ers = config.getOrdinalExtractor().extract(text);
        ers.addAll(config.getIntegerExtractor().extract(text));

        for (ExtractResult result : ers) {
            int num;
            try {
                ParseResult parseResult = config.getNumberParser().parse(result);
                num = Float.valueOf(parseResult.getValue().toString()).intValue();
            } catch (NumberFormatException e) {
                num = 0;
            }

            if (num < 1 || num > 31) {
                continue;
            }

            if (result.getStart() >= 0) {
                // Handling cases like '(Monday,) Jan twenty two'
                String frontStr = text.substring(0, result.getStart());

                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getMonthEnd(), frontStr)).findFirst();
                if (match.isPresent()) {
                    int startIndex = match.get().index;
                    int endIndex = match.get().index + match.get().length + result.getLength();

                    int month = config.getMonthOfYear().getOrDefault(match.get().getGroup("month").value.toLowerCase(), reference.getMonthValue());

                    Pair<Integer, Integer> startEnd = extendWithWeekdayAndYear(startIndex, endIndex, month, num, text, reference);

                    tokens.add(new Token(startEnd.getValue0(), startEnd.getValue1()));
                    continue;
                }

                // Handling cases like 'for the 25th'
                Match[] matches = RegExpUtility.getMatches(config.getForTheRegex(), text);
                boolean isFound = false;

                for (Match matchCase : matches) {
                    if (matchCase != null) {
                        String ordinalNum = matchCase.getGroup("DayOfMonth").value;
                        if (ordinalNum.equals(result.getText())) {
                            int endLenght = 0;
                            if (!matchCase.getGroup("end").value.equals("")) {
                                endLenght = matchCase.getGroup("end").value.length();
                            }

                            tokens.add(new Token(matchCase.index, matchCase.index + matchCase.length - endLenght));
                            isFound = true;
                        }
                    }
                }

                if (isFound) {
                    continue;
                }

                // Handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                matches = RegExpUtility.getMatches(config.getWeekDayAndDayOfMonthRegex(), text);
                isFound = false;
                for (Match matchCase : matches) {
                    if (matchCase != null) {
                        String ordinalNum = matchCase.getGroup("DayOfMonth").value;
                        if (ordinalNum.equals(result.getText())) {
                            // Get week of day for the ordinal number which is regarded as a date of reference month
                            LocalDateTime date = DateUtil.safeCreateFromMinValue(reference.getYear(), reference.getMonthValue(), num);
                            String numWeekDayStr = date.getDayOfWeek().toString().toLowerCase();

                            // Get week day from text directly, compare it with the weekday generated above
                            // to see whether they refer to the same week day
                            String extractedWeekDayStr = matchCase.getGroup("weekday").value.toLowerCase();
                            int numWeekDay = config.getDayOfWeek().get(numWeekDayStr);
                            int extractedWeekDay = config.getDayOfWeek().get(extractedWeekDayStr);

                            if (date != DateUtil.minValue() && numWeekDay == extractedWeekDay) {
                                tokens.add(new Token(matchCase.index, result.getStart() + result.getLength()));
                                isFound = true;
                            }
                        }
                    }
                }

                if (isFound) {
                    continue;
                }

                // Handling cases like '20th of next month'
                String suffixStr = text.substring(result.getStart() + result.getLength());
                ConditionalMatch beginMatch = RegexExtension.matchBegin(config.getRelativeMonthRegex(), suffixStr.trim(), true);
                if (beginMatch.getSuccess() && beginMatch.getMatch().get().index == 0) {
                    int spaceLen = suffixStr.length() - suffixStr.trim().length();
                    int resStart = result.getStart();
                    int resEnd = resStart + result.getLength() + spaceLen + beginMatch.getMatch().get().length;

                    // Check if prefix contains 'the', include it if any
                    String prefix = text.substring(0, resStart);
                    Optional<Match> prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPrefixArticleRegex(), prefix)).findFirst();
                    if (prefixMatch.isPresent()) {
                        resStart = prefixMatch.get().index;
                    }

                    tokens.add(new Token(resStart, resEnd));
                }

                // Handling cases like 'second Sunday'
                suffixStr = text.substring(result.getStart() + result.getLength());
                beginMatch = RegexExtension.matchBegin(config.getWeekDayRegex(), suffixStr.trim(), true);
                if (beginMatch.getSuccess() && num >= 1 && num <= 5 && result.getType().equals("builtin.num.ordinal")) {
                    String weekDayStr = beginMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
                    if (config.getDayOfWeek().containsKey(weekDayStr)) {
                        int spaceLen = suffixStr.length() - suffixStr.trim().length();
                        tokens.add(new Token(result.getStart(), result.getStart() + result.getLength() + spaceLen + beginMatch.getMatch().get().length));
                    }
                }
            }

            // For cases like "I'll go back twenty second of June"
            if (result.getStart() + result.getLength() < text.length()) {
                String afterStr = text.substring(result.getStart() + result.getLength());

                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getOfMonth(), afterStr)).findFirst();
                if (match.isPresent()) {
                    int startIndex = result.getStart();
                    int endIndex = result.getStart() + result.getLength() + match.get().length;

                    int month = config.getMonthOfYear().getOrDefault(match.get().getGroup("month").value.toLowerCase(), reference.getMonthValue());

                    Pair<Integer, Integer> startEnd = extendWithWeekdayAndYear(startIndex, endIndex, month, num, text, reference);
                    tokens.add(new Token(startEnd.getValue0(), startEnd.getValue1()));
                }
            }
        }

        return tokens;
    }

    private Pair<Integer, Integer> extendWithWeekdayAndYear(int startIndex, int endIndex, int month, int day, String text, LocalDateTime reference) {
        int year = reference.getYear();
        int startIndexResult = startIndex;
        int endIndexResult = endIndex;

        // Check whether there's a year
        String suffix = text.substring(endIndexResult);
        Optional<Match> matchYear = Arrays.stream(RegExpUtility.getMatches(config.getYearSuffix(), suffix)).findFirst();

        if (matchYear.isPresent() && matchYear.get().index == 0) {
            year = getYearFromText(matchYear.get());

            if (year >= Constants.MinYearNum && year <= Constants.MaxYearNum) {
                endIndexResult += matchYear.get().length;
            }
        }

        LocalDateTime date = DateUtil.safeCreateFromMinValue(year, month, day);

        // Check whether there's a weekday
        String prefix = text.substring(0, startIndexResult);
        Optional<Match> matchWeekDay = Arrays.stream(RegExpUtility.getMatches(config.getWeekDayEnd(), prefix)).findFirst();
        if (matchWeekDay.isPresent()) {
            // Get weekday from context directly, compare it with the weekday extraction above
            // to see whether they are referred to the same weekday
            String extractedWeekDayStr = matchWeekDay.get().getGroup("weekday").value.toLowerCase();
            String numWeekDayStr = date.getDayOfWeek().toString().toLowerCase();

            if (config.getDayOfWeek().containsKey(numWeekDayStr) && config.getDayOfWeek().containsKey(extractedWeekDayStr)) {
                int weekDay1 = config.getDayOfWeek().get(numWeekDayStr);
                int weekday2 = config.getDayOfWeek().get(extractedWeekDayStr);
                if (date != DateUtil.minValue() && weekDay1 == weekday2) {
                    startIndexResult = matchWeekDay.get().index;
                }

            }
        }

        return new Pair<>(startIndexResult, endIndexResult);
    }

    // Cases like "3 days from today", "5 weeks before yesterday", "2 months after tomorrow"
    // Note that these cases are of type "date"
    private Collection<Token> extractRelativeDurationDate(String text, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        List<ExtractResult> durations = config.getDurationExtractor().extract(text, reference);

        for (ExtractResult duration : durations) {
            // if it is a multiple duration but its type is not equal to Date, skip it here
            if (isMultipleDuration(duration) && !isMultipleDurationDate(duration)) {
                continue;
            }

            // Some types of duration can be compounded with "before", "after" or "from" suffix to create a "date"
            // While some other types of durations, when compounded with such suffix, it will not create a "date", but create a "dateperiod"
            // For example, durations like "3 days", "2 weeks", "1 week and 2 days", can be compounded with such suffix to create a "date"
            // But "more than 3 days", "less than 2 weeks", when compounded with such suffix, it will become cases
            // like "more than 3 days from today" which is a "dateperiod", not a "date"
            // As this parent method is aimed to extract RelativeDurationDate, so for cases with "more than" or "less than",
            // we remove the prefix so as to extract the expected RelativeDurationDate
            if (isInequalityDuration(duration)) {
                duration = stripInequalityDuration(duration);
            }

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), duration.getText())).findFirst();

            if (match.isPresent()) {
                tokens = AgoLaterUtil.extractorDurationWithBeforeAndAfter(text, duration, tokens, config.getUtilityConfiguration());
            }

        }

        // Extract cases like "in 3 weeks", which equals to "3 weeks from today"
        List<Token> relativeDurationDateWithInPrefix = extractRelativeDurationDateWithInPrefix(text, durations, reference);

        // For cases like "in 3 weeks from today", we should choose "3 weeks from today" as the extract result rather than "in 3 weeks" or "in 3 weeks from today"
        for (Token erWithInPrefix : relativeDurationDateWithInPrefix) {
            if (!isOverlapWithExistExtractions(erWithInPrefix, tokens)) {
                tokens.add(erWithInPrefix);
            }
        }

        return tokens;
    }

    public boolean isOverlapWithExistExtractions(Token er, List<Token> existErs) {
        for (Token existEr : existErs) {
            if (er.getStart() < existEr.getEnd() && er.getEnd() > existEr.getStart()) {
                return true;
            }
        }

        return false;
    }

    // "In 3 days/weeks/months/years" = "3 days/weeks/months/years from now"
    public List<Token> extractRelativeDurationDateWithInPrefix(String text, List<ExtractResult> durationEr, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        List<Token> durations = new ArrayList<>();

        for (ExtractResult durationExtraction : durationEr) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateUnitRegex(), durationExtraction.getText())).findFirst();
            if (match.isPresent()) {
                int start = durationExtraction.getStart() != null ? durationExtraction.getStart() : 0;
                int end = start + (durationExtraction.getLength() != null ? durationExtraction.getLength() : 0);
                durations.add(new Token(start, end));
            }
        }

        for (Token duration : durations) {
            String beforeStr = text.substring(0, duration.getStart()).toLowerCase();
            String afterStr = text.substring(duration.getStart() + duration.getLength()).toLowerCase();

            if (StringUtility.isNullOrWhiteSpace(beforeStr) && StringUtility.isNullOrWhiteSpace(afterStr)) {
                continue;
            }

            ConditionalMatch match = RegexExtension.matchEnd(config.getInConnectorRegex(), beforeStr, true);

            if (match.getSuccess() && match.getMatch().isPresent()) {
                int startToken = match.getMatch().get().index;
                Optional<Match> rangeUnitMatch = Arrays.stream(
                        RegExpUtility.getMatches(config.getRangeUnitRegex(),
                        text.substring(duration.getStart(),
                        duration.getStart() + duration.getLength()))).findFirst();

                if (rangeUnitMatch.isPresent()) {
                    tokens.add(new Token(startToken, duration.getEnd()));
                }
            }
        }

        return tokens;
    }

    private ExtractResult stripInequalityDuration(ExtractResult er) {
        ExtractResult result = er;
        result = stripInequalityPrefix(result, config.getMoreThanRegex());
        result = stripInequalityPrefix(result, config.getLessThanRegex());
        return result;
    }

    private ExtractResult stripInequalityPrefix(ExtractResult er, Pattern regex) {
        ExtractResult result = er;
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, er.getText())).findFirst();

        if (match.isPresent()) {
            int originalLength = er.getText().length();
            String text = er.getText().replace(match.get().value, "").trim();
            int start = er.getStart() + originalLength - text.length();
            int length = text.length();
            String data = "";
            result.setStart(start);
            result.setLength(length);
            result.setText(text);
            result.setData(data);
        }

        return result;
    }

    // Cases like "more than 3 days", "less than 4 weeks"
    private boolean isInequalityDuration(ExtractResult er) {
        return er.getData() != null && (er.getData().toString().equals(Constants.MORE_THAN_MOD) || er.getData().toString().equals(Constants.LESS_THAN_MOD));
    }

    private boolean isMultipleDurationDate(ExtractResult er) {
        return er.getData() != null && er.getData().toString().equals(Constants.MultipleDuration_Date);
    }

    private boolean isMultipleDuration(ExtractResult er) {
        return er.getData() != null && er.getData().toString().startsWith(Constants.MultipleDuration_Prefix);
    }
}
