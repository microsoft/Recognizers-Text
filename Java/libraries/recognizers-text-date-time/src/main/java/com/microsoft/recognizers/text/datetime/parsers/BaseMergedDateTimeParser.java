package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DatePeriodTimexType;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.DateTimeResolutionKey;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDate;
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
import java.util.stream.Stream;

public class BaseMergedDateTimeParser implements IDateTimeParser {

    private final String parserName = "datetimeV2";
    private final IMergedParserConfiguration config;
    private static final String dateMinString = DateTimeFormatUtil.formatDate(DateUtil.minValue());
    private static final String dateTimeMinString = DateTimeFormatUtil.formatDateTime(DateUtil.minValue());
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
            er = new ExtractResult(er.getStart(), newLength, newText, er.getType(), er.getData(), er.getMetadata());
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

        if (er.getMetadata() != null && er.getMetadata().getHasMod()) {
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
                (er.getType().equals(Constants.SYS_DATETIME_DATE)) || (er.getType().equals(Constants.SYS_DATETIME_TIME))) {
                // This has to be put at the end of the if, or cases like "before 2012" and "after 2012" would fall into this
                // 2012 or after/above, 3 pm or later
                ConditionalMatch match = RegexExtension.matchEnd(config.getSuffixAfterRegex(), er.getText(), true);
                if (match.getSuccess()) {
                    hasYearAfter = true;
                    er.setLength(er.getLength() - match.getMatch().get().length);
                    er.setText(er.getLength() > 0 ? er.getText().substring(0, er.getLength()) : "");
                    modStr = match.getMatch().get().value;
                }
            }
        }

        if (er.getType().equals(Constants.SYS_DATETIME_DATE)) {
            if (er.getMetadata() != null && er.getMetadata().getIsHoliday()) {
                pr = config.getHolidayParser().parse(er, reference);
            } else {
                pr = this.config.getDateParser().parse(er, reference);
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

        if (pr == null) {
            return null;
        }

        // Pop, restore the MOD string
        if (hasBefore && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();

            if (!hasInclusiveModifier) {
                val.setMod(combineMod(val.getMod(), Constants.BEFORE_MOD));
            } else {
                val.setMod(combineMod(val.getMod(), Constants.UNTIL_MOD));
            }

            pr.setValue(val);
        }

        if (hasAfter && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();

            if (!hasInclusiveModifier) {
                val.setMod(combineMod(val.getMod(), Constants.AFTER_MOD));
            } else {
                val.setMod(combineMod(val.getMod(), Constants.SINCE_MOD));
            }

            pr.setValue(val);
        }

        if (hasSince && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(combineMod(val.getMod(), Constants.SINCE_MOD));
            pr.setValue(val);
        }

        if (hasAround && pr != null && pr.getValue() != null) {

            pr.setStart(pr.getStart() - modStr.length());
            pr.setText(modStr + pr.getText());
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(combineMod(val.getMod(), Constants.APPROX_MOD));
            pr.setValue(val);
        }

        if (hasYearAfter && pr != null && pr.getValue() != null) {

            pr.setText(pr.getText() + modStr);
            pr.setLength(pr.getLength() + modStr.length());

            DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
            val.setMod(combineMod(val.getMod(), Constants.SINCE_MOD));
            pr.setValue(val);
            hasSince = true;
        }

        // For cases like "3 pm or later on Monday"
        if (pr != null && pr.getValue() != null && pr.getType().equals(Constants.SYS_DATETIME_DATETIME)) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getSuffixAfterRegex(), pr.getText())).findFirst();
            if (match.isPresent() && match.get().index != 0) {
                DateTimeResolutionResult val = (DateTimeResolutionResult)pr.getValue();
                val.setMod(combineMod(val.getMod(), Constants.SINCE_MOD));
                pr.setValue(val);
                hasSince = true;
            }
        }

        if (config.getOptions().match(DateTimeOptions.SplitDateAndTime) && pr != null && pr.getValue() != null &&
                ((DateTimeResolutionResult)pr.getValue()).getSubDateTimeEntities() != null) {
            pr.setValue(dateTimeResolutionForSplit(pr));
        } else {
            boolean hasModifier = hasBefore || hasAfter || hasSince;
            if (pr.getValue() != null) {
                ((DateTimeResolutionResult)pr.getValue()).setHasRangeChangingMod(hasModifier);
            }

            pr = setParseResult(pr, hasModifier);
        }

        // In this version, ExperimentalMode only cope with the "IncludePeriodEnd" case
        if (this.config.getOptions().match(DateTimeOptions.ExperimentalMode)) {
            if (pr.getMetadata() != null && pr.getMetadata().getIsPossiblyIncludePeriodEnd()) {
                pr = setInclusivePeriodEnd(pr);
            }
        }

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            int prLength = pr.getLength() + originText.length() - pr.getText().length();
            pr = new DateTimeParseResult(pr.getStart(), prLength, originText, pr.getType(), pr.getData(), pr.getValue(), pr.getResolutionStr(), pr.getTimexStr());
        }

        return pr;
    }


    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
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

    public DateTimeParseResult setInclusivePeriodEnd(DateTimeParseResult slot) {
        String currentType =  parserName + "." + Constants.SYS_DATETIME_DATEPERIOD;
        if (slot.getType().equals(currentType)) {
            Stream<String> timexStream = Arrays.asList(slot.getTimexStr().split(",|\\(|\\)")).stream();
            String[] timexComponents = timexStream.filter(str -> !str.isEmpty()).collect(Collectors.toList()).toArray(new String[0]);

            // Only handle DatePeriod like "(StartDate,EndDate,Duration)"
            if (timexComponents.length == 3) {
                TreeMap<String, Object> value = (TreeMap<String, Object>)slot.getValue();
                String altTimex = "";

                if (value != null && value.containsKey(ResolutionKey.ValueSet)) {
                    if (value.get(ResolutionKey.ValueSet) instanceof List) {
                        List<HashMap<String, String>> valueSet = (List<HashMap<String, String>>)value.get(ResolutionKey.ValueSet);
                        if (!value.isEmpty()) {

                            for (HashMap<String, String> values : valueSet) {
                                // This is only a sanity check, as here we only handle DatePeriod like "(StartDate,EndDate,Duration)"
                                if (values.containsKey(DateTimeResolutionKey.START) &&
                                    values.containsKey(DateTimeResolutionKey.END) &&
                                    values.containsKey(DateTimeResolutionKey.Timex)) {

                                    DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
                                    LocalDateTime startDate = LocalDate.parse(values.get(DateTimeResolutionKey.START), formatter).atStartOfDay();
                                    LocalDateTime endDate = LocalDate.parse(values.get(DateTimeResolutionKey.END), formatter).atStartOfDay();
                                    String durationStr = timexComponents[2];
                                    DatePeriodTimexType datePeriodTimexType = TimexUtility.getDatePeriodTimexType(durationStr);

                                    endDate = TimexUtility.offsetDateObject(endDate, 1, datePeriodTimexType);
                                    values.put(DateTimeResolutionKey.END, DateTimeFormatUtil.luisDate(endDate));
                                    values.put(DateTimeResolutionKey.Timex, generateEndInclusiveTimex(slot.getTimexStr(), datePeriodTimexType, startDate, endDate));

                                    if (StringUtility.isNullOrEmpty(altTimex)) {
                                        altTimex = values.get(DateTimeResolutionKey.Timex);
                                    }
                                }
                            }
                        }
                    }
                }

                slot.setValue(value);
                slot.setTimexStr(altTimex);
            }
        }
        return slot;
    }

    public String generateEndInclusiveTimex(String originalTimex, DatePeriodTimexType datePeriodTimexType, LocalDateTime startDate, LocalDateTime endDate) {
        String timexEndInclusive = TimexUtility.generateDatePeriodTimex(startDate, endDate, datePeriodTimexType);

        // Sometimes the original timex contains fuzzy part like "XXXX-05-31"
        // The fuzzy part needs to stay the same in the new end-inclusive timex
        if (originalTimex.contains(Character.toString(Constants.TimexFuzzy)) && originalTimex.length() == timexEndInclusive.length()) {
            char[] timexCharSet = new char[timexEndInclusive.length()];

            for (int i = 0; i < originalTimex.length(); i++) {
                if (originalTimex.charAt(i) != Constants.TimexFuzzy) {
                    timexCharSet[i] = timexEndInclusive.charAt(i);
                } else {
                    timexCharSet[i] = Constants.TimexFuzzy;
                }
            }

            timexEndInclusive = new String(timexCharSet);
        }

        return timexEndInclusive;
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

    public String determineSourceEntityType(String sourceType, String newType, boolean hasMod) {
        if (!hasMod) {
            return null;
        }

        if (!newType.equals(sourceType)) {
            return Constants.SYS_DATETIME_DATETIMEPOINT;
        }

        if (newType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            return Constants.SYS_DATETIME_DATETIMEPERIOD;
        }

        return null;
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
        LinkedHashMap<String, Object> res = new LinkedHashMap<>();

        DateTimeResolutionResult val = (DateTimeResolutionResult)slot.getValue();
        if (val == null) {
            return null;
        }

        boolean islunar = val.getIsLunar() != null ? val.getIsLunar() : false;
        String mod = val.getMod();

        String list = null;

        // Resolve dates list for date periods
        if (slot.getType().equals(Constants.SYS_DATETIME_DATEPERIOD) && val.getList() != null) {
            list = String.join(",", val.getList().stream().map(o -> DateTimeFormatUtil.luisDate((LocalDateTime)o)).collect(Collectors.toList()));
        }

        // With modifier, output Type might not be the same with type in resolution comments
        // For example, if the resolution type is "date", with modifier the output type should be "daterange"
        String typeOutput = determineDateTimeType(slot.getType(), !StringUtility.isNullOrEmpty(mod));
        String sourceEntity = determineSourceEntityType(slot.getType(), typeOutput, val.getHasRangeChangingMod());
        String comment = val.getComment();

        String type = slot.getType();
        String timex = slot.getTimexStr();

        // The following should be added to res first, since ResolveAmPm requires these fields.
        addResolutionFields(res, DateTimeResolutionKey.Timex, timex);
        addResolutionFields(res, Constants.Comment, comment);
        addResolutionFields(res, DateTimeResolutionKey.Mod, mod);
        addResolutionFields(res, ResolutionKey.Type, typeOutput);
        addResolutionFields(res, DateTimeResolutionKey.IsLunar, islunar ? Boolean.toString(islunar) : "");

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
        if (((DateTimeResolutionResult)slot.getValue()).getPastResolution() != null) {
            pastResolutionStr.putAll(((DateTimeResolutionResult)slot.getValue()).getPastResolution());
        }

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

        if (comment != null && !comment.isEmpty() && TimexUtility.hasDoubleTimex(comment)) {
            res = TimexUtility.processDoubleTimex(res, Constants.ResolveToFuture, Constants.ResolveToPast, timex);
        }

        for (Map.Entry<String,Object> p : res.entrySet()) {
            if (p.getValue() instanceof Map) {
                Map<String, String> value = new LinkedHashMap<>();

                addResolutionFields(value, DateTimeResolutionKey.Timex, timex);
                addResolutionFields(value, DateTimeResolutionKey.Mod, mod);
                addResolutionFields(value, ResolutionKey.Type, typeOutput);
                addResolutionFields(value, DateTimeResolutionKey.IsLunar, islunar ? Boolean.toString(islunar) : "");
                addResolutionFields(value, DateTimeResolutionKey.List, list);
                addResolutionFields(value, DateTimeResolutionKey.SourceEntity, sourceEntity);

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
    
    private String combineMod(String originalMod, String newMod) {
        String combinedMod = newMod;
        if (originalMod != null && originalMod != "") {
            combinedMod = newMod + "-" + originalMod;
        }
        return combinedMod;
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
        if (value != null) {
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
                    resolutionPm.put(ResolutionKey.Value, DateTimeFormatUtil.toPm(resolution.get(ResolutionKey.Value)));
                    resolutionPm.put(DateTimeResolutionKey.Timex, DateTimeFormatUtil.toPm(timex));
                    break;
                case Constants.SYS_DATETIME_DATETIME:
                    String[] splited = resolution.get(ResolutionKey.Value).split(" ");
                    resolutionPm.put(ResolutionKey.Value, splited[0] + " " + DateTimeFormatUtil.toPm(splited[1]));
                    resolutionPm.put(DateTimeResolutionKey.Timex, DateTimeFormatUtil.allStringToPm(timex));
                    break;
                case Constants.SYS_DATETIME_TIMEPERIOD:
                    if (resolution.containsKey(DateTimeResolutionKey.START)) {
                        resolutionPm.put(DateTimeResolutionKey.START, DateTimeFormatUtil.toPm(resolution.get(DateTimeResolutionKey.START)));
                    }

                    if (resolution.containsKey(DateTimeResolutionKey.END)) {
                        resolutionPm.put(DateTimeResolutionKey.END, DateTimeFormatUtil.toPm(resolution.get(DateTimeResolutionKey.END)));
                    }

                    resolutionPm.put(DateTimeResolutionKey.Timex, DateTimeFormatUtil.allStringToPm(timex));
                    break;
                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    if (resolution.containsKey(DateTimeResolutionKey.START)) {
                        LocalDateTime start = LocalDateTime.parse(resolution.get(DateTimeResolutionKey.START), DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
                        start = start.getHour() == Constants.HalfDayHourCount ? start.minusHours(Constants.HalfDayHourCount) : start.plusHours(Constants.HalfDayHourCount);

                        resolutionPm.put(DateTimeResolutionKey.START, DateTimeFormatUtil.formatDateTime(start));
                    }

                    if (resolution.containsKey(DateTimeResolutionKey.END)) {
                        LocalDateTime end = LocalDateTime.parse(resolution.get(DateTimeResolutionKey.END), DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
                        end = end.getHour() == Constants.HalfDayHourCount ? end.minusHours(Constants.HalfDayHourCount) : end.plusHours(Constants.HalfDayHourCount);

                        resolutionPm.put(DateTimeResolutionKey.END, DateTimeFormatUtil.formatDateTime(end));
                    }

                    resolutionPm.put(DateTimeResolutionKey.Timex, DateTimeFormatUtil.allStringToPm(timex));
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
        // If an "invalid" Date or DateTime is extracted, it should not have an assigned resolution.
        // Only valid entities should pass this condition.
        if (resolutionDic.containsKey(type) && !resolutionDic.get(type).startsWith(dateMinString)) {
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
            if (resolutionDic.get(startType).startsWith(dateMinString)) {
                return;
            }
            start = resolutionDic.get(startType);
        }

        if (resolutionDic.containsKey(endType)) {
            if (resolutionDic.get(endType).startsWith(dateMinString)) {
                return;
            }
            end = resolutionDic.get(endType);
        }

        if (!StringUtility.isNullOrEmpty(mod)) {
            // For the 'before' mod
            // 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
            // 2. Cases like "More than 3 days before today", the date point should be the end of the new period
            if (mod.startsWith(Constants.BEFORE_MOD)) {
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end) && !mod.endsWith(Constants.LATE_MOD)) {
                    res.put(DateTimeResolutionKey.END, start);
                } else {
                    res.put(DateTimeResolutionKey.END, end);
                }
                
                return;
            }

            // For the 'after' mod
            // 1. Cases like "After January", the end of the period should be the start of the new period, not the end 
            // 2. Cases like "More than 3 days after today", the date point should be the start of the new period
            if (mod.startsWith(Constants.AFTER_MOD)) {
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end) && !mod.endsWith(Constants.EARLY_MOD)) {
                    res.put(DateTimeResolutionKey.START, end);
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
}
