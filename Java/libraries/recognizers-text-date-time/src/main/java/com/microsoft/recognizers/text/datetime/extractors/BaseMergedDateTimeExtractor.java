package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.Metadata;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ProcessedSuperfluousWords;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
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
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

import org.javatuples.Pair;

public class BaseMergedDateTimeExtractor implements IDateTimeExtractor {

    private final IMergedExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return "";
    }

    public BaseMergedDateTimeExtractor(IMergedExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {

        List<ExtractResult> ret = new ArrayList<>();
        String originInput = input;
        Iterable<MatchResult<String>> superfluousWordMatches = null;

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            ProcessedSuperfluousWords processedSuperfluousWords = MatchingUtil.preProcessTextRemoveSuperfluousWords(input, this.config.getSuperfluousWordMatcher());
            input = processedSuperfluousWords.getText();
            superfluousWordMatches = processedSuperfluousWords.getSuperfluousWordMatches();
        }

        // The order is important, since there is a problem in merging
        addTo(ret, this.config.getDateExtractor().extract(input, reference), input);
        addTo(ret, this.config.getTimeExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDatePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDurationExtractor().extract(input, reference), input);
        addTo(ret, this.config.getTimePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDateTimePeriodExtractor().extract(input, reference), input);
        addTo(ret, this.config.getDateTimeExtractor().extract(input, reference), input);
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

        // Remove common ambiguous cases
        ret = filterAmbiguity(ret, input);

        ret = addMod(ret, input);

        // filtering
        if (this.config.getOptions().match(DateTimeOptions.CalendarMode)) {
            checkCalendarFilterList(ret, input);
        }

        ret.sort(Comparator.comparingInt(r -> r.getStart()));

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            ret = MatchingUtil.posProcessExtractionRecoverSuperfluousWords(ret, superfluousWordMatches, originInput);
        }

        return ret;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    private List<ExtractResult> filterAmbiguity(List<ExtractResult> extractResults, String input) {
        if (config.getAmbiguityFiltersDict() != null) {
            for (Pair<Pattern, Pattern> pair : config.getAmbiguityFiltersDict()) {
                final Pattern key = pair.getValue0();
                final Pattern value = pair.getValue1();

                for (ExtractResult extractResult : extractResults) {
                    Optional<Match> keyMatch = Arrays.stream(RegExpUtility.getMatches(key, extractResult.getText())).findFirst();
                    if (keyMatch.isPresent()) {
                        final Match[] matches = RegExpUtility.getMatches(value, input);
                        extractResults = extractResults.stream()
                            .filter(er -> Arrays.stream(matches).noneMatch(m -> m.index < er.getStart() + er.getLength() && m.index + m.length > er.getStart()))
                            .collect(Collectors.toList());
                    }
                }
            }
        }

        return extractResults;
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

            dst.sort(Comparator.comparingInt(a -> a.getStart()));
        }
    }

    private boolean shouldSkipFromToMerge(ExtractResult er) {
        return Arrays.stream(RegExpUtility.getMatches(config.getFromToRegex(), er.getText())).findFirst().isPresent();
    }

    private List<ExtractResult> numberEndingRegexMatch(String text, List<ExtractResult> extractResults) {
        List<Token> tokens = new ArrayList<>();

        for (ExtractResult extractResult : extractResults) {
            if (extractResult.getType().equals(Constants.SYS_DATETIME_TIME) || extractResult.getType().equals(Constants.SYS_DATETIME_DATETIME)) {
                String stringAfter = text.substring(extractResult.getStart() + extractResult.getLength());
                Pattern numberEndingPattern = this.config.getNumberEndingPattern();
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(numberEndingPattern, stringAfter)).findFirst();
                if (match.isPresent()) {
                    MatchGroup newTime = match.get().getGroup("newTime");
                    List<ExtractResult> numRes = this.config.getIntegerExtractor().extract(newTime.value);
                    if (numRes.size() == 0) {
                        continue;
                    }

                    int startPosition = extractResult.getStart() + extractResult.getLength() + newTime.index;
                    tokens.add(new Token(startPosition, startPosition + newTime.length));
                }

            }
        }

        return Token.mergeAllTokens(tokens, text, Constants.SYS_DATETIME_TIME);
    }

    private List<ExtractResult> filterUnspecificDatePeriod(List<ExtractResult> ers, String text) {
        ers.removeIf(er -> Arrays.stream(RegExpUtility.getMatches(config.getUnspecificDatePeriodRegex(), er.getText())).findFirst().isPresent());
        return ers;
    }

    private List<ExtractResult> addMod(List<ExtractResult> ers, String text) {
        int index = 0;

        for (ExtractResult er : ers.toArray(new ExtractResult[0])) {
            MergeModifierResult modifiedToken = tryMergeModifierToken(er, config.getBeforeRegex(), text);

            if (!modifiedToken.result) {
                modifiedToken = tryMergeModifierToken(er, config.getAfterRegex(), text);
            }

            if (!modifiedToken.result) {
                // SinceRegex in English contains the term "from" which is potentially ambiguous with ranges in the form "from X to Y"
                modifiedToken = tryMergeModifierToken(er, config.getSinceRegex(), text, true);
            }

            if (!modifiedToken.result) {
                modifiedToken = tryMergeModifierToken(er, config.getAroundRegex(), text);
            }

            ers.set(index, modifiedToken.er);

            final ExtractResult newEr = modifiedToken.er;

            if (newEr.getType().equals(Constants.SYS_DATETIME_DATEPERIOD) || newEr.getType().equals(Constants.SYS_DATETIME_DATE) || 
                newEr.getType().equals(Constants.SYS_DATETIME_TIME)) {
                
                // 2012 or after/above, 3 pm or later
                String afterStr = text.substring(newEr.getStart() + newEr.getLength()).toLowerCase();

                ConditionalMatch match = RegexExtension.matchBegin(config.getSuffixAfterRegex(), StringUtility.trimStart(afterStr), true);

                if (match.getSuccess()) {
                    boolean isFollowedByOtherEntity = true;

                    if (match.getMatch().get().length == afterStr.trim().length()) {
                        isFollowedByOtherEntity = false;

                    } else {
                        String nextStr = afterStr.trim().substring(match.getMatch().get().length).trim();
                        ExtractResult nextEr = ers.stream().filter(t -> t.getStart() > newEr.getStart()).findFirst().orElse(null);

                        if (nextEr == null || !nextStr.startsWith(nextEr.getText())) {
                            isFollowedByOtherEntity = false;
                        }
                    }

                    if (!isFollowedByOtherEntity) {
                        int modLength = match.getMatch().get().length + afterStr.indexOf(match.getMatch().get().value);
                        int length = newEr.getLength() + modLength;
                        String newText = text.substring(newEr.getStart(), newEr.getStart() + length);

                        er.setMetadata(assignModMetadata(er.getMetadata()));

                        ers.set(index, new ExtractResult(er.getStart(), length, newText, er.getType(), er.getData(), er.getMetadata()));
                    }
                }
            }

            index++;
        }

        return ers;
    }

    private MergeModifierResult tryMergeModifierToken(ExtractResult er, Pattern tokenRegex, String text) {
        return tryMergeModifierToken(er, tokenRegex, text, false);
    }

    private MergeModifierResult tryMergeModifierToken(ExtractResult er, Pattern tokenRegex, String text, boolean potentialAmbiguity) {
        String beforeStr = text.substring(0, er.getStart()).toLowerCase();

        // Avoid adding mod for ambiguity cases, such as "from" in "from ... to ..." should not add mod
        if (potentialAmbiguity &&  config.getAmbiguousRangeModifierPrefix() != null &&
            Arrays.stream(RegExpUtility.getMatches(config.getAmbiguousRangeModifierPrefix(), text)).findFirst().isPresent()) {
            final Match[] matches = RegExpUtility.getMatches(config.getPotentialAmbiguousRangeRegex(), text);
            if (Arrays.stream(matches).anyMatch(m -> m.index < er.getStart() + er.getLength() && m.index + m.length > er.getStart())) {
                return new MergeModifierResult(false, er);
            }
        }

        ResultIndex result = hasTokenIndex(StringUtility.trimEnd(beforeStr), tokenRegex);
        if (result.getResult()) {
            int modLength = beforeStr.length() - result.getIndex();
            int length = er.getLength() + modLength;
            int start = er.getStart() - modLength;
            String newText = text.substring(start, start + length);

            er.setText(newText);
            er.setLength(length);
            er.setStart(start);

            er.setMetadata(assignModMetadata(er.getMetadata()));

            return new MergeModifierResult(true, er);
        }

        return new MergeModifierResult(false, er);
    }

    private Metadata assignModMetadata(Metadata metadata) {

        if (metadata == null) {
            metadata = new Metadata() {
                {
                    setHasMod(true);
                }
            };
        } else {
            metadata.setHasMod(true);
        }

        return metadata;
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
        List<ExtractResult> shallowCopy = new ArrayList<>(ers);
        Collections.reverse(shallowCopy);
        for (ExtractResult er : shallowCopy) {
            for (Pattern negRegex : this.config.getFilterWordRegexList()) {
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(negRegex, er.getText())).findFirst();
                if (match.isPresent()) {
                    ers.remove(er);
                }
            }
        }
    }

    private class MergeModifierResult {
        public final boolean result;
        public final ExtractResult er;

        private MergeModifierResult(boolean result, ExtractResult er) {
            this.result = result;
            this.er = er;
        }
    }
}
