// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.utilities.QueryProcessor;
import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;
import org.apache.commons.lang3.StringUtils;

public final class NumberFormatUtility {

    private NumberFormatUtility() {
    }

    private static final Map<String, LongFormatType> supportedCultures;

    static {
        supportedCultures = new HashMap<>();
        supportedCultures.put(Culture.English, LongFormatType.DoubleNumCommaDot);
        supportedCultures.put(Culture.Spanish, LongFormatType.DoubleNumDotComma);
        supportedCultures.put(Culture.Portuguese, LongFormatType.DoubleNumDotComma);
        supportedCultures.put(Culture.French, LongFormatType.DoubleNumDotComma);
        supportedCultures.put(Culture.German, LongFormatType.DoubleNumDotComma);
        supportedCultures.put(Culture.Chinese, null);
        supportedCultures.put(Culture.Japanese, LongFormatType.DoubleNumDotComma);
    }

    public static String format(Object value, CultureInfo culture) {

        Double doubleValue = (Double)value;
        String result;

        // EXPONENTIAL_AT: [-5, 15] });
        // For small positive decimal places. E.g.: 0,000015 or 0,0000015 -> 1.5E-05 or 1.5E-06
        try {
            if (doubleValue > 0 && doubleValue != Math.round(doubleValue) && doubleValue < 1E-4) {
                result = doubleValue.toString();
            } else {
                BigDecimal bc = new BigDecimal(doubleValue, new MathContext(15, RoundingMode.HALF_EVEN));
                result = bc.toString();
            }
        } catch (NumberFormatException ex) {
            return value.toString();
        }

        result = result.replace('e', 'E');
        if (result.contains("E-")) {
            String[] parts = result.split(Pattern.quote("E-"));
            parts[0] = QueryProcessor.trimEnd(parts[0], ".0");
            parts[1] = StringUtils.leftPad(parts[1], 2, '0');
            result = String.join("E-", parts);
        }

        if (result.contains("E+")) {
            String[] parts = result.split(Pattern.quote("E+"));
            parts[0] = QueryProcessor.trimEnd(parts[0], "0");
            result = String.join("E+", parts);
        }

        if (result.contains(".")) {
            result = QueryProcessor.trimEnd(result, "0");
            result = QueryProcessor.trimEnd(result, ".");
        }

        if (supportedCultures.containsKey(culture.cultureCode)) {
            LongFormatType longFormat = supportedCultures.get(culture.cultureCode);
            if (longFormat != null) {
                Character[] chars = result.chars().mapToObj(i -> (char)i)
                        .map(c -> changeMark(c, longFormat))
                        .toArray(Character[]::new);

                StringBuilder sb = new StringBuilder(chars.length);
                for (Character c : chars) {
                    sb.append(c.charValue());
                }

                result = sb.toString();
            }
        }

        return result;
    }

    private static Character changeMark(Character c, LongFormatType longFormat) {
        if (c == '.') {
            return longFormat.decimalsMark;
        } else if (c == ',') {
            return longFormat.thousandsMark;
        }

        return c;
    }
}
