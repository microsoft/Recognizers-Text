package com.microsoft.recognizers.text.number.chinese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseCJKNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.List;
import java.util.regex.Pattern;

public class ChineseNumberParserConfiguration extends BaseCJKNumberParserConfiguration {

    public ChineseNumberParserConfiguration() {
        super(
                ChineseNumeric.LangMarker,
                new CultureInfo(Culture.Chinese),
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
                ChineseNumeric.UnitMap,
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
                Pattern.compile(ChineseNumeric.RoundNumberIntegerRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS)
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
