package com.microsoft.recognizers.text.number.chinese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseCJKNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.TreeMap;
import java.util.regex.Pattern;

public class ChineseNumberParserConfiguration extends BaseCJKNumberParserConfiguration {

    public ChineseNumberParserConfiguration() {
        super(
                ChineseNumeric.LangMarker,
                new CultureInfo(Culture.Chinese),
                ChineseNumeric.CompoundNumberLanguage,
                ChineseNumeric.MultiDecimalSeparatorCulture,
                NumberOptions.None,
                ChineseNumeric.NonDecimalSeparatorChar,
                ChineseNumeric.DecimalSeparatorChar,
                ChineseNumeric.FractionMarkerToken,
                ChineseNumeric.HalfADozenText,
                ChineseNumeric.WordSeparatorToken,
                Collections.<String>emptyList(),
                Collections.emptyList(),
                Collections.emptyList(),
                Collections.emptyList(),
                Collections.emptyMap(),
                Collections.emptyMap(),
                ChineseNumeric.RoundNumberMap,
                null,
                Pattern.compile(ChineseNumeric.DigitalNumberRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.NegativeNumberTermsRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                null,
                ChineseNumeric.ZeroToNineMap,
                ChineseNumeric.RoundNumberMapChar,
                ChineseNumeric.FullToHalfMap,
                new TreeMap<String, String>(new Comparator<String>() {
                    @Override
                    public int compare(String a, String b) {
                        return a.length() > b.length() ? 1 : -1;
                    }
                }) {
                {
                    putAll(ChineseNumeric.UnitMap);
                }
            },
                ChineseNumeric.TratoSimMap,
                ChineseNumeric.RoundDirectList,
                Pattern.compile(ChineseNumeric.FracSplitRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.DigitNumRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.SpeGetNumberRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                RegExpUtility.getSafeRegExp(ChineseNumeric.PercentageRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.PointRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.DoubleAndRoundRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.PairRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.DozenRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(ChineseNumeric.RoundNumberIntegerRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                ChineseNumeric.ZeroChar,
                ChineseNumeric.TenChars,
                ChineseNumeric.PairChar,
                RegExpUtility.getSafeRegExp(ChineseNumeric.PercentageNumRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS)
        );
    }

    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        return tokens;
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {
        return 0;
    }
}
