package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.DateTimeResolutionKey;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.FormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
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

public class BaseMergedParser implements IDateTimeParser {

    private final String parserName = "datetimeV2";
    private final IMergedParserConfiguration config;
    private static final String dateMinString = FormatUtil.formatDate(DateUtil.minValue());
    private static final String dateTimeMinString = FormatUtil.formatDateTime(DateUtil.minValue());
    //private static final Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

    public BaseMergedParser(IMergedParserConfiguration config) {
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

        String originText = er.text;
        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            String newText = MatchingUtil.preProcessTextRemoveSuperfluousWords(er.text, config.getSuperfluousWordMatcher()).text;
            int newLength = er.length + er.text.length() - originText.length();
            er = new ExtractResult(er.start, newLength, newText, er.type, er.data);
        }

        // Push, save the MOD string
        boolean hasBefore = false;
        boolean hasAfter = false;
        boolean hasSince = false;
        boolean hasYearAfter = false;

        // "InclusiveModifier" means MOD should include the start/end time
        // For example, cases like "on or later than", "earlier than or in" have inclusive modifier
        boolean hasInclusiveModifier = false;
        String modStr = "";
        Optional<Match> beforeMatch = Arrays.stream(RegExpUtility.getMatches(config.getBeforeRegex(), er.text)).findFirst();
        Optional<Match> afterMatch = Arrays.stream(RegExpUtility.getMatches(config.getAfterRegex(), er.text)).findFirst();
        Optional<Match> sinceMatch = Arrays.stream(RegExpUtility.getMatches(config.getSinceRegex(), er.text)).findFirst();

        if (beforeMatch.isPresent() && beforeMatch.get().index == 0) {
            hasBefore = true;
            er = er.withStart(er.start + beforeMatch.get().length)
                .withLength(er.length - beforeMatch.get().length)
                .withText(er.text.substring(beforeMatch.get().length));
            modStr = beforeMatch.get().value;

            if (!StringUtility.isNullOrEmpty(beforeMatch.get().getGroup("include").value)) {
                hasInclusiveModifier = true;
            }
        } else if (afterMatch.isPresent() && afterMatch.get().index == 0) {
            hasAfter = true;
            er = er.withStart(er.start + afterMatch.get().length)
                .withLength(er.length - afterMatch.get().length)
                .withText(er.text.substring(afterMatch.get().length));
            modStr = afterMatch.get().value;

            if (!StringUtility.isNullOrEmpty(afterMatch.get().getGroup("include").value)) {
                hasInclusiveModifier = true;
            }
        } else if (sinceMatch.isPresent() && sinceMatch.get().index == 0) {
            hasSince = true;
            er = er.withStart(er.start + sinceMatch.get().length)
                .withLength(er.length - sinceMatch.get().length)
                .withText(er.text.substring(sinceMatch.get().length));
            modStr = sinceMatch.get().value;
        } else if (er.type.equals(Constants.SYS_DATETIME_DATEPERIOD) && Arrays.stream(RegExpUtility.getMatches(config.getYearRegex(), er.text)).findFirst().isPresent()) {
            // This has to be put at the end of the if, or cases like "before 2012" and "after 2012" would fall into this
            // 2012 or after/above
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateAfterRegex(),er.text)).findFirst();
            if (match.isPresent() && er.text.endsWith(match.get().value)) {
                hasYearAfter = true;
                er = er.withLength(er.length - match.get().length);
                er = er.withText(er.length > 0 ? er.text.substring(0, er.length) : "");
                modStr = match.get().value;
            }
        }

        if (er.type.equals(Constants.SYS_DATETIME_DATE)) {
            pr = this.config.getDateParser().parse(er, reference);
            if (pr.value == null) {
                pr = config.getHolidayParser().parse(er, reference);
            }
        } else if (er.type.equals(Constants.SYS_DATETIME_TIME)) {
            pr = this.config.getTimeParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_DATETIME)) {
            pr = this.config.getDateTimeParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            pr = this.config.getDatePeriodParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            pr = this.config.getTimePeriodParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_DATETIMEPERIOD)) {
            pr = this.config.getDateTimePeriodParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_DURATION)) {
            pr = this.config.getDurationParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_SET)) {
            pr = this.config.getGetParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_DATETIMEALT)) {
            pr = this.config.getDateTimeAltParser().parse(er, reference);
        } else if (er.type.equals(Constants.SYS_DATETIME_TIMEZONE)) {
            if (config.getOptions().match(DateTimeOptions.EnablePreview)) {
                pr = this.config.getTimeZoneParser().parse(er, reference);
            }
        } else {
            return null;
        }

        // Pop, restore the MOD string
        if (hasBefore && pr != null && pr.value != null) {
            pr = new DateTimeParseResult(new ParseResult(
                    pr.withStart(pr.start - modStr.length())
                    .withText(modStr + pr.text))
                    .withLength(pr.length + modStr.length())
                    .withValue(pr.value))
                    .withTimexStr(pr.timexStr);
            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.value;

            if (!hasInclusiveModifier) {
                val.setMod(Constants.BEFORE_MOD);
            } else {
                val.setMod(Constants.UNTIL_MOD);
            }

            pr = new DateTimeParseResult(pr.withValue(val)).withTimexStr(pr.timexStr);
        }

        if (hasAfter && pr != null && pr.value != null) {
            pr = new DateTimeParseResult(new ParseResult(
                    pr.withStart(pr.start - modStr.length())
                    .withText(modStr + pr.text))
                    .withLength(pr.length + modStr.length())
                    .withValue(pr.value))
                    .withTimexStr(pr.timexStr);
            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.value;

            if (!hasInclusiveModifier) {
                val.setMod(Constants.AFTER_MOD);
            } else {
                val.setMod(Constants.SINCE_MOD);
            }

            pr = new DateTimeParseResult(pr.withValue(val)).withTimexStr(pr.timexStr);
        }

        if (hasSince && pr != null && pr.value != null) {
            pr = new DateTimeParseResult(new ParseResult(
                    pr.withStart(pr.start - modStr.length())
                    .withText(modStr + pr.text))
                    .withLength(pr.length + modStr.length())
                    .withValue(pr.value))
                    .withTimexStr(pr.timexStr);
            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.value;
            val.setMod(Constants.SINCE_MOD);
            pr = new DateTimeParseResult(pr.withValue(val)).withTimexStr(pr.timexStr);
        }

        if (hasYearAfter && pr != null && pr.value != null) {
            pr = new DateTimeParseResult(new ParseResult(
                    pr.withLength(pr.length + modStr.length())
                    .withText(pr.text + modStr))
                    .withValue(pr.value))
                    .withTimexStr(pr.timexStr);
            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.value;
            val.setMod(Constants.SINCE_MOD);
            pr = new DateTimeParseResult(pr.withValue(val)).withTimexStr(pr.timexStr);
            hasSince = true;
        }

        if (config.getOptions().match(DateTimeOptions.SplitDateAndTime) && pr != null && pr.value != null &&
                ((DateTimeResolutionResult)pr.value).getSubDateTimeEntities() != null) {
            pr = new DateTimeParseResult(pr.withValue(dateTimeResolutionForSplit(pr))).withTimexStr(pr.timexStr);
        } else {
            boolean hasModifier = hasBefore || hasAfter || hasSince;
            pr = setParseResult(pr, hasModifier);
        }

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            int prLength = pr.length + originText.length() - pr.text.length();
            pr = new DateTimeParseResult(pr.start, prLength, originText, pr.type, pr.data, pr.value, pr.resolutionStr, pr.timexStr);
        }

        return pr;
    }


    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        if (config.getAmbiguousMonthP0Regex() != null) {
            if (candidateResults != null && !candidateResults.isEmpty()) {

                List<Match> matches = Arrays.asList(RegExpUtility.getMatches(config.getAmbiguousMonthP0Regex(),query));

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
        return !(match.index < pr.start + pr.length && pr.start < match.index + match.length);
    }

    public DateTimeParseResult setParseResult(DateTimeParseResult slot, boolean hasMod) {
        SortedMap<String, Object> slotValue = dateTimeResolution(slot);
        // Change the type at last for the after or before modes
        String type = String.format("%s.%s", parserName, determineDateTimeType(slot.type, hasMod));

        slot = new DateTimeParseResult(new ParseResult(slot.withType(type))
                .withValue(slotValue))
                .withTimexStr(slot.timexStr);

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
        if (((DateTimeResolutionResult)slot.value).getSubDateTimeEntities() != null) {
            List<Object> subEntities = ((DateTimeResolutionResult)slot.value).getSubDateTimeEntities();
            for (Object subEntity : subEntities) {
                DateTimeParseResult result = (DateTimeParseResult)subEntity;
                results.addAll(dateTimeResolutionForSplit(result));
            }
        } else {
            slot = new DateTimeParseResult(new ParseResult(
                    slot.withType(String.format("%s.%s",parserName, determineDateTimeType(slot.type, false))))
                    .withValue(dateTimeResolution(slot)))
                    .withTimexStr(slot.timexStr);
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

        String type = slot.type;
        String timex = slot.timexStr;

        DateTimeResolutionResult val = (DateTimeResolutionResult)slot.value;
        if (val == null) {
            return null;
        }

        Boolean islunar = val.getIsLunar() != null ? val.getIsLunar() : false;
        String mod = val.getMod();

        // With modifier, output Type might not be the same with type in resolution result
        // For example, if the resolution type is "date", with modifier the output type should be "daterange"
        String typeOutput = determineDateTimeType(slot.type, !StringUtility.isNullOrEmpty(mod));
        String comment = val.getComment();

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
            if (slot.type.equals(Constants.SYS_DATETIME_TIMEZONE)) {
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
        pastResolutionStr.putAll(((DateTimeResolutionResult)slot.value).getPastResolution());
        Map<String, String> futureResolutionStr = ((DateTimeResolutionResult)slot.value).getFutureResolution();

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

                if (hasTimeZone) {
                    addResolutionFields(value, Constants.TimeZone, val.getTimeZoneResolution().getValue());
                    addResolutionFields(value, Constants.TimeZoneText, val.getTimeZoneResolution().getTimeZoneText());
                    addResolutionFields(value, Constants.UtcOffsetMinsKey, val.getTimeZoneResolution().getUtcOffsetMins().toString());
                }

                for (Map.Entry<String, String> q : ((Map<String, String>)p.getValue()).entrySet()) {
                    value.put(q.getKey(),q.getValue());
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
