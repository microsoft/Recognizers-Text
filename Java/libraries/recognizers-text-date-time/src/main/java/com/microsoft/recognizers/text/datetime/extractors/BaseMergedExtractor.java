package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ProcessedSuperfluousWords;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.matcher.MatchResult;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.Locale;
import java.util.Optional;
import java.util.regex.Pattern;

public class BaseMergedExtractor implements IDateTimeExtractor {

    private final IMergedExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return "";
    }

    public BaseMergedExtractor(IMergedExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<ExtractResult> ret = new ArrayList<>();
        String originInput = input;
        Iterable<MatchResult<String>> superfluousWordMatches = null;
        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            ProcessedSuperfluousWords processedSuperfluousWords = MatchingUtil.preProcessTextRemoveSuperfluousWords(input, this.config.getSuperfluousWordMatcher());
            input = processedSuperfluousWords.text;
            superfluousWordMatches = processedSuperfluousWords.superfluousWordMatches;
        }
        // The order is important, since there is a problem in merging
        addTo(ret, this.config.getDateExtractor().extract(input, reference), input);
        addTo(ret, this.config.getTimeExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDurationExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDatePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDateTimeExtractor().extract(input, reference), input);
        addTo(ret, this.config.getTimePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDateTimePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getSetExtractor().extract(input, reference), input);
        addTo(ret, this.config.getHolidayExtractor().extract(input, reference), input);

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            addTo(ret, this.config.getTimeZoneExtractor().extract(input, reference), input);
            ret = this.config.getTimeZoneExtractor().removeAmbiguousTimezone(ret);
        }

        // This should be at the end since if need the extractor to determine the previous text contains time or not
        addTo(ret, numberEndingRegexMatch(input, ret), input);

        // modify time entity to an alternative DateTime expression if it follows a DateTime entity
        if (this.config.getOptions().match(DateTimeOptions.ExtendedTypes)) {
            ret = this.config.getDateTimeAltExtractor().extract(ret, input, reference);
        }

        ret = filterUnspecificDatePeriod(ret, input);
        addMod(ret, input);

        // filtering
        if (this.config.getOptions().match(DateTimeOptions.CalendarMode)) {
            checkCalendarFilterList(ret, input);
        }

        ret.sort(Comparator.comparingInt(r -> r.start));

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            ret = MatchingUtil.posProcessExtractionRecoverSuperfluousWords(ret, superfluousWordMatches, originInput);
        }

        return ret;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    private void addTo(List<ExtractResult> dst, List<ExtractResult> src, String text) {
        for (ExtractResult result : src) {
            if (config.getOptions().match(DateTimeOptions.SkipFromToMerge)) {
                if (shouldSkipFromToMerge(result)) {
                    continue;
                }
            }

            boolean isFound = false;
            List<Integer> overlapIndexes = new ArrayList<>();
            int firstIndex = -1;
            for (int i = 0; i < dst.size(); i++) {
                if (dst.get(i).isOverlap(result)) {
                    isFound = true;
                    if (dst.get(i).isCover(result)) {
                        if (firstIndex == -1) {
                            firstIndex = i;
                        }

                        overlapIndexes.add(i);
                    } else {
                        break;
                    }
                }
            }

            if (!isFound) {
                dst.add(result);
            } else if (overlapIndexes.size() > 0) {
                List<ExtractResult> tempDst = new ArrayList<>();
                for (int i = 0; i < dst.size(); i++) {
                    if (!overlapIndexes.contains(i)) {
                        tempDst.add(dst.get(i));
                    }
                }

                // insert at the first overlap occurrence to keep the order
                tempDst.add(firstIndex, result);
                dst.clear();
                dst.addAll(tempDst);
            }
        }
    }

    private boolean shouldSkipFromToMerge(ExtractResult er) {
        return Arrays.stream(RegExpUtility.getMatches(config.getFromToRegex(), er.text)).findFirst().isPresent();
    }

    private List<ExtractResult> numberEndingRegexMatch(String text, List<ExtractResult> extractResults) {
        List<Token> tokens = new ArrayList<>();

        for (ExtractResult extractResult : extractResults) {
            if (extractResult.type.equals(Constants.SYS_DATETIME_TIME) || extractResult.type.equals(Constants.SYS_DATETIME_DATETIME)) {
                String stringAfter = text.substring(extractResult.start + extractResult.length);
                Pattern numberEndingPattern = this.config.getNumberEndingPattern();
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(numberEndingPattern, stringAfter)).findFirst();
                if (match.isPresent()) {
                    MatchGroup newTime = match.get().getGroup("newTime");
                    List<ExtractResult> numRes = this.config.getIntegerExtractor().extract(newTime.value);
                    if (numRes.size() == 0) {
                        continue;
                    }

                    int startPosition = extractResult.start + extractResult.length + newTime.index;
                    tokens.add(new Token(startPosition, startPosition + newTime.length));
                }

            }
        }

        return Token.mergeAllTokens(tokens, text, Constants.SYS_DATETIME_TIME);
    }

    private List<ExtractResult> filterUnspecificDatePeriod(List<ExtractResult> ers, String text) {
        ers.removeIf(er -> Arrays.stream(RegExpUtility.getMatches(config.getUnspecificDatePeriodRegex(), er.text)).findFirst().isPresent());
        return ers;
    }

    private void addMod(List<ExtractResult> ers, String text) {
        int lastEnd = 0;
        int idx = 0;
        for (ExtractResult er : ers.toArray(new ExtractResult[0])) {
            String beforeStr = text.substring(lastEnd, er.start).toLowerCase(Locale.ROOT);

            ResultIndex resultIndex = hasTokenIndex(beforeStr.replaceAll("\\s+$", ""), config.getBeforeRegex());
            if (resultIndex.result) {
                int modLenght = beforeStr.length() - resultIndex.index;
                er = er.withLength(er.length + modLenght);
                er = er.withStart(er.start - modLenght);
                er = er.withText(text.substring(er.start, er.start + er.length));
            }

            resultIndex = hasTokenIndex(beforeStr.replaceAll("\\s+$", ""), config.getAfterRegex());
            if (resultIndex.result) {
                int modLenght = beforeStr.length() - resultIndex.index;
                er = er.withLength(er.length + modLenght);
                er = er.withStart(er.start - modLenght);
                er = er.withText(text.substring(er.start, er.start + er.length));
            }

            resultIndex = hasTokenIndex(beforeStr.replaceAll("\\s+$", ""), config.getSinceRegex());
            if (resultIndex.result) {
                int modLenght = beforeStr.length() - resultIndex.index;
                er = er.withLength(er.length + modLenght);
                er = er.withStart(er.start - modLenght);
                er = er.withText(text.substring(er.start, er.start + er.length));
            }

            if (er.type.equals(Constants.SYS_DATETIME_DATEPERIOD)) {
                // 2012 or after/above
                String afterStr = text.substring(er.start + er.length).toLowerCase(Locale.ROOT);

                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getDateAfterRegex(), StringUtility.trimStart(afterStr))).findFirst();
                if (match.isPresent() && match.get().index == 0 && match.get().length == afterStr.trim().length()) {
                    int modLenght = match.get().length + afterStr.indexOf(match.get().value);
                    er = er.withLength(er.length + modLenght);
                    er = er.withText(text.substring(er.start, er.start + er.length));
                }
            }
            ers.set(idx, er);
            idx++;
        }
    }

    private ResultIndex hasTokenIndex(String text, Pattern pattern) {
        Match[] matches = RegExpUtility.getMatches(pattern, text);

        // Support cases has two or more specific tokens
        // For example, "show me sales after 2010 and before 2018 or before 2000"
        // When extract "before 2000", we need the second "before" which will be matched in the second Regex match
        for (Match match : matches) {
            if (StringUtility.isNullOrWhiteSpace(text.substring(match.index + match.length))) {
                return new ResultIndex(true, match.index);
            }
        }

        return new ResultIndex(false, -1);
    }

    private void checkCalendarFilterList(List<ExtractResult> ers, String text) {
        List<ExtractResult> shallowCopy = ers.subList(0, ers.size());
        Collections.reverse(shallowCopy);
        for (ExtractResult er : shallowCopy) {
            for (Pattern negRegex : this.config.getFilterWordRegexList()) {
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(negRegex, er.text)).findFirst();
                if (match.isPresent()) {
                    ers.remove(er);
                }
            }
        }
    }
}
