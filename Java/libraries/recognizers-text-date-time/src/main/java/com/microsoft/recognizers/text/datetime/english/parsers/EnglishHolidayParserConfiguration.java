package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.function.IntFunction;

public class EnglishHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public EnglishHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(EnglishHolidayExtractorConfiguration.HolidayRegexList);

        HashMap<String, Iterable<String>> newMap = new HashMap<>();
        for (Map.Entry<String, String[]> entry : EnglishDateTime.HolidayNames.entrySet()) {
            if (entry.getValue() instanceof String[]) {
                newMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        this.setHolidayNames(ImmutableMap.copyOf(newMap));
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {

        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.initHolidayFuncs());
        holidays.put("mayday", EnglishHolidayParserConfiguration::mayday);
        holidays.put("yuandan", EnglishHolidayParserConfiguration::newYear);
        holidays.put("newyear", EnglishHolidayParserConfiguration::newYear);
        holidays.put("youthday", EnglishHolidayParserConfiguration::youthDay);
        holidays.put("girlsday", EnglishHolidayParserConfiguration::girlsDay);
        holidays.put("xmas", EnglishHolidayParserConfiguration::christmasDay);
        holidays.put("newyearday", EnglishHolidayParserConfiguration::newYear);
        holidays.put("aprilfools", EnglishHolidayParserConfiguration::foolDay);
        holidays.put("easterday", EnglishHolidayParserConfiguration::easterDay);
        holidays.put("newyearsday", EnglishHolidayParserConfiguration::newYear);
        holidays.put("femaleday", EnglishHolidayParserConfiguration::femaleDay);
        holidays.put("singleday", EnglishHolidayParserConfiguration::singlesDay);
        holidays.put("newyeareve", EnglishHolidayParserConfiguration::newYearEve);
        holidays.put("arborday", EnglishHolidayParserConfiguration::treePlantDay);
        holidays.put("loverday", EnglishHolidayParserConfiguration::valentinesDay);
        holidays.put("christmas", EnglishHolidayParserConfiguration::christmasDay);
        holidays.put("teachersday", EnglishHolidayParserConfiguration::teacherDay);
        holidays.put("stgeorgeday", EnglishHolidayParserConfiguration::stGeorgeDay);
        holidays.put("baptisteday", EnglishHolidayParserConfiguration::baptisteDay);
        holidays.put("bastilleday", EnglishHolidayParserConfiguration::bastilleDay);
        holidays.put("allsoulsday", EnglishHolidayParserConfiguration::allSoulsDay);
        holidays.put("veteransday", EnglishHolidayParserConfiguration::veteransDay);
        holidays.put("childrenday", EnglishHolidayParserConfiguration::childrenDay);
        holidays.put("maosbirthday", EnglishHolidayParserConfiguration::maoBirthday);
        holidays.put("allsaintsday", EnglishHolidayParserConfiguration::halloweenDay);
        holidays.put("stpatrickday", EnglishHolidayParserConfiguration::stPatrickDay);
        holidays.put("halloweenday", EnglishHolidayParserConfiguration::halloweenDay);
        holidays.put("allhallowday", EnglishHolidayParserConfiguration::allHallowDay);
        holidays.put("guyfawkesday", EnglishHolidayParserConfiguration::guyFawkesDay);
        holidays.put("christmaseve", EnglishHolidayParserConfiguration::christmasEve);
        holidays.put("groundhougday", EnglishHolidayParserConfiguration::groundhogDay);
        holidays.put("whiteloverday", EnglishHolidayParserConfiguration::whiteLoverDay);
        holidays.put("valentinesday", EnglishHolidayParserConfiguration::valentinesDay);
        holidays.put("treeplantingday", EnglishHolidayParserConfiguration::treePlantDay);
        holidays.put("cincodemayoday", EnglishHolidayParserConfiguration::cincoDeMayoDay);
        holidays.put("inaugurationday", EnglishHolidayParserConfiguration::inaugurationDay);
        holidays.put("independenceday", EnglishHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("usindependenceday", EnglishHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("juneteenth", EnglishHolidayParserConfiguration::juneteenth);

        return holidays;
    }

    private static LocalDateTime easterDay(int year) {
        return DateUtil.minValue();
    }

    private static LocalDateTime mayday(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    private static LocalDateTime newYear(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime foolDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 1);
    }

    private static LocalDateTime girlsDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 7);
    }

    private static LocalDateTime youthDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 4);
    }

    private static LocalDateTime femaleDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 8);
    }

    private static LocalDateTime childrenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 1);
    }

    private static LocalDateTime teacherDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 10);
    }

    private static LocalDateTime groundhogDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 2);
    }

    private static LocalDateTime stGeorgeDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 23);
    }

    private static LocalDateTime baptisteDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 24);
    }

    private static LocalDateTime bastilleDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 7, 14);
    }

    private static LocalDateTime allSoulsDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 2);
    }

    private static LocalDateTime singlesDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime newYearEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 31);
    }

    private static LocalDateTime treePlantDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 12);
    }

    private static LocalDateTime stPatrickDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 17);
    }

    private static LocalDateTime allHallowDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 1);
    }

    private static LocalDateTime guyFawkesDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 5);
    }

    private static LocalDateTime veteransDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime maoBirthday(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 26);
    }

    private static LocalDateTime valentinesDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 14);
    }

    private static LocalDateTime whiteLoverDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 14);
    }

    private static LocalDateTime cincoDeMayoDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 5);
    }

    private static LocalDateTime halloweenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 31);
    }

    private static LocalDateTime christmasEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 24);
    }

    private static LocalDateTime christmasDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 25);
    }

    private static LocalDateTime inaugurationDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 20);
    }

    private static LocalDateTime usaIndependenceDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 7, 4);
    }

    private static LocalDateTime juneteenth(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 19);
    }

    @Override
    public int getSwiftYear(String text) {

        String trimmedText = StringUtility.trimStart(StringUtility.trimEnd(text)).toLowerCase(Locale.ROOT);
        int swift = -10;

        if (trimmedText.startsWith("next")) {
            swift = 1;
        } else if (trimmedText.startsWith("last")) {
            swift = -1;
        } else if (trimmedText.startsWith("this")) {
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(String holiday) {
        return holiday.replace(" ", "").replace("'", "");
    }
}
