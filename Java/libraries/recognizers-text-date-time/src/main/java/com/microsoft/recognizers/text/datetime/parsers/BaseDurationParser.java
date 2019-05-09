package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IDurationParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;

public class BaseDurationParser implements IDateTimeParser {

    private final IDurationParserConfiguration config;

    public BaseDurationParser(IDurationParserConfiguration configuration) {
        this.config = configuration;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_DURATION;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {

        Object value = null;

        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult;

            innerResult = parseMergedDuration(er.getText(), reference);

            if (!innerResult.getSuccess()) {
                innerResult = parseNumberWithUnit(er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseImplicitDuration(er.getText(), reference);
            }

            if (innerResult.getSuccess()) {
                innerResult.setFutureResolution(ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.DURATION, StringUtility.format((Double)innerResult.getFutureValue()))
                        .build());

                innerResult.setPastResolution(ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.DURATION, StringUtility.format((Double)innerResult.getPastValue()))
                        .build());

                if (er.getData() != null) {
                    if (er.getData().equals(Constants.MORE_THAN_MOD)) {
                        innerResult.setMod(Constants.MORE_THAN_MOD);
                    } else if (er.getData().equals(Constants.LESS_THAN_MOD)) {
                        innerResult.setMod(Constants.LESS_THAN_MOD);
                    }
                }

                value = innerResult;
            }
        }

        DateTimeParseResult result = new DateTimeParseResult(
                er.getStart(),
                er.getLength(),
                er.getText(),
                er.getType(),
                er.getData(),
                value,
                "",
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex()
        );

        return result;
    }

    private DateTimeResolutionResult parseMergedDuration(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        IExtractor durationExtractor = config.getDurationExtractor();

        // DurationExtractor without parameter will not extract merged duration
        List<ExtractResult> ers = durationExtractor.extract(text);

        // only handle merged duration cases like "1 month 21 days"
        if (ers.size() <= 1) {
            result.setSuccess(false);
            return result;
        }

        int start = ers.get(0).getStart();
        if (start != 0) {
            String beforeStr = text.substring(0, start - 1);
            if (!StringUtility.isNullOrWhiteSpace(beforeStr)) {
                return result;
            }
        }

        int end = ers.get(ers.size() - 1).getStart() + ers.get(ers.size() - 1).getLength();
        if (end != text.length()) {
            String afterStr = text.substring(end);
            if (!StringUtility.isNullOrWhiteSpace(afterStr)) {
                return result;
            }
        }

        List<DateTimeParseResult> prs = new ArrayList<>();
        Map<String, String> timexMap = new HashMap<>();

        // insert timex into a dictionary
        for (ExtractResult er : ers) {
            Pattern unitRegex = config.getDurationUnitRegex();
            Optional<Match> unitMatch = Arrays.stream(RegExpUtility.getMatches(unitRegex, er.getText())).findFirst();
            if (unitMatch.isPresent()) {
                DateTimeParseResult pr = (DateTimeParseResult)parse(er);
                if (pr.getValue() != null) {
                    timexMap.put(unitMatch.get().getGroup("unit").value, pr.getTimexStr());
                    prs.add(pr);
                }
            }
        }

        // sort the timex using the granularity of the duration, "P1M23D" for "1 month 23 days" and "23 days 1 month"
        if (prs.size() == ers.size()) {

            result.setTimex(TimexUtility.generateCompoundDurationTimex(timexMap, config.getUnitValueMap()));

            double value = 0;
            for (DateTimeParseResult pr : prs) {
                value += Double.parseDouble(((DateTimeResolutionResult)pr.getValue()).getFutureValue().toString());
            }

            result.setFutureValue(value);
            result.setPastValue(value);
        }

        result.setSuccess(true);
        return result;
    }

    private DateTimeResolutionResult parseNumberWithUnit(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = parseNumberSpaceUnit(text);
        if (!result.getSuccess()) {
            result = parseNumberCombinedUnit(text);
        }

        if (!result.getSuccess()) {
            result = parseAnUnit(text);
        }

        if (!result.getSuccess()) {
            result = parseInexactNumberUnit(text);
        }

        return result;
    }

    // check {and} suffix after a {number} {unit}
    private double parseNumberWithUnitAndSuffix(String text) {
        double numVal = 0;

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getSuffixAndRegex(), text)).findFirst();
        if (match.isPresent()) {
            String numStr = match.get().getGroup("suffix_num").value.toLowerCase();

            if (config.getDoubleNumbers().containsKey(numStr)) {
                numVal = config.getDoubleNumbers().get(numStr);
            }
        }

        return numVal;
    }

    private DateTimeResolutionResult parseNumberSpaceUnit(String text) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        // if there are spaces between nubmer and unit
        List<ExtractResult> ers = config.getCardinalExtractor().extract(text);
        if (ers.size() == 1) {
            ExtractResult er = ers.get(0);
            ParseResult pr = config.getNumberParser().parse(er);

            // followed unit: {num} (<followed unit>and a half hours)
            String srcUnit = "";
            String noNum = text.substring(er.getStart() + er.getLength()).trim().toLowerCase();
            String suffixStr = text;

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getFollowedUnit(), noNum)).findFirst();
            if (match.isPresent()) {
                srcUnit = match.get().getGroup("unit").value.toLowerCase();
                suffixStr = match.get().getGroup(Constants.SuffixGroupName).value.toLowerCase();
            }

            if (match.isPresent() && !StringUtility.isNullOrEmpty(match.get().getGroup(Constants.BusinessDayGroupName).value)) {
                int numVal = Math.round(Double.valueOf(pr.getValue().toString()).floatValue());

                String timex = TimexUtility.generateDurationTimex(numVal, Constants.TimexBusinessDay, false);
                double timeValue = numVal * config.getUnitValueMap().get(srcUnit.split(" ")[1]);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
            }

            if (config.getUnitMap().containsKey(srcUnit)) {
                double numVal = Double.parseDouble(pr.getValue().toString()) + parseNumberWithUnitAndSuffix(suffixStr);

                String unitStr = config.getUnitMap().get(srcUnit);

                String timex = TimexUtility.generateDurationTimex(numVal, unitStr, isLessThanDay(unitStr));
                double timeValue = numVal * config.getUnitValueMap().get(srcUnit);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseNumberCombinedUnit(String text) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        String suffixStr = text;

        // if there are NO spaces between number and unit
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getNumberCombinedWithUnit(), text)).findFirst();
        if (match.isPresent()) {
            Double numVal = Double.parseDouble(match.get().getGroup("num").value) + parseNumberWithUnitAndSuffix(suffixStr);
            String numStr = StringUtility.format(numVal);

            String srcUnit = match.get().getGroup("unit").value.toLowerCase();

            if (config.getUnitMap().containsKey(srcUnit)) {
                String unitStr = config.getUnitMap().get(srcUnit);

                if ((numVal > 1000) && (unitStr.equals("Y") || unitStr.equals("MON") || unitStr.equals("W"))) {
                    return result;
                }

                String timex = String.format("P%s%s%c", isLessThanDay(unitStr) ? "T" : "", numStr, unitStr.charAt(0));
                double timeValue = numVal * config.getUnitValueMap().get(srcUnit);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
                return result;
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseAnUnit(String text) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        String suffixStr = text;

        // if there are NO spaces between number and unit
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getAnUnitRegex(), text)).findFirst();
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getHalfDateUnitRegex(), text)).findFirst();
        }

        if (match.isPresent()) {
            double numVal = StringUtility.isNullOrEmpty(match.get().getGroup("half").value) ? 1 : 0.5;
            numVal += parseNumberWithUnitAndSuffix(suffixStr);
            String numStr = StringUtility.format(numVal);

            String srcUnit = match.get().getGroup("unit").value.toLowerCase();

            if (config.getUnitMap().containsKey(srcUnit)) {
                String unitStr = config.getUnitMap().get(srcUnit);

                double timeValue = numVal * config.getUnitValueMap().get(srcUnit);

                result.setTimex(TimexUtility.generateDurationTimex(numVal, unitStr, isLessThanDay(unitStr)));
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);

            } else if (!StringUtility.isNullOrEmpty(match.get().getGroup(Constants.BusinessDayGroupName).value)) {
                String timex = TimexUtility.generateDurationTimex(numVal, Constants.TimexBusinessDay, false);
                double timeValue = numVal * config.getUnitValueMap().get(srcUnit.split(" ")[1]);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseInexactNumberUnit(String text) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getInexactNumberUnitRegex(), text)).findFirst();
        if (match.isPresent()) {
            double numVal;

            if (!StringUtility.isNullOrEmpty(match.get().getGroup("NumTwoTerm").value)) {
                numVal = 2;
            } else {
                // set the inexact number "few", "some" to 3 for now
                numVal = 3;
            }

            String numStr = StringUtility.format(numVal);

            String srcUnit = match.get().getGroup("unit").value.toLowerCase();

            if (config.getUnitMap().containsKey(srcUnit)) {
                String unitStr = config.getUnitMap().get(srcUnit);

                String timex = String.format("P%s%s%c", isLessThanDay(unitStr) ? "T" : "", numStr, unitStr.charAt(0));
                double timeValue = numVal * config.getUnitValueMap().get(srcUnit);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
                return result;
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseImplicitDuration(String text, LocalDateTime reference) {
        // handle "all day" "all year"
        DateTimeResolutionResult result = getResultFromRegex(config.getAllDateUnitRegex(), text, "1");

        // handle "during/for the day/week/month/year"
        if (config.getOptions().match(DateTimeOptions.CalendarMode) && !result.getSuccess()) {
            result = getResultFromRegex(config.getDuringRegex(), text, "1");
        }

        // handle "half day", "half year"
        if (!result.getSuccess()) {
            result = getResultFromRegex(config.getHalfDateUnitRegex(), text, "0.5");
        }

        // handle single duration unit, it is filtered in the extraction that there is a relative word in advance
        if (!result.getSuccess()) {
            result = getResultFromRegex(config.getFollowedUnit(), text, "1");
        }

        return result;
    }

    private DateTimeResolutionResult getResultFromRegex(Pattern pattern, String text, String numStr) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(pattern, text)).findFirst();
        if (match.isPresent()) {
            String srcUnit = match.get().getGroup("unit").value.toLowerCase();
            if (config.getUnitMap().containsKey(srcUnit)) {
                String unitStr = config.getUnitMap().get(srcUnit);

                String timex = String.format("P%s%s%c", isLessThanDay(unitStr) ? "T" : "", numStr, unitStr.charAt(0));
                double timeValue = Double.parseDouble(numStr) * config.getUnitValueMap().get(srcUnit);

                result.setTimex(timex);
                result.setFutureValue(timeValue);
                result.setPastValue(timeValue);

                result.setSuccess(true);
            }
        }

        return result;
    }

    private boolean isLessThanDay(String unit) {
        return unit.equals("S") || unit.equals("M") || unit.equals("H");
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }
}
