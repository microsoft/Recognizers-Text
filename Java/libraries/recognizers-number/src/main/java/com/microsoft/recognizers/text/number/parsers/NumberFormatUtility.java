package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.utilities.FormatUtility;
import org.apache.commons.lang3.StringUtils;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

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
    }

    public static String format(Object value, CultureInfo culture) {

        BigDecimal bc = new BigDecimal((Double)value, new MathContext(15, RoundingMode.HALF_EVEN));
        String result = bc.toString();

        result = result.replace('e', 'E');

        if(result.contains("E-")) {
            String[] parts = result.split(Pattern.quote("E-"));
            parts[0] = FormatUtility.trimEnd(parts[0], ".0");
            parts[1] = StringUtils.leftPad(parts[1], 2, '0');
            result = String.join("E-", parts);
        }

        if(result.contains("E+")) {
            String[] parts = result.split(Pattern.quote("E+"));
            parts[0] = FormatUtility.trimEnd(parts[0], "0");
            result = String.join("E-", parts);
        }

        if (result.contains(".")) {
            result = FormatUtility.trimEnd(result, "0");
            result = FormatUtility.trimEnd(result, ".");
        }

        if (supportedCultures.containsKey(culture.cultureCode)) {
            LongFormatType longFormat = supportedCultures.get(culture.cultureCode);
            Character[] chars = result.chars().mapToObj(i -> (char) i)
                    .map(c -> changeMark(c, longFormat))
                    .toArray(Character[]::new);

            StringBuilder sb = new StringBuilder(chars.length);
            for (Character c : chars) {
                sb.append(c.charValue());
            }

            result = sb.toString();
        }
        return result;


        /*
        result = str(value)
        result = result.replace('e', 'E')
        if '.' in result:
        result = result.rstrip('0').rstrip('.')
        if 'E-' in result:
        parts = result.split('E-')
        parts[1] = parts[1].rjust(2, '0')
        result = 'E-'.join(parts)
        if 'E+' in result:
        parts = result.split('E+')
        parts[0] = parts[0].rstrip('0')
        result = 'E+'.join(parts)
        long_format = SUPPORTED_CULTURES.get(self.code)
        if long_format:
        result = ''.join(map(lambda x: self.change_mark(x, long_format), result))
        return result
        */
    }

    private static Character changeMark(Character c, LongFormatType longFormat) {
        if (c == '.') {
            return longFormat.decimalsMark;
        } else if (c == ',') {
            return longFormat.thousandsMark;
        }

        return c;
    }

    private static int getNumberOfDecimalPlaces(BigDecimal bigDecimal) {
        String string = bigDecimal.stripTrailingZeros().toPlainString();
        int index = string.indexOf(".");
        return index < 0 ? 0 : string.length() - index - 1;
    }
}
