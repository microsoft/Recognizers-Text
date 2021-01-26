package com.microsoft.recognizers.text.utilities;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Stack;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.atomic.AtomicReference;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.javatuples.Pair;

public abstract class RegExpUtility {

    private static final Pattern matchGroup = Pattern.compile("\\?<(?<name>\\w+)>", Pattern.CASE_INSENSITIVE);
    private static final Pattern matchGroupNames = Pattern.compile("\\(\\?<([a-zA-Z][a-zA-Z0-9]*)>", Pattern.CASE_INSENSITIVE);
    private static final Pattern matchPositiveLookbehind = Pattern.compile("\\(\\?<=", Pattern.CASE_INSENSITIVE);
    private static final Pattern matchNegativeLookbehind = Pattern.compile("\\(\\?<!", Pattern.CASE_INSENSITIVE);
    private static final String groupNameIndexSep = "iii";
    private static final String groupNameIndexSepRegex = Pattern.quote(groupNameIndexSep);
    private static final Pattern lookBehindCheckRegex = Pattern.compile("(\\\\?<[!=])");
    private static final Map<Character, String> bindings = new HashMap<Character, String>() {
        {
            put('+', "{1,10}");
            put('*', "{0,10}");
        }
    };

    public static Pattern getSafeRegExp(String source) {
        return getSafeRegExp(source, 0);
    }

    public static Pattern getSafeRegExp(String source, int flags) {
        String sanitizedSource = sanitizeGroups(source);
        return Pattern.compile(sanitizedSource, flags);
    }

    public static Map<String, String> getNamedGroups(Matcher groupedMatcher) {
        return getNamedGroups(groupedMatcher, false);
    }

    public static Map<String, String> getNamedGroups(Matcher groupedMatcher, boolean sanitize) {

        Map<String, String> matchedGroups = new LinkedHashMap<>();
        Matcher m = matchGroupNames.matcher(groupedMatcher.pattern().pattern());

        while (m.find()) {
            String groupName = m.group(1);
            String groupValue = groupedMatcher.group(groupName);
            if (sanitize && groupName.contains(groupNameIndexSep)) {
                groupName = groupName.substring(0, groupName.lastIndexOf(groupNameIndexSep));
            }

            if (!groupName.contains(groupNameIndexSep)) {
                groupName = groupName.replace("ii", "_");
            }

            // If matchedGroups previously contained a mapping for groupName, the old value is replaced.
            if (groupValue != null) {
                matchedGroups.put(groupName, groupValue);
            }
        }

        return matchedGroups;
    }

    public static Match[] getMatches(Pattern regex, String source) {

        if (regex == null) {
            return new Match[0];
        }

        String rawRegex = regex.pattern();
        if (!rawRegex.contains("(?<nlbii")) {
            return getMatchesSimple(regex, source);
        }

        List<Match> realMatches = new ArrayList<>();
        List<Pair<Pattern, Pattern>> negativeLookbehindRegexes = new ArrayList<>();
        int flags = regex.flags();

        int closePos = 0;
        int startPos = rawRegex.indexOf("(?<nlbii", 0);

        while (startPos >= 0) {
            closePos = getClosePos(rawRegex, startPos);
            Pattern nlbRegex = Pattern.compile(rawRegex.substring(startPos, closePos + 1), flags);
            String nextRegex = getNextRegex(rawRegex, startPos);

            negativeLookbehindRegexes.add(Pair.with(nlbRegex, nextRegex != null ? Pattern.compile(nextRegex, flags) : null));

            rawRegex = rawRegex.substring(0, startPos) + rawRegex.substring(closePos + 1);
            startPos = rawRegex.indexOf("(?<nlbii");
        }

        Pattern tempRegex = Pattern.compile(rawRegex, flags);
        Match[] tempMatches = getMatchesSimple(tempRegex, source);

        Arrays.stream(tempMatches).forEach(match -> {

            AtomicBoolean isClean = new AtomicBoolean(true);
            negativeLookbehindRegexes.forEach((pair) -> {

                Pattern currRegex = pair.getValue0();
                Match[] negativeLookbehindMatches = getMatchesSimple(currRegex, source);
                Arrays.stream(negativeLookbehindMatches).forEach(negativeLookbehindMatch -> {

                    int negativeLookbehindEnd = negativeLookbehindMatch.index + negativeLookbehindMatch.length;
                    Pattern nextRegex = pair.getValue1();

                    if (match.index == negativeLookbehindEnd) {

                        if (nextRegex == null) {
                            isClean.set(false);
                            return;
                        } else {
                            Match nextMatch = getFirstMatchIndex(nextRegex, source.substring(negativeLookbehindMatch.index));
                            if (nextMatch != null && ((nextMatch.index == negativeLookbehindMatch.length) || (source.contains(nextMatch.value + match.value)))) {
                                isClean.set(false);
                                return;
                            }

                        }
                    }

                    if (negativeLookbehindMatch.value.contains(match.value)) {

                        Match[] preMatches = getMatchesSimple(regex, source.substring(0, match.index));
                        Arrays.stream(preMatches).forEach(preMatch -> {
                            if (source.contains(preMatch.value + match.value)) {
                                isClean.set(false);
                                return;
                            }
                        });
                    }
                });

                if (!isClean.get()) {
                    return;
                }
            });

            if (isClean.get()) {
                realMatches.add(match);
            }
        });

        return realMatches.toArray(new Match[realMatches.size()]);
    }

    private static String sanitizeGroups(String source) {

        String result = source;

        AtomicInteger index = new AtomicInteger(0);
        result = replace(result, matchGroup, (Matcher m) -> m.group(0).replace(m.group(1), m.group(1).replace("_", "ii") + groupNameIndexSep + index.getAndIncrement()));

        index.set(0);
        result = replace(result, matchPositiveLookbehind, (Matcher m) -> String.format("(?<plb%s%s>", groupNameIndexSep, index.getAndIncrement()));

        index.set(0);
        result = replace(result, matchNegativeLookbehind, (Matcher m) -> String.format("(?<nlb%s%s>", groupNameIndexSep, index.getAndIncrement()));

        return result;
    }

    public static Pattern getSafeLookbehindRegExp(String source) {
        return getSafeLookbehindRegExp(source, 0);
    }

    public static Pattern getSafeLookbehindRegExp(String source, int flags) {
        String result = source;
        result = bindLookbehinds(result);
        return Pattern.compile(result, flags);
    }

    private static String bindLookbehinds(String regex) {

        String result = regex;
        Stack<Integer> replaceStack = new Stack<>();

        Matcher matcher = lookBehindCheckRegex.matcher(regex);

        while (matcher.find()) {
            getReplaceIndexes(result, matcher.start(), replaceStack);
        }

        if (!replaceStack.empty()) {

            StringBuilder buffer = new StringBuilder(result);
            while (!replaceStack.isEmpty()) {
                int idx = replaceStack.peek();
                buffer.replace(idx, idx + 1, bindings.get(result.charAt(idx)));
                replaceStack.pop();
            }

            result = buffer.toString();
        }

        return result;
    }

    private static void getReplaceIndexes(String input, int startIndex, Stack<Integer> replaceStack) {

        int idx = startIndex + 3;
        Stack<Character> stack = new Stack<>();

        while (idx < input.length()) {
            switch (input.charAt(idx)) {
                case ')':
                    if (stack.isEmpty()) {
                        idx = input.length();
                    } else {
                        stack.pop();
                    }
                    break;
                case '(':
                    stack.push('(');
                    break;
                case '*':
                case '+':
                    replaceStack.push(idx);
                    break;
                case '|':
                    if (stack.isEmpty()) {
                        idx = input.length();
                    }
                    break;
                default:
                    break;
            }

            idx += 1;
        }
    }

    private static Match[] getMatchesSimple(Pattern regex, String source) {

        List<Match> matches = new ArrayList<>();

        Matcher match = regex.matcher(source);
        while (match.find()) {

            List<Pair<String, String>> positiveLookbehinds = new ArrayList<>();
            Map<String, MatchGroup> groups = new HashMap<>();
            AtomicReference<String> lastGroup = new AtomicReference<>("");

            getNamedGroups(match).forEach((key, groupValue) -> {

                if (!key.contains(groupNameIndexSep)) {
                    return;
                }

                if (key.startsWith("plb") && !StringUtility.isNullOrEmpty(match.group(key))) {

                    if (match.group(0).indexOf(match.group(key)) != 0 && !StringUtility.isNullOrEmpty(lastGroup.get())) {

                        int index = match.start() + match.group(0).indexOf(match.group(key));
                        int length = match.group(key).length();
                        String value = source.substring(index, index + length);

                        MatchGroup lastMatchGroup = groups.get(lastGroup.get());
                        groups.replace(lastGroup.get(), new MatchGroup(
                                lastMatchGroup.value + value,
                                lastMatchGroup.index,
                                lastMatchGroup.length,
                                lastMatchGroup.captures));
                    }

                    positiveLookbehinds.add(Pair.with(key, match.group(key)));
                    return;
                }

                if (key.startsWith("nlb")) {
                    return;
                }

                String groupKey = key.substring(0, key.lastIndexOf(groupNameIndexSep)).replace("ii", "_");
                lastGroup.set(groupKey);

                if (!groups.containsKey(groupKey)) {
                    groups.put(groupKey, new MatchGroup("", 0, 0, new Capture[0]));
                }

                if (!StringUtility.isNullOrEmpty(match.group(key))) {

                    int index = match.start(key);
                    int length = match.group(key).length();
                    String value = source.substring(index, match.end(key));
                    List<Capture> captures = new ArrayList<>(Arrays.asList(groups.get(groupKey).captures));
                    captures.add(new Capture(value, index, length));

                    groups.replace(groupKey, new MatchGroup(value, index, length, captures.toArray(new Capture[0])));
                }
            });

            String value = match.group(0);
            int index = match.start();
            int length = value.length();

            if (positiveLookbehinds.size() > 0 && value.indexOf(positiveLookbehinds.get(0).getValue1()) == 0) {
                int valueLength = positiveLookbehinds.get(0).getValue1().length();
                value = source.substring(index, index + length).substring(valueLength);
                index += valueLength;
                length -= valueLength;
            } else {
                value = source.substring(index, index + length);
            }

            matches.add(new Match(index, length, value, groups));
        }

        return matches.toArray(new Match[matches.size()]);
    }

    private static Match getFirstMatchIndex(Pattern regex, String source) {

        Match[] matches = getMatches(regex, source);
        if (matches.length > 0) {
            return matches[0];
        }

        return null;
    }

    private static String getNextRegex(String source, int startPos) {

        startPos = getClosePos(source, startPos) + 1;
        int closePos = getClosePos(source, startPos);
        if (source.charAt(startPos) != '(') {
            closePos--;
        }

        String next = (startPos == closePos) ?
                null :
                source.substring(startPos, closePos + 1);

        return next;
    }

    private static int getClosePos(String rawRegex, int startPos) {

        int counter = 1;
        int closePos = startPos;

        while (counter > 0 && closePos < rawRegex.length()) {

            ++closePos;
            if (closePos < rawRegex.length()) {
                char c = rawRegex.charAt(closePos);
                if (c == '(') {
                    counter++;
                } else if (c == ')') {
                    counter--;
                }
            }
        }

        return closePos;
    }

    public static String replace(String input, Pattern regex, StringReplacerCallback callback) {

        StringBuffer resultString = new StringBuffer();
        Matcher regexMatcher = regex.matcher(input);

        while (regexMatcher.find()) {
            String replacement = callback.replace(regexMatcher);
            regexMatcher.appendReplacement(resultString, replacement);
        }

        regexMatcher.appendTail(resultString);

        return resultString.toString();
    }
}