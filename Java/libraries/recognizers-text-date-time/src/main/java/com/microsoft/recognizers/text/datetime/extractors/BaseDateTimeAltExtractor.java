package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeAltExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.function.BiConsumer;
import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Stream;

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
        List<ExtractResult> ers = addImplicitDates(extractResults, text);

        // Sort the extracted results for the further sequential process.
        ers.sort(Comparator.comparingInt(erA -> erA.getStart()));

        int i = 0;
        while (i < ers.size() - 1) {
            List<ExtractResult> altErs = getAltErsWithSameParentText(ers, i, text);

            if (altErs.size() == 0) {
                i++;
                continue;
            }

            int j = i + altErs.size() - 1;

            int parentTextStart = ers.get(i).getStart();
            int parentTextLen = ers.get(j).getStart() + ers.get(j).getLength() - ers.get(i).getStart();
            String parentText = text.substring(parentTextStart, parentTextStart + parentTextLen);

            boolean success = extractAndApplyMetadata(altErs, parentText);

            if (success) {
                i = j + 1;
            } else {
                i++;
            }
        }

        ers = resolveImplicitRelativeDatePeriod(ers, text);
        ers = pruneInvalidImplicitDate(ers);

        return ers;
    }

    private List<ExtractResult> getAltErsWithSameParentText(List<ExtractResult> ers, int startIndex, String text) {
        int pivot = startIndex + 1;
        HashSet types = new HashSet<String>();
        types.add(ers.get(startIndex).getType());

        while (pivot < ers.size()) {
            // Currently only support merge two kinds of types
            if (!types.contains(ers.get(pivot).getType()) && types.size() > 1) {
                break;
            }

            // Check whether middle string is a connector
            int middleBegin = ers.get(pivot - 1).getStart() + ers.get(pivot - 1).getLength();
            int middleEnd = ers.get(pivot).getStart();

            if (!isConnectorOrWhiteSpace(middleBegin, middleEnd, text)) {
                break;
            }

            int prefixEnd = ers.get(pivot - 1).getStart();
            String prefixStr = text.substring(0, prefixEnd);

            if (isEndsWithRangePrefix(prefixStr)) {
                break;
            }

            if (isSupportedAltEntitySequence(ers.subList(startIndex, startIndex + (pivot - startIndex + 1)))) {
                types.add(ers.get(pivot).getType());
                pivot++;
            } else {
                break;
            }
        }

        pivot--;

        if (startIndex == pivot) {
            startIndex++;
        }

        return ers.subList(startIndex, startIndex + (pivot - startIndex + 1));
    }

    private List<ExtractResult> addImplicitDates(List<ExtractResult> originalErs, String text) {
        List<ExtractResult> result = new ArrayList<>();

        Match[] implicitDateMatches = RegExpUtility.getMatches(config.getDayRegex(), text);
        int i = 0;
        originalErs.sort(Comparator.comparingInt(er -> er.getStart()));

        for (Match dateMatch : implicitDateMatches) {
            boolean notBeContained = true;
            while (i < originalErs.size()) {
                if (originalErs.get(i).getStart() <= dateMatch.index && originalErs.get(i).getStart() + originalErs.get(i).getLength() >= dateMatch.index + dateMatch.length) {
                    notBeContained = false;
                    break;
                }

                if (originalErs.get(i).getStart() + originalErs.get(i).getLength() < dateMatch.index + dateMatch.length) {
                    i++;
                } else if (originalErs.get(i).getStart() + originalErs.get(i).getLength() >= dateMatch.index + dateMatch.length) {
                    break;
                }
            }

            ExtractResult dateEr = new ExtractResult(
                dateMatch.index,
                dateMatch.length,
                dateMatch.value,
                Constants.SYS_DATETIME_DATE);

            dateEr.setData(getExtractorName());
            if (notBeContained) {
                result.add(dateEr);
            } else if (i + 1 < originalErs.size()) {
                // For cases like "I am looking at 18 and 19 June"
                // in which "18" is wrongly recognized as time without context.
                ExtractResult nextEr = originalErs.get(i + 1);
                if (nextEr.getType().equals(Constants.SYS_DATETIME_DATE) &&
                    originalErs.get(i).getText().equals(dateEr.getText()) &&
                    isConnectorOrWhiteSpace(dateEr.getStart() + dateEr.getLength(), nextEr.getStart(), text)) {
                    result.add(dateEr);
                    originalErs.remove(i);
                }
            }
        }

        result.addAll(originalErs);
        result.sort(Comparator.comparingInt(er -> er.getStart()));

        return result;
    }

    private List<ExtractResult> pruneInvalidImplicitDate(List<ExtractResult> ers) {
        ers.removeIf(er -> {
            if (er.getData() != null && er.getType().equals(Constants.SYS_DATETIME_DATE) && er.getData().equals(getExtractorName())) {
                return true;
            }
            return false;
        });

        return ers;
    }

    // Resolve cases like "this week or next".
    private List<ExtractResult> resolveImplicitRelativeDatePeriod(List<ExtractResult> ers, String text) {
        List<Match> relativeTermsMatches = new ArrayList<>();
        for (Pattern regex : config.getRelativePrefixList()) {
            relativeTermsMatches.addAll(Arrays.asList(RegExpUtility.getMatches(regex, text)));
        }

        List<ExtractResult> results = new ArrayList<>();

        List<ExtractResult> relativeDatePeriodErs = new ArrayList<>();
        int i = 0;
        for (ExtractResult result : ers.toArray(new ExtractResult[0])) {
            if (!result.getType().equals(Constants.SYS_DATETIME_DATETIMEALT)) {
                int resultEnd = result.getStart() + result.getLength();
                for (Match relativeTermsMatch : relativeTermsMatches) {
                    int relativeTermsMatchEnd = relativeTermsMatch.index + relativeTermsMatch.length;
                    if (relativeTermsMatch.index > resultEnd || relativeTermsMatchEnd < result.getStart()) {
                        // Check whether middle string is a connector
                        int middleBegin = relativeTermsMatch.index > resultEnd ? resultEnd : relativeTermsMatchEnd;
                        int middleEnd = relativeTermsMatch.index > resultEnd ? relativeTermsMatch.index : result.getStart();
                        String middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
                        Match[] orTermMatches = RegExpUtility.getMatches(config.getOrRegex(), middleStr);
                        if (orTermMatches.length == 1 && orTermMatches[0].index == 0 && orTermMatches[0].length == middleStr.length()) {
                            int parentTextStart = relativeTermsMatch.index > resultEnd ? result.getStart() : relativeTermsMatch.index;
                            int parentTextEnd = relativeTermsMatch.index > resultEnd ? relativeTermsMatchEnd : resultEnd;
                            String parentText = text.substring(parentTextStart, parentTextEnd);

                            ExtractResult contextErs = new ExtractResult();
                            for (Pattern regex : config.getRelativePrefixList()) {
                                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, result.getText())).findFirst();
                                if (match.isPresent()) {
                                    int matchEnd = match.get().index + match.get().length;
                                    contextErs = new ExtractResult(
                                        matchEnd,
                                        result.getLength() - matchEnd,
                                        result.getText().substring(matchEnd, result.getLength()),
                                        Constants.ContextType_RelativeSuffix);
                                    break;
                                }
                            }

                            Map<String, Object> customData = new LinkedHashMap<>();
                            customData.put(Constants.SubType, result.getType());
                            customData.put(ExtendedModelResult.ParentTextKey, parentText);
                            customData.put(Constants.Context, contextErs);

                            relativeDatePeriodErs.add(new ExtractResult(
                                relativeTermsMatch.index,
                                relativeTermsMatch.length,
                                relativeTermsMatch.value,
                                Constants.SYS_DATETIME_DATETIMEALT,
                                customData));

                            Map<String, Object> resultData = new LinkedHashMap<>();
                            resultData.put(Constants.SubType, result.getType());
                            resultData.put(ExtendedModelResult.ParentTextKey, parentText);

                            result.setData(resultData);
                            result.setType(Constants.SYS_DATETIME_DATETIMEALT);
                            ers.set(i, result);
                        }
                    }
                }
            }
            i++;
        }

        results.addAll(ers);
        results.addAll(relativeDatePeriodErs);
        results.sort(Comparator.comparingInt(er -> er.getStart()));

        return results;
    }

    private boolean isConnectorOrWhiteSpace(int start, int end, String text) {
        if (end <= start) {
            return false;
        }

        String middleStr = text.substring(start, end).trim().toLowerCase();

        if (StringUtility.isNullOrEmpty(middleStr)) {
            return true;
        }

        Match[] orTermMatches = RegExpUtility.getMatches(config.getOrRegex(), middleStr);

        return orTermMatches.length == 1 && orTermMatches[0].index == 0 && orTermMatches[0].length == middleStr.length();
    }

    private boolean isEndsWithRangePrefix(String prefixText) {
        return RegexExtension.matchEnd(config.getRangePrefixRegex(), prefixText, true).getSuccess();
    }

    private boolean extractAndApplyMetadata(List<ExtractResult> extractResults, String parentText) {
        boolean success = extractAndApplyMetadata(extractResults, parentText, false);

        if (!success) {
            success = extractAndApplyMetadata(extractResults, parentText, true);
        }

        if (!success && shouldApplyParentText(extractResults)) {
            success = applyParentText(extractResults, parentText);
        }

        return success;
    }

    private boolean extractAndApplyMetadata(List<ExtractResult> extractResults, String parentText, boolean reverse) {
        if (reverse) {
            Collections.reverse(extractResults);
        }

        boolean success = false;

        // Currently, we support alt entity sequence only when the second alt entity to the last alt entity share the same type
        if (isSupportedAltEntitySequence(extractResults)) {
            HashMap<String, Object> metadata = extractMetadata(extractResults.stream().findFirst().get(), parentText, extractResults);
            HashMap<String, Object> metadataCandidate = null;

            int i = 0;
            while (i < extractResults.size()) {
                if (metadata == null) {
                    break;
                }

                int j = i + 1;

                while (j < extractResults.size()) {
                    metadataCandidate = extractMetadata(extractResults.get(j), parentText, extractResults);

                    // No context extracted, the context would follow the previous one
                    // Such as "Wednesday" in "next Tuesday or Wednesday"
                    if (metadataCandidate == null) {
                        j++;
                    } else {
                        // Current extraction has context, the context would not follow the previous ones
                        // Such as "Wednesday" in "next Monday or Tuesday or previous Wednesday"
                        break;
                    }
                }
                List<ExtractResult> ersShareContext = extractResults.subList(i, j);
                applyMetadata(ersShareContext, metadata, parentText);
                metadata = metadataCandidate;

                i = j;
                success = true;
            }
        }

        return  success;
    }

    private boolean shouldApplyParentText(List<ExtractResult> extractResults) {
        boolean shouldApply = false;

        if (isSupportedAltEntitySequence(extractResults)) {
            String firstEntityType = extractResults.stream().findFirst().get().getType();
            String lastEntityType = extractResults.get(extractResults.size() - 1).getType();

            if (firstEntityType.equals(Constants.SYS_DATETIME_DATE) && lastEntityType.equals(Constants.SYS_DATETIME_DATE)) {
                shouldApply = true; // "11/20 or 11/22"
            } else if (firstEntityType.equals(Constants.SYS_DATETIME_TIME) && lastEntityType.equals(Constants.SYS_DATETIME_TIME)) {
                shouldApply = true; // "7 oclock or 8 oclock"
            } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.equals(Constants.SYS_DATETIME_DATETIME)) {
                shouldApply = true; // "Monday 1pm or Tuesday 2pm"
            }
        }

        return shouldApply;
    }

    private boolean applyParentText(List<ExtractResult> extractResults, String parentText) {
        boolean success = false;

        if (isSupportedAltEntitySequence(extractResults)) {
            for (int i = 0; i < extractResults.size(); i++) {
                ExtractResult extractResult = extractResults.get(i);
                Map<String, Object> metadata = createMetadata(extractResult.getType(), parentText, null);
                Map<String, Object> data = mergeMetadata(extractResult.getData(), metadata);
                extractResult.setData(data);
                extractResult.setType(getExtractorName());
                extractResults.set(i, extractResult);
            }

            success = true;
        }

        return success;
    }

    private boolean isSupportedAltEntitySequence(List<ExtractResult> altEntities) {
        Stream<ExtractResult> subSeq = altEntities.stream().skip(1);
        List<String> entityTypes = new ArrayList<>();

        for (ExtractResult er : subSeq.toArray(ExtractResult[]::new)) {
            if (!entityTypes.contains(er.getType())) {
                entityTypes.add(er.getType());
            }
        }

        return entityTypes.size() == 1;
    }

    // This method is to extract metadata from the targeted ExtractResult
    // For cases like "next week Monday or Tuesday or previous Wednesday", ExtractMethods can be more than one
    private HashMap<String, Object> extractMetadata(ExtractResult targetEr, String parentText, List<ExtractResult> ers) {
        HashMap<String, Object> metadata = null;
        ArrayList<Function<String, List<ExtractResult>>> extractMethods = getExtractMethods(targetEr.getType(), ers.get(ers.size() - 1).getType());
        BiConsumer<ExtractResult, ExtractResult> postProcessMethod = getPostProcessMethod(targetEr.getType(), ers.get(ers.size() - 1).getType());
        ExtractResult contextEr = extractContext(targetEr, extractMethods, postProcessMethod);

        if (shouldCreateMetadata(ers, contextEr)) {
            metadata = createMetadata(targetEr.getType(), parentText, contextEr);
        }

        return metadata;
    }

    private ExtractResult extractContext(ExtractResult er, ArrayList<Function<String,
        List<ExtractResult>>> extractMethods, BiConsumer<ExtractResult, ExtractResult> postProcessMethod) {
        ExtractResult contextEr = null;

        for (Function<String, List<ExtractResult>> extractMethod : extractMethods) {
            List<ExtractResult> contextErCandidates = extractMethod.apply(er.getText());
            if (contextErCandidates.size() == 1) {
                contextEr = contextErCandidates.get(contextErCandidates.size() - 1);
                break;
            }
        }

        if (contextEr != null && postProcessMethod != null) {
            postProcessMethod.accept(contextEr, er);
        }

        if (contextEr != null && StringUtility.isNullOrEmpty(contextEr.getText())) {
            contextEr = null;
        }

        return contextEr;
    }

    private boolean shouldCreateMetadata(List<ExtractResult> originalErs, ExtractResult contextEr) {
        // For alternative entities sequence which are all DatePeriod, we should create metadata even if context is null
        return (contextEr != null ||
            (originalErs.get(0).getType() == Constants.SYS_DATETIME_DATEPERIOD && originalErs.get(originalErs.size() - 1).getType() == Constants.SYS_DATETIME_DATEPERIOD));
    }

    private void applyMetadata(List<ExtractResult> ers, HashMap<String, Object> metadata, String parentText) {
        // The first extract results don't need any context
        ExtractResult first = ers.stream().findFirst().orElse(null);

        // Share the timeZone info
        Map<String, Object> metaDataOrigin = (HashMap<String, Object>)first.getData();
        if (metaDataOrigin != null && metaDataOrigin.containsKey(Constants.SYS_DATETIME_TIMEZONE)) {
            metadata.put(Constants.SYS_DATETIME_TIMEZONE, metaDataOrigin.get(Constants.SYS_DATETIME_TIMEZONE));
        }

        HashMap<String, Object> metadataWithoutContext = createMetadata(first.getType(), parentText, null);
        first.setData(mergeMetadata(first.getData(), metadataWithoutContext));
        first.setType(getExtractorName());
        ers.set(0, first);

        for (int i = 1; i < ers.size(); i++) {
            ExtractResult er = ers.get(i);
            er.setData(mergeMetadata(ers.get(i).getData(), metadata));
            er.setType(getExtractorName());
            ers.set(i, er);
        }
    }

    private Map<String, Object> mergeMetadata(Object originalMetadata, Map<String, Object> newMetadata) {
        Map<String, Object> result = new HashMap<>();

        if (originalMetadata instanceof Map) {
            result = (Map)originalMetadata;
        }

        if (originalMetadata == null) {
            result = newMetadata;
        } else {
            for (Map.Entry<String, Object> data : newMetadata.entrySet()) {
                result.put(data.getKey(), data.getValue());
            }
        }

        return result;
    }

    private HashMap<String, Object> createMetadata(String subType, String parentText, ExtractResult contextEr) {
        HashMap<String, Object> data = new HashMap<>();

        if (!StringUtility.isNullOrEmpty(subType)) {
            data.put(Constants.SubType, subType);
        }

        if (!StringUtility.isNullOrEmpty(parentText)) {
            data.put(ExtendedModelResult.ParentTextKey, parentText);
        }

        if (contextEr != null) {
            data.put(Constants.Context, contextEr);
        }

        return data;
    }

    private List<ExtractResult> extractRelativePrefixContext(String entityText) {
        List<ExtractResult> results = new ArrayList<>();

        for (Pattern pattern : config.getRelativePrefixList()) {
            Matcher match = pattern.matcher(entityText);

            if (match.find()) {
                ExtractResult er = new ExtractResult();
                er.setText(match.group());
                er.setStart(match.start());
                er.setLength(match.end() - match.start());
                er.setType(Constants.ContextType_RelativePrefix);
                results.add(er);
            }
        }

        return  results;
    }

    private List<ExtractResult> extractAmPmContext(String entityText) {
        List<ExtractResult> results = new ArrayList<>();
        for (Pattern pattern : config.getAmPmRegexList()) {
            Matcher match = pattern.matcher(entityText);
            if (match.find()) {
                ExtractResult er = new ExtractResult();
                er.setText(match.group());
                er.setStart(match.start());
                er.setLength(match.end() - match.start());
                er.setType(Constants.ContextType_AmPm);
                results.add(er);
            }
        }

        return results;
    }

    private BiConsumer<ExtractResult, ExtractResult> getPostProcessMethod(String firstEntityType, String lastEntityType) {
        if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.equals(Constants.SYS_DATETIME_DATE)) {
            return (contextEr, originalEr) -> {
                contextEr.setText(originalEr.getText().substring(0, contextEr.getStart()) + originalEr.getText().substring(contextEr.getStart() + contextEr.getLength()));
                contextEr.setType(Constants.ContextType_RelativeSuffix);
            };
        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATE) && lastEntityType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
            return (contextEr, originalEr) -> {
                contextEr.setText(originalEr.getText().substring(0, contextEr.getStart()));
            };
        }

        return null;
    }

    private ArrayList<Function<String, List<ExtractResult>>> getExtractMethods(String firstEntityType, String lastEntityType) {
        ArrayList<Function<String, List<ExtractResult>>> methods = new ArrayList<>();

        if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.equals(Constants.SYS_DATETIME_TIME)) {

            // "Monday 7pm or 8pm"
            methods.add(config.getDateExtractor()::extract);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATE) && lastEntityType.equals(Constants.SYS_DATETIME_DATE)) {

            // "next week Monday or Tuesday", "previous Monday or Wednesday"
            methods.add(config.getDatePeriodExtractor()::extract);
            methods.add(this::extractRelativePrefixContext);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_TIME) && lastEntityType.equals(Constants.SYS_DATETIME_TIME)) {

            // "in the morning at 7 oclock or 8 oclock"
            methods.add(this::extractAmPmContext);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIME) && lastEntityType.equals(Constants.SYS_DATETIME_DATETIME)) {

            // "next week Mon 9am or Tue 1pm"
            methods.add(config.getDatePeriodExtractor()::extract);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.equals(Constants.SYS_DATETIME_TIMEPERIOD)) {

            // "Monday 7-8 am or 9-10am"
            methods.add(config.getDateExtractor()::extract);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATEPERIOD) && lastEntityType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {

            // For alt entities that are all DatePeriod, no need to share context

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATETIMEPERIOD) && lastEntityType.equals(Constants.SYS_DATETIME_DATE)) {

            // "Tuesday or Wednesday morning"
            methods.add(config.getDateExtractor()::extract);

        } else if (firstEntityType.equals(Constants.SYS_DATETIME_DATE) && lastEntityType.equals(Constants.SYS_DATETIME_DATEPERIOD)) {

            // "Monday this week or next week"
            methods.add(config.getDatePeriodExtractor()::extract);

        }

        return methods;
    }
}
