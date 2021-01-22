package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DatePeriodTimexType;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.parsers.config.IDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateContext;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.DurationParsingUtil;
import com.microsoft.recognizers.text.datetime.utilities.GetModAndDateResult;
import com.microsoft.recognizers.text.datetime.utilities.NthBusinessDayResult;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.IntegerUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.sql.Time;
import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.time.Month;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import org.javatuples.Pair;

public class BaseDatePeriodParser implements IDateTimeParser {

    private static final String parserName = Constants.SYS_DATETIME_DATEPERIOD; //"DatePeriod";
    private static boolean inclusiveEndPeriod = false;

    private final IDatePeriodParserConfiguration config;

    public BaseDatePeriodParser(IDatePeriodParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return parserName;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime refDate) {

        DateTimeResolutionResult value = null;
        if (er.getType().equals(parserName)) {
            DateTimeResolutionResult innerResult = parseBaseDatePeriod(er.getText(), refDate);

            if (!innerResult.getSuccess()) {
                innerResult = parseComplexDatePeriod(er.getText(), refDate);
            }

            if (innerResult.getSuccess()) {
                if (innerResult.getMod() != null && innerResult.getMod().equals(Constants.BEFORE_MOD)) {
                    innerResult.setFutureResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.END_DATE,
                                    DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getFutureValue()))
                            .build());

                    innerResult.setPastResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.END_DATE,
                                    DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getPastValue()))
                            .build());
                } else if (innerResult.getMod() != null && innerResult.getMod().equals(Constants.AFTER_MOD)) {
                    innerResult.setFutureResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATE,
                                    DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getFutureValue()))
                            .build());

                    innerResult.setPastResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATE,
                                    DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getPastValue()))
                            .build());
                } else if (innerResult.getFutureValue() != null && innerResult.getPastValue() != null) {
                    innerResult.setFutureResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATE,
                                    DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue0()))
                            .put(TimeTypeConstants.END_DATE,
                                    DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue1()))
                            .build());

                    innerResult.setPastResolution(ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATE,
                                    DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue0()))
                            .put(TimeTypeConstants.END_DATE,
                                    DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue1()))
                            .build());
                } else {
                    innerResult.setFutureResolution(new HashMap<>());
                    innerResult.setPastResolution(new HashMap<>());
                }
                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(er.getStart(), er.getLength(), er.getText(), er.getType(), er.getData(), value, "", "", er.getMetadata());

        if (value != null) {
            ret.setTimexStr(value.getTimex());
        }

        return ret;
    }

    // Process case like "from|between START to|and END" where START/END can be dateRange or datePoint
    private DateTimeResolutionResult parseComplexDatePeriod(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getComplexDatePeriodRegex(), text)).findFirst();

        if (match.isPresent()) {
            LocalDateTime futureBegin = LocalDateTime.MIN;
            LocalDateTime futureEnd = LocalDateTime.MIN;
            LocalDateTime pastBegin = LocalDateTime.MIN;
            LocalDateTime pastEnd = LocalDateTime.MIN;
            boolean isSpecificDate = false;
            boolean isStartByWeek = false;
            boolean isEndByWeek = false;
            DateContext dateContext = getYearContext(match.get().getGroup("start").value.trim(), match.get().getGroup("end").value.trim(), text);

            DateTimeResolutionResult startResolution = parseSingleTimePoint(match.get().getGroup("start").value.trim(), referenceDate, dateContext);

            if (startResolution.getSuccess()) {
                futureBegin = (LocalDateTime)startResolution.getFutureValue();
                pastBegin = (LocalDateTime)startResolution.getPastValue();
                isSpecificDate = true;
            } else {
                startResolution = parseBaseDatePeriod(match.get().getGroup("start").value.trim(), referenceDate, dateContext);
                if (startResolution.getSuccess()) {
                    futureBegin = ((Pair<LocalDateTime, LocalDateTime>)startResolution.getFutureValue()).getValue0();
                    pastBegin = ((Pair<LocalDateTime, LocalDateTime>)startResolution.getPastValue()).getValue0();

                    if (startResolution.getTimex().contains("-W")) {
                        isStartByWeek = true;
                    }
                }
            }

            if (startResolution.getSuccess()) {
                DateTimeResolutionResult endResolution = parseSingleTimePoint(match.get().getGroup("end").value.trim(), referenceDate, dateContext);

                if (endResolution.getSuccess()) {
                    futureEnd = (LocalDateTime)endResolution.getFutureValue();
                    pastEnd = (LocalDateTime)endResolution.getPastValue();
                    isSpecificDate = true;
                } else {
                    endResolution = parseBaseDatePeriod(match.get().getGroup("end").value.trim(), referenceDate, dateContext);

                    if (endResolution.getSuccess()) {
                        futureEnd = ((Pair<LocalDateTime, LocalDateTime>)endResolution.getFutureValue()).getValue0();
                        pastEnd = ((Pair<LocalDateTime, LocalDateTime>)endResolution.getPastValue()).getValue0();

                        if (endResolution.getTimex().contains("-W")) {
                            isEndByWeek = true;
                        }
                    }
                }

                if (endResolution.getSuccess()) {
                    if (futureBegin.isAfter(futureEnd)) {
                        if (dateContext == null || dateContext.isEmpty()) {
                            futureBegin = pastBegin;
                        } else {
                            futureBegin = dateContext.swiftDateObject(futureBegin, futureEnd);
                        }
                    }

                    if (pastEnd.isBefore(pastBegin)) {
                        if (dateContext == null || dateContext.isEmpty()) {
                            pastEnd = futureEnd;
                        } else {
                            pastBegin = dateContext.swiftDateObject(pastBegin, pastEnd);
                        }
                    }

                    // If both begin/end are date ranges in "Month", the Timex should be ByMonth
                    // The year period case should already be handled in Basic Cases
                    DatePeriodTimexType datePeriodTimexType = DatePeriodTimexType.ByMonth;

                    if (isSpecificDate) {
                        // If at least one of the begin/end is specific date, the Timex should be ByDay
                        datePeriodTimexType = DatePeriodTimexType.ByDay;
                    } else if (isStartByWeek && isEndByWeek) {
                        // If both begin/end are date ranges in "Week", the Timex should be ByWeek
                        datePeriodTimexType = DatePeriodTimexType.ByWeek;
                    }

                    ret.setTimex(TimexUtility.generateDatePeriodTimex(futureBegin, futureEnd, datePeriodTimexType, pastBegin, pastEnd));

                    ret.setFutureValue(new Pair<>(futureBegin, futureEnd));
                    ret.setPastValue(new Pair<>(pastBegin, pastEnd));
                    ret.setSuccess(true);
                }
            }
        }
        return ret;
    }

    private DateTimeResolutionResult parseBaseDatePeriod(String text, LocalDateTime referenceDate) {
        return parseBaseDatePeriod(text, referenceDate, null);
    }

    private DateTimeResolutionResult parseBaseDatePeriod(String text, LocalDateTime referenceDate, DateContext dateContext) {
        DateTimeResolutionResult innerResult = parseMonthWithYear(text, referenceDate);

        if (!innerResult.getSuccess()) {
            innerResult = parseSimpleCases(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseOneWordPeriod(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = mergeTwoTimePoints(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseYear(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseWeekOfMonth(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseWeekOfYear(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseHalfYear(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseQuarter(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseSeason(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseWhichWeek(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseWeekOfDate(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseMonthOfDate(text, referenceDate);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseDecade(text, referenceDate);
        }

        // Cases like "within/less than/more than x weeks from/before/after today"
        if (!innerResult.getSuccess()) {
            innerResult = parseDatePointWithAgoAndLater(text, referenceDate);
        }

        // Parse duration should be at the end since it will extract "the last week" from "the last week of July"
        if (!innerResult.getSuccess()) {
            innerResult = parseDuration(text, referenceDate);
        }

        // Cases like "21st century"
        if (!innerResult.getSuccess()) {
            innerResult = parseOrdinalNumberWithCenturySuffix(text, referenceDate);
        }

        if (innerResult.getSuccess() && dateContext != null) {
            innerResult = dateContext.processDatePeriodEntityResolution(innerResult);
        }

        return innerResult;
    }

    private DateTimeResolutionResult parseOrdinalNumberWithCenturySuffix(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        Optional<ExtractResult> er = this.config.getOrdinalExtractor().extract(text).stream().findFirst();

        if (er.isPresent() && er.get().getStart() + er.get().getLength() < text.length()) {
            String afterString = text.substring(er.get().getStart() + er.get().getLength()).trim();

            // It falls into the cases like "21st century"
            if (Arrays.stream(RegExpUtility.getMatches(this.config.getCenturySuffixRegex(), afterString)).findFirst().isPresent()) {
                ParseResult number = this.config.getNumberParser().parse(er.get());

                if (number.getValue() != null) {
                    // Note that 1st century means from year 0 - 100
                    int startYear = (Math.round(((Double)number.getValue()).floatValue()) - 1) * Constants.CenturyYearsCount;
                    LocalDateTime startDate = DateUtil.safeCreateFromMinValue(startYear, 1, 1);
                    LocalDateTime endDate = DateUtil.safeCreateFromMinValue(startYear + Constants.CenturyYearsCount, 1, 1);

                    String startLuisStr = DateTimeFormatUtil.luisDate(startDate);
                    String endLuisStr = DateTimeFormatUtil.luisDate(endDate);
                    String durationTimex = "P" + Constants.CenturyYearsCount + "Y";

                    ret.setTimex(String.format("(%s,%s,%s)", startLuisStr, endLuisStr, durationTimex));
                    ret.setFutureValue(new Pair<>(startDate, endDate));
                    ret.setPastValue(new Pair<>(startDate, endDate));
                    ret.setSuccess(true);
                }
            }
        }

        return ret;
    }

    private DateTimeResolutionResult parseDatePointWithAgoAndLater(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        Optional<ExtractResult> er = this.config.getDateExtractor().extract(text, referenceDate).stream().findFirst();

        if (er.isPresent()) {
            String beforeString = text.substring(0, er.get().getStart());
            boolean isAgo = Arrays.stream(RegExpUtility.getMatches(this.config.getAgoRegex(), er.get().getText())).findFirst().isPresent();
            boolean isLater = Arrays.stream(RegExpUtility.getMatches(this.config.getLaterRegex(), er.get().getText())).findFirst().isPresent();

            if (!StringUtility.isNullOrEmpty(beforeString) && (isAgo || isLater)) {
                boolean isLessThanOrWithIn = false;
                boolean isMoreThan = false;

                // cases like "within 3 days from yesterday/tomorrow" does not make any sense
                if (er.get().getText().contains("today") || er.get().getText().contains("now")) {
                    Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getWithinNextPrefixRegex(), beforeString)).findFirst();
                    if (match.isPresent()) {
                        boolean isNext = !StringUtility.isNullOrEmpty(match.get().getGroup("next").value);

                        // cases like "within the next 5 days before today" is not acceptable
                        if (!(isNext && isAgo)) {
                            isLessThanOrWithIn = true;
                        }
                    }
                }

                isLessThanOrWithIn = isLessThanOrWithIn || (Arrays.stream(RegExpUtility.getMatches(this.config.getLessThanRegex(), beforeString)).findFirst().isPresent());
                isMoreThan = Arrays.stream(RegExpUtility.getMatches(this.config.getMoreThanRegex(), beforeString)).findFirst().isPresent();

                DateTimeParseResult pr = this.config.getDateParser().parse(er.get(), referenceDate);
                Optional<ExtractResult> durationExtractionResult = this.config.getDurationExtractor().extract(er.get().getText()).stream().findFirst();

                if (durationExtractionResult.isPresent()) {
                    ParseResult duration = this.config.getDurationParser().parse(durationExtractionResult.get());
                    long durationInSeconds = Math.round((Double)((DateTimeResolutionResult)(duration.getValue())).getPastValue());

                    if (isLessThanOrWithIn) {
                        LocalDateTime startDate;
                        LocalDateTime endDate;

                        if (isAgo) {
                            startDate = (LocalDateTime)((DateTimeResolutionResult)(pr.getValue())).getPastValue();
                            endDate = startDate.plusSeconds(durationInSeconds);
                        } else {
                            endDate = (LocalDateTime)((DateTimeResolutionResult)(pr.getValue())).getFutureValue();
                            startDate = endDate.minusSeconds(durationInSeconds);
                        }

                        if (startDate != LocalDateTime.MIN) {
                            String startLuisStr = DateTimeFormatUtil.luisDate(startDate);
                            String endLuisStr = DateTimeFormatUtil.luisDate(endDate);
                            String durationTimex = ((DateTimeResolutionResult)(duration.getValue())).getTimex();

                            ret.setTimex(String.format("(%s,%s,%s)", startLuisStr, endLuisStr, durationTimex));
                            ret.setFutureValue(new Pair<>(startDate, endDate));
                            ret.setPastValue(new Pair<>(startDate, endDate));
                            ret.setSuccess(true);
                        }
                    } else if (isMoreThan) {
                        ret.setMod(isAgo ? Constants.BEFORE_MOD : Constants.AFTER_MOD);

                        ret.setTimex(pr.getTimexStr());
                        ret.setFutureValue(((DateTimeResolutionResult)(pr.getValue())).getFutureValue());
                        ret.setPastValue(((DateTimeResolutionResult)(pr.getValue())).getPastValue());
                        ret.setSuccess(true);
                    }
                }
            }
        }

        return ret;
    }

    private DateTimeResolutionResult parseSingleTimePoint(String text, LocalDateTime referenceDate, DateContext dateContext) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        ExtractResult er = this.config.getDateExtractor().extract(text, referenceDate).stream().findFirst().orElse(null);

        if (er != null) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getWeekWithWeekDayRangeRegex(), text)).findFirst();
            String weekPrefix = null;
            if (match.isPresent()) {
                weekPrefix = match.get().getGroup("week").value;
            }

            if (!StringUtility.isNullOrEmpty(weekPrefix)) {
                er.setText(weekPrefix + " " + er.getText());
            }

            ParseResult pr = this.config.getDateParser().parse(er, referenceDate);

            if (pr != null) {
                ret.setTimex("(" + ((DateTimeParseResult)pr).getTimexStr());
                ret.setFutureValue(((DateTimeResolutionResult)pr.getValue()).getFutureValue());
                ret.setPastValue(((DateTimeResolutionResult)pr.getValue()).getPastValue());
                ret.setSuccess(true);
            }

            if (dateContext != null) {
                ret = dateContext.processDateEntityResolution(ret);
            }
        }

        return ret;
    }

    private DateTimeResolutionResult parseSimpleCases(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        int year = referenceDate.getYear();
        int month = referenceDate.getMonthValue();
        int beginDay;
        int endDay;
        boolean noYear = true;

        ConditionalMatch match = RegexExtension.matchExact(this.config.getMonthFrontBetweenRegex(), text, true);
        String beginLuisStr;
        String endLuisStr;

        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getBetweenRegex(), text, true);
        }

        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getMonthFrontSimpleCasesRegex(), text, true);
        }

        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getSimpleCasesRegex(), text, true);
        }

        if (match.getSuccess()) {
            MatchGroup days = match.getMatch().get().getGroup("day");
            beginDay = this.config.getDayOfMonth().get(days.captures[0].value.toLowerCase());
            endDay = this.config.getDayOfMonth().get(days.captures[1].value.toLowerCase());

            // parse year
            year = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(match.getMatch().get());
            if (year != Constants.InvalidYear) {
                noYear = false;
            } else {
                year = referenceDate.getYear();
            }

            String monthStr = match.getMatch().get().getGroup("month").value;
            if (!StringUtility.isNullOrEmpty(monthStr)) {
                month = this.config.getMonthOfYear().get(monthStr.toLowerCase());
            } else {
                monthStr = match.getMatch().get().getGroup("relmonth").value.trim().toLowerCase();
                int swiftMonth = this.config.getSwiftDayOrMonth(monthStr);
                switch (swiftMonth) {
                    case 1:
                        if (month != 12) {
                            month += 1;
                        } else {
                            month = 1;
                            year += 1;
                        }
                        break;
                    case -1:
                        if (month != 1) {
                            month -= 1;
                        } else {
                            month = 12;
                            year -= 1;
                        }
                        break;
                    default:
                        break;
                }

                if (this.config.isFuture(monthStr)) {
                    noYear = false;
                }
            }
        } else {
            return ret;
        }

        if (noYear) {
            beginLuisStr = DateTimeFormatUtil.luisDate(-1, month, beginDay);
            endLuisStr = DateTimeFormatUtil.luisDate(-1, month, endDay);
        } else {
            beginLuisStr = DateTimeFormatUtil.luisDate(year, month, beginDay);
            endLuisStr = DateTimeFormatUtil.luisDate(year, month, endDay);
        }

        int futureYear = year;
        int pastYear = year;
        LocalDateTime startDate = DateUtil.safeCreateFromMinValue(year, month, beginDay);

        if (noYear && startDate.isBefore(referenceDate)) {
            futureYear++;
        }

        if (noYear && (startDate.isAfter(referenceDate) || startDate.isEqual(referenceDate))) {
            pastYear--;
        }

        HashMap<String, LocalDateTime> futurePastBeginDates = DateContext.generateDates(noYear, referenceDate, year, month, beginDay);
        HashMap<String, LocalDateTime> futurePastEndDates = DateContext.generateDates(noYear, referenceDate, year, month, endDay);

        ret.setTimex(String.format("(%s,%s,P%sD)", beginLuisStr, endLuisStr, (endDay - beginDay)));
        ret.setFutureValue(new Pair<>(futurePastBeginDates.get(Constants.FutureDate),
                futurePastEndDates.get(Constants.FutureDate)));
        ret.setPastValue(new Pair<>(futurePastBeginDates.get(Constants.PastDate),
                futurePastEndDates.get(Constants.PastDate)));
        ret.setSuccess(true);

        return ret;
    }

    private boolean isPresent(int swift) {
        return swift == 0;
    }

    private DateTimeResolutionResult parseOneWordPeriod(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        int year = referenceDate.getYear();
        int month = referenceDate.getMonthValue();
        int futureYear = year;
        int pastYear = year;
        boolean earlyPrefix = false;
        boolean latePrefix = false;
        boolean midPrefix = false;
        boolean isRef = false;

        boolean earlierPrefix = false;
        boolean laterPrefix = false;

        String trimmedText = text.trim().toLowerCase();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getOneWordPeriodRegex(), trimmedText, true);

        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getLaterEarlyPeriodRegex(), trimmedText, true);
        }

        // For cases "that week|month|year"
        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getReferenceDatePeriodRegex(), trimmedText, true);
            isRef = true;
            ret.setMod(Constants.REF_UNDEF_MOD);
        }

        if (match.getSuccess()) {
            if (!match.getMatch().get().getGroup("EarlyPrefix").value.equals("")) {
                earlyPrefix = true;
                trimmedText = match.getMatch().get().getGroup(Constants.SuffixGroupName).value;
                ret.setMod(Constants.EARLY_MOD);
            } else if (!match.getMatch().get().getGroup("LatePrefix").value.equals("")) {
                latePrefix = true;
                trimmedText = match.getMatch().get().getGroup(Constants.SuffixGroupName).value;
                ret.setMod(Constants.LATE_MOD);
            } else if (!match.getMatch().get().getGroup("MidPrefix").value.equals("")) {
                midPrefix = true;
                trimmedText = match.getMatch().get().getGroup(Constants.SuffixGroupName).value;
                ret.setMod(Constants.MID_MOD);
            }

            int swift = 0;
            String monthStr = match.getMatch().get().getGroup("month").value;
            if (!StringUtility.isNullOrEmpty(monthStr)) {
                swift = this.config.getSwiftYear(trimmedText);
            } else {
                swift = this.config.getSwiftDayOrMonth(trimmedText);
            }

            // Handle the abbreviation of DatePeriod, e.g., 'eoy(end of year)', the behavior of 'eoy' should be the same as 'end of year'
            Optional<Match> unspecificEndOfRangeMatch = Arrays.stream(RegExpUtility.getMatches(config.getUnspecificEndOfRangeRegex(), match.getMatch().get().value)).findFirst();
            if (unspecificEndOfRangeMatch.isPresent()) {
                latePrefix = true;
                trimmedText = match.getMatch().get().value;
                ret.setMod(Constants.LATE_MOD);
            }

            if (!match.getMatch().get().getGroup("RelEarly").value.equals("")) {
                earlierPrefix = true;
                if (isPresent(swift)) {
                    ret.setMod(null);
                }
            } else if (!match.getMatch().get().getGroup("RelLate").value.equals("")) {
                laterPrefix = true;
                if (isPresent(swift)) {
                    ret.setMod(null);
                }
            }

            if (this.config.isYearToDate(trimmedText)) {
                ret.setTimex(String.format("%04d", referenceDate.getYear()));
                ret.setFutureValue(new Pair<>(
                        DateUtil.safeCreateFromMinValue(referenceDate.getYear(), 1, 1), referenceDate));
                ret.setPastValue(new Pair<>(
                        DateUtil.safeCreateFromMinValue(referenceDate.getYear(), 1, 1), referenceDate));

                ret.setSuccess(true);
                return ret;
            }

            if (this.config.isMonthToDate(trimmedText)) {
                ret.setTimex(String.format("%04d-%02d", referenceDate.getYear(), referenceDate.getMonthValue()));
                ret.setFutureValue(new Pair<>(
                        DateUtil.safeCreateFromMinValue(referenceDate.getYear(), referenceDate.getMonthValue(), 1),
                        referenceDate));
                ret.setPastValue(new Pair<>(
                        DateUtil.safeCreateFromMinValue(referenceDate.getYear(), referenceDate.getMonthValue(), 1),
                        referenceDate));

                ret.setSuccess(true);
                return ret;
            }

            if (!StringUtility.isNullOrEmpty(monthStr)) {
                swift = this.config.getSwiftYear(trimmedText);

                month = this.config.getMonthOfYear().get(monthStr.toLowerCase());

                if (swift >= -1) {
                    ret.setTimex(String.format("%04d-%02d", referenceDate.getYear() + swift, month));
                    year = year + swift;
                    futureYear = pastYear = year;
                } else {
                    ret.setTimex(String.format("XXXX-%02d", month));
                    if (month < referenceDate.getMonthValue()) {
                        futureYear++;
                    }

                    if (month >= referenceDate.getMonthValue()) {
                        pastYear--;
                    }
                }
            } else {
                swift = this.config.getSwiftDayOrMonth(trimmedText);

                if (this.config.isWeekOnly(trimmedText)) {
                    LocalDateTime thursday = DateUtil.thisDate(referenceDate, DayOfWeek.THURSDAY.getValue()).plusDays(Constants.WeekDayCount * swift);

                    ret.setTimex(isRef ? TimexUtility.generateWeekTimex() : TimexUtility.generateWeekTimex(thursday));

                    LocalDateTime beginDate = DateUtil.thisDate(referenceDate, DayOfWeek.MONDAY.getValue()).plusDays(Constants.WeekDayCount * swift);

                    LocalDateTime endValue = DateUtil.thisDate(referenceDate, DayOfWeek.SUNDAY.getValue()).plusDays(Constants.WeekDayCount * swift);

                    LocalDateTime endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);

                    if (earlyPrefix) {
                        endValue = DateUtil.thisDate(referenceDate, DayOfWeek.WEDNESDAY.getValue()).plusDays(Constants.WeekDayCount * swift);
                        endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);
                    } else if (midPrefix) {
                        beginDate = DateUtil.thisDate(referenceDate, DayOfWeek.TUESDAY.getValue()).plusDays(Constants.WeekDayCount * swift);
                        endValue = DateUtil.thisDate(referenceDate, DayOfWeek.FRIDAY.getValue()).plusDays(Constants.WeekDayCount * swift);
                        endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);
                    } else if (latePrefix) {
                        beginDate = DateUtil.thisDate(referenceDate, DayOfWeek.THURSDAY.getValue()).plusDays(Constants.WeekDayCount * swift);
                    }

                    if (earlierPrefix && swift == 0) {
                        if (endDate.isAfter(referenceDate)) {
                            endDate = referenceDate;
                        }
                    } else if (laterPrefix && swift == 0) {
                        if (beginDate.isBefore(referenceDate)) {
                            beginDate = referenceDate;
                        }
                    }

                    if (latePrefix && swift != 0) {
                        ret.setMod(Constants.LATE_MOD);
                    }

                    ret.setFutureValue(new Pair<>(beginDate, endDate));
                    ret.setPastValue(new Pair<>(beginDate, endDate));

                    ret.setSuccess(true);
                    return ret;
                }

                if (this.config.isWeekend(trimmedText)) {
                    LocalDateTime beginDate = DateUtil.thisDate(referenceDate, DayOfWeek.SATURDAY.getValue()).plusDays(Constants.WeekDayCount * swift);
                    LocalDateTime endValue = DateUtil.thisDate(referenceDate, DayOfWeek.SUNDAY.getValue()).plusDays(Constants.WeekDayCount * swift);

                    ret.setTimex(isRef ? TimexUtility.generateWeekendTimex() : TimexUtility.generateWeekendTimex(beginDate));

                    LocalDateTime endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);

                    ret.setFutureValue(new Pair<>(beginDate, endDate));
                    ret.setPastValue(new Pair<>(beginDate, endDate));

                    ret.setSuccess(true);
                    return ret;
                }

                if (this.config.isMonthOnly(trimmedText)) {
                    LocalDateTime date = referenceDate.plusMonths(swift);
                    month = date.getMonthValue();
                    year = date.getYear();

                    ret.setTimex(isRef ? TimexUtility.generateMonthTimex() : TimexUtility.generateMonthTimex(date));

                    futureYear = pastYear = year;
                } else if (this.config.isYearOnly(trimmedText)) {
                    LocalDateTime date = referenceDate.plusYears(swift);
                    year = date.getYear();

                    if (!StringUtility.isNullOrEmpty(match.getMatch().get().getGroup("special").value)) {
                        String specialYearPrefixes = this.config.getSpecialYearPrefixesMap().get(match.getMatch().get().getGroup("special").value.toLowerCase());
                        swift = this.config.getSwiftYear(trimmedText);
                        year = swift < -1 ? Constants.InvalidYear : year;
                        ret.setTimex(TimexUtility.generateYearTimex(year, specialYearPrefixes));
                        ret.setSuccess(true);
                        return ret;
                    }

                    LocalDateTime beginDate = DateUtil.safeCreateFromMinValue(year, 1, 1);

                    LocalDateTime endValue = DateUtil.safeCreateFromMinValue(year, 12, 31);
                    LocalDateTime endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);

                    if (earlyPrefix) {
                        endValue = DateUtil.safeCreateFromMinValue(year, 6, 30);
                        endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);
                    } else if (midPrefix) {
                        beginDate = DateUtil.safeCreateFromMinValue(year, 4, 1);
                        endValue = DateUtil.safeCreateFromMinValue(year, 9, 30);
                        endDate = inclusiveEndPeriod ? endValue : endValue.plusDays(1);
                    } else if (latePrefix) {
                        beginDate = DateUtil.safeCreateFromMinValue(year, 7, 1);
                    }

                    if (earlierPrefix && swift == 0) {
                        if (endDate.isAfter(referenceDate)) {
                            endDate = referenceDate;
                        }
                    } else if (laterPrefix && swift == 0) {
                        if (beginDate.isBefore(referenceDate)) {
                            beginDate = referenceDate;
                        }
                    }

                    year = isRef ? Constants.InvalidYear : year;
                    ret.setTimex(TimexUtility.generateYearTimex(year));

                    ret.setFutureValue(new Pair<>(beginDate, endDate));
                    ret.setPastValue(new Pair<>(beginDate, endDate));

                    ret.setSuccess(true);
                    return ret;
                }
            }
        } else {
            return ret;
        }

        // only "month" will come to here
        LocalDateTime futureStart = DateUtil.safeCreateFromMinValue(futureYear, month, 1);
        LocalDateTime futureEnd = inclusiveEndPeriod ? futureStart.plusMonths(1).minusDays(1) : futureStart.plusMonths(1);


        LocalDateTime pastStart = DateUtil.safeCreateFromMinValue(pastYear, month, 1);
        LocalDateTime pastEnd = inclusiveEndPeriod ? pastStart.plusMonths(1).minusDays(1) : pastStart.plusMonths(1);

        if (earlyPrefix) {
            futureEnd = inclusiveEndPeriod ?
                    DateUtil.safeCreateFromMinValue(futureYear, month, 15) :
                    DateUtil.safeCreateFromMinValue(futureYear, month, 15).plusDays(1);
            pastEnd = inclusiveEndPeriod ?
                    DateUtil.safeCreateFromMinValue(pastYear, month, 15) :
                    DateUtil.safeCreateFromMinValue(pastYear, month, 15).plusDays(1);
        } else if (midPrefix) {
            futureStart = DateUtil.safeCreateFromMinValue(futureYear, month, 10);
            pastStart = DateUtil.safeCreateFromMinValue(pastYear, month, 10);
            futureEnd = inclusiveEndPeriod ?
                    DateUtil.safeCreateFromMinValue(futureYear, month, 20) :
                    DateUtil.safeCreateFromMinValue(futureYear, month, 20).plusDays(1);
            pastEnd = inclusiveEndPeriod ?
                    DateUtil.safeCreateFromMinValue(pastYear, month, 20) :
                    DateUtil.safeCreateFromMinValue(pastYear, month, 20).plusDays(1);
        } else if (latePrefix) {
            futureStart = DateUtil.safeCreateFromMinValue(futureYear, month, 16);
            pastStart = DateUtil.safeCreateFromMinValue(pastYear, month, 16);
        }

        if (earlierPrefix && futureEnd.isEqual(pastEnd)) {
            if (futureEnd.isAfter(referenceDate)) {
                futureEnd = pastEnd = referenceDate;
            }
        } else if (laterPrefix && futureStart.isEqual(pastStart)) {
            if (futureStart.isBefore(referenceDate)) {
                futureStart = pastStart = referenceDate;
            }
        }

        ret.setFutureValue(new Pair<>(futureStart, futureEnd));

        ret.setPastValue(new Pair<>(pastStart, pastEnd));

        ret.setSuccess(true);

        return ret;
    }

    private DateTimeResolutionResult parseMonthWithYear(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        ConditionalMatch match = RegexExtension.matchExact(this.config.getMonthWithYear(), text, true);
        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getMonthNumWithYear(), text, true);
        }

        if (match.getSuccess()) {
            String monthStr = match.getMatch().get().getGroup("month").value.toLowerCase();
            String orderStr = match.getMatch().get().getGroup("order").value.toLowerCase();

            int month = this.config.getMonthOfYear().get(monthStr.toLowerCase());

            int year = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(match.getMatch().get());
            if (year == Constants.InvalidYear) {
                int swift = this.config.getSwiftYear(orderStr);
                if (swift < -1) {
                    return ret;
                }
                year = referenceDate.getYear() + swift;
            }

            LocalDateTime startValue = DateUtil.safeCreateFromMinValue(year, month, 1);
            LocalDateTime endValue = inclusiveEndPeriod ?
                    DateUtil.safeCreateFromMinValue(year, month, 1).plusMonths(1).minusDays(1) :
                    DateUtil.safeCreateFromMinValue(year, month, 1).plusMonths(1);

            ret.setFutureValue(new Pair<>(startValue, endValue));
            ret.setPastValue(new Pair<>(startValue, endValue));

            ret.setTimex(String.format("%04d-%02d", year, month));

            ret.setSuccess(true);
        }

        return ret;
    }

    private DateTimeResolutionResult parseYear(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        int year = Constants.InvalidYear;

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getYearPeriodRegex(), text)).findFirst();
        Optional<Match> matchMonth = Arrays.stream(RegExpUtility.getMatches(this.config.getMonthWithYear(), text)).findFirst();
        ;

        if (match.isPresent() && !matchMonth.isPresent()) {
            int beginYear = Constants.InvalidYear;
            int endYear = Constants.InvalidYear;

            Match[] matches = RegExpUtility.getMatches(this.config.getYearRegex(), text);
            if (matches.length == 2) {
                // (from|during|in|between)? 2012 (till|to|until|through|-) 2015
                if (!matches[0].value.equals("")) {
                    beginYear = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(matches[0]);
                    if (!(beginYear >= Constants.MinYearNum && beginYear <= Constants.MaxYearNum)) {
                        beginYear = Constants.InvalidYear;
                    }
                }

                if (!matches[1].value.equals("")) {
                    endYear = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(matches[1]);
                    if (!(endYear >= Constants.MinYearNum && endYear <= Constants.MaxYearNum)) {
                        endYear = Constants.InvalidYear;
                    }
                }
            }

            if (beginYear != Constants.InvalidYear && endYear != Constants.InvalidYear) {
                LocalDateTime beginDay = DateUtil.safeCreateFromMinValue(beginYear, 1, 1);

                LocalDateTime endDayValue = DateUtil.safeCreateFromMinValue(endYear, 1, 1);
                LocalDateTime endDay = inclusiveEndPeriod ? endDayValue.minusDays(1) : endDayValue;

                ret.setTimex(String.format("(%s,%s,P%sY)", DateTimeFormatUtil.luisDate(beginDay), DateTimeFormatUtil.luisDate(endDay), (endYear - beginYear)));
                ret.setFutureValue(new Pair<>(beginDay, endDay));
                ret.setPastValue(new Pair<>(beginDay, endDay));
                ret.setSuccess(true);

                return ret;
            }
        } else {
            ConditionalMatch exactMatch = RegexExtension.matchExact(this.config.getYearRegex(), text, true);
            if (exactMatch.getSuccess()) {
                year = this.config.getDateExtractor().getYearFromText(exactMatch.getMatch().get());
                if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                    year = Constants.InvalidYear;
                }
            } else {
                exactMatch = RegexExtension.matchExact(this.config.getYearPlusNumberRegex(), text, true);
                if (exactMatch.getSuccess()) {
                    year = this.config.getDateExtractor().getYearFromText(exactMatch.getMatch().get());
                    if (!StringUtility.isNullOrEmpty(exactMatch.getMatch().get().getGroup("special").value)) {
                        String specialYearPrefixes = this.config.getSpecialYearPrefixesMap().get(exactMatch.getMatch().get().getGroup("special").value.toLowerCase());
                        ret.setTimex(TimexUtility.generateYearTimex(year, specialYearPrefixes));
                        ret.setSuccess(true);
                        return ret;
                    }
                }
            }

            if (year != Constants.InvalidYear) {
                LocalDateTime beginDay = DateUtil.safeCreateFromMinValue(year, 1, 1);

                LocalDateTime endDayValue = DateUtil.safeCreateFromMinValue(year + 1, 1, 1);
                LocalDateTime endDay = inclusiveEndPeriod ? endDayValue.minusDays(1) : endDayValue;

                ret.setTimex(TimexUtility.generateYearTimex(year));
                ret.setFutureValue(new Pair<>(beginDay, endDay));
                ret.setPastValue(new Pair<>(beginDay, endDay));
                ret.setSuccess(true);

                return ret;
            }
        }

        return ret;
    }

    // parse entities that made up by two time points
    private DateTimeResolutionResult mergeTwoTimePoints(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        List<ExtractResult> er = this.config.getDateExtractor().extract(text, referenceDate);
        DateTimeParseResult pr1 = null;
        DateTimeParseResult pr2 = null;
        if (er.size() < 2) {
            er = this.config.getDateExtractor().extract(this.config.getTokenBeforeDate() + text, referenceDate);
            if (er.size() >= 2) {
                er.get(0).setStart(er.get(0).getStart() - this.config.getTokenBeforeDate().length());
                er.get(1).setStart(er.get(1).getStart() - this.config.getTokenBeforeDate().length());
                er.set(0, er.get(0));
                er.set(1, er.get(1));
            } else {
                DateTimeParseResult nowPr = parseNowAsDate(text, referenceDate);
                if (nowPr == null || er.size() < 1) {
                    return ret;
                }

                DateTimeParseResult datePr = this.config.getDateParser().parse(er.get(0), referenceDate);
                pr1 = datePr.getStart() < nowPr.getStart() ? datePr : nowPr;
                pr2 = datePr.getStart() < nowPr.getStart() ? nowPr : datePr;
            }

        }
        if (er.size() >= 2) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getWeekWithWeekDayRangeRegex(), text)).findFirst();
            String weekPrefix = null;
            if (match.isPresent()) {
                weekPrefix = match.get().getGroup("week").value;
            }

            if (!StringUtility.isNullOrEmpty(weekPrefix)) {
                er.get(0).setText(String.format("%s %s", weekPrefix, er.get(0).getText()));
                er.get(1).setText(String.format("%s %s", weekPrefix, er.get(1).getText()));
                er.set(0, er.get(0));
                er.set(1, er.get(1));
            }

            DateContext dateContext = getYearContext(er.get(0).getText(), er.get(1).getText(), text);

            pr1 = this.config.getDateParser().parse(er.get(0), referenceDate);
            pr2 = this.config.getDateParser().parse(er.get(1), referenceDate);

            if (pr1.getValue() == null || pr2.getValue() == null) {
                return ret;
            }

            pr1 = dateContext.processDateEntityParsingResult(pr1);
            pr2 = dateContext.processDateEntityParsingResult(pr2);

            // When the case has no specified year, we should sync the future/past year due to invalid date Feb 29th.
            if (dateContext.isEmpty() && (DateContext.isFeb29th((LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue()) ||
                    DateContext.isFeb29th((LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue()))) {

                HashMap<String, DateTimeParseResult> parseResultHashMap = dateContext.syncYear(pr1, pr2);
                pr1 = parseResultHashMap.get(Constants.ParseResult1);
                pr2 = parseResultHashMap.get(Constants.ParseResult2);
            }
        }

        List<Object> subDateTimeEntities = new ArrayList<Object>();
        subDateTimeEntities.add(pr1);
        subDateTimeEntities.add(pr2);
        ret.setSubDateTimeEntities(subDateTimeEntities);

        LocalDateTime futureBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue();
        LocalDateTime futureEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue();

        LocalDateTime pastBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getPastValue();
        LocalDateTime pastEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getPastValue();

        if (futureBegin.isAfter(futureEnd)) {
            futureBegin = pastBegin;
        }

        if (pastEnd.isBefore(pastBegin)) {
            pastEnd = futureEnd;
        }

        ret.setTimex(TimexUtility.generateDatePeriodTimexStr(futureBegin, futureEnd, DatePeriodTimexType.ByDay, pr1.getTimexStr(), pr2.getTimexStr()));

        if (pr1.getTimexStr().startsWith(Constants.TimexFuzzyYear) && futureBegin.compareTo(DateUtil.safeCreateFromMinValue(futureBegin.getYear(), 2, 28)) <= 0 &&
                futureEnd.compareTo(DateUtil.safeCreateFromMinValue(futureBegin.getYear(), 3, 1)) >= 0) {

            // Handle cases like "Feb 29th to March 1st".
            // There may be different timexes for FutureValue and PastValue due to the different validity of Feb 29th.
            ret.setComment(Constants.Comment_DoubleTimex);
            String pastTimex = TimexUtility.generateDatePeriodTimexStr(pastBegin, pastEnd, DatePeriodTimexType.ByDay, pr1.getTimexStr(), pr2.getTimexStr());
            ret.setTimex(TimexUtility.mergeTimexAlternatives(ret.getTimex(), pastTimex));
        }
        ret.setFutureValue(new Pair<>(futureBegin, futureEnd));
        ret.setPastValue(new Pair<>(pastBegin, pastEnd));
        ret.setSuccess(true);

        return ret;
    }

    // parse entities that made up by two time points with now
    private DateTimeParseResult parseNowAsDate(String text, LocalDateTime referenceDate) {
        DateTimeParseResult nowPr = null;
        LocalDateTime value = referenceDate.toLocalDate().atStartOfDay();
        Match[] matches = RegExpUtility.getMatches(this.config.getNowRegex(), text);
        for (Match match : matches) {
            DateTimeResolutionResult retNow = new DateTimeResolutionResult();
            retNow.setTimex(DateTimeFormatUtil.luisDate(value));
            retNow.setFutureValue(value);
            retNow.setPastValue(value);

            nowPr = new DateTimeParseResult(
                match.index,
                match.length,
                match.value,
                Constants.SYS_DATETIME_DATE,
                null,
                retNow,
                "",
                value == null ? "" : ((DateTimeResolutionResult)retNow).getTimex());
        }

        return nowPr;
    }

    private DateTimeResolutionResult parseDuration(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        LocalDateTime beginDate = referenceDate;
        LocalDateTime endDate = referenceDate;
        String timex = "";
        boolean restNowSunday = false;
        List<LocalDateTime> dateList = null;

        List<ExtractResult> durationErs = config.getDurationExtractor().extract(text, referenceDate);
        if (durationErs.size() == 1) {
            ParseResult durationPr = config.getDurationParser().parse(durationErs.get(0));
            String beforeStr = text.substring(0, (durationPr.getStart() != null) ? durationPr.getStart() : 0).trim().toLowerCase();
            String afterStr = text.substring(
                    ((durationPr.getStart() != null) ? durationPr.getStart() : 0) + ((durationPr.getLength() != null) ? durationPr.getLength() : 0))
                    .trim().toLowerCase();

            List<ExtractResult> numbersInSuffix = config.getCardinalExtractor().extract(beforeStr);
            List<ExtractResult> numbersInDuration = config.getCardinalExtractor().extract(durationErs.get(0).getText());

            // Handle cases like "2 upcoming days", "5 previous years"
            if (!numbersInSuffix.isEmpty() && numbersInDuration.isEmpty()) {
                ExtractResult numberEr = numbersInSuffix.stream().findFirst().get();
                String numberText = numberEr.getText();
                String durationText = durationErs.get(0).getText();
                String combinedText = String.format("%s %s", numberText, durationText);
                List<ExtractResult> combinedDurationEr = config.getDurationExtractor().extract(combinedText, referenceDate);

                if (!combinedDurationEr.isEmpty()) {
                    durationPr = config.getDurationParser().parse(combinedDurationEr.stream().findFirst().get());
                    int startIndex = numberEr.getStart() + numberEr.getLength();
                    beforeStr = beforeStr.substring(startIndex).trim();
                }
            }

            GetModAndDateResult getModAndDateResult = new GetModAndDateResult();

            if (durationPr.getValue() != null) {
                DateTimeResolutionResult durationResult = (DateTimeResolutionResult)durationPr.getValue();

                if (StringUtility.isNullOrEmpty(durationResult.getTimex())) {
                    return ret;
                }

                Optional<Match> prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), beforeStr)).findFirst();
                Optional<Match> suffixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), afterStr)).findFirst();
                if (prefixMatch.isPresent() || suffixMatch.isPresent()) {
                    getModAndDateResult = getModAndDate(beginDate, endDate, referenceDate, durationResult.getTimex(), false);
                    beginDate = getModAndDateResult.beginDate;
                }

                // Handle the "within two weeks" case which means from today to the end of next two weeks
                // Cases like "within 3 days before/after today" is not handled here (4th condition)
                boolean isMatch = false;
                if (RegexExtension.isExactMatch(config.getWithinNextPrefixRegex(), beforeStr, true)) {
                    getModAndDateResult = getModAndDate(beginDate, endDate, referenceDate, durationResult.getTimex(), true);
                    beginDate = getModAndDateResult.beginDate;
                    endDate = getModAndDateResult.endDate;

                    // In GetModAndDate, this "future" resolution will add one day to beginDate/endDate, but for the "within" case it should start from the current day.
                    beginDate = beginDate.minusDays(1);
                    endDate = endDate.minusDays(1);
                    isMatch = true;
                }

                if (RegexExtension.isExactMatch(config.getFutureRegex(), beforeStr, true)) {
                    getModAndDateResult = getModAndDate(beginDate, endDate, referenceDate, durationResult.getTimex(), true);
                    beginDate = getModAndDateResult.beginDate;
                    endDate = getModAndDateResult.endDate;
                    isMatch = true;
                }

                Optional<Match> futureSuffixMatch = Arrays.stream(RegExpUtility.getMatches(config.getFutureSuffixRegex(), afterStr)).findFirst();
                if (futureSuffixMatch.isPresent()) {
                    getModAndDateResult = getModAndDate(beginDate, endDate, referenceDate, durationResult.getTimex(), true);
                    beginDate = getModAndDateResult.beginDate;
                    endDate = getModAndDateResult.endDate;
                }

                // Handle the "in two weeks" case which means the second week
                if (RegexExtension.isExactMatch(config.getInConnectorRegex(), beforeStr, true) &&
                        !DurationParsingUtil.isMultipleDuration(durationResult.getTimex()) && !isMatch) {
                    getModAndDateResult = getModAndDate(beginDate, endDate, referenceDate, durationResult.getTimex(), true);
                    beginDate = getModAndDateResult.beginDate;
                    endDate = getModAndDateResult.endDate;

                    // Change the duration value and the beginDate
                    String unit = durationResult.getTimex().substring(durationResult.getTimex().length() - 1);

                    durationResult.setTimex(String.format("P1%s", unit));
                    beginDate = DurationParsingUtil.shiftDateTime(durationResult.getTimex(), endDate, false);
                }

                if (!StringUtility.isNullOrEmpty(getModAndDateResult.mod)) {
                    ((DateTimeResolutionResult)durationPr.getValue()).setMod(getModAndDateResult.mod);
                }

                timex = durationResult.getTimex();

                List<Object> subDateTimeEntities = new ArrayList<>();
                subDateTimeEntities.add(durationPr);
                ret.setSubDateTimeEntities(subDateTimeEntities);

                if (getModAndDateResult.dateList != null) {
                    ret.setList(getModAndDateResult.dateList.stream().map(e -> (Object)e).collect(Collectors.toList()));
                }
            }
        }

        // Parse "rest of"
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getRestOfDateRegex(), text)).findFirst();
        if (match.isPresent()) {
            String durationStr = match.get().getGroup("duration").value;
            String durationUnit = this.config.getUnitMap().get(durationStr);
            switch (durationUnit) {
                case "W":
                    int diff = Constants.WeekDayCount - ((beginDate.getDayOfWeek().getValue()) == 0 ? Constants.WeekDayCount : beginDate.getDayOfWeek().getValue());
                    endDate = beginDate.plusDays(diff);
                    timex = String.format("P%s%s", diff, Constants.TimexDay);
                    if (diff == 0) {
                        restNowSunday = true;
                    }
                    break;

                case "MON":
                    endDate = DateUtil.safeCreateFromMinValue(beginDate.getYear(), beginDate.getMonthValue(), 1);
                    endDate = endDate.plusMonths(1).minusDays(1);
                    diff = (int)ChronoUnit.DAYS.between(beginDate, endDate) + 1;
                    timex = String.format("P%s%s", diff, Constants.TimexDay);
                    break;

                case "Y":
                    endDate = DateUtil.safeCreateFromMinValue(beginDate.getYear(), 12, 1);
                    endDate = endDate.plusMonths(1).minusDays(1);
                    diff = (int)ChronoUnit.DAYS.between(beginDate, endDate) + 1;
                    timex = String.format("P%s%s", diff, Constants.TimexDay);
                    break;
                default:
                    break;
            }
        }

        if (!beginDate.equals(endDate) || restNowSunday) {
            endDate = inclusiveEndPeriod ? endDate.minusDays(1) : endDate;

            ret.setTimex(String.format("(%s,%s,%s)", DateTimeFormatUtil.luisDate(beginDate), DateTimeFormatUtil.luisDate(endDate), timex));
            ret.setFutureValue(new Pair<>(beginDate, endDate));
            ret.setPastValue(new Pair<>(beginDate, endDate));
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }

    private GetModAndDateResult getModAndDate(LocalDateTime beginDate, LocalDateTime endDate, LocalDateTime referenceDate, String timex, boolean future) {
        LocalDateTime beginDateResult = beginDate;
        LocalDateTime endDateResult = endDate;
        boolean isBusinessDay = timex.endsWith(Constants.TimexBusinessDay);
        int businessDayCount = 0;

        if (isBusinessDay) {
            businessDayCount = Integer.parseInt(timex.substring(1, timex.length() - 2));
        }

        if (future) {
            String mod = Constants.AFTER_MOD;

            // For future the beginDate should add 1 first
            if (isBusinessDay) {
                beginDateResult = DurationParsingUtil.getNextBusinessDay(referenceDate);
                NthBusinessDayResult nthBusinessDayResult = DurationParsingUtil.getNthBusinessDay(beginDateResult, businessDayCount - 1, true);
                endDateResult = nthBusinessDayResult.result.plusDays(1);
                return new GetModAndDateResult(beginDateResult, endDateResult, mod, nthBusinessDayResult.dateList);
            } else {
                beginDateResult = referenceDate.plusDays(1);
                endDateResult = DurationParsingUtil.shiftDateTime(timex, beginDateResult, true);
                return new GetModAndDateResult(beginDateResult, endDateResult, mod, null);
            }

        } else {
            String mod = Constants.BEFORE_MOD;

            if (isBusinessDay) {
                endDateResult = DurationParsingUtil.getNextBusinessDay(endDateResult, false);
                NthBusinessDayResult nthBusinessDayResult = DurationParsingUtil.getNthBusinessDay(endDateResult, businessDayCount - 1, false);
                endDateResult = endDateResult.plusDays(1);
                beginDateResult = nthBusinessDayResult.result;
                return new GetModAndDateResult(beginDateResult, endDateResult, mod, nthBusinessDayResult.dateList);
            } else {
                beginDateResult = DurationParsingUtil.shiftDateTime(timex, endDateResult, false);
                return new GetModAndDateResult(beginDateResult, endDateResult, mod, null);
            }
        }
    }

    // To be consistency, we follow the definition of "week of year":
    // "first week of the month" - it has the month's first Thursday in it
    // "last week of the month" - it has the month's last Thursday in it
    private DateTimeResolutionResult parseWeekOfMonth(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getWeekOfMonthRegex(), trimmedText, true);
        if (!match.getSuccess()) {
            return ret;
        }

        String cardinalStr = match.getMatch().get().getGroup("cardinal").value;
        String monthStr = match.getMatch().get().getGroup("month").value;
        boolean noYear = false;
        int year;

        int month;
        if (StringUtility.isNullOrEmpty(monthStr)) {
            int swift = this.config.getSwiftDayOrMonth(trimmedText);

            month = referenceDate.plusMonths(swift).getMonthValue();
            year = referenceDate.plusMonths(swift).getYear();
        } else {
            month = this.config.getMonthOfYear().get(monthStr);
            year = config.getDateExtractor().getYearFromText(match.getMatch().get());

            if (year == Constants.InvalidYear) {
                year = referenceDate.getYear();
                noYear = true;
            }
        }

        ret = getWeekOfMonth(cardinalStr, month, year, referenceDate, noYear);

        return ret;
    }

    private DateTimeResolutionResult parseWeekOfYear(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getWeekOfYearRegex(), trimmedText, true);
        if (!match.getSuccess()) {
            return ret;
        }

        String cardinalStr = match.getMatch().get().getGroup("cardinal").value;
        String orderStr = match.getMatch().get().getGroup("order").value.toLowerCase();

        int year = this.config.getDateExtractor().getYearFromText(match.getMatch().get());
        if (year == Constants.InvalidYear) {
            int swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return ret;
            }
            year = referenceDate.getYear() + swift;
        }

        LocalDateTime targetWeekMonday;
        if (this.config.isLastCardinal(cardinalStr)) {
            targetWeekMonday = DateUtil.thisDate(getLastThursday(year), DayOfWeek.MONDAY.getValue());

            ret.setTimex(TimexUtility.generateWeekTimex(targetWeekMonday));
        } else {
            int weekNum = this.config.getCardinalMap().get(cardinalStr);
            targetWeekMonday = DateUtil.thisDate(getFirstThursday(year), DayOfWeek.MONDAY.getValue())
                    .plusDays(Constants.WeekDayCount * (weekNum - 1));

            ret.setTimex(TimexUtility.generateWeekOfYearTimex(year, weekNum));
        }

        ret.setFutureValue(inclusiveEndPeriod ?
                new Pair<>(targetWeekMonday, targetWeekMonday.plusDays(Constants.WeekDayCount - 1)) :
                new Pair<>(targetWeekMonday, targetWeekMonday.plusDays(Constants.WeekDayCount)));

        ret.setPastValue(inclusiveEndPeriod ?
                new Pair<>(targetWeekMonday, targetWeekMonday.plusDays(Constants.WeekDayCount - 1)) :
                new Pair<>(targetWeekMonday, targetWeekMonday.plusDays(Constants.WeekDayCount)));

        ret.setSuccess(true);

        return ret;
    }

    private DateTimeResolutionResult parseHalfYear(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getAllHalfYearRegex(), text, true);

        if (!match.getSuccess()) {
            return ret;
        }

        String cardinalStr = match.getMatch().get().getGroup("cardinal").value.toLowerCase();
        String orderStr = match.getMatch().get().getGroup("order").value.toLowerCase();
        String numberStr = match.getMatch().get().getGroup("number").value;

        int year = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(match.getMatch().get());

        if (year == Constants.InvalidYear) {
            int swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return ret;
            }
            year = referenceDate.getYear() + swift;
        }

        int halfNum;
        if (!StringUtility.isNullOrEmpty(numberStr)) {
            halfNum = Integer.parseInt(numberStr);
        } else {
            halfNum = this.config.getCardinalMap().get(cardinalStr);
        }

        LocalDateTime beginDate = DateUtil.safeCreateFromMinValue(year, (halfNum - 1) * Constants.SemesterMonthCount + 1, 1);
        LocalDateTime endDate = DateUtil.safeCreateFromMinValue(year, halfNum * Constants.SemesterMonthCount, 1).plusMonths(1);
        ret.setFutureValue(new Pair<>(beginDate, endDate));
        ret.setPastValue(new Pair<>(beginDate, endDate));
        ret.setTimex(String.format("(%s,%s,P6M)", DateTimeFormatUtil.luisDate(beginDate), DateTimeFormatUtil.luisDate(endDate)));
        ret.setSuccess(true);

        return ret;
    }

    private DateTimeResolutionResult parseQuarter(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getQuarterRegex(), text, true);

        if (!match.getSuccess()) {
            match = RegexExtension.matchExact(this.config.getQuarterRegexYearFront(), text, true);
        }

        if (!match.getSuccess()) {
            return ret;
        }

        String cardinalStr = match.getMatch().get().getGroup("cardinal").value.toLowerCase();
        String orderQuarterStr = match.getMatch().get().getGroup("orderQuarter").value.toLowerCase();
        String orderStr = StringUtility.isNullOrEmpty(orderQuarterStr) ? match.getMatch().get().getGroup("order").value.toLowerCase() : null;
        String numberStr = match.getMatch().get().getGroup("number").value;

        boolean noSpecificYear = false;
        int year = this.config.getDateExtractor().getYearFromText(match.getMatch().get());

        if (year == Constants.InvalidYear) {
            int swift = StringUtility.isNullOrEmpty(orderQuarterStr) ? this.config.getSwiftYear(orderStr) : 0;
            if (swift < -1) {
                swift = 0;
                noSpecificYear = true;
            }
            year = referenceDate.getYear() + swift;
        }

        int quarterNum;
        if (!StringUtility.isNullOrEmpty(numberStr)) {
            quarterNum = Integer.parseInt(numberStr);
        } else if (!StringUtility.isNullOrEmpty(orderQuarterStr)) {
            int month = referenceDate.getMonthValue();
            quarterNum = (int)Math.ceil((double)month / Constants.TrimesterMonthCount);
            int swift = this.config.getSwiftYear(orderQuarterStr);
            quarterNum += swift;
            if (quarterNum <= 0) {
                quarterNum += Constants.QuarterCount;
                year -= 1;
            } else if (quarterNum > Constants.QuarterCount) {
                quarterNum -= Constants.QuarterCount;
                year += 1;
            }
        } else {
            quarterNum = this.config.getCardinalMap().get(cardinalStr);
        }

        LocalDateTime beginDate = DateUtil.safeCreateFromMinValue(year, (quarterNum - 1) * Constants.TrimesterMonthCount + 1, 1);
        LocalDateTime endDate = DateUtil.safeCreateFromMinValue(year, quarterNum * Constants.TrimesterMonthCount, 1).plusMonths(1);

        if (noSpecificYear) {
            if (endDate.compareTo(referenceDate) < 0) {
                ret.setPastValue(new Pair<>(beginDate, endDate));

                LocalDateTime futureBeginDate = DateUtil.safeCreateFromMinValue(year + 1, (quarterNum - 1) * Constants.TrimesterMonthCount + 1, 1);
                LocalDateTime futureEndDate = DateUtil.safeCreateFromMinValue(year + 1, quarterNum * Constants.TrimesterMonthCount, 1).plusMonths(1);
                ret.setFutureValue(new Pair<>(futureBeginDate, futureEndDate));
            } else if (endDate.compareTo(referenceDate) > 0) {
                ret.setFutureValue(new Pair<>(beginDate, endDate));

                LocalDateTime pastBeginDate = DateUtil.safeCreateFromMinValue(year - 1, (quarterNum - 1) * Constants.TrimesterMonthCount + 1, 1);
                LocalDateTime pastEndDate = DateUtil.safeCreateFromMinValue(year - 1, quarterNum * Constants.TrimesterMonthCount, 1).plusMonths(1);
                ret.setPastValue(new Pair<>(pastBeginDate, pastEndDate));
            } else {
                ret.setFutureValue(new Pair<>(beginDate, endDate));
                ret.setPastValue(new Pair<>(beginDate, endDate));
            }

            ret.setTimex(String.format("(%s,%s,P3M)", DateTimeFormatUtil.luisDate(-1, beginDate.getMonthValue(), 1), DateTimeFormatUtil.luisDate(-1, endDate.getMonthValue(), 1)));
        } else {
            ret.setFutureValue(new Pair<>(beginDate, endDate));
            ret.setPastValue(new Pair<>(beginDate, endDate));
            ret.setTimex(String.format("(%s,%s,P3M)", DateTimeFormatUtil.luisDate(beginDate), DateTimeFormatUtil.luisDate(endDate)));
        }

        ret.setSuccess(true);

        return ret;
    }

    private DateTimeResolutionResult parseSeason(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getSeasonRegex(), text, true);
        if (match.getSuccess()) {
            String seasonStr = this.config.getSeasonMap().get(match.getMatch().get().getGroup("seas").value.toLowerCase());

            if (!match.getMatch().get().getGroup("EarlyPrefix").value.equals("")) {
                ret.setMod(Constants.EARLY_MOD);
            } else if (!match.getMatch().get().getGroup("MidPrefix").value.equals("")) {
                ret.setMod(Constants.MID_MOD);
            } else if (!match.getMatch().get().getGroup("LatePrefix").value.equals("")) {
                ret.setMod(Constants.LATE_MOD);
            }

            int year = ((BaseDateExtractor)this.config.getDateExtractor()).getYearFromText(match.getMatch().get());
            if (year == Constants.InvalidYear) {
                int swift = this.config.getSwiftYear(text);
                if (swift < -1) {
                    ret.setTimex(seasonStr);
                    ret.setSuccess(true);
                    return ret;
                }
                year = referenceDate.getYear() + swift;
            }

            String yearStr = String.format("%04d", year);
            ret.setTimex(String.format("%s-%s", yearStr, seasonStr));

            ret.setSuccess(true);
            return ret;
        }
        return ret;
    }

    private DateTimeResolutionResult parseWeekOfDate(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getWeekOfRegex(), text)).findFirst();
        List<ExtractResult> dateErs = config.getDateExtractor().extract(text, referenceDate);

        if (dateErs.isEmpty()) {
            // For cases like "week of the 18th"
            dateErs.addAll(
                    config.getCardinalExtractor().extract(text).stream()
                            .peek(o -> o.setType(Constants.SYS_DATETIME_DATE))
                            .filter(o -> dateErs.stream().noneMatch(er -> er.isOverlap(o)))
                            .collect(Collectors.toList()));
        }

        if (match.isPresent() && dateErs.size() == 1) {
            DateTimeResolutionResult pr = (DateTimeResolutionResult)config.getDateParser().parse(dateErs.get(0), referenceDate).getValue();
            if (config.getOptions().match(DateTimeOptions.CalendarMode)) {
                LocalDateTime monday = DateUtil.thisDate((LocalDateTime)pr.getFutureValue(), DayOfWeek.MONDAY.getValue());
                ret.setTimex(DateTimeFormatUtil.toIsoWeekTimex(monday));
            } else {
                ret.setTimex(pr.getTimex());
            }
            ret.setComment(Constants.Comment_WeekOf);
            ret.setFutureValue(getWeekRangeFromDate((LocalDateTime)pr.getFutureValue()));
            ret.setPastValue(getWeekRangeFromDate((LocalDateTime)pr.getPastValue()));
            ret.setSuccess(true);
        }
        return ret;
    }

    private DateTimeResolutionResult parseMonthOfDate(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getMonthOfRegex(), text)).findFirst();
        List<ExtractResult> ex = config.getDateExtractor().extract(text, referenceDate);

        if (match.isPresent() && ex.size() == 1) {
            DateTimeResolutionResult pr = (DateTimeResolutionResult)config.getDateParser().parse(ex.get(0), referenceDate).getValue();
            ret.setTimex(pr.getTimex());
            ret.setComment(Constants.Comment_MonthOf);
            ret.setFutureValue(getMonthRangeFromDate((LocalDateTime)pr.getFutureValue()));
            ret.setPastValue(getMonthRangeFromDate((LocalDateTime)pr.getPastValue()));
            ret.setSuccess(true);
        }
        return ret;
    }

    private Pair<LocalDateTime, LocalDateTime> getWeekRangeFromDate(LocalDateTime date) {
        LocalDateTime startDate = DateUtil.thisDate(date, DayOfWeek.MONDAY.getValue());
        LocalDateTime endDate = inclusiveEndPeriod ? startDate.plusDays(Constants.WeekDayCount - 1) : startDate.plusDays(Constants.WeekDayCount);
        return new Pair<>(startDate, endDate);
    }

    private Pair<LocalDateTime, LocalDateTime> getMonthRangeFromDate(LocalDateTime date) {
        LocalDateTime startDate = DateUtil.safeCreateFromMinValue(date.getYear(), date.getMonthValue(), 1);
        LocalDateTime endDate;

        if (date.getMonthValue() < 12) {
            endDate = DateUtil.safeCreateFromMinValue(date.getYear(), date.getMonthValue() + 1, 1);
        } else {
            endDate = DateUtil.safeCreateFromMinValue(date.getYear() + 1, 1, 1);
        }

        endDate = inclusiveEndPeriod ? endDate.minusDays(1) : endDate;
        return new Pair<>(startDate, endDate);
    }

    private DateTimeResolutionResult parseWhichWeek(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getWhichWeekRegex(), text, true);
        if (match.getSuccess()) {
            int num = Integer.parseInt(match.getMatch().get().getGroup("number").value);
            int year = referenceDate.getYear();
            ret.setTimex(String.format("%04d-W%02d", year, num));
            LocalDateTime firstDay = DateUtil.safeCreateFromMinValue(year, 1, 1);
            LocalDateTime firstThursday = DateUtil.thisDate(firstDay, DayOfWeek.of(4).getValue());

            if (DateUtil.weekOfYear(firstThursday) == 1) {
                num -= 1;
            }

            LocalDateTime value = firstThursday.plusDays(Constants.WeekDayCount * num - 3);
            ret.setFutureValue(new Pair<>(value, value.plusDays(7)));
            ret.setPastValue(new Pair<>(value, value.plusDays(7)));
            ret.setSuccess(true);
        }
        return ret;
    }

    private DateTimeResolutionResult getWeekOfMonth(String cardinalStr, int month, int year, LocalDateTime referenceDate, boolean noYear) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        LocalDateTime targetMonday = getMondayOfTargetWeek(cardinalStr, month, year);

        LocalDateTime futureDate = targetMonday;
        LocalDateTime pastDate = targetMonday;

        if (noYear && futureDate.isBefore(referenceDate)) {
            futureDate = getMondayOfTargetWeek(cardinalStr, month, year + 1);
        }

        if (noYear && pastDate.compareTo(referenceDate) >= 0) {
            pastDate = getMondayOfTargetWeek(cardinalStr, month, year - 1);
        }

        if (noYear) {
            year = Constants.InvalidYear;
        }

        // Note that if the cardinalStr equals to "last", the weekNumber would be fixed at "5"
        // This may lead to some inconsistency between Timex and Resolution
        // the StartDate and EndDate of the resolution would always be correct (following ISO week definition)
        // But week number for "last week" might be inconsistency with the resolution as we only have one Timex,
        // but we may have past and future resolution which may have different week number
        int weekNum = getWeekNumberForMonth(cardinalStr);

        String timex = TimexUtility.generateWeekOfMonthTimex(year, month, weekNum);
        ret.setTimex(timex);

        ret.setFutureValue(inclusiveEndPeriod ?
                new Pair<>(futureDate, futureDate.plusDays(Constants.WeekDayCount - 1)) :
                new Pair<>(futureDate, futureDate.plusDays(Constants.WeekDayCount)));
        ret.setPastValue(inclusiveEndPeriod ?
                new Pair<>(pastDate, pastDate.plusDays(Constants.WeekDayCount - 1)) :
                new Pair<>(pastDate, pastDate.plusDays(Constants.WeekDayCount)));

        ret.setSuccess(true);

        return ret;
    }

    private LocalDateTime getFirstThursday(int year) {
        return getFirstThursday(year, Constants.InvalidMonth);
    }

    private LocalDateTime getFirstThursday(int year, int month) {
        int targetMonth = month;

        if (month == Constants.InvalidMonth) {
            targetMonth = Month.JANUARY.getValue();
        }

        LocalDateTime firstDay = LocalDateTime.of(year, targetMonth, 1, 0, 0);
        LocalDateTime firstThursday = DateUtil.thisDate(firstDay, DayOfWeek.THURSDAY.getValue());

        // Thursday fall into next year or next month
        if (firstThursday.getMonthValue() != targetMonth) {
            firstThursday = firstThursday.plusDays(Constants.WeekDayCount);
        }

        return firstThursday;
    }

    private LocalDateTime getLastThursday(int year) {
        return getLastThursday(year, Constants.InvalidMonth);
    }

    private LocalDateTime getLastThursday(int year, int month) {
        int targetMonth = month;

        if (month == Constants.InvalidMonth) {
            targetMonth = Month.DECEMBER.getValue();
        }

        LocalDateTime lastDay = getLastDay(year, targetMonth);
        LocalDateTime lastThursday = DateUtil.thisDate(lastDay, DayOfWeek.THURSDAY.getValue());

        // Thursday fall into next year or next month
        if (lastThursday.getMonthValue() != targetMonth) {
            lastThursday = lastThursday.minusDays(Constants.WeekDayCount);
        }

        return lastThursday;
    }

    private LocalDateTime getLastDay(int year, int month) {
        month++;
        if (month == 13) {
            year++;
            month = 1;
        }

        LocalDateTime firstDayOfNextMonth = LocalDateTime.of(year, month, 1, 0, 0);
        return firstDayOfNextMonth.minusDays(1);
    }

    private LocalDateTime getMondayOfTargetWeek(String cardinalStr, int month, int year) {
        LocalDateTime result;
        if (config.isLastCardinal(cardinalStr)) {
            LocalDateTime lastThursday = getLastThursday(year, month);
            result = DateUtil.thisDate(lastThursday, DayOfWeek.MONDAY.getValue());
        } else {
            int cardinal = getWeekNumberForMonth(cardinalStr);
            LocalDateTime firstThursday = getFirstThursday(year, month);

            result = DateUtil.thisDate(firstThursday, DayOfWeek.MONDAY.getValue()).plusDays(Constants.WeekDayCount * (cardinal - 1));
        }

        return result;
    }

    private int getWeekNumberForMonth(String cardinalStr) {
        int cardinal;

        if (config.isLastCardinal(cardinalStr)) {
            // "last week of month" might not be "5th week of month"
            // Sometimes it can also be "4th week of month" depends on specific year and month
            // But as we only have one Timex, so we use "5" to indicate last week of month
            cardinal = Constants.MaxWeekOfMonth;
        } else {
            cardinal = config.getCardinalMap().get(cardinalStr);
        }

        return cardinal;
    }

    private DateTimeResolutionResult parseDecade(String text, LocalDateTime referenceDate) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        int firstTwoNumOfYear = referenceDate.getYear() / 100;
        int decade = 0;
        int decadeLastYear = 10;
        int swift = 1;
        boolean inputCentury = false;

        String trimmedText = text.trim();
        ConditionalMatch match = RegexExtension.matchExact(this.config.getDecadeWithCenturyRegex(), text, true);
        String beginLuisStr;
        String endLuisStr;

        if (match.getSuccess()) {

            String decadeStr = match.getMatch().get().getGroup("decade").value.toLowerCase();
            if (!IntegerUtility.canParse(decadeStr)) {
                if (this.config.getWrittenDecades().containsKey(decadeStr)) {
                    decade = this.config.getWrittenDecades().get(decadeStr);
                } else if (this.config.getSpecialDecadeCases().containsKey(decadeStr)) {
                    firstTwoNumOfYear = this.config.getSpecialDecadeCases().get(decadeStr) / 100;
                    decade = this.config.getSpecialDecadeCases().get(decadeStr) % 100;
                    inputCentury = true;
                }
            } else {
                decade = Integer.parseInt(decadeStr);
            }

            String centuryStr = match.getMatch().get().getGroup("century").value.toLowerCase();
            if (!StringUtility.isNullOrEmpty(centuryStr)) {
                if (!IntegerUtility.canParse(centuryStr)) {
                    if (this.config.getNumbers().containsKey(centuryStr)) {
                        firstTwoNumOfYear = this.config.getNumbers().get(centuryStr);
                    } else {
                        // handle the case like "one/two thousand", "one/two hundred", etc.
                        List<ExtractResult> er = this.config.getIntegerExtractor().extract(centuryStr);

                        if (er.size() == 0) {
                            return ret;
                        }

                        firstTwoNumOfYear = Math.round(((Double)(this.config.getNumberParser().parse(er.get(0)).getValue() != null ?
                                this.config.getNumberParser().parse(er.get(0)).getValue() :
                                0)).floatValue());
                        if (firstTwoNumOfYear >= 100) {
                            firstTwoNumOfYear = firstTwoNumOfYear / 100;
                        }
                    }
                } else {
                    firstTwoNumOfYear = Integer.parseInt(centuryStr);
                }

                inputCentury = true;
            }
        } else {
            // handle cases like "the last 2 decades" "the next decade"
            match = RegexExtension.matchExact(this.config.getRelativeDecadeRegex(), trimmedText, true);
            if (match.getSuccess()) {
                inputCentury = true;

                swift = this.config.getSwiftDayOrMonth(trimmedText);

                String numStr = match.getMatch().get().getGroup("number").value.toLowerCase();
                List<ExtractResult> er = this.config.getIntegerExtractor().extract(numStr);
                if (er.size() == 1) {
                    int swiftNum = Math.round(((Double)(this.config.getNumberParser().parse(er.get(0)).getValue() != null ?
                            this.config.getNumberParser().parse(er.get(0)).getValue() :
                            0)).floatValue());
                    swift = swift * swiftNum;
                }

                int beginDecade = (referenceDate.getYear() % 100) / 10;
                if (swift < 0) {
                    beginDecade += swift;
                } else if (swift > 0) {
                    beginDecade += 1;
                }

                decade = beginDecade * 10;
            } else {
                return ret;
            }
        }

        int beginYear = firstTwoNumOfYear * 100 + decade;
        // swift = 0 corresponding to the/this decade
        int totalLastYear = decadeLastYear * Math.abs(swift == 0 ? 1 : swift);

        if (inputCentury) {
            beginLuisStr = DateTimeFormatUtil.luisDate(beginYear, 1, 1);
            endLuisStr = DateTimeFormatUtil.luisDate(beginYear + totalLastYear, 1, 1);
        } else {
            String beginYearStr = String.format("XX%s", decade);
            beginLuisStr = DateTimeFormatUtil.luisDate(-1, 1, 1);
            beginLuisStr = beginLuisStr.replace("XXXX", beginYearStr);

            String endYearStr = String.format("XX%s", (decade + totalLastYear));
            endLuisStr = DateTimeFormatUtil.luisDate(-1, 1, 1);
            endLuisStr = endLuisStr.replace("XXXX", endYearStr);
        }
        ret.setTimex(String.format("(%s,%s,P%sY)", beginLuisStr, endLuisStr, totalLastYear));

        int futureYear = beginYear;
        int pastYear = beginYear;
        LocalDateTime startDate = DateUtil.safeCreateFromMinValue(beginYear, 1, 1);
        if (!inputCentury && startDate.isBefore(referenceDate)) {
            futureYear += 100;
        }

        if (!inputCentury && startDate.compareTo(referenceDate) >= 0) {
            pastYear -= 100;
        }

        ret.setFutureValue(new Pair<>(DateUtil.safeCreateFromMinValue(futureYear, 1, 1),
                DateUtil.safeCreateFromMinValue(futureYear + totalLastYear, 1, 1)));

        ret.setPastValue(new Pair<>(DateUtil.safeCreateFromMinValue(pastYear, 1, 1),
                DateUtil.safeCreateFromMinValue(pastYear + totalLastYear, 1, 1)));

        ret.setSuccess(true);

        return ret;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }

    private DateContext getYearContext(String startDateStr, String endDateStr, String text) {
        boolean isEndDatePureYear = false;
        boolean isDateRelative = false;
        int contextYear = Constants.InvalidYear;

        Optional<Match> yearMatchForEndDate = Arrays.stream(RegExpUtility.getMatches(this.config.getYearRegex(), endDateStr)).findFirst();

        if (yearMatchForEndDate.isPresent() && yearMatchForEndDate.get().length == endDateStr.length()) {
            isEndDatePureYear = true;
        }

        Optional<Match> relativeMatchForStartDate = Arrays.stream(RegExpUtility.getMatches(this.config.getRelativeRegex(), startDateStr)).findFirst();
        Optional<Match> relativeMatchForEndDate = Arrays.stream(RegExpUtility.getMatches(this.config.getRelativeRegex(), endDateStr)).findFirst();
        isDateRelative = relativeMatchForStartDate.isPresent() || relativeMatchForEndDate.isPresent();

        if (!isEndDatePureYear && !isDateRelative) {
            for (Match match : RegExpUtility.getMatches(config.getYearRegex(), text)) {
                int year = config.getDateExtractor().getYearFromText(match);

                if (year != Constants.InvalidYear) {
                    if (contextYear == Constants.InvalidYear) {
                        contextYear = year;
                    } else {
                        // This indicates that the text has two different year value, no common context year
                        if (contextYear != year) {
                            contextYear = Constants.InvalidYear;
                            break;
                        }
                    }
                }
            }
        }

        DateContext dateContext = new DateContext();
        dateContext.setYear(contextYear);
        return dateContext;
    }
}
