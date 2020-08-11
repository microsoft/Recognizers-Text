package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeAltParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;

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
        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult = parseDateTimeAndTimeAlt(er, reference);

            if (innerResult.getSuccess()) {
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
                value == null ? "" : value.getTimex());

        return ret;
    }

    // merge the entity with its related contexts and then parse the combine text
    private DateTimeResolutionResult parseDateTimeAndTimeAlt(ExtractResult er, LocalDateTime referenceTime) {

        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        // Original type of the extracted entity
        String subType = ((Map<String, Object>)(er.getData())).get(Constants.SubType).toString();
        ExtractResult dateTimeEr = new ExtractResult();

        // e.g. {next week Mon} or {Tue}, formmer--"next week Mon" doesn't contain "context" key
        boolean hasContext = false;
        ExtractResult contextEr = null;
        if (((Map<String, Object>)er.getData()).containsKey(Constants.Context)) {
            contextEr = (ExtractResult)((Map<String, Object>)er.getData()).get(Constants.Context);
            if (contextEr.getType().equals(Constants.ContextType_RelativeSuffix)) {
                dateTimeEr.setText(String.format("%s %s", er.getText(), contextEr.getText()));
            } else {
                dateTimeEr.setText(String.format("%s %s", contextEr.getText(), er.getText()));
            }

            hasContext = true;
        } else {
            dateTimeEr.setText(er.getText());
        }

        dateTimeEr.setData(er.getData());
        DateTimeParseResult dateTimePr = null;

        if (subType.equals(Constants.SYS_DATETIME_DATE)) {
            dateTimeEr.setType(Constants.SYS_DATETIME_DATE);
            dateTimePr = this.config.getDateParser().parse(dateTimeEr, referenceTime);
        } else if (subType.equals(Constants.SYS_DATETIME_TIME)) {
            if (!hasContext) {
                dateTimeEr.setType(Constants.SYS_DATETIME_TIME);
                dateTimePr = this.config.getTimeParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.getType().equals(Constants.SYS_DATETIME_DATE) || contextEr.getType().equals(Constants.ContextType_RelativePrefix)) {
                // For cases:
                //      Monday 9 am or 11 am
                //      next 9 am or 11 am
                dateTimeEr.setType(Constants.SYS_DATETIME_DATETIME);
                dateTimePr = this.config.getDateTimeParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.getType().equals(Constants.ContextType_AmPm)) {
                // For cases: in the afternoon 3 o'clock or 5 o'clock
                dateTimeEr.setType(Constants.SYS_DATETIME_TIME);
                dateTimePr = this.config.getTimeParser().parse(dateTimeEr, referenceTime);
            }
        } else if (subType.equals(Constants.SYS_DATETIME_DATETIME)) {
            // "next week Mon 9 am or Tue 1 pm"
            dateTimeEr.setType(Constants.SYS_DATETIME_DATETIME);
            dateTimePr = this.config.getDateTimeParser().parse(dateTimeEr, referenceTime);
        } else if (subType.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            if (!hasContext) {
                dateTimeEr.setType(Constants.SYS_DATETIME_TIMEPERIOD);
                dateTimePr = this.config.getTimePeriodParser().parse(dateTimeEr, referenceTime);
            } else if (contextEr.getType().equals(Constants.SYS_DATETIME_DATE) || contextEr.getType().equals(Constants.ContextType_RelativePrefix)) {
                dateTimeEr.setType(Constants.SYS_DATETIME_DATETIMEPERIOD);
                dateTimePr = this.config.getDateTimePeriodParser().parse(dateTimeEr, referenceTime);
            }
        } else if (subType.equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            dateTimeEr.setType(Constants.SYS_DATETIME_DATETIMEPERIOD);
            dateTimePr = this.config.getDateTimePeriodParser().parse(dateTimeEr, referenceTime);
        } else if (subType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            dateTimeEr.setType(Constants.SYS_DATETIME_DATEPERIOD);
            dateTimePr = this.config.getDatePeriodParser().parse(dateTimeEr, referenceTime);
        }

        if (dateTimePr != null && dateTimePr.getValue() != null) {
            ret.setFutureValue(((DateTimeResolutionResult)dateTimePr.getValue()).getFutureValue());
            ret.setPastValue(((DateTimeResolutionResult)dateTimePr.getValue()).getPastValue());
            ret.setTimex(dateTimePr.getTimexStr());

            // Create resolution
            getResolution(er, dateTimePr, ret);

            ret.setSuccess(true);
        }

        return ret;
    }

    private void getResolution(ExtractResult er, DateTimeParseResult pr, DateTimeResolutionResult ret) {
        String parentText = ((Map<String, Object>)er.getData()).get(ExtendedModelResult.ParentTextKey).toString();
        String type = pr.getType();

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
                    pastStartPointResolution = DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                    pastEndPointResolution = DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                    futureStartPointResolution = DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                    futureEndPointResolution = DateTimeFormatUtil.formatDate(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    break;

                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    startPointType = TimeTypeConstants.START_DATETIME;
                    endPointType = TimeTypeConstants.END_DATETIME;

                    if (ret.getPastValue() instanceof Pair<?, ?>) {
                        pastStartPointResolution = DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                        pastEndPointResolution = DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                        futureStartPointResolution = DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                        futureEndPointResolution = DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    } else if (ret.getPastValue() instanceof LocalDateTime) {
                        pastStartPointResolution = DateTimeFormatUtil.formatDateTime((LocalDateTime)ret.getPastValue());
                        futureStartPointResolution = DateTimeFormatUtil.formatDateTime((LocalDateTime)ret.getFutureValue());
                    }

                    break;

                case Constants.SYS_DATETIME_TIMEPERIOD:
                    startPointType = TimeTypeConstants.START_TIME;
                    endPointType = TimeTypeConstants.END_TIME;
                    pastStartPointResolution = DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue0());
                    pastEndPointResolution = DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getPastValue()).getValue1());
                    futureStartPointResolution = DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue0());
                    futureEndPointResolution = DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)ret.getFutureValue()).getValue1());
                    break;
                default:
                    break;
            }
        } else {
            isSinglePoint = true;
            switch (type) {
                case Constants.SYS_DATETIME_DATE:
                    singlePointType = TimeTypeConstants.DATE;
                    singlePointResolution = DateTimeFormatUtil.formatDate((LocalDateTime)ret.getFutureValue());
                    break;

                case Constants.SYS_DATETIME_DATETIME:
                    singlePointType = TimeTypeConstants.DATETIME;
                    singlePointResolution = DateTimeFormatUtil.formatDateTime((LocalDateTime)ret.getFutureValue());
                    break;

                case Constants.SYS_DATETIME_TIME:
                    singlePointType = TimeTypeConstants.TIME;
                    singlePointResolution = DateTimeFormatUtil.formatTime((LocalDateTime)ret.getFutureValue());
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

        if (((DateTimeResolutionResult)pr.getValue()).getMod() != null) {
            ret.setMod(((DateTimeResolutionResult)pr.getValue()).getMod());
        }

        if (((DateTimeResolutionResult)pr.getValue()).getTimeZoneResolution() != null) {
            ret.setTimeZoneResolution(((DateTimeResolutionResult)pr.getValue()).getTimeZoneResolution());
        }
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {

        return candidateResults;
    }
}
