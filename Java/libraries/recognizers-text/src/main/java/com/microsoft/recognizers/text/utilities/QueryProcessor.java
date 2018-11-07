package com.microsoft.recognizers.text.utilities;

import java.util.Arrays;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class QueryProcessor {


    public static String preprocess(String query) {
        return QueryProcessor.preprocess(query, false, true);
    }

    public static String preprocess(String query, boolean caseSensitive) {
        return QueryProcessor.preprocess(query, true);
    }

    public static String preprocess(String query, boolean caseSensitive, boolean recode) {

        if (recode) {
            query = query.replace('０', '0')
                         .replace('１', '1')
                         .replace('２', '2')
                         .replace('３', '3')
                         .replace('４', '4')
                         .replace('５', '5')
                         .replace('６', '6')
                         .replace('７', '7')
                         .replace('８', '8')
                         .replace('９', '9')
                         .replace('：', ':')
                         .replace('－', '-')
                         .replace('，', ',')
                         .replace('／', '/')
                         .replace('Ｇ', 'G')
                         .replace('Ｍ', 'M')
                         .replace('Ｔ', 'T')
                         .replace('Ｋ', 'K')
                         .replace('ｋ', 'k')
                         .replace('．', '.')
                         .replace('（', '(')
                         .replace('）', ')')
                         .replace('％', '%')
                         .replace('、', ',');
        }

        if (!caseSensitive) {
            query = query.toLowerCase();
        }

        return query;
    }

    public static String trimEnd(String input) {
        return input.replaceAll("\\s+$", "");
    }

    public static String trimEnd(String input, CharSequence chars) {
        return input.replaceAll("[" + Pattern.quote(chars.toString()) + "]+$", "");
    }

    public static List<String> split(String input, List<String> delimiters) {
        String delimitersRegex = String.join(
                "|",
                delimiters.stream()
                        .map(s -> Pattern.quote(s))
                        .collect(Collectors.toList()));

        return Arrays.stream(input.split(delimitersRegex)).filter(s -> !s.isEmpty())
                .collect(Collectors.toList());
    }
}
