package com.microsoft.recognizers.text.number.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.english.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.parsers.INumberRangeParserConfiguration;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishNumberRangeParserConfiguration implements INumberRangeParserConfiguration {

    @Override
    public CultureInfo getCultureInfo() { return this.cultureInfo; }
    @Override
    public IExtractor getNumberExtractor() { return this.numberExtractor; }
    @Override
    public IExtractor getOrdinalExtractor() { return this.ordinalExtractor; }
    @Override
    public IParser getNumberParser() { return this.numberParser; }
    @Override
    public Pattern getMoreOrEqual() { return this.moreOrEqual; }
    @Override
    public Pattern getLessOrEqual() { return this.lessOrEqual; }
    @Override
    public Pattern getMoreOrEqualSuffix() { return this.moreOrEqualSuffix; }
    @Override
    public Pattern getLessOrEqualSuffix() { return this.lessOrEqualSuffix; }
    @Override
    public Pattern getMoreOrEqualSeparate() { return this.moreOrEqualSeparate; }
    @Override
    public Pattern getLessOrEqualSeparate() { return this.lessOrEqualSeparate; }

    private final CultureInfo cultureInfo;
    private final IExtractor numberExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final Pattern moreOrEqual;
    private final Pattern lessOrEqual;
    private final Pattern moreOrEqualSuffix;
    private final Pattern lessOrEqualSuffix;
    private final Pattern moreOrEqualSeparate;
    private final Pattern lessOrEqualSeparate;

    public EnglishNumberRangeParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public EnglishNumberRangeParserConfiguration(CultureInfo cultureInfo) {
        this.cultureInfo = cultureInfo;

        this.numberExtractor = NumberExtractor.getInstance();
        this.ordinalExtractor = OrdinalExtractor.getInstance();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());

        this.moreOrEqual = Pattern.compile(EnglishNumeric.MoreOrEqual, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqual = Pattern.compile(EnglishNumeric.LessOrEqual, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.moreOrEqualSuffix = Pattern.compile(EnglishNumeric.MoreOrEqualSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqualSuffix = Pattern.compile(EnglishNumeric.LessOrEqualSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.moreOrEqualSeparate = RegExpUtility.getSafeRegExp(EnglishNumeric.OneNumberRangeMoreSeparateRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        this.lessOrEqualSeparate = RegExpUtility.getSafeRegExp(EnglishNumeric.OneNumberRangeLessSeparateRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }
}
