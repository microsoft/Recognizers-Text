package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.DateTimeResolutionKey;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.FormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.SortedMap;
import java.util.TreeMap;
import java.util.stream.Collectors;

public class BaseMergedDateTimeParser implements IDateTimeParser {

    private final String parserName = "datetimeV2";
    private final IMergedParserConfiguration config;
    private static final String dateMinString = FormatUtil.formatDate(DateUtil.minValue());
    private static final String dateTimeMinString = FormatUtil.formatDateTime(DateUtil.minValue());
    //private static final Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

    public BaseMergedDateTimeParser(IMergedParserConfiguration config) {
        this.config = config;
    }

    public String getDateMinString() {
        return dateMinString;
    }

    public String getDateTimeMinString() {
        return dateTimeMinString;
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

        DateTimeParseResult pr = null;

        String originText = er.getText();
        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            String newText = MatchingUtil.preProcessTextRemoveSuperfluousWords(er.getText(), config.getSuperfluousWordMatcher()).getText();
            int newLength = er.getLength() + er.getText().length() - originText.length();
            er = new ExtractResult(er.getStart(), newLength, newText, er.getType(), er.getData());
        }

        // Push, save the MOD string
        boolean hasBefore = false;
        boolean hasAfter = false;
        boolean hasSince = false;
        boolean hasAround = false;
        boolean hasYearAfter = false;

        // "InclusiveModifier" means MOD should include the start/end time
        // For example, cases like "on or later than", "earlier than or in" have inclusive modifier
        boolean hasInclusiveModifier = false;
        String modStr = "";
        ConditionalMatch beforeMatch = RegexExtension.matchBegin(config.getBeforeRegex(), er.getText(), true);
        ConditionalMatch afterMatch = RegexExtension.matchBegin(config.getAfterRegex(), er.getText(), true);
        ConditionalMatch sinceMatch = RegexExtension.matchBegin(config.getSinceRegex(), er.getText(), true);
        ConditionalMatch aroundMatch = RegexExtension.matchBegin(config.getAroundRegex(), er.getText(), true);

        if (beforeMatch.getSuccess()) {
            hasBefore = true;
            er.setStart(er.getStart() + beforeMatch.getMatch().get().length);
            er.setLength(er.getLength() - beforeMatch.getMatch().get().length);
            er.setText(er.getText().substring(beforeMatch.getMatch().get().length));
            modStr = beforeMatch.getMatch().get().value;

            if (!StringUtility.isNullOrEmpty(beforeMatch.getMatch().get().getGroup("include").value)) {
                hasInclusiveModifier = true;
            }
        } else if (afterMatch.getSuccess()) {
            hasAfter = true;
            er.setStart(er.getStart() + afterMatch.getMatch().get().length);
            er.setLength(er.getLength() - afterMatch.getMatch().get().length);
            er.setText(er.getText().substring(afterMatch.getMatch().get().length));
            modStr = afterMatch.getMatch().get().value;

            if (!StringUtility.isNullOrEmpty(afterMatch.getMatch().get().getGroup("include").value)) {
                hasInclusiveModifier = true;
            }
        } else if (sinceMatch.getSuccess()) {
            hasSince = true;
            er.setStart(er.getStart() + sinceMatch.getMatch().get().length);
            er.setLength(er.getLength() - sinceMatch.getMatch().get().length);
            er.setText(er.getText().substring(sinceMatch.getMatch().get().length));
            modStr = sinceMatch.getMatch().get().value;
        } else if (aroundMatch.getSuccess()) {
            hasAround = true;
            er.setStart(er.getStart() + aroundMatch.getMatch().get().length);
            er.setLength(er.getLength() - aroundMatch.getMatch().get().length);
            er.setText(er.getText().substring(aroundMatch.getMatch().get().length));
            modStr = aroundMatch.getMatch().get().value;
        } else if ((er.getType().equals(Constants.SYS_DATETIME_DATEPERIOD) &&
                Arrays.stream(RegExpUtility.getMatches(config.getYearRegex(), er.getText())).findFirst().isPresent()) ||
                (er.getType().equals(Constants.SYS_DATETIME_DATE))) {
            // This has to be put at the end of the if, or cases like "before 2012" and "after 2012" would fall into this
            // 2012 or after/above
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateAfterRegex(), er.getText())).findFirst();
            if (match.isPresent() && er.getText().endsWith(match.get().value)) {
                hasYearAfter = true;
                er.setLength(er.getLength() - match.get().length);
                er.setText(er.getLength() > 0 ? er.getText().substring(0, er.getLength()) : "");
                modStr = match.get().value;
            }
        }

        if (er.getType().equals(Constants.SYS_DATETIME_DATE)) {
            pr = this.config.getDateParser().parse(er, reference);
            if (pr.getValue() == null) {
                pr = config.getHolidayParser().parse(er, reference);
            }
        } else if (er.getType().equals(Constants.SYS_DATETIME_TIME)) {
            pr = this.config.getTimeParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_DATETIME)) {
            pr = this.config.getDateTimeParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            pr = this.config.getDatePeriodParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            pr = this.config.getTimePeriodParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            pr = this.config.getDateTimePeriodParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_DURATION)) {
            pr = this.config.getDurationParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_SET)) {
            pr = this.config.getGetParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_DATETIMEALT)) {
            pr = this.config.getDateTimeAltParser().parse(er, reference);
        } else if (er.getType().equals(Constants.SYS_DATETIME_TIMEZONE)) {
            if (config.getOptions().match(DateTimeOptions.EnablePreview)) {
                pr = this.config.getTimeZoneParser().parse(er, reference);
            }
        } else {
            return null;
        }

        // Pop, restore the MOD string
        if (hasBefore && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();

            if (!hasInclusiveModifier) {
                val.setMod(Constants.BEFORE_MOD);
            } else {
                val.setMod(Constants.UNTIL_MOD);
            }

            pr.setValue(val);
        }

        if (hasAfter && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();

            if (!hasInclusiveModifier) {
                val.setMod(Constants.AFTER_MOD);
            } else {
                val.setMod(Constants.SINCE_MOD);
            }

            pr.setValue(val);
        }

        if (hasSince && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(Constants.SINCE_MOD);
            pr.setValue(val);
        }

        if (hasAround && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(Constants.APPROX_MOD);
            pr.setValue(val);
        }

        if (hasYearAfter && pr != null && pr.getValue() != null) {

            pr.setText(pr.getText() + modStr);
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(Constants.SINCE_MOD);
            pr.setValue(val);
            hasSince = true;
        }

        if (config.getOptions().match(DateTimeOptions.SplitDateAndTime) && pr != null && pr.getValue() != null &&
                ((DateTimeResolutionResult)pr.getValue()).getSubDateTimeEntities() != null) {
            pr.setValue(dateTimeResolutionForSplit(pr));
        } else {
            boolean hasModifier = hasBefore || hasAfter || hasSince;
            pr = setParseResult(pr, hasModifier);
        }

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            int prLength = pr.getLength() + originText.length() - pr.getText().length();
            pr = new DateTimeParseResult(pr.getStart(), prLength, originText, pr.getType(), pr.getData(), pr.getValue(), pr.getResolutionStr(), pr.getTimexStr());
        }

        return pr;
    }


    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        if (config.getAmbiguousMonthP0Regex() != null) {
            if (candidateResults != null && !candidateResults.isEmpty()) {

                List<Match> matches = Arrays.asList(RegExpUtility.getMatches(config.getAmbiguousMonthP0Regex(), query));

                for (Match match : matches) {
                    // Check for intersections/overlaps
                    candidateResults = candidateResults.stream().filter(
                        c -> filterResultsPredicate(c, match))
                        .collect(Collectors.toList());
                }

            }
        }

        return candidateResults;
    }

    private boolean filterResultsPredicate(DateTimeParseResult pr, Match match) {
        return !(match.index < pr.getStart() + pr.getLength() && pr.getStart() < match.index + match.length);
    }

    public DateTimeParseResult setParseResult(DateTimeParseResult slot, boolean hasMod) {
        SortedMap<String, Object> slotValue = dateTimeResolution(slot);
        // Change the type at last for the after or before modes
        String type = String.format("%s.%s", parserName, determineDateTimeType(slot.getType(), hasMod));

        slot.setValue(slotValue);
        slot.setType(type);

        return slot;
    }

    public String determineDateTimeType(String type, boolean hasMod) {
        if (config.getOptions().match(DateTimeOptions.SplitDateAndTime)) {
            if (type.equals(Constants.SYS_DATETIME_DATETIME)) {
                return Constants.SYS_DATETIME_TIME;
            }
        } else {
            if (hasMod) {
                if (type.equals(Constants.SYS_DATETIME_DATE)) {
                    return Constants.SYS_DATETIME_DATEPERIOD;
                }

                if (type.equals(Constants.SYS_DATETIME_TIME)) {
                    return Constants.SYS_DATETIME_TIMEPERIOD;
                }

                if (type.equals(Constants.SYS_DATETIME_DATETIME)) {
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;
                }
            }
        }

        return type;
    }

    public List<DateTimeParseResult> dateTimeResolutionForSplit(DateTimeParseResult slot) {
        List<DateTimeParseResult> results = new ArrayList<>();
        if (((DateTimeResolutionResult)slot.getValue()).getSubDateTimeEntities() != null) {
            List<Object> subEntities = ((DateTimeResolutionResult)slot.getValue()).getSubDateTimeEntities();
            for (Object subEntity : subEntities) {
                DateTimeParseResult result = (DateTimeParseResult)subEntity;
                result.setStart(result.getStart() + slot.getStart());
                results.addAll(dateTimeResolutionForSplit(result));
            }
        } else {
            slot.setValue(dateTimeResolution(slot));
            slot.setType(String.format("%s.%s",parserName, determineDateTimeType(slot.getType(), false)));

            results.add(slot);
        }

        return results;
    }

    public SortedMap<String, Object> dateTimeResolution(DateTimeParseResult slot) {
        if (slot == null) {
            return null;
        }

        List<Map<String, String>> resolutions = new ArrayList<>();
        Map<String, Object> res = new HashMap<>();

        DateTimeResolutionResult val = (DateTimeResolutionResult)slot.getValue();
        if (val == null) {
            return null;
        }

        Boolean islunar = val.getIsLunar() != null ? val.getIsLunar() : false;
        String mod = val.getMod();

        String list = null;

        // Resolve dates list for date periods
        if (slot.getType().equals(Constants.SYS_DATETIME_DATEPERIOD) && val.getList() != null) {
            list = String.join(",", val.getList().stream().map(o -> DateTimeFormatUtil.luisDate((LocalDateTime)o)).collect(Collectors.toList()));
        }

        // With modifier, output Type might not be the same with type in resolution comments
        // For example, if the resolution type is "date", with modifier the output type should be "daterange"
        String typeOutput = determineDateTimeType(slot.getType(), !StringUtility.isNullOrEmpty(mod));
        String comment = val.getComment();

        String type = slot.getType();
        String timex = slot.getTimexStr();

        // The following should be added to res first, since ResolveAmPm requires these fields.
        addResolutionFields(res, DateTimeResolutionKey.Timex, timex);
        addResolutionFields(res, Constants.Comment, comment);
        addResolutionFields(res, DateTimeResolutionKey.Mod, mod);
        addResolutionFields(res, ResolutionKey.Type, typeOutput);
        addResolutionFields(res, DateTimeResolutionKey.IsLunar, islunar ? islunar.toString() : "");

        boolean hasTimeZone = false;

        // For standalone timezone entity recognition, we generate TimeZoneResolution for each entity we extracted.
        // We also merge time entity with timezone entity and add the information in TimeZoneResolution to every DateTime resolutions.
        if (val.getTimeZoneResolution() != null) {
            if (slot.getType().equals(Constants.SYS_DATETIME_TIMEZONE)) {
                // single timezone
                Map<String, String> resolutionField = new LinkedHashMap<>();
                resolutionField.put(ResolutionKey.Value, val.getTimeZoneResolution().getValue());
                resolutionField.put(Constants.UtcOffsetMinsKey, val.getTimeZoneResolution().getUtcOffsetMins().toString());

                addResolutionFields(res, Constants.ResolveTimeZone, resolutionField);
            } else {
                // timezone as clarification of datetime
                hasTimeZone = true;
                addResolutionFields(res, Constants.TimeZone, val.getTimeZoneResolution().getValue());
                addResolutionFields(res, Constants.TimeZoneText, val.getTimeZoneResolution().getTimeZoneText());
                addResolutionFields(res, Constants.UtcOffsetMinsKey, val.getTimeZoneResolution().getUtcOffsetMins().toString());
            }
        }

        LinkedHashMap<String, String> pastResolutionStr = new LinkedHashMap<>();
        pastResolutionStr.putAll(((DateTimeResolutionResult)slot.getValue()).getPastResolution());
        Map<String, String> futureResolutionStr = ((DateTimeResolutionResult)slot.getValue()).getFutureResolution();

        if (typeOutput.equals(Constants.SYS_DATETIME_DATETIMEALT) && pastResolutionStr.size() > 0) {
            typeOutput = determineResolutionDateTimeType(pastResolutionStr);
        }

        Map<String, String> resolutionPast = generateResolution(type, pastResolutionStr, mod);
        Map<String, String> resolutionFuture = generateResolution(type, futureResolutionStr, mod);

        // If past and future are same, keep only one
        if (resolutionFuture.equals(resolutionPast)) {
            if (resolutionPast.size() > 0) {
                addResolutionFields(res, Constants.Resolve, resolutionPast);
            }
        } else {
            if (resolutionPast.size() > 0) {
                addResolutionFields(res, Constants.ResolveToPast, resolutionPast);
            }

            if (resolutionFuture.size() > 0) {
                addResolutionFields(res, Constants.ResolveToFuture, resolutionFuture);
            }
        }

        // If 'ampm', double our resolution accordingly
        if (!StringUtility.isNullOrEmpty(comment) && comment.equals(Constants.Comment_AmPm)) {
            if (res.containsKey(Constants.Resolve)) {
                resolveAmPm(res, Constants.Resolve);
            } else {
                resolveAmPm(res, Constants.ResolveToPast);
                resolveAmPm(res, Constants.ResolveToFuture);
            }
        }

        // If WeekOf and in CalendarMode, modify the past part of our resolution
        if (config.getOptions().match(DateTimeOptions.CalendarMode) &&
                !StringUtility.isNullOrEmpty(comment) && comment.equals(Constants.Comment_WeekOf)) {
            resolveWeekOf(res, Constants.ResolveToPast);
        }

        for (Map.Entry<String,Object> p : res.entrySet()) {
            if (p.getValue() instanceof Map) {
                Map<String, String> value = new LinkedHashMap<>();

                addResolutionFields(value, DateTimeResolutionKey.Timex, timex);
                addResolutionFields(value, DateTimeResolutionKey.Mod, mod);
                addResolutionFields(value, ResolutionKey.Type, typeOutput);
                addResolutionFields(value, DateTimeResolutionKey.IsLunar, islunar ? islunar.toString() : "");
                addResolutionFields(value, DateTimeResolutionKey.List, list);

                if (hasTimeZone) {
                    addResolutionFields(value, Constants.TimeZone, val.getTimeZoneResolution().getValue());
                    addResolutionFields(value, Constants.TimeZoneText, val.getTimeZoneResolution().getTimeZoneText());
                    addResolutionFields(value, Constants.UtcOffsetMinsKey, val.getTimeZoneResolution().getUtcOffsetMins().toString());
                }

                for (Map.Entry<String, String> q : ((Map<String, String>)p.getValue()).entrySet()) {
                    value.put(q.getKey(), q.getValue());
                }

                resolutions.add(value);
            }
        }

        if (resolutionPast.size() == 0 && resolutionFuture.size() == 0 && val.getTimeZoneResolution() == null) {
            Map<String, String> notResolved = new LinkedHashMap<>();
            notResolved.put(DateTimeResolutionKey.Timex, timex);
            notResolved.put(ResolutionKey.Type, typeOutput);
            notResolved.put(ResolutionKey.Value, "not resolved");

            resolutions.add(notResolved);
        }

        SortedMap<String, Object> result = new TreeMap<>();
        result.put(ResolutionKey.ValueSet, resolutions);

        return result;
    }

    private String determineResolutionDateTimeType(LinkedHashMap<String, String> pastResolutionStr) {
        switch (pastResolutionStr.keySet().stream().findFirst().get()) {
            case TimeTypeConstants.START_DATE:
                return Constants.SYS_DATETIME_DATEPERIOD;
            case TimeTypeConstants.START_DATETIME:
                return Constants.SYS_DATETIME_DATETIMEPERIOD;
            case TimeTypeConstants.START_TIME:
                return Constants.SYS_DATETIME_TIMEPERIOD;
            default:
                return pastResolutionStr.keySet().stream().findFirst().get().toLowerCase();
        }
    }

    private void addResolutionFields(Map<String, Object> dic, String key, Object value) {
        if (value instanceof String) {
            if (!StringUtility.isNullOrEmpty((String)value)) {
                dic.put(key, value);
            }
        } else {
            dic.put(key, value);
        }
    }

    private void addResolutionFields(Map<String, String> dic, String key, String value) {
        if (!StringUtility.isNullOrEmpty(value)) {
            dic.put(key, value);
        }
    }

    private void resolveAmPm(Map<String, Object> resolutionDic, String keyName) {
        if (resolutionDic.containsKey(keyName)) {
            Map<String, String> resolution = (Map<String, String>)resolutionDic.get(keyName);

            Map<String, String> resolutionPm = new LinkedHashMap<>();

            if (!resolutionDic.containsKey(DateTimeResolutionKey.Timex)) {
                return;
            }

            String timex = (String)resolutionDic.get(DateTimeResolutionKey.Timex);
            timex = timex != null ? timex : "";

            resolutionDic.remove(keyName);
            resolutionDic.put(keyName + "Am", resolution);

            switch ((String)resolutionDic.get(ResolutionKey.Type)) {
                case Constants.SYS_DATETIME_TIME:
                    resolutionPm.put(ResolutionKey.Value, FormatUtil.toPm(resolution.get(ResolutionKey.Value)));
                    resolutionPm.put(DateTimeResolutionKey.Timex, FormatUtil.toPm(timex));
                    break;
                case Constants.SYS_DATETIME_DATETIME:
                    String[] splited = resolution.get(ResolutionKey.Value).split(" ");
                    resolutionPm.put(ResolutionKey.Value, splited[0] + " " + FormatUtil.toPm(splited[1]));
                    resolutionPm.put(DateTimeResolutionKey.Timex, FormatUtil.allStringToPm(timex));
                    break;
                case Constants.SYS_DATETIME_TIMEPERIOD:
                    if (resolution.containsKey(DateTimeResolutionKey.START)) {
                        resolutionPm.put(DateTimeResolutionKey.START, FormatUtil.toPm(resolution.get(DateTimeResolutionKey.START)));
                    }

                    if (resolution.containsKey(DateTimeResolutionKey.END)) {
                        resolutionPm.put(DateTimeResolutionKey.END, FormatUtil.toPm(resolution.get(DateTimeResolutionKey.END)));
                    }

                    resolutionPm.put(DateTimeResolutionKey.Timex, FormatUtil.allStringToPm(timex));
                    break;
                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    if (resolution.containsKey(DateTimeResolutionKey.START)) {
                        LocalDateTime start = LocalDateTime.parse(resolution.get(DateTimeResolutionKey.START), DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
                        start = start.getHour() == Constants.HalfDayHourCount ? start.minusHours(Constants.HalfDayHourCount) : start.plusHours(Constants.HalfDayHourCount);

                        resolutionPm.put(DateTimeResolutionKey.START, FormatUtil.formatDateTime(start));
                    }

                    if (resolution.containsKey(DateTimeResolutionKey.END)) {
                        LocalDateTime end = LocalDateTime.parse(resolution.get(DateTimeResolutionKey.END), DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
                        end = end.getHour() == Constants.HalfDayHourCount ? end.minusHours(Constants.HalfDayHourCount) : end.plusHours(Constants.HalfDayHourCount);

                        resolutionPm.put(DateTimeResolutionKey.END, FormatUtil.formatDateTime(end));
                    }

                    resolutionPm.put(DateTimeResolutionKey.Timex, FormatUtil.allStringToPm(timex));
                    break;
                default:
                    break;
            }
            resolutionDic.put(keyName + "Pm", resolutionPm);
        }
    }

    private void resolveWeekOf(Map<String, Object> resolutionDic, String keyName) {
        if (resolutionDic.containsKey(keyName)) {
            Map<String, String> resolution = (Map<String, String>)resolutionDic.get(keyName);

            LocalDateTime monday = DateUtil.tryParse(resolution.get(DateTimeResolutionKey.START));
            resolution.put(DateTimeResolutionKey.Timex, TimexUtility.generateWeekTimex(monday));

            resolutionDic.put(keyName, resolution);
        }
    }

    private Map<String, String> generateResolution(String type, Map<String, String> resolutionDic, String mod) {
        Map<String, String> res = new LinkedHashMap<>();

        if (type.equals(Constants.SYS_DATETIME_DATETIME)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_TIME)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_DATE)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_DURATION)) {
            if (resolutionDic.containsKey(TimeTypeConstants.DURATION)) {
                res.put(ResolutionKey.Value, resolutionDic.get(TimeTypeConstants.DURATION));
            }
        } else if (type.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
        } else if (type.equals(Constants.SYS_DATETIME_DATETIMEALT)) {
            // for a period
            if (resolutionDic.size() > 2) {
                addAltPeriodToResolution(resolutionDic, mod, res);
            } else {
                // for a datetime point
                addAltSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIMEALT, mod, res);
            }
        }

        return res;
    }

    public void addAltPeriodToResolution(Map<String, String> resolutionDic, String mod, Map<String, String> res) {
        if (resolutionDic.containsKey(TimeTypeConstants.START_DATETIME) && resolutionDic.containsKey(TimeTypeConstants.END_DATETIME)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
        } else if (resolutionDic.containsKey(TimeTypeConstants.START_DATE) && resolutionDic.containsKey(TimeTypeConstants.END_DATE)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
        } else if (resolutionDic.containsKey(TimeTypeConstants.START_TIME) && resolutionDic.containsKey(TimeTypeConstants.END_TIME)) {
            addPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
        }
    }

    public void addAltSingleDateTimeToResolution(Map<String, String> resolutionDic, String type, String mod, Map<String, String> res) {
        if (resolutionDic.containsKey(TimeTypeConstants.DATE)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
        } else if (resolutionDic.containsKey(TimeTypeConstants.DATETIME)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
        } else if (resolutionDic.containsKey(TimeTypeConstants.TIME)) {
            addSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
        }
    }

    public void addSingleDateTimeToResolution(Map<String, String> resolutionDic, String type, String mod, Map<String, String> res) {
        if (resolutionDic.containsKey(type) && !resolutionDic.get(type).equals(dateMinString) && !resolutionDic.get(type).equals(dateTimeMinString)) {
            if (!StringUtility.isNullOrEmpty(mod)) {
                if (mod.equals(Constants.BEFORE_MOD)) {
                    res.put(DateTimeResolutionKey.END, resolutionDic.get(type));
                    return;
                }

                if (mod.equals(Constants.AFTER_MOD)) {
                    res.put(DateTimeResolutionKey.START, resolutionDic.get(type));
                    return;
                }

                if (mod.equals(Constants.SINCE_MOD)) {
                    res.put(DateTimeResolutionKey.START, resolutionDic.get(type));
                    return;
                }

                if (mod.equals(Constants.UNTIL_MOD)) {
                    res.put(DateTimeResolutionKey.END, resolutionDic.get(type));
                    return;
                }
            }

            res.put(ResolutionKey.Value, resolutionDic.get(type));
        }
    }

    public void addPeriodToResolution(Map<String, String> resolutionDic, String startType, String endType, String mod, Map<String, String> res) {
        String start = "";
        String end = "";

        if (resolutionDic.containsKey(startType)) {
            start = resolutionDic.get(startType);
        }

        if (resolutionDic.containsKey(endType)) {
            end = resolutionDic.get(endType);
        }

        if (!StringUtility.isNullOrEmpty(mod)) {
            // For the 'before' mod
            // 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
            // 2. Cases like "More than 3 days before today", the date point should be the end of the new period
            if (mod.equals(Constants.BEFORE_MOD)) {
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end)) {
                    res.put(DateTimeResolutionKey.END, start);
                } else {
                    res.put(DateTimeResolutionKey.END, end);
                }
                
                return;
            }

            // For the 'after' mod
            // 1. Cases like "After January", the end of the period should be the start of the new period, not the end 
            // 2. Cases like "More than 3 days after today", the date point should be the start of the new period
            if (mod.equals(Constants.AFTER_MOD)) {
                // For cases like "After January" or "After 2018"
                // The "end" of the period is not inclusive by default ("January", the end should be "XXXX-02-01" / "2018", the end should be "2019-01-01")
                // Mod "after" is also not inclusive the "start" ("After January", the start should be "XXXX-01-31" / "After 2018", the start should be "2017-12-31")
                // So here the START day should be the inclusive end of the period, which is one day previous to the default end (exclusive end)
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end)) {
                    res.put(DateTimeResolutionKey.START, getPreviousDay(end));
                } else {
                    res.put(DateTimeResolutionKey.START, start);
                }
                
                return;
            }

            // For the 'since' mod, the start of the period should be the start of the new period, not the end 
            if (mod.equals(Constants.SINCE_MOD)) {
                res.put(DateTimeResolutionKey.START, start);
                return;
            }

            // For the 'until' mod, the end of the period should be the end of the new period, not the start 
            if (mod.equals(Constants.UNTIL_MOD)) {
                res.put(DateTimeResolutionKey.END, end);
                return;
            }
        }

        if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end)) {
            res.put(DateTimeResolutionKey.START, start);
            res.put(DateTimeResolutionKey.END, end);
        }
    }

    public String getPreviousDay(String dateStr) {
        // Here the dateString is in standard format, so Parse should work perfectly
        LocalDateTime date = DateUtil.tryParse(dateStr)
            .minusDays(1);
        return FormatUtil.luisDate(date);
    }
}
