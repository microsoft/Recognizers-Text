package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeAltExtractorConfiguration;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class BaseDateTimeAltExtractor implements IDateTimeListExtractor {

    private final IDateTimeAltExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DATETIMEALT;
    }

    public BaseDateTimeAltExtractor(IDateTimeAltExtractorConfiguration config) {
        this.config = config;
    }

    public List<ExtractResult> extract(List<ExtractResult> extractResults, String text) {
        return this.extract(extractResults, text, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(List<ExtractResult> extractResults, String text, LocalDateTime reference) {
        return extractAlt(extractResults, text, reference);
    }

    // Modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
    // or "Thursday" in "next week on Tuesday or Thursday"
    private List<ExtractResult> extractAlt(List<ExtractResult> extractResults, String text, LocalDateTime reference) {
        List<ExtractResult> result = addImplicitDates(extractResults, text);

        // Sort the extracted results for the further sequential process.
        result.sort(Comparator.comparingInt(erA -> erA.start));

        int i = 0;
        while (i < result.size() - 1) {
            int j = i + 1;

            while (j < result.size()) {
                // check whether middle string is a connector
                int middleBegin = result.get(j - 1).start + result.get(j - 1).length;
                int middleEnd = result.get(j).start;

                if (middleBegin >= middleEnd) {
                    break;
                }

                String middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
                Match[] matches = RegExpUtility.getMatches(config.getOrRegex(), middleStr);

                boolean matchCheck = matches.length != 1 || matches[0].index != 0 || matches[0].length != middleStr.length();
                if (!StringUtility.isNullOrEmpty(middleStr) && matchCheck) {
                    break;
                }

                j++;
            }

            j--;

            if (i == j) {
                i++;
                continue;
            }

            // Extract different data accordingly
            List<ExtractResult> altErs = new ArrayList<>(result.subList(i, j - i + 1));
            Map<String, Object> data = extractSingleAlt(altErs);

            int parentTextStart = result.get(i).start;
            int parentTextEnd = result.get(j).start + result.get(j).length;
            String parentText = text.substring(parentTextStart, parentTextEnd);

            if (data.size() > 0) {
                data.put(ExtendedModelResult.ParentTextKey, parentText);

                Map<String, Object> modifiedData = new LinkedHashMap<>();
                modifiedData.put(Constants.SubType, result.get(i).type);
                modifiedData.put(ExtendedModelResult.ParentTextKey, parentText);

                ExtractResult modified = result.get(i)
                        .withData(modifiedData)
                        .withType(Constants.SYS_DATETIME_DATETIMEALT);

                result.set(i, modified);

                for (int k = i + 1; k <= j; k++) {
                    result.set(k, result.get(k)
                            .withType(Constants.SYS_DATETIME_DATETIMEALT)
                            .withData(data)
                    );
                }

                i = j + 1;
            } else {
                Collections.reverse(altErs);
                data = extractSingleAlt(altErs);
                if (data.size() > 0) {
                    data.put(ExtendedModelResult.ParentTextKey, parentText);

                    Map<String, Object> modifiedData = new LinkedHashMap<>();
                    modifiedData.put(Constants.SubType, result.get(i).type);
                    modifiedData.put(ExtendedModelResult.ParentTextKey, parentText);

                    ExtractResult modified = result.get(j)
                            .withData(modifiedData)
                            .withType(Constants.SYS_DATETIME_DATETIMEALT);

                    result.set(j, modified);

                    for (int k = i; k < j; k++) {
                        result.set(k, result.get(k)
                                .withType(Constants.SYS_DATETIME_DATETIMEALT)
                                .withData(data)
                        );
                    }

                    i = j + 1;
                    continue;
                }
            }
            i = j;
        }

        result = resolveImplicitRelativeDatePeriod(result, text);
        result = pruneInvalidImplicitDate(result);

        return result;
    }

    private List<ExtractResult> addImplicitDates(List<ExtractResult> originalErs, String text) {
        List<ExtractResult> result = new ArrayList<>();

        Match[] implicitDateMatches = RegExpUtility.getMatches(config.getDayRegex(), text);
        int i = 0;
        originalErs.sort(Comparator.comparingInt(er -> er.start));

        for (Match dateMatch : implicitDateMatches) {
            boolean notBeContained = true;
            while (i < originalErs.size()) {
                if (originalErs.get(i).start <= dateMatch.index && originalErs.get(i).start + originalErs.get(i).length >= dateMatch.index + dateMatch.length) {
                    notBeContained = false;
                    break;
                }

                if (originalErs.get(i).start + originalErs.get(i).length < dateMatch.index + dateMatch.length) {
                    i++;
                } else if (originalErs.get(i).start + originalErs.get(i).length >= dateMatch.index + dateMatch.length) {
                    break;
                }
            }

            if (notBeContained) {
                result.add(new ExtractResult(
                        dateMatch.index,
                        dateMatch.length,
                        dateMatch.value,
                        Constants.SYS_DATETIME_DATE));
            }
        }

        result.addAll(originalErs);
        result.sort(Comparator.comparingInt(er -> er.start));

        return result;
    }

    private List<ExtractResult> pruneInvalidImplicitDate(List<ExtractResult> ers) {
        ers.removeIf(er -> {
            if (er.data != null || !er.type.equals(Constants.SYS_DATETIME_DATE)) {
                return false;
            }

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDayRegex(), er.text)).findFirst();
            return match.isPresent() && match.get().index == 0 && match.get().length == er.length;
        });

        return ers;
    }

    // Resolve cases like "this week or next".
    private List<ExtractResult> resolveImplicitRelativeDatePeriod(List<ExtractResult> ers, String text) {
        List<Match> relativeTermsMatches = new ArrayList<>();
        for (Pattern regex : config.getRelativePrefixList()) {
            relativeTermsMatches.addAll(Arrays.asList(RegExpUtility.getMatches(regex, text)));
        }

        List<ExtractResult> relativeDatePeriodErs = new ArrayList<>();
        int i = 0;
        for (ExtractResult result : ers.toArray(new ExtractResult[0])) {
            if (!result.type.equals(Constants.SYS_DATETIME_DATETIMEALT)) {
                int resultEnd = result.start + result.length;
                for (Match relativeTermsMatch : relativeTermsMatches) {
                    if (relativeTermsMatch.index > resultEnd) {
                        // Check whether middle string is a connector
                        int middleBegin = resultEnd;
                        int middleEnd = relativeTermsMatch.index;
                        String middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
                        Match[] orTermMatches = RegExpUtility.getMatches(config.getOrRegex(), middleStr);
                        if (orTermMatches.length == 1 && orTermMatches[0].index == 0 && orTermMatches[0].length == middleStr.length()) {
                            int parentTextStart = result.start;
                            int parentTextEnd = relativeTermsMatch.index + relativeTermsMatch.length;
                            String parentText = text.substring(parentTextStart, parentTextEnd);

                            ExtractResult contextErs = new ExtractResult();
                            for (Pattern regex : config.getRelativePrefixList()) {
                                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, result.text)).findFirst();
                                if (match.isPresent()) {
                                    int matchEnd = match.get().index + match.get().length;
                                    contextErs = new ExtractResult(
                                            matchEnd,
                                            result.length - matchEnd,
                                            result.text.substring(matchEnd, result.length),
                                            Constants.ContextType_RelativeSuffix);
                                    break;
                                }
                            }

                            Map<String, Object> customData = new LinkedHashMap<>();
                            customData.put(Constants.SubType, result.type);
                            customData.put(ExtendedModelResult.ParentTextKey, parentText);
                            customData.put(Constants.Context, contextErs);

                            relativeDatePeriodErs.add(new ExtractResult(
                                    relativeTermsMatch.index,
                                    relativeTermsMatch.length,
                                    relativeTermsMatch.value,
                                    Constants.SYS_DATETIME_DATETIMEALT,
                                    customData));

                            Map<String, Object> resultData = new LinkedHashMap<>();
                            resultData.put(Constants.SubType, result.type);
                            resultData.put(ExtendedModelResult.ParentTextKey, parentText);

                            ers.set(i, result
                                    .withData(resultData)
                                    .withType(Constants.SYS_DATETIME_DATETIMEALT));
                        }
                    }
                }
            }
            i++;
        }

        List<ExtractResult> result = new ArrayList<>();
        result.addAll(ers);
        result.addAll(relativeDatePeriodErs);
        result.sort(Comparator.comparingInt(er -> er.start));

        return result;
    }

    private Map<String, Object> extractSingleAlt(List<ExtractResult> extractResults) {
        if (extractResults.isEmpty()) {
            return new LinkedHashMap<>();
        }

        ExtractResult former = extractResults.get(0);
        ExtractResult latter = extractResults.get(extractResults.size() - 1);
        Map<String, Object> data = extractDateTime_Time(former, latter);

        if (data.isEmpty()) {
            data = extractDates(extractResults);
        }

        if (data.isEmpty()) {
            data = extractTime_Time(former, latter);
        }

        if (data.isEmpty()) {
            data = extractDateTime_DateTime(former, latter);
        }

        if (data.isEmpty()) {
            data = extractDateTimeRange_TimeRange(former, latter);
        }

        if (data.isEmpty()) {
            data = extractDateRange_DateRange(former, latter);
        }

        return data;
    }

    private Map<String, Object> extractDateTime_Time(ExtractResult former, ExtractResult latter) {
        Map<String, Object> data = new LinkedHashMap<>();

        // Modify time entity to an alternative DateTime expression, such as "8pm" in "Monday 7pm or 8pm"
        if (former.type.equals(Constants.SYS_DATETIME_DATETIME) && latter.type.equals(Constants.SYS_DATETIME_TIME)) {
            List<ExtractResult> ers = config.getDateExtractor().extract(former.text);
            if (ers.size() == 1) {
                data.put(Constants.Context, ers.get(0));
                data.put(Constants.SubType, Constants.SYS_DATETIME_TIME);
            }
        }

        return data;
    }

    private Map<String, Object> extractDates(List<ExtractResult> extractResults) {
        Map<String, Object> data = new LinkedHashMap<>();
        boolean allAreDates = true;

        for (ExtractResult er : extractResults) {
            if (!er.type.equals(Constants.SYS_DATETIME_DATE)) {
                allAreDates = false;
                break;
            }
        }

        // Modify time entity to an alternative Date expression, such as "Thursday" in "next week on Tuesday or Thursday"
        if (allAreDates) {
            List<ExtractResult> ers = config.getDatePeriodExtractor().extract(extractResults.get(0).text);
            if (ers.size() == 1) {
                data.put(Constants.Context, ers.get(0));
                data.put(Constants.SubType, Constants.SYS_DATETIME_DATE);
            } else {
                // "Thursday" in "next/last/this Tuesday or Thursday"
                for (Pattern regex : config.getRelativePrefixList()) {
                    Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, extractResults.get(0).text)).findFirst();
                    if (match.isPresent()) {
                        data.put(Constants.Context, new ExtractResult(
                                match.get().index,
                                match.get().length,
                                match.get().value,
                                Constants.ContextType_RelativePrefix));
                        data.put(Constants.SubType, Constants.SYS_DATETIME_DATE);
                    }
                }
            }
        }

        return data;
    }

    private Map<String, Object> extractTime_Time(ExtractResult former, ExtractResult latter) {
        Map<String, Object> data = new LinkedHashMap<>();

        if (former.type.equals(Constants.SYS_DATETIME_TIME) && latter.type.equals(Constants.SYS_DATETIME_TIME)) {
            // "8 oclock" in "in the morning at 7 oclock or 8 oclock"
            for (Pattern regex : config.getAmPmRegexList()) {
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, former.text)).findFirst();
                if (match.isPresent()) {
                    data.put(Constants.Context, new ExtractResult(
                            match.get().index,
                            match.get().length,
                            match.get().value,
                            Constants.ContextType_AmPm));
                    data.put(Constants.SubType, Constants.SYS_DATETIME_TIME);
                }
            }
        }

        return data;
    }

    private Map<String, Object> extractDateTime_DateTime(ExtractResult former, ExtractResult latter) {
        Map<String, Object> data = new LinkedHashMap<>();

        // Modify time entity to an alternative DateTime expression, such as "Tue 1 pm" in "next week Mon 9 am or Tue 1 pm"
        if (former.type.equals(Constants.SYS_DATETIME_DATETIME) && latter.type.equals(Constants.SYS_DATETIME_DATETIME)) {
            List<ExtractResult> ers = config.getDatePeriodExtractor().extract(former.text);
            if (ers.size() == 1) {
                data.put(Constants.Context, ers.get(0));
                data.put(Constants.SubType, Constants.SYS_DATETIME_DATETIME);
            }
        }

        return data;
    }

    private Map<String, Object> extractDateTimeRange_TimeRange(ExtractResult former, ExtractResult latter) {
        Map<String, Object> data = new LinkedHashMap<>();

        // Modify time entity to an alternative DateTimeRange expression, such as "9-10 am" in "Monday 7-8 am or 9-10 am"
        if (former.type.equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && latter.type.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {
            List<ExtractResult> ers = config.getDateExtractor().extract(former.text);
            if (ers.size() == 1) {
                data.put(Constants.Context, ers.get(0));
                data.put(Constants.SubType, Constants.SYS_DATETIME_TIMEPERIOD);
            }
        }

        return data;
    }

    private Map<String, Object> extractDateRange_DateRange(ExtractResult former, ExtractResult latter) {
        Map<String, Object> data = new LinkedHashMap<>();

        if (former.type.equals(Constants.SYS_DATETIME_DATEPERIOD) && latter.type.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            data.put(Constants.SubType, Constants.SYS_DATETIME_DATEPERIOD);
        }

        return data;
    }
}
