package com.microsoft.recognizers.text.number.chinese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.chinese.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.parsers.BaseCJKNumberParser;
import com.microsoft.recognizers.text.number.parsers.INumberRangeParserConfiguration;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;

import java.util.regex.Pattern;

public class ChineseNumberRangeParserConfiguration implements INumberRangeParserConfiguration {

    public final CultureInfo cultureInfo;
    public final IExtractor numberExtractor;
    public final IExtractor ordinalExtractor;
    public final IParser numberParser;
    public final Pattern moreOrEqual;
    public final Pattern lessOrEqual;
    public final Pattern moreOrEqualSuffix;
    public final Pattern lessOrEqualSuffix;
    public final Pattern moreOrEqualSeparate;
    public final Pattern lessOrEqualSeparate;

    public ChineseNumberRangeParserConfiguration() {
        this(new CultureInfo(Culture.Chinese));
    }

    public ChineseNumberRangeParserConfiguration(CultureInfo ci) {
        this.cultureInfo = ci
        ;
        this.numberExtractor = new NumberExtractor();
        this.ordinalExtractor = new OrdinalExtractor();
        this.numberParser = new BaseCJKNumberParser(new ChineseNumberParserConfiguration());

        this.moreOrEqual = Pattern.compile(ChineseNumeric.MoreOrEqual, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqual = Pattern.compile(ChineseNumeric.LessOrEqual, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.moreOrEqualSuffix = Pattern.compile(ChineseNumeric.MoreOrEqualSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqualSuffix = Pattern.compile(ChineseNumeric.LessOrEqualSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.moreOrEqualSeparate = Pattern.compile(ChineseNumeric.OneNumberRangeMoreSeparateRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqualSeparate = Pattern.compile(ChineseNumeric.OneNumberRangeLessSeparateRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    @Override
    public CultureInfo getCultureInfo() {
        return this.cultureInfo;
    }

    @Override
    public IExtractor getNumberExtractor() {
        return this.numberExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return this.ordinalExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return this.numberParser;
    }

    @Override
    public Pattern getMoreOrEqual() {
        return this.moreOrEqual;
    }

    @Override
    public Pattern getLessOrEqual() {
        return this.lessOrEqual;
    }

    @Override
    public Pattern getMoreOrEqualSuffix() {
        return this.moreOrEqualSuffix;
    }

    @Override
    public Pattern getLessOrEqualSuffix() {
        return this.lessOrEqualSuffix;
    }

    @Override
    public Pattern getMoreOrEqualSeparate() {
        return this.moreOrEqualSeparate;
    }

    @Override
    public Pattern getLessOrEqualSeparate() {
        return this.lessOrEqualSeparate;
    }
}
