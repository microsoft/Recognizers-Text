package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import java.time.LocalDateTime;
import java.util.HashMap;

import org.javatuples.Pair;

// Currently only Year is enabled as context, we may support Month or Week in the future
public class DateContext {
    private int year = Constants.InvalidYear;

    public int getYear() {
        return year;
    }

    public void setYear(int year) {
        this.year = year;
    }

    public DateTimeParseResult processDateEntityParsingResult(DateTimeParseResult originalResult) {
        if (!isEmpty()) {
            originalResult.setTimexStr(TimexUtility.setTimexWithContext(originalResult.getTimexStr(), this));
            originalResult.setValue(processDateEntityResolution((DateTimeResolutionResult)originalResult.getValue()));
        }

        return originalResult;
    }

    public DateTimeResolutionResult processDateEntityResolution(DateTimeResolutionResult resolutionResult) {
        if (!isEmpty()) {
            resolutionResult.setTimex(TimexUtility.setTimexWithContext(resolutionResult.getTimex(), this));
            resolutionResult.setFutureValue(setDateWithContext((LocalDateTime)resolutionResult.getFutureValue()));
            resolutionResult.setPastValue(setDateWithContext((LocalDateTime)resolutionResult.getPastValue()));
        }

        return resolutionResult;
    }

    public DateTimeResolutionResult processDatePeriodEntityResolution(DateTimeResolutionResult resolutionResult) {
        if (!isEmpty()) {
            resolutionResult.setTimex(TimexUtility.setTimexWithContext(resolutionResult.getTimex(), this));
            resolutionResult.setFutureValue(setDateRangeWithContext((Pair<LocalDateTime, LocalDateTime>)resolutionResult.getFutureValue()));
            resolutionResult.setPastValue(setDateRangeWithContext((Pair<LocalDateTime, LocalDateTime>)resolutionResult.getPastValue()));
        }

        return resolutionResult;
    }

    // Generate future/past date for cases without specific year like "Feb 29th"
    public static HashMap<String, LocalDateTime> generateDates(boolean noYear, LocalDateTime referenceDate, int year, int month, int day) {
        HashMap<String, LocalDateTime> result = new HashMap<>();
        LocalDateTime futureDate = DateUtil.safeCreateFromMinValue(year, month, day);
        LocalDateTime pastDate = DateUtil.safeCreateFromMinValue(year, month, day);
        int futureYear = year;
        int pastYear = year;
        if (noYear) {
            if (isFeb29th(year, month, day)) {
                if (isLeapYear(year)) {
                    if (futureDate.compareTo(referenceDate) < 0) {
                        futureDate = DateUtil.safeCreateFromMinValue(futureYear + 4, month, day);
                    } else {
                        pastDate = DateUtil.safeCreateFromMinValue(pastYear - 4, month, day);
                    }
                } else {
                    pastYear = pastYear >> 2 << 2;
                    if (!isLeapYear(pastYear)) {
                        pastYear -= 4;
                    }

                    futureYear = pastYear + 4;
                    if (!isLeapYear(futureYear)) {
                        futureYear += 4;
                    }
                    futureDate = DateUtil.safeCreateFromMinValue(futureYear, month, day);
                    pastDate = DateUtil.safeCreateFromMinValue(pastYear, month, day);
                }
            } else {
                if (futureDate.compareTo(referenceDate) < 0 && DateUtil.isValidDate(year, month, day)) {
                    futureDate = DateUtil.safeCreateFromMinValue(year + 1, month, day);
                }

                if (pastDate.compareTo(referenceDate) >= 0 && DateUtil.isValidDate(year, month, day)) {
                    pastDate = DateUtil.safeCreateFromMinValue(year - 1, month, day);
                }
            }
        }
        result.put(Constants.FutureDate, futureDate);
        result.put(Constants.PastDate, pastDate);
        return result;
    }

    private static boolean isLeapYear(int year) {
        return (((year % 4) == 0) && (((year % 100) != 0) || ((year % 400) == 0)));
    }

    private static boolean isFeb29th(int year, int month, int day) {
        return month == 2 && day == 29;
    }

    public static boolean isFeb29th(LocalDateTime date) {
        return date.getMonthValue() == 2 && date.getDayOfMonth() == 29;
    }

    // This method is to ensure the year of begin date is same with the end date in no year situation.
    public HashMap<String, DateTimeParseResult> syncYear(DateTimeParseResult pr1, DateTimeParseResult pr2) {
        if (this.isEmpty()) {
            int futureYear;
            int pastYear;
            if (isFeb29th((LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue())) {
                futureYear = ((LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue()).getYear();
                pastYear = ((LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getPastValue()).getYear();
                pr2.setValue(syncYearResolution((DateTimeResolutionResult)pr2.getValue(), futureYear, pastYear));
            } else if (isFeb29th((LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue())) {
                futureYear = ((LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue()).getYear();
                pastYear = ((LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getPastValue()).getYear();
                pr1.setValue(syncYearResolution((DateTimeResolutionResult)pr1.getValue(), futureYear, pastYear));
            }
        }

        HashMap<String, DateTimeParseResult> result = new HashMap<>();
        result.put(Constants.ParseResult1, pr1);
        result.put(Constants.ParseResult2, pr2);
        return result;
    }

    public DateTimeResolutionResult syncYearResolution(DateTimeResolutionResult resolutionResult, int futureYear, int pastYear) {
        resolutionResult.setFutureValue(setDateWithContext((LocalDateTime)resolutionResult.getFutureValue(), futureYear));
        resolutionResult.setPastValue(setDateWithContext((LocalDateTime)resolutionResult.getPastValue(), pastYear));
        return resolutionResult;
    }

    public boolean isEmpty() {
        return this.year == Constants.InvalidYear;
    }

    // This method is to ensure the begin date is less than the end date
    // As DateContext only support common Year as context, so decrease year part of begin date to ensure the begin date is less than end date
    public LocalDateTime swiftDateObject(LocalDateTime beginDate, LocalDateTime endDate) {
        if (beginDate.isAfter(endDate)) {
            beginDate = beginDate.plusYears(-1);
        }

        return beginDate;
    }

    private LocalDateTime setDateWithContext(LocalDateTime originalDate) {
        return setDateWithContext(originalDate, this.year);
    }

    private LocalDateTime setDateWithContext(LocalDateTime originalDate, int year) {
        if (!DateUtil.isDefaultValue(originalDate)) {
            return DateUtil.safeCreateFromMinValue(year, originalDate.getMonthValue(), originalDate.getDayOfMonth());
        }
        return originalDate;
    }

    private Pair<LocalDateTime, LocalDateTime> setDateRangeWithContext(Pair<LocalDateTime, LocalDateTime> originalDateRange) {
        LocalDateTime startDate = setDateWithContext(originalDateRange.getValue0());
        LocalDateTime endDate = setDateWithContext(originalDateRange.getValue1());

        return new Pair<>(startDate, endDate);
    }
}