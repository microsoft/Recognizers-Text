package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.function.IntFunction;

public class FrenchHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public FrenchHolidayParserConfiguration() {

        setHolidayRegexList(FrenchHolidayExtractorConfiguration.HolidayRegexList);

        final HashMap<String, Iterable<String>> holidayNamesMap = new HashMap<>();
        for (final Map.Entry<String, String[]> entry : FrenchDateTime.HolidayNames.entrySet()) {
            if (entry.getValue() instanceof String[]) {
                holidayNamesMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        setHolidayNames(ImmutableMap.copyOf(holidayNamesMap));
    }

    private static LocalDateTime newYear(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime newYearEve(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 31);
    }

    private static LocalDateTime christmasDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 25);
    }

    private static LocalDateTime christmasEve(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 24);
    }

    private static LocalDateTime valentinesDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 14);
    }

    private static LocalDateTime whiteLoverDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 14);
    }

    private static LocalDateTime foolDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 1);
    }

    private static LocalDateTime girlsDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 7);
    }

    private static LocalDateTime treePlantDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 12);
    }

    private static LocalDateTime femaleDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 8);
    }

    private static LocalDateTime childrenDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 1);
    }

    private static LocalDateTime youthDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 4);
    }

    private static LocalDateTime teacherDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 10);
    }

    private static LocalDateTime singlesDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime maoBirthday(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 26);
    }

    private static LocalDateTime inaugurationDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 20);
    }

    private static LocalDateTime groundhogDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 2);
    }

    private static LocalDateTime stPatrickDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 17);
    }

    private static LocalDateTime stGeorgeDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 23);
    }

    private static LocalDateTime mayday(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    private static LocalDateTime cincoDeMayoday(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 5);
    }

    private static LocalDateTime baptisteDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 24);
    }

    private static LocalDateTime usaIndependenceDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 7, 4);
    }

    private static LocalDateTime bastilleDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 7, 14);
    }

    private static LocalDateTime halloweenDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 31);
    }

    private static LocalDateTime allHallowDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 1);
    }

    private static LocalDateTime allSoulsday(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 2);
    }

    private static LocalDateTime guyFawkesDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 5);
    }

    private static LocalDateTime veteransday(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    protected static LocalDateTime fathersDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 17);
    }

    protected static LocalDateTime mothersDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 27);
    }

    protected static LocalDateTime labourDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    @Override
    public int getSwiftYear(final String text) {
        final String trimmedText = text.trim();
        int swift = -10;
        if (trimmedText.endsWith("prochain")) {
            // next - 'l'annee prochain'
            swift = 1;
        } else if (trimmedText.endsWith("dernier")) {
            // last - 'l'annee dernier'
            swift = -1;
        } else if (trimmedText.endsWith("cette")) {
            // this - 'cette annees'
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(final String holiday) {
        return holiday
            .replaceAll(" ", "")
            .replaceAll("'", "");
    }

    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {
        return new HashMap<String, IntFunction<LocalDateTime>>(super.initHolidayFuncs()) {{
                put("maosbirthday", FrenchHolidayParserConfiguration::maoBirthday);
                put("yuandan", FrenchHolidayParserConfiguration::newYear);
                put("teachersday", FrenchHolidayParserConfiguration::teacherDay);
                put("singleday", FrenchHolidayParserConfiguration::singlesDay);
                put("allsaintsday", FrenchHolidayParserConfiguration::halloweenDay);
                put("youthday", FrenchHolidayParserConfiguration::youthDay);
                put("childrenday", FrenchHolidayParserConfiguration::childrenDay);
                put("femaleday", FrenchHolidayParserConfiguration::femaleDay);
                put("treeplantingday", FrenchHolidayParserConfiguration::treePlantDay);
                put("arborday", FrenchHolidayParserConfiguration::treePlantDay);
                put("girlsday", FrenchHolidayParserConfiguration::girlsDay);
                put("whiteloverday", FrenchHolidayParserConfiguration::whiteLoverDay);
                put("loverday", FrenchHolidayParserConfiguration::valentinesDay);
                put("christmas", FrenchHolidayParserConfiguration::christmasDay);
                put("xmas", FrenchHolidayParserConfiguration::christmasDay);
                put("newyear", FrenchHolidayParserConfiguration::newYear);
                put("newyearday", FrenchHolidayParserConfiguration::newYear);
                put("newyearsday", FrenchHolidayParserConfiguration::newYear);
                put("inaugurationday", FrenchHolidayParserConfiguration::inaugurationDay);
                put("groundhougday", FrenchHolidayParserConfiguration::groundhogDay);
                put("valentinesday", FrenchHolidayParserConfiguration::valentinesDay);
                put("stpatrickday", FrenchHolidayParserConfiguration::stPatrickDay);
                put("aprilfools", FrenchHolidayParserConfiguration::foolDay);
                put("stgeorgeday", FrenchHolidayParserConfiguration::stGeorgeDay);
                put("mayday", FrenchHolidayParserConfiguration::mayday);
                put("cincodemayoday", FrenchHolidayParserConfiguration::cincoDeMayoday);
                put("baptisteday", FrenchHolidayParserConfiguration::baptisteDay);
                put("usindependenceday", FrenchHolidayParserConfiguration::usaIndependenceDay);
                put("independenceday", FrenchHolidayParserConfiguration::usaIndependenceDay);
                put("bastilleday", FrenchHolidayParserConfiguration::bastilleDay);
                put("halloweenday", FrenchHolidayParserConfiguration::halloweenDay);
                put("allhallowday", FrenchHolidayParserConfiguration::allHallowDay);
                put("allsoulsday", FrenchHolidayParserConfiguration::allSoulsday);
                put("guyfawkesday", FrenchHolidayParserConfiguration::guyFawkesDay);
                put("veteransday", FrenchHolidayParserConfiguration::veteransday);
                put("christmaseve", FrenchHolidayParserConfiguration::christmasEve);
                put("newyeareve", FrenchHolidayParserConfiguration::newYearEve);
                put("fathersday", FrenchHolidayParserConfiguration::fathersDay);
                put("mothersday", FrenchHolidayParserConfiguration::mothersDay);
                put("labourday", FrenchHolidayParserConfiguration::labourDay);
            }
        };
    }
}
