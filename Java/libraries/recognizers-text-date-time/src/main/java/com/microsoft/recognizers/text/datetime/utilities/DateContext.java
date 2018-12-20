package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import java.time.LocalDateTime;
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
        return LocalDateTime.of(this.year, originalDate.getMonthValue(), originalDate.getDayOfMonth(), 0, 0);
    }

    private Pair<LocalDateTime, LocalDateTime> setDateRangeWithContext(Pair<LocalDateTime, LocalDateTime> originalDateRange) {
        LocalDateTime startDate = setDateWithContext(originalDateRange.getValue0());
        LocalDateTime endDate = setDateWithContext(originalDateRange.getValue1());

        return new Pair<>(startDate, endDate);
    }
}