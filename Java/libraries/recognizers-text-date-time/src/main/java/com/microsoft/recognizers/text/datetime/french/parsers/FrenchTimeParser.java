package com.microsoft.recognizers.text.datetime.french.parsers;

import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.StringUtility;
import java.time.LocalDateTime;

public class FrenchTimeParser extends BaseTimeParser {

    public FrenchTimeParser(final ITimeParserConfiguration config) {
        super(config);
    }

    @Override
    protected DateTimeResolutionResult internalParse(final String text, final LocalDateTime referenceTime) {
        DateTimeResolutionResult innerResult = super.internalParse(text, referenceTime);

        if (!innerResult.getSuccess()) {
            innerResult = parseIsh(text, referenceTime);
        }

        return innerResult;
    }

    // parse "noonish", "11-ish"
    private DateTimeResolutionResult parseIsh(final String text, final LocalDateTime referenceTime) {
        final DateTimeResolutionResult result = new DateTimeResolutionResult();

        final ConditionalMatch match = RegexExtension.matchExact(FrenchTimeExtractorConfiguration.IshRegex, text, true);
        if (match.getSuccess()) {
            final String hourStr = match.getMatch().get().getGroup(Constants.HourGroupName).value;
            int hour = Constants.HalfDayHourCount;

            if (!StringUtility.isNullOrEmpty(hourStr)) {
                hour = Integer.parseInt(hourStr);
            }

            result.setTimex(String.format("T%02d", hour));
            final LocalDateTime resultTime = DateUtil.safeCreateFromMinValue(
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
