package com.microsoft.recognizers.text.utilities;

import java.util.*;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public abstract class RegExpUtility {

    private static final Pattern matchGroup = Pattern.compile("\\?<(?<name>\\w+)>", Pattern.CASE_INSENSITIVE);
    private static final Pattern matchGroupNames = Pattern.compile("\\(\\?<([a-zA-Z][a-zA-Z0-9]*)>");
    private static final String groupNameIndexSep = "iii";
    private static final String groupNameIndexSepRegex = Pattern.quote(groupNameIndexSep);


    public static String sanitize(String source) {
     return sanitizeGroups(source);
    }

    private static String sanitizeGroups(String source) {
        AtomicInteger index = new AtomicInteger(0);

        // JAVE 9
        // . return matchGroup.matcher(source).replaceAll((MatchResult r) -> r.group() + "__" + index.getAndIncrement());

        StringBuffer resultString = new StringBuffer();
        Matcher matcher = matchGroup.matcher(source);
        while (matcher.find()) {
            // You can vary the replacement text for each match on-the-fly
            // String computed_replacement = ....
            String replacement = matcher.group(0).replace(matcher.group(1), matcher.group(1) + groupNameIndexSep + index.getAndIncrement());
            matcher.appendReplacement(resultString, replacement);
        }

        matcher.appendTail(resultString);

        return resultString.toString();
    }

    public static Map<String, String> getNamedGroups(Matcher groupedMatcher) {
        Map<String, String> matchedGroups = new HashMap<>();
        Matcher m = matchGroupNames.matcher(groupedMatcher.pattern().pattern());
        while (m.find()) {
            String groupName = m.group(1);
            String groupValue = groupedMatcher.group(groupName);
            groupName = groupName.split(groupNameIndexSepRegex)[0];
            if(groupValue != null) {
                matchedGroups.put(groupName, groupValue);
            }
        }

        return matchedGroups;
    }

}
