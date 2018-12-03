package com.microsoft.recognizers.text.datetime.parsers;

import static java.lang.Integer.parseInt;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.FormatUtil;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Optional;
import java.util.function.IntFunction;
import java.util.regex.Pattern;
import java.util.stream.StreamSupport;

public class BaseHolidayParser implements IDateTimeParser {

    private final IHolidayParserConfiguration config;

    public BaseHolidayParser(IHolidayParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_DATE;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        throw new UnsupportedOperationException();
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        
        LocalDateTime referenceDate = reference;
        Object value = null;

        if (er.type.equals(getParserName())) {
            
            DateTimeResolutionResult innerResult = parseHolidayRegexMatch(er.text, referenceDate);

            if (innerResult.getSuccess()) {
                HashMap<String, String> futureResolution = new HashMap<>();
                futureResolution.put(TimeTypeConstants.DATE, FormatUtil.formatDate((LocalDateTime)innerResult.getFutureValue()));
                innerResult.setFutureResolution(futureResolution);

                HashMap<String, String> pastResolution = new HashMap<>();
                pastResolution.put(TimeTypeConstants.DATE, FormatUtil.formatDate((LocalDateTime)innerResult.getPastValue()));
                innerResult.setPastResolution(pastResolution);
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
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex()
        );

        return ret;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    private DateTimeResolutionResult parseHolidayRegexMatch(String text, LocalDateTime referenceDate) {
        
        String trimmedText = StringUtility.trimEnd(StringUtility.trimEnd(text));
        for (Pattern pattern : this.config.getHolidayRegexList()) {
            
            int offset = 0;
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(pattern, text)).findFirst();
            if (match.isPresent() && match.get().index == offset && match.get().length == trimmedText.length()) {
                // LUIS value string will be set in Match2Date method
                DateTimeResolutionResult ret = match2Date(match.get(), referenceDate);

                return ret;
            }
        }

        return new DateTimeResolutionResult();
    }

    private DateTimeResolutionResult match2Date(Match match, LocalDateTime referenceDate) {
        
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        String holidayStr = this.config.sanitizeHolidayToken(match.getGroup("holiday").value.toLowerCase(Locale.ROOT));

        // get year (if exist)
        String yearStr = match.getGroup("year").value.toLowerCase();
        String orderStr = match.getGroup("order").value.toLowerCase();
        int year;
        boolean hasYear = false;

        if (!StringUtility.isNullOrEmpty(yearStr)) {
            year = parseInt(yearStr);
            hasYear = true;
        } else if (!StringUtility.isNullOrEmpty(orderStr)) {
            int swift = this.config.getSwiftYear((orderStr));
            if (swift < -1) {
                return  ret;
            }

            year = referenceDate.getYear() + swift;
            hasYear = true;
        } else {
            year = referenceDate.getYear();
        }

        String holidayKey = "";
        for (ImmutableMap.Entry<String, Iterable<String>> holidayPair : this.config.getHolidayNames().entrySet()) {
            if (StreamSupport.stream(holidayPair.getValue().spliterator(), false).anyMatch(name -> holidayStr.equals(name))) {
                holidayKey = holidayPair.getKey();
                break;
            }
        }

        String timexStr = "";
        if (!StringUtility.isNullOrEmpty(holidayKey)) {
            
            LocalDateTime value = referenceDate;
            IntFunction<LocalDateTime> function = this.config.getHolidayFuncDictionary().get(holidayKey);
            if (function != null) {
                
                value = function.apply(year);

                timexStr = this.config.getVariableHolidaysTimexDictionary().get(holidayKey);
                if (StringUtility.isNullOrEmpty(timexStr)) {
                    timexStr = String.format("-%02d-%02d", value.getMonthValue(), value.getDayOfMonth());
                }
            }

            if (function == null) {
                return ret;
            }

            if (value.equals(DateUtil.minValue())) {
                ret.setTimex("");
                ret.setPastValue(DateUtil.minValue());
                ret.setFutureValue(DateUtil.minValue());
                ret.setSuccess(true);
                return ret;
            }

            if (hasYear) {
                ret.setTimex(String.format("%04d", year) + timexStr);
                ret.setFutureValue(DateUtil.safeCreateFromMinValue(year, value.getMonthValue(), value.getDayOfMonth()));
                ret.setPastValue(DateUtil.safeCreateFromMinValue(year, value.getMonthValue(), value.getDayOfMonth()));
                ret.setSuccess(true);
                return ret;
            }

            ret.setTimex("XXXX" + timexStr);
            ret.setFutureValue(getFutureValue(value, referenceDate, holidayKey));
            ret.setPastValue(getPastValue(value, referenceDate, holidayKey));
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }
    
    private LocalDateTime getFutureValue(LocalDateTime value, LocalDateTime referenceDate, String holiday) {

        if (value.isBefore(referenceDate)) {
            IntFunction<LocalDateTime> function = this.config.getHolidayFuncDictionary().get(holiday);
            if (function != null) {
                return function.apply(value.getYear() + 1);
            }
        }

        return value;
    }

    private LocalDateTime getPastValue(LocalDateTime value, LocalDateTime referenceDate, String holiday) {

        if (value.isAfter(referenceDate) || value == referenceDate) {
            IntFunction<LocalDateTime> function = this.config.getHolidayFuncDictionary().get(holiday);
            if (function != null) {
                return function.apply(value.getYear() - 1);
            }
        }

        return value;
    }
}
