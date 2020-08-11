package com.microsoft.recognizers.text.utilities;

import java.text.Normalizer;
import java.util.Arrays;
import java.util.List;
import java.util.Locale;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class QueryProcessor {


    public static String preprocess(String query) {
        return QueryProcessor.preprocess(query, false, true);
    }

    public static String preprocess(String query, boolean caseSensitive) {
        return QueryProcessor.preprocess(query, caseSensitive, true);
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
        } else {
            query = toLowerTermSensitive(query);
        }

        return query;
    }

    private static final String tokens = "(kB|K[Bb]?|M[BbM]?|G[Bb]?|B)";
    private static final String expression = "(?<=(\\s|\\d))" + tokens + "\\b";
    private static final Pattern special_tokens_regex = Pattern.compile(expression, Pattern.UNICODE_CHARACTER_CLASS);

    private static String toLowerTermSensitive(String input) {

        char[] inputChars = input.toLowerCase(Locale.ROOT).toCharArray();

        Match[] matches = RegExpUtility.getMatches(special_tokens_regex, input);
        for (Match match : matches) {
            QueryProcessor.applyReverse(match.index, inputChars, match.value);
        }

        return new String(inputChars);
    }

    private static void applyReverse(int index, char[] inputChars, String value) {
        for (int i = 0; i < value.length(); ++i) {
            inputChars[index + i] = value.charAt(i);
        }
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

    public static String removeDiacritics(String query) {
        if (query == null) {
            return null;
        }

        String norm = Normalizer.normalize(query, Normalizer.Form.NFD);
        int j = 0;
        char[] out = new char[query.length()];
        for (int i = 0, n = norm.length(); i < n; ++i) {
            char c = norm.charAt(i);
            int type = Character.getType(c);

            if (type != Character.NON_SPACING_MARK) {
                out[j] = c;
                j++;
            }
        }

        return new String(out);
    }
}
