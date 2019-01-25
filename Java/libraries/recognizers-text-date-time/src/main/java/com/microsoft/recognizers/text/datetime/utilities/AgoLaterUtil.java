package com.microsoft.recognizers.text.datetime.utilities;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.function.Function;
import java.util.regex.Pattern;

public class AgoLaterUtil {
    public static DateTimeResolutionResult parseDurationWithAgoAndLater(String text, LocalDateTime referenceTime,
            IDateTimeExtractor durationExtractor, IDateTimeParser durationParser, ImmutableMap<String, String> unitMap,
            Pattern unitRegex, IDateTimeUtilityConfiguration utilityConfiguration,
            Function<String, Integer> getSwiftDay) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        List<ExtractResult> durationRes = durationExtractor.extract(text, referenceTime);
        if (durationRes.size() > 0) {
            DateTimeParseResult pr = durationParser.parse(durationRes.get(0), referenceTime);
            Match[] matches = RegExpUtility.getMatches(unitRegex, text);
            if (matches.length > 0) {
                String afterStr = text.substring(durationRes.get(0).getStart() + durationRes.get(0).getLength()).trim()
                        .toLowerCase();

                String beforeStr = text.substring(0, durationRes.get(0).getStart()).trim().toLowerCase();

                AgoLaterMode mode = AgoLaterMode.DATE;
                if (pr.getTimexStr().contains("T")) {
                    mode = AgoLaterMode.DATETIME;
                }

                if (pr.getValue() != null) {
                    return getAgoLaterResult(pr, afterStr, beforeStr, referenceTime, utilityConfiguration, mode,
                            getSwiftDay);
                }
            }
        }
        return ret;
    }

    private static DateTimeResolutionResult getAgoLaterResult(DateTimeParseResult durationParseResult, String afterStr,
            String beforeStr, LocalDateTime referenceTime, IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode, Function<String, Integer> getSwiftDay) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        LocalDateTime resultDateTime = referenceTime;
        String timex = durationParseResult.getTimexStr();

        if (((DateTimeResolutionResult)durationParseResult.getValue()).getMod() == Constants.MORE_THAN_MOD) {
            ret.setMod(Constants.MORE_THAN_MOD);
        } else if (((DateTimeResolutionResult)durationParseResult.getValue()).getMod() == Constants.LESS_THAN_MOD) {
            ret.setMod(Constants.LESS_THAN_MOD);
        }

        if (MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.getAgoRegex())) {
            Optional<Match> match = Arrays
                    .stream(RegExpUtility.getMatches(utilityConfiguration.getAgoRegex(), afterStr)).findFirst();
            int swift = 0;

            // Handle cases like "3 days before yesterday"
            if (match.isPresent() && !StringUtility.isNullOrEmpty(match.get().getGroup("day").value)) {
                swift = getSwiftDay.apply(match.get().getGroup("day").value);
            }

            resultDateTime = DurationParsingUtil.shiftDateTime(timex, referenceTime.plusDays(swift), false);

            ((DateTimeResolutionResult)durationParseResult.getValue()).setMod(Constants.BEFORE_MOD);
        } else if (MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.getLaterRegex()) ||
                MatchingUtil.containsTermIndex(beforeStr, utilityConfiguration.getInConnectorRegex())) {
            Optional<Match> match = Arrays
                    .stream(RegExpUtility.getMatches(utilityConfiguration.getLaterRegex(), afterStr)).findFirst();
            int swift = 0;

            // Handle cases like "3 days after tomorrow"
            if (match.isPresent() && !StringUtility.isNullOrEmpty(match.get().getGroup("day").value)) {
                swift = getSwiftDay.apply(match.get().getGroup("day").value);
            }

            resultDateTime = DurationParsingUtil.shiftDateTime(timex, referenceTime.plusDays(swift), true);

            ((DateTimeResolutionResult)durationParseResult.getValue()).setMod(Constants.AFTER_MOD);
        }

        if (resultDateTime != referenceTime) {
            if (mode.equals(AgoLaterMode.DATE)) {
                ret.setTimex(DateTimeFormatUtil.luisDate(resultDateTime));
            } else if (mode.equals(AgoLaterMode.DATETIME)) {
                ret.setTimex(DateTimeFormatUtil.luisDateTime(resultDateTime));
            }

            ret.setFutureValue(resultDateTime);
            ret.setPastValue(resultDateTime);

            List<Object> subDateTimeEntities = new ArrayList<>();
            subDateTimeEntities.add(durationParseResult);

            ret.setSubDateTimeEntities(subDateTimeEntities);
            ret.setSuccess(true);
        }

        return ret;
    }

    public static List<Token> extractorDurationWithBeforeAndAfter(String text, ExtractResult er, List<Token> result,
            IDateTimeUtilityConfiguration utilityConfiguration) {
        int pos = er.getStart() + er.getLength();
        if (pos <= text.length()) {
            String afterString = text.substring(pos);
            String beforeString = text.substring(0, er.getStart());
            boolean isTimeDuration = RegExpUtility.getMatches(utilityConfiguration.getTimeUnitRegex(),
                    er.getText()).length != 0;

            MatchingUtilResult resultIndex = MatchingUtil.getAgoLaterIndex(afterString,
                    utilityConfiguration.getAgoRegex());
            if (resultIndex.result) {
                // We don't support cases like "5 minutes from today" for now
                // Cases like "5 minutes ago" or "5 minutes from now" are supported
                // Cases like "2 days before today" or "2 weeks from today" are also supported
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(utilityConfiguration.getAgoRegex(), afterString)).findFirst();
                boolean isDayMatchInAfterString = match.isPresent() && !match.get().getGroup("day").value.equals("");

                if (!(isTimeDuration && isDayMatchInAfterString)) {
                    result.add(new Token(er.getStart(), er.getStart() + er.getLength() + resultIndex.index));
                }
            } else {
                resultIndex = MatchingUtil.getAgoLaterIndex(afterString, utilityConfiguration.getLaterRegex());
                if (resultIndex.result) {
                    Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(utilityConfiguration.getLaterRegex(), afterString)).findFirst();
                    boolean isDayMatchInAfterString = match.isPresent() && !match.get().getGroup("day").value.equals("");

                    if (!(isTimeDuration && isDayMatchInAfterString)) {
                        result.add(new Token(er.getStart(), er.getStart() + er.getLength() + resultIndex.index));
                    }
                } else {
                    resultIndex = MatchingUtil.getTermIndex(beforeString, utilityConfiguration.getInConnectorRegex());
                    if (resultIndex.result) {
                        // For range unit like "week, month, year", it should output dateRange or
                        // datetimeRange
                        Optional<Match> match = Arrays
                                .stream(RegExpUtility.getMatches(utilityConfiguration.getRangeUnitRegex(), er.getText()))
                                .findFirst();
                        if (!match.isPresent()) {
                            if (er.getStart() >= resultIndex.index) {
                                result.add(new Token(er.getStart() - resultIndex.index, er.getStart() + er.getLength()));
                            }
                        }
                    } else {
                        resultIndex = MatchingUtil.getTermIndex(beforeString,
                                utilityConfiguration.getWithinNextPrefixRegex());
                        if (resultIndex.result) {
                            // For range unit like "week, month, year, day, second, minute, hour", it should
                            // output dateRange or datetimeRange
                            Optional<Match> matchDateUnitRegex = Arrays
                                    .stream(RegExpUtility.getMatches(utilityConfiguration.getDateUnitRegex(), er.getText()))
                                    .findFirst();
                            Optional<Match> matchTimeUnitRegex = Arrays
                                    .stream(RegExpUtility.getMatches(utilityConfiguration.getTimeUnitRegex(), er.getText()))
                                    .findFirst();
                            if (!matchDateUnitRegex.isPresent() && !matchTimeUnitRegex.isPresent()) {
                                if (er.getStart() >= resultIndex.index) {
                                    result.add(new Token(er.getStart() - resultIndex.index, er.getStart() + er.getLength()));
                                }
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    private static boolean isDayMatchInAfterString(String text, Pattern pattern, String group) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(pattern, text)).findFirst();

        if (match.isPresent()) {
            MatchGroup matchGroup = match.get().getGroup(group);
            return !StringUtility.isNullOrEmpty(matchGroup.value);
        }

        return false;
    }

    public enum AgoLaterMode {
        DATE, DATETIME
    }
}
