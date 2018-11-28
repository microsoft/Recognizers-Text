package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public abstract class RegexExtension {
    // Regex match with match length equals to text length
    public static boolean isExactMatch(Pattern regex, String text, boolean trim) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, text)).findFirst();
        int length = trim ? text.trim().length() : text.length();

        return (match.isPresent() && match.get().length == length);
    }

    // We can't trim before match as we may use the match index later
    public static ConditionalMatch matchExact(Pattern regex, String text, boolean trim) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, text)).findFirst();
        int length = trim ? text.trim().length() : text.length();

        return new ConditionalMatch(match, (match.isPresent() && match.get().length == length));
    }

    // We can't trim before match as we may use the match index later
    public static ConditionalMatch matchEnd(Pattern regex, String text, boolean trim) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, text)).reduce((f, s) -> s);
        String strAfter = "";
        if (match.isPresent()) {
            strAfter = text.substring(match.get().index + match.get().length);

            if (trim) {
                strAfter = strAfter.trim();
            }
        }

        return new ConditionalMatch(match, (match.isPresent() && StringUtility.isNullOrEmpty(strAfter)));
    }

    // We can't trim before match as we may use the match index later
    public static ConditionalMatch matchBegin(Pattern regex, String text, boolean trim) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, text)).findFirst();
        String strBefore = "";

        if (match.isPresent()) {
            strBefore = text.substring(0, match.get().index);

            if (trim) {
                strBefore = strBefore.trim();
            }
        }

        return new ConditionalMatch(match, (match.isPresent() && StringUtility.isNullOrEmpty(strBefore)));
    }
}
