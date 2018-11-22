package com.microsoft.recognizers.text.datetime.english.parsers;

import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.Optional;

public class TimeParser extends BaseTimeParser {

    public TimeParser(ITimeParserConfiguration config) {
        super(config);
    }

    @Override
    protected DateTimeResolutionResult internalParse(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult innerResult = super.internalParse(text, referenceTime);

        if (!innerResult.getSuccess()) {
            innerResult = parseIsh(text, referenceTime);
        }

        return innerResult;
    }

    // parse "noonish", "11-ish"
    private DateTimeResolutionResult parseIsh(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.IshRegex, trimmedText)).findFirst();
        if (match.isPresent() && match.get().length == trimmedText.length()) {
            String hourStr = match.get().getGroup(Constants.HourGroupName).value;
            int hour = Constants.HalfDayHourCount;

            if (!StringUtility.isNullOrEmpty(hourStr)) {
                hour = Integer.parseInt(hourStr);
            }

            result.setTimex(String.format("T%02d", hour));
            LocalDateTime resultTime = DateUtil.safeCreateFromMinValue(
                referenceTime.getYear(),
                referenceTime.getMonthValue(),
                referenceTime.getDayOfMonth(),
                hour, 0, 0);
            result.setFutureValue(resultTime);
            result.setPastValue(resultTime);
            result.setSuccess(true);
        }

        return result;
    }
}