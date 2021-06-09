package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.AgoLaterUtil;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateContext;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

public class BaseDateParser implements IDateTimeParser {

    private final IDateParserConfiguration config;

    public BaseDateParser(IDateParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_DATE;
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {
        return parse(extResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {

        LocalDateTime referenceDate = reference;

        Object value = null;

        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult = this.parseBasicRegexMatch(er.getText(), referenceDate);

            if (!innerResult.getSuccess()) {
                innerResult = this.parseImplicitDate(er.getText(), referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseWeekdayOfMonth(er.getText(), referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseDurationWithAgoAndLater(er.getText(), referenceDate);
            }

            // NumberWithMonth must be the second last one, because it only need to find a number and a month to get a "success"
            if (!innerResult.getSuccess()) {
                innerResult = this.parseNumberWithMonth(er.getText(), referenceDate);
            }

            // SingleNumber last one
            if (!innerResult.getSuccess()) {
                innerResult = this.parseSingleNumber(er.getText(), referenceDate);
            }

            if (innerResult.getSuccess()) {
                ImmutableMap.Builder<String, String> futureResolution = ImmutableMap.builder();
                futureResolution.put(TimeTypeConstants.DATE, DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getFutureValue()));

                innerResult.setFutureResolution(futureResolution.build());

                ImmutableMap.Builder<String, String> pastResolution = ImmutableMap.builder();
                pastResolution.put(TimeTypeConstants.DATE, DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getPastValue()));

                innerResult.setPastResolution(pastResolution.build());

                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(
                er.getStart(),
                er.getLength(),
                er.getText(),
                er.getType(),
                er.getData(),
                value,
                "",
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex());

        return ret;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }

    // parse basic patterns in DateRegexList
    private DateTimeResolutionResult parseBasicRegexMatch(String text, LocalDateTime referenceDate) {
        String trimmedText = text.trim();

        for (Pattern regex : this.config.getDateRegexes()) {
            int offset = 0;
            String relativeStr = null;
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, trimmedText)).findFirst();

            if (!match.isPresent()) {
                match = Arrays.stream(RegExpUtility.getMatches(regex, this.config.getDateTokenPrefix() + trimmedText)).findFirst();
                // Handing cases like "(this)? 5.12" which only be recognized in "on (this)? 5.12"
                if (match.isPresent()) {
                    offset = this.config.getDateTokenPrefix().length();
                    relativeStr = match.get().getGroup("order").value.toLowerCase();
                }
            }

            if (match.isPresent()) {
                ConditionalMatch relativeRegex = RegexExtension.matchEnd(config.getStrictRelativeRegex(), text.substring(0, match.get().index), true);
                boolean isContainRelative = relativeRegex.getSuccess() && match.get().index + match.get().length == trimmedText.length();
                if ((match.get().index == offset && match.get().length == trimmedText.length()) || isContainRelative) {
                    // Handing cases which contain relative term like "this 5/12"
                    if (match.get().index != offset) {
                        relativeStr = relativeRegex.getMatch().get().value;
                    }

                    // LUIS value string will be set in Match2Date method
                    DateTimeResolutionResult ret = this.match2Date(match, referenceDate, relativeStr);
                    return ret;
                }
            }
        }

        return new DateTimeResolutionResult();
    }

    // match several other cases
    // including 'today', 'the day after tomorrow', 'on 13'
    private DateTimeResolutionResult parseImplicitDate(String text, LocalDateTime referenceDate) {
        String trimmedText = text.trim();

        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        // handle "on 12"
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getOnRegex(), this.config.getDateTokenPrefix() + trimmedText)).findFirst();
        if (match.isPresent() && match.get().index == 3 && match.get().length == trimmedText.length()) {
            int month = referenceDate.getMonthValue();
            int year = referenceDate.getYear();
            String dayStr = match.get().getGroup("day").value.toLowerCase();
            int day = this.config.getDayOfMonth().get(dayStr);

            ret.setTimex(DateTimeFormatUtil.luisDate(-1, -1, day));

            LocalDateTime futureDate;
            LocalDateTime pastDate;
            String tryStr = DateTimeFormatUtil.luisDate(year, month, day);
            if (DateUtil.tryParse(tryStr) != null) {
                futureDate = DateUtil.safeCreateFromMinValue(year, month, day);
                pastDate = DateUtil.safeCreateFromMinValue(year, month, day);

                if (futureDate.isBefore(referenceDate)) {
                    futureDate = futureDate.plusMonths(1);
                }

                if (pastDate.isEqual(referenceDate) || pastDate.isAfter(referenceDate)) {
                    pastDate = pastDate.minusMonths(1);
                }
            } else {
                futureDate = DateUtil.safeCreateFromMinValue(year, month + 1, day);
                pastDate = DateUtil.safeCreateFromMinValue(year, month - 1, day);
            }

            ret.setFutureValue(futureDate);
            ret.setPastValue(pastDate);
            ret.setSuccess(true);

            return ret;
        }

        // handle "today", "the day before yesterday"
        ConditionalMatch exactMatch = RegexExtension.matchExact(this.config.getSpecialDayRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            int swift = getSwiftDay(exactMatch.getMatch().get().value);

            LocalDateTime value = referenceDate.toLocalDate().atStartOfDay().plusDays(swift);

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "two days from tomorrow"
        exactMatch = RegexExtension.matchExact(this.config.getSpecialDayWithNumRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {

            int swift = getSwiftDay(exactMatch.getMatch().get().getGroup("day").value);
            List<ExtractResult> numErs = this.config.getIntegerExtractor().extract(trimmedText);
            Object numberParsed = this.config.getNumberParser().parse(numErs.get(0)).getValue();
            int numOfDays = Math.round(((Double)numberParsed).floatValue());

            LocalDateTime value = referenceDate.plusDays(numOfDays + swift);

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "two sundays from now"
        exactMatch = RegexExtension.matchExact(this.config.getRelativeWeekDayRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            List<ExtractResult> numErs = this.config.getIntegerExtractor().extract(trimmedText);
            Object numberParsed = this.config.getNumberParser().parse(numErs.get(0)).getValue();
            int num = Math.round(((Double)numberParsed).floatValue());

            String weekdayStr = exactMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
            LocalDateTime value = referenceDate;

            // Check whether the determined day of this week has passed.
            if (value.getDayOfWeek().getValue() > this.config.getDayOfWeek().get(weekdayStr)) {
                num--;
            }

            while (num-- > 0) {
                value = DateUtil.next(value, this.config.getDayOfWeek().get(weekdayStr));
            }

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "next Sunday"
        exactMatch = RegexExtension.matchExact(this.config.getNextRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            String weekdayStr = exactMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
            LocalDateTime value = DateUtil.next(referenceDate, this.config.getDayOfWeek().get(weekdayStr));

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "this Friday"
        exactMatch = RegexExtension.matchExact(this.config.getThisRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            String weekdayStr = exactMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
            LocalDateTime value = DateUtil.thisDate(referenceDate, this.config.getDayOfWeek().get(weekdayStr));

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "last Friday", "last mon"
        exactMatch = RegexExtension.matchExact(this.config.getLastRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            String weekdayStr = exactMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
            LocalDateTime value = DateUtil.last(referenceDate, this.config.getDayOfWeek().get(weekdayStr));

            ret.setTimex(DateTimeFormatUtil.luisDate(value));
            ret.setFutureValue(value);
            ret.setPastValue(value);
            ret.setSuccess(true);

            return ret;
        }

        // handle "Friday"
        exactMatch = RegexExtension.matchExact(this.config.getWeekDayRegex(), trimmedText, true);

        if (exactMatch.getSuccess()) {
            String weekdayStr = exactMatch.getMatch().get().getGroup("weekday").value.toLowerCase();
            int weekDay = this.config.getDayOfWeek().get(weekdayStr);
            LocalDateTime value = DateUtil.thisDate(referenceDate, this.config.getDayOfWeek().get(weekdayStr));

            if (weekDay == 0) {
                weekDay = 7;
            }

            if (weekDay < referenceDate.getDayOfWeek().getValue()) {
                value = DateUtil.next(referenceDate, weekDay);
            }

            ret.setTimex("XXXX-WXX-" + weekDay);
            LocalDateTime futureDate = value;
            LocalDateTime pastDate = value;
            if (futureDate.isBefore(referenceDate)) {
                futureDate = futureDate.plusDays(7);
            }

            if (pastDate.isEqual(referenceDate) || pastDate.isAfter(referenceDate)) {
                pastDate = pastDate.minusDays(7);
            }

            ret.setFutureValue(futureDate);
            ret.setPastValue(pastDate);
            ret.setSuccess(true);

            return ret;
        }

        // handle "for the 27th."
        match = Arrays.stream(RegExpUtility.getMatches(this.config.getForTheRegex(), text)).findFirst();
        if (match.isPresent()) {
            int day;
            int month = referenceDate.getMonthValue();
            int year = referenceDate.getYear();
            String dayStr = match.get().getGroup("DayOfMonth").value.toLowerCase();

            int start = match.get().getGroup("DayOfMonth").index;
            int length = match.get().getGroup("DayOfMonth").length;

            // create a extract comments which content ordinal string of text
            ExtractResult er = new ExtractResult(start, length, dayStr, null, null);

            Object numberParsed = this.config.getNumberParser().parse(er).getValue();
            day = Math.round(((Double)numberParsed).floatValue());

            ret.setTimex(DateTimeFormatUtil.luisDate(-1, -1, day));

            LocalDateTime futureDate;
            String tryStr = DateTimeFormatUtil.luisDate(year, month, day);

            if (DateUtil.tryParse(tryStr) != null) {
                futureDate = DateUtil.safeCreateFromMinValue(year, month, day);
            } else {
                futureDate = DateUtil.safeCreateFromMinValue(year, month + 1, day);
            }

            ret.setFutureValue(futureDate);
            ret.setPastValue(ret.getFutureValue());
            ret.setSuccess(true);

            return ret;
        }

        // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
        match = Arrays.stream(RegExpUtility.getMatches(this.config.getWeekDayAndDayOfMonthRegex(), text)).findFirst();
        if (match.isPresent()) {
            int month = referenceDate.getMonthValue();
            int year = referenceDate.getYear();
            String dayStr = match.get().getGroup("DayOfMonth").value.toLowerCase();

            int start = match.get().getGroup("DayOfMonth").index;
            int length = match.get().getGroup("DayOfMonth").length;

            // create a extract comments which content ordinal string of text
            ExtractResult erTmp = new ExtractResult(start, length, dayStr, null, null);

            Object numberParsed = this.config.getNumberParser().parse(erTmp).getValue();
            int day = Math.round(((Double)numberParsed).floatValue());

            // the validity of the phrase is guaranteed in the Date Extractor
            ret.setTimex(DateTimeFormatUtil.luisDate(year, month, day));
            ret.setFutureValue(LocalDateTime.of(year, month, day, 0, 0));
            ret.setPastValue(LocalDateTime.of(year, month, day, 0, 0));
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }

    private DateTimeResolutionResult parseWeekdayOfMonth(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getWeekDayOfMonthRegex(), this.config.getDateTokenPrefix() + trimmedText)).findFirst();
        if (!match.isPresent()) {
            return ret;
        }

        String cardinalStr = match.get().getGroup("cardinal").value;
        String weekdayStr = match.get().getGroup("weekday").value;
        String monthStr = match.get().getGroup("month").value;
        Boolean noYear = false;
        int year;

        int cardinal = this.config.isCardinalLast(cardinalStr) ? 5 : this.config.getCardinalMap().get(cardinalStr);

        int weekday = this.config.getDayOfWeek().get(weekdayStr);
        int month;
        if (StringUtility.isNullOrEmpty(monthStr)) {
            int swift = this.config.getSwiftMonthOrYear(trimmedText);

            month = referenceDate.plusMonths(swift).getMonthValue();
            year = referenceDate.plusMonths(swift).getYear();
        } else {
            month = this.config.getMonthOfYear().get(monthStr);
            year = referenceDate.getYear();
            noYear = true;
        }

        LocalDateTime value = computeDate(cardinal, weekday, month, year);
        if (value.getMonthValue() != month) {
            cardinal -= 1;
            value = value.minusDays(7);
        }

        LocalDateTime futureDate = value;
        LocalDateTime pastDate = value;
        if (noYear && futureDate.isBefore(referenceDate)) {
            futureDate = computeDate(cardinal, weekday, month, year + 1);
            if (futureDate.getMonthValue() != month) {
                futureDate = futureDate.minusDays(7);
            }
        }

        if (noYear && (pastDate.isEqual(referenceDate) || pastDate.isAfter(referenceDate))) {
            pastDate = computeDate(cardinal, weekday, month, year - 1);
            if (pastDate.getMonthValue() != month) {
                pastDate = pastDate.minusDays(7);
            }
        }

        // here is a very special case, timeX followe future date
        ret.setTimex("XXXX-" + String.format("%02d", month) + "-WXX-" + weekday + "-#" + cardinal);
        ret.setFutureValue(futureDate);
        ret.setPastValue(pastDate);
        ret.setSuccess(true);

        return ret;
    }

    // Handle cases like "two days ago"
    private DateTimeResolutionResult parseDurationWithAgoAndLater(String text, LocalDateTime referenceDate) {

        return AgoLaterUtil.parseDurationWithAgoAndLater(text, referenceDate,
                config.getDurationExtractor(), config.getDurationParser(), config.getUnitMap(), config.getUnitRegex(),
                config.getUtilityConfiguration(), this::getSwiftDay);
    }

    // handle cases like "January first", "twenty-two of August"
    // handle cases like "20th of next month"
    private DateTimeResolutionResult parseNumberWithMonth(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();
        int month = 0;
        int day = 0;
        int year = referenceDate.getYear();
        Boolean ambiguous = true;

        List<ExtractResult> er = this.config.getOrdinalExtractor().extract(trimmedText);
        if (er.size() == 0) {
            er = this.config.getIntegerExtractor().extract(trimmedText);
        }

        if (er.size() == 0) {
            return ret;
        }

        Object numberParsed = this.config.getNumberParser().parse(er.get(0)).getValue();
        int num = Math.round(((Double)numberParsed).floatValue());

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getMonthRegex(), trimmedText)).findFirst();
        if (match.isPresent()) {
            month = this.config.getMonthOfYear().get(match.get().value.trim());
            day = num;

            String suffix = trimmedText.substring((er.get(0).getStart() + er.get(0).getLength()));

            Optional<Match> matchYear = Arrays.stream(RegExpUtility.getMatches(this.config.getYearSuffix(), suffix)).findFirst();
            if (matchYear.isPresent()) {
                year = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(matchYear.get());
                if (year != Constants.InvalidYear) {
                    ambiguous = false;
                }
            }
        }

        // handling relatived month
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(this.config.getRelativeMonthRegex(), trimmedText)).findFirst();
            if (match.isPresent()) {
                String monthStr = match.get().getGroup("order").value;
                int swift = this.config.getSwiftMonthOrYear(monthStr);
                month = referenceDate.plusMonths(swift).getMonthValue();
                year = referenceDate.plusMonths(swift).getYear();
                day = num;
                ambiguous = false;
            }
        }

        // handling casesd like 'second Sunday'
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(this.config.getWeekDayRegex(), trimmedText)).findFirst();
            if (match.isPresent()) {
                month = referenceDate.getMonthValue();
                // resolve the date of wanted week day
                int wantedWeekDay = this.config.getDayOfWeek().get(match.get().getGroup("weekday").value);
                LocalDateTime firstDate = DateUtil.safeCreateFromMinValue(referenceDate.getYear(), referenceDate.getMonthValue(), 1);
                int firstWeekDay = firstDate.getDayOfWeek().getValue();
                LocalDateTime firstWantedWeekDay = firstDate.plusDays(wantedWeekDay > firstWeekDay ? wantedWeekDay - firstWeekDay : wantedWeekDay - firstWeekDay + 7);
                int answerDay = firstWantedWeekDay.getDayOfMonth() + (num - 1) * 7;
                day = answerDay;
                ambiguous = false;
            }
        }

        if (!match.isPresent()) {
            return ret;
        }

        // for LUIS format value string
        LocalDateTime futureDate = DateUtil.safeCreateFromMinValue(year, month, day);
        LocalDateTime pastDate = DateUtil.safeCreateFromMinValue(year, month, day);

        if (ambiguous) {
            ret.setTimex(DateTimeFormatUtil.luisDate(-1, month, day));
            if (futureDate.isBefore(referenceDate)) {
                futureDate = futureDate.plusYears(1);
            }

            if (pastDate.isEqual(referenceDate) || pastDate.isAfter(referenceDate)) {
                pastDate = pastDate.minusYears(1);
            }
        } else {
            ret.setTimex(DateTimeFormatUtil.luisDate(year, month, day));
        }

        ret.setFutureValue(futureDate);
        ret.setPastValue(pastDate);
        ret.setSuccess(true);

        return ret;
    }

    // handle cases like "the 27th". In the extractor, only the unmatched weekday and date will output this date.
    private DateTimeResolutionResult parseSingleNumber(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();
        int day = 0;

        List<ExtractResult> er = this.config.getOrdinalExtractor().extract(trimmedText);
        if (er.size() == 0) {
            er = this.config.getIntegerExtractor().extract(trimmedText);
        }

        if (er.size() == 0) {
            return ret;
        }

        Object numberParsed = this.config.getNumberParser().parse(er.get(0)).getValue();
        day = Math.round(((Double)numberParsed).floatValue());

        int month = referenceDate.getMonthValue();
        int year = referenceDate.getYear();

        // for LUIS format value string
        ret.setTimex(DateTimeFormatUtil.luisDate(-1, -1, day));
        LocalDateTime pastDate = DateUtil.safeCreateFromMinValue(year, month, day);
        LocalDateTime futureDate = DateUtil.safeCreateFromMinValue(year, month, day);

        if (!futureDate.isEqual(LocalDateTime.MIN) && futureDate.isBefore(referenceDate)) {
            futureDate = futureDate.plusMonths(1);
        }

        if (!pastDate.isEqual(LocalDateTime.MIN) && (pastDate.isEqual(referenceDate) || pastDate.isAfter(referenceDate))) {
            pastDate = pastDate.minusMonths(1);
        }

        ret.setFutureValue(futureDate);
        ret.setPastValue(pastDate);
        ret.setSuccess(true);

        return ret;
    }

    private static LocalDateTime computeDate(int cardinal, int weekday, int month, int year) {
        LocalDateTime firstDay = DateUtil.safeCreateFromMinValue(year, month, 1);
        LocalDateTime firstWeekday = DateUtil.thisDate(firstDay, weekday);
        int dayOfWeekOfFirstDay = firstDay.getDayOfWeek().getValue();

        if (weekday == 0) {
            weekday = 7;
        }

        if (dayOfWeekOfFirstDay == 0) {
            dayOfWeekOfFirstDay = 7;
        }

        if (weekday < dayOfWeekOfFirstDay) {
            firstWeekday = DateUtil.next(firstDay, weekday);
        }

        return firstWeekday.plusDays(7 * (cardinal - 1));
    }

    // parse a regex match which includes 'day', 'month' and 'year' (optional) group
    private DateTimeResolutionResult match2Date(Optional<Match> match, LocalDateTime referenceDate, String relativeStr) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        int month = 0;
        int day = 0;
        int year = 0;

        String monthStr = match.get().getGroup("month").value.toLowerCase();
        String dayStr = match.get().getGroup("day").value.toLowerCase();
        String weekdayStr = match.get().getGroup("weekday").value.toLowerCase();
        String yearStr = match.get().getGroup("year").value.toLowerCase();
        String writtenYearStr = match.get().getGroup("fullyear").value.toLowerCase();

        if (this.config.getMonthOfYear().containsKey(monthStr) && this.config.getDayOfMonth().containsKey(dayStr)) {

            month = this.config.getMonthOfYear().get(monthStr);
            day = this.config.getDayOfMonth().get(dayStr);

            if (!StringUtility.isNullOrEmpty(yearStr)) {

                year = this.config.getDateExtractor().getYearFromText(match.get());

            } else if (!StringUtility.isNullOrEmpty(yearStr)) {

                year = Integer.parseInt(yearStr);
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) {
                    year += 1900;
                } else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) {
                    year += 2000;
                }
            }
        }

        Boolean noYear = false;
        if (year == 0) {
            year = referenceDate.getYear();
            if (!StringUtility.isNullOrEmpty(relativeStr)) {
                int swift = config.getSwiftMonthOrYear(relativeStr);

                // @TODO Improve handling of next/last in particular cases "next friday 5/12" when the next friday is not 5/12.
                if (!StringUtility.isNullOrEmpty(weekdayStr)) {
                    swift = 0;
                }
                year += swift;
            } else {
                noYear = true;
            }

            ret.setTimex(DateTimeFormatUtil.luisDate(-1, month, day));
        } else {
            ret.setTimex(DateTimeFormatUtil.luisDate(year, month, day));
        }

        HashMap<String, LocalDateTime> futurePastDates = DateContext.generateDates(noYear, referenceDate, year, month, day);
        LocalDateTime futureDate = futurePastDates.get(Constants.FutureDate);
        LocalDateTime pastDate = futurePastDates.get(Constants.PastDate);

        ret.setFutureValue(futureDate);
        ret.setPastValue(pastDate);
        ret.setSuccess(true);

        return ret;
    }

    private int getSwiftDay(String text) {
        String trimmedText = this.config.normalize(text.trim().toLowerCase());
        int swift = 0;

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getRelativeDayRegex(), text)).findFirst();

        // The sequence here is important
        // As suffix "day before yesterday" should be matched before suffix "day before" or "yesterday"
        if (config.getSameDayTerms().contains(trimmedText)) {
            swift = 0;
        } else if (endsWithTerms(trimmedText, config.getPlusTwoDayTerms())) {
            swift = 2;
        } else if (endsWithTerms(trimmedText, config.getMinusTwoDayTerms())) {
            swift = -2;
        } else if (endsWithTerms(trimmedText, config.getPlusOneDayTerms())) {
            swift = 1;
        } else if (endsWithTerms(trimmedText, config.getMinusOneDayTerms())) {
            swift = -1;
        } else if (match.isPresent()) {
            swift = getSwift(text);
        }

        return swift;
    }

    private int getSwift(String text) {
        String trimmedText = text.trim().toLowerCase();

        int swift = 0;
        if (RegExpUtility.getMatches(this.config.getNextPrefixRegex(), trimmedText).length > 0) {
            swift = 1;
        } else if (RegExpUtility.getMatches(this.config.getPastPrefixRegex(), trimmedText).length > 0) {
            swift = -1;
        }

        return swift;
    }

    private boolean endsWithTerms(String text, List<String> terms) {
        boolean result = false;

        for (String term : terms) {
            if (text.endsWith(term)) {
                result = true;
                break;
            }
        }

        return result;
    }
}
