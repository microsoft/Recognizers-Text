package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Locale;
import java.util.Optional;

import org.javatuples.Pair;

public class DateTimePeriodParser extends BaseDateTimePeriodParser {

    public DateTimePeriodParser(IDateTimePeriodParserConfiguration configuration) {

        super(configuration);
    }

    @Override
    protected DateTimeResolutionResult parseSpecificTimeOfDay(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        // handle morning, afternoon..
        MatchedTimeRangeResult matchedTimeRangeResult = this.config.getMatchedTimeRange(trimmedText, "-1", -1, -1, -1);
        if (!matchedTimeRangeResult.getMatched()) {
            return ret;
        }

        boolean exactMatch = RegexExtension.isExactMatch(this.config.getSpecificTimeOfDayRegex(),trimmedText, true);

        if (exactMatch) {
            int swift = this.config.getSwiftPrefix(trimmedText);
            LocalDateTime date = referenceTime.plusDays(swift);
            int day = date.getDayOfMonth();
            int month = date.getMonthValue();
            int year = date.getYear();

            ret.setTimex(DateTimeFormatUtil.formatDate(date) + matchedTimeRangeResult.getTimeStr());
            ret.setFutureValue(new Pair<>(
                DateUtil.safeCreateFromValue(
                    date, year, month, day, matchedTimeRangeResult.getBeginHour(), 0, 0),
                DateUtil.safeCreateFromValue(
                    date, year, month, day, matchedTimeRangeResult.getEndHour(), matchedTimeRangeResult.getEndMin(), matchedTimeRangeResult.getEndMin())));
            ret.setPastValue(new Pair<>(
                DateUtil.safeCreateFromValue(
                    date, year, month, day, matchedTimeRangeResult.getBeginHour(), 0, 0),
                DateUtil.safeCreateFromValue(
                    date, year, month, day, matchedTimeRangeResult.getEndHour(), matchedTimeRangeResult.getEndMin(), matchedTimeRangeResult.getEndMin())));
            ret.setSuccess(true);

            return ret;
        }

        int startIndex = trimmedText.indexOf(SpanishDateTime.Tomorrow);
        if (startIndex == 0) {
            startIndex = SpanishDateTime.Tomorrow.length();
        } else {
            startIndex = 0;
        }

        // handle Date followed by morning, afternoon
        // Add handling code to handle morning, afternoon followed by Date
        // Add handling code to handle early/late morning, afternoon
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config .getTimeOfDayRegex(), trimmedText.substring(startIndex))).findFirst();
        if (match.isPresent()) {
            String beforeStr = trimmedText.substring(0, match.get().index).trim();
            List<ExtractResult> ers = this.config.getDateExtractor().extract(beforeStr, referenceTime);

            if (ers.size() == 0) {
                return ret;
            }

            DateTimeParseResult pr = this.config.getDateParser().parse(ers.get(0), referenceTime);
            LocalDateTime futureDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
            LocalDateTime pastDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getPastValue();

            ret.setTimex(pr.getTimexStr() + matchedTimeRangeResult.getTimeStr());

            ret.setFutureValue(new Pair<>(
                DateUtil.safeCreateFromValue(
                    futureDate,
                    futureDate.getYear(),
                    futureDate.getMonthValue(),
                    futureDate.getDayOfMonth(),
                    matchedTimeRangeResult.getBeginHour(),
                    0,
                    0),
                DateUtil.safeCreateFromValue(
                    futureDate,
                    futureDate.getYear(),
                    futureDate.getMonthValue(),
                    futureDate.getDayOfMonth(),
                    matchedTimeRangeResult.getEndHour(),
                    matchedTimeRangeResult.getEndMin(),
                    matchedTimeRangeResult.getEndMin())));
            ret.setPastValue(new Pair<>(
                DateUtil.safeCreateFromValue(pastDate,
                    pastDate.getYear(),
                    pastDate.getMonthValue(),
                    pastDate.getDayOfMonth(),
                    matchedTimeRangeResult.getBeginHour(),
                    0,
                    0),
                DateUtil.safeCreateFromValue(
                    pastDate,
                    pastDate.getYear(),
                    pastDate.getMonthValue(),
                    pastDate.getDayOfMonth(),
                    matchedTimeRangeResult.getEndHour(),
                    matchedTimeRangeResult.getEndMin(),
                    matchedTimeRangeResult.getEndMin())));
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }

}
