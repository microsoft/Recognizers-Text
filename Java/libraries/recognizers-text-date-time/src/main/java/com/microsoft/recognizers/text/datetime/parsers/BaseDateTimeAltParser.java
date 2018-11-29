package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeAltParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.FormatUtil;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

import org.javatuples.Pair;

public class BaseDateTimeAltParser implements IDateTimeParser {

    private static final String parserName = Constants.SYS_DATETIME_DATETIMEALT;
    private final IDateTimeAltParserConfiguration config;

    public BaseDateTimeAltParser(IDateTimeAltParserConfiguration config) {
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
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        DateTimeResolutionResult value = null;
        if (er.type.equals(getParserName())) {
            DateTimeResolutionResult innerResult = parseDateTimeAndTimeAlt(er, reference);

            if (innerResult.getSuccess()) {
                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(
                er.start,
                er.length,
                er.text,
                er.type,
                er.data,
                value,
                "",
                value == null ? "" : value.getTimex());

        return ret;
    }

    // merge the entity with its related contexts and then parse the combine text
    public DateTimeResolutionResult parseDateTimeAndTimeAlt(ExtractResult er, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        // Original type of the extracted entity
        String subType = ((Map<String, Object>)(er.data)).get(Constants.SubType).toString();
        ExtractResult dateTimeEr = new ExtractResult();

        // e.g. {next week Mon} or {Tue}, formmer--"next week Mon" doesn't contain "context" key
        boolean hasContext = false;
        ExtractResult contextEr = null;
        if (((Map<String, Object>)er.data).containsKey(Constants.Context)) {
            contextEr = (ExtractResult)((Map<String, Object>)er.data).get(Constants.Context);
            if (contextEr.type.equals(Constants.ContextType_RelativeSuffix)) {
                dateTimeEr = dateTimeEr.withText(String.format("%s %s", er.text, contextEr.text));
            } else {
                dateTimeEr = dateTimeEr.withText(String.format("%s %s", contextEr.text, er.text));
            }

            hasContext = true;
        } else {
            dateTimeEr = dateTimeEr.withText(er.text);
        }

        DateTimeParseResult dateTimePr = null;

        if (subType.equals(Constants.SYS_DATETIME_DATE)) {
            dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATE);
            dateTimePr = this.config.getDateParser().parse(dateTimeEr, referenceTime);
        } else if (subType.equals(Constants.SYS_DATETIME_TIME)) {
            if (!hasContext) {
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_TIME);
                dateTimePr = this.config.getTimeParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.type.equals(Constants.SYS_DATETIME_DATE) || contextEr.type.equals(Constants.ContextType_RelativePrefix)) {
                // For cases:
                //      Monday 9 am or 11 am
                //      next 9 am or 11 am
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATETIME);
                dateTimePr = this.config.getDateTimeParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.type.equals(Constants.ContextType_AmPm)) {
                // For cases: in the afternoon 3 o'clock or 5 o'clock
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_TIME);
                dateTimePr = this.config.getTimeParser().parse(dateTimeEr, referenceTime);
            }
        } else if (subType.equals(Constants.SYS_DATETIME_DATETIME)) {
            // "next week Mon 9 am or Tue 1 pm"
            dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATETIME);
            dateTimePr = this.config.getDateTimeParser().parse(dateTimeEr, referenceTime);
        } else if (subType.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            if (!hasContext) {
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_TIMEPERIOD);
                dateTimePr = this.config.getTimePeriodParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.type.equals(Constants.SYS_DATETIME_DATE) || contextEr.type.equals(Constants.ContextType_RelativePrefix)) {
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATETIMEPERIOD);
                dateTimePr = this.config.getDateTimePeriodParser().parse(dateTimeEr, referenceTime);
            }
        } else if (subType.equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            if (!hasContext) {
                dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATETIMEPERIOD);
                dateTimePr = this.config.getDateTimePeriodParser().parse(dateTimeEr, referenceTime);
            }
        } else if (subType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            dateTimeEr = dateTimeEr.withType(Constants.SYS_DATETIME_DATEPERIOD);
            dateTimePr = this.config.getDatePeriodParser().parse(dateTimeEr, referenceTime);
        }

        if (dateTimePr != null && dateTimePr.value != null) {
            ret.setFutureValue(((DateTimeResolutionResult)dateTimePr.value).getFutureValue());
            ret.setPastValue(((DateTimeResolutionResult)dateTimePr.value).getPastValue());
            ret.setTimex(dateTimePr.timexStr);

            // Create resolution
            getResolution(er, dateTimePr, ret);

            ret.setSuccess(true);
        }

        return ret;
    }

    private void getResolution(ExtractResult er, DateTimeParseResult pr, DateTimeResolutionResult ret) {
        String parentText = ((Map<String, Object>)er.data).get(ExtendedModelResult.ParentTextKey).toString();
        String type = pr.type;

        boolean isPeriod = false;
        boolean isSinglePoint = false;
        String singlePointResolution = "";
        String pastStartPointResolution = "";
        String pastEndPointResolution = "";
        String futureStartPointResolution = "";
        String futureEndPointResolution = "";
        String singlePointType = "";
        String startPointType = "";
        String endPointType = "";

        if (type.equals(Constants.SYS_DATETIME_DATEPERIOD) || type.equalsIgnoreCase(Constants.SYS_DATETIME_TIMEPERIOD) ||
                type.equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            isPeriod = true;
            switch (type) {
                case Constants.SYS_DATETIME_DATEPERIOD:
                    startPointType = TimeTypeConstants.START_DATE;
                    endPointType = TimeTypeConstants.END_DATE;
                    pastStartPointResolution = FormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                    pastEndPointResolution = FormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                    futureStartPointResolution = FormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                    futureEndPointResolution = FormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    break;

                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    startPointType = TimeTypeConstants.START_DATETIME;
                    endPointType = TimeTypeConstants.END_DATETIME;
                    pastStartPointResolution = FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                    pastEndPointResolution = FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                    futureStartPointResolution = FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                    futureEndPointResolution = FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    break;

                case Constants.SYS_DATETIME_TIMEPERIOD:
                    startPointType = TimeTypeConstants.START_TIME;
                    endPointType = TimeTypeConstants.END_TIME;
                    pastStartPointResolution = FormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                    pastEndPointResolution = FormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                    futureStartPointResolution = FormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                    futureEndPointResolution = FormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    break;
                default:
                    break;
            }
        } else {
            isSinglePoint = true;
            switch (type) {
                case Constants.SYS_DATETIME_DATE:
                    singlePointType = TimeTypeConstants.DATE;
                    singlePointResolution = FormatUtil.formatDate((LocalDateTime)ret.getFutureValue());
                    break;

                case Constants.SYS_DATETIME_DATETIME:
                    singlePointType = TimeTypeConstants.DATETIME;
                    singlePointResolution = FormatUtil.formatDateTime((LocalDateTime)ret.getFutureValue());
                    break;

                case Constants.SYS_DATETIME_TIME:
                    singlePointType = TimeTypeConstants.TIME;
                    singlePointResolution = FormatUtil.formatTime((LocalDateTime)ret.getFutureValue());
                    break;
                default:
                    break;
            }
        }

        if (isPeriod) {
            ret.setFutureResolution(ImmutableMap.<String, String>builder()
                    .put(startPointType, futureStartPointResolution)
                    .put(endPointType, futureEndPointResolution)
                    .put(ExtendedModelResult.ParentTextKey, parentText)
                    .build());

            ret.setPastResolution(ImmutableMap.<String, String>builder()
                    .put(startPointType, pastStartPointResolution)
                    .put(endPointType, pastEndPointResolution)
                    .put(ExtendedModelResult.ParentTextKey, parentText)
                    .build());
        } else if (isSinglePoint) {
            ret.setFutureResolution(ImmutableMap.<String, String>builder()
                    .put(singlePointType, singlePointResolution)
                    .put(ExtendedModelResult.ParentTextKey, parentText)
                    .build());

            ret.setPastResolution(ImmutableMap.<String, String>builder()
                    .put(singlePointType, singlePointResolution)
                    .put(ExtendedModelResult.ParentTextKey, parentText)
                    .build());
        }
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {

        return candidateResults;
    }
}
