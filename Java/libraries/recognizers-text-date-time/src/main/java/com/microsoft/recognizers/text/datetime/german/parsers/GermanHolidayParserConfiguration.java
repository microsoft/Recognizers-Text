// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.function.IntFunction;

public class GermanHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public GermanHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(GermanHolidayExtractorConfiguration.HolidayRegexList);

        HashMap<String, Iterable<String>> newMap = new HashMap<>();
        for (Map.Entry<String, String[]> entry : GermanDateTime.HolidayNames.entrySet()) {
            if (entry.getValue() instanceof String[]) {
                newMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        this.setHolidayNames(ImmutableMap.copyOf(newMap));
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {

        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.initHolidayFuncs());
        holidays.put("mayday", GermanHolidayParserConfiguration::mayday);
        holidays.put("yuandan", GermanHolidayParserConfiguration::newYear);
        holidays.put("newyear", GermanHolidayParserConfiguration::newYear);
        holidays.put("youthday", GermanHolidayParserConfiguration::youthDay);
        holidays.put("girlsday", GermanHolidayParserConfiguration::girlsDay);
        holidays.put("xmas", GermanHolidayParserConfiguration::christmasDay);
        holidays.put("newyearday", GermanHolidayParserConfiguration::newYear);
        holidays.put("aprilfools", GermanHolidayParserConfiguration::foolDay);
        holidays.put("easterday", GermanHolidayParserConfiguration::easterDay);
        holidays.put("newyearsday", GermanHolidayParserConfiguration::newYear);
        holidays.put("femaleday", GermanHolidayParserConfiguration::femaleDay);
        holidays.put("singleday", GermanHolidayParserConfiguration::singlesDay);
        holidays.put("newyeareve", GermanHolidayParserConfiguration::newYearEve);
        holidays.put("arborday", GermanHolidayParserConfiguration::treePlantDay);
        holidays.put("loverday", GermanHolidayParserConfiguration::valentinesDay);
        holidays.put("christmas", GermanHolidayParserConfiguration::christmasDay);
        holidays.put("teachersday", GermanHolidayParserConfiguration::teacherDay);
        holidays.put("stgeorgeday", GermanHolidayParserConfiguration::stGeorgeDay);
        holidays.put("baptisteday", GermanHolidayParserConfiguration::baptisteDay);
        holidays.put("bastilleday", GermanHolidayParserConfiguration::bastilleDay);
        holidays.put("allsoulsday", GermanHolidayParserConfiguration::allSoulsDay);
        holidays.put("veteransday", GermanHolidayParserConfiguration::veteransDay);
        holidays.put("childrenday", GermanHolidayParserConfiguration::childrenDay);
        holidays.put("maosbirthday", GermanHolidayParserConfiguration::maoBirthday);
        holidays.put("allsaintsday", GermanHolidayParserConfiguration::halloweenDay);
        holidays.put("stpatrickday", GermanHolidayParserConfiguration::stPatrickDay);
        holidays.put("halloweenday", GermanHolidayParserConfiguration::halloweenDay);
        holidays.put("allhallowday", GermanHolidayParserConfiguration::allHallowDay);
        holidays.put("guyfawkesday", GermanHolidayParserConfiguration::guyFawkesDay);
        holidays.put("christmaseve", GermanHolidayParserConfiguration::christmasEve);
        holidays.put("groundhougday", GermanHolidayParserConfiguration::groundhogDay);
        holidays.put("whiteloverday", GermanHolidayParserConfiguration::whiteLoverDay);
        holidays.put("valentinesday", GermanHolidayParserConfiguration::valentinesDay);
        holidays.put("treeplantingday", GermanHolidayParserConfiguration::treePlantDay);
        holidays.put("cincodemayoday", GermanHolidayParserConfiguration::cincoDeMayoDay);
        holidays.put("inaugurationday", GermanHolidayParserConfiguration::inaugurationDay);
        holidays.put("independenceday", GermanHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("usindependenceday", GermanHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("juneteenth", GermanHolidayParserConfiguration::juneteenth);

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
