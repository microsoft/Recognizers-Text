package com.microsoft.recognizers.text.datetime.french.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.regex.Pattern;

public class FrenchHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static final Pattern H1 = RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex1);

    public static final Pattern H2 = RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex2);

    public static final Pattern H3 = RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex3);

    public static final Pattern H4 = RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex4);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(H1);
            add(H2);
            add(H3);
            add(H4);
        }
    };

    public FrenchHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    @Override
    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
