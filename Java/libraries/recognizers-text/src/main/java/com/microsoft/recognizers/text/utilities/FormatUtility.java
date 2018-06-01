package com.microsoft.recognizers.text.utilities;

public class FormatUtility {
    public static String Preprocess(String query) {
        return FormatUtility.Preprocess(query, true);
    }

    public static String Preprocess(String query, boolean toLower) {
        if(toLower) {
            query = query.toLowerCase();
        }

        return query
                .replace('０', '0')
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
                .replace('）', ')');
    }
}
