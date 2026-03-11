package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;

import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.function.IntFunction;

public class ChineseHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public ChineseHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(ChineseHolidayExtractorConfiguration.HolidayRegexList);

        this.setVariableHolidaysTimexDictionary(ChineseDateTime.HolidayNoFixedTimex);

        this.setHolidayNames(ImmutableMap.of());
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {

        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.initHolidayFuncs());

        holidays.put("新年", ChineseHolidayParserConfiguration::newYear);
        holidays.put("元旦", ChineseHolidayParserConfiguration::newYear);
        holidays.put("元旦节", ChineseHolidayParserConfiguration::newYear);
        holidays.put("五一", ChineseHolidayParserConfiguration::laborDay);
        holidays.put("劳动节", ChineseHolidayParserConfiguration::laborDay);
        holidays.put("愚人节", ChineseHolidayParserConfiguration::foolDay);
        holidays.put("平安夜", ChineseHolidayParserConfiguration::christmasEve);
        holidays.put("圣诞节", ChineseHolidayParserConfiguration::christmasDay);
        holidays.put("植树节", ChineseHolidayParserConfiguration::treePlantDay);
        holidays.put("国庆节", ChineseHolidayParserConfiguration::nationalDay);
        holidays.put("情人节", ChineseHolidayParserConfiguration::valentinesDay);
        holidays.put("教师节", ChineseHolidayParserConfiguration::teacherDay);
        holidays.put("儿童节", ChineseHolidayParserConfiguration::childrenDay);
        holidays.put("妇女节", ChineseHolidayParserConfiguration::femaleDay);
        holidays.put("青年节", ChineseHolidayParserConfiguration::youthDay);
        holidays.put("建军节", ChineseHolidayParserConfiguration::armyDay);
        holidays.put("女生节", ChineseHolidayParserConfiguration::girlsDay);
        holidays.put("光棍节", ChineseHolidayParserConfiguration::singlesDay);
        holidays.put("双十一", ChineseHolidayParserConfiguration::singlesDay);
        holidays.put("清明节", ChineseHolidayParserConfiguration::qingmingDay);
        holidays.put("清明", ChineseHolidayParserConfiguration::qingmingDay);
        holidays.put("万圣节", ChineseHolidayParserConfiguration::halloweenDay);

        holidays.put("春节", ChineseHolidayParserConfiguration::springFestival);
        holidays.put("除夕", ChineseHolidayParserConfiguration::chineseNewYearEve);
        holidays.put("元宵节", ChineseHolidayParserConfiguration::lanternFestival);
        holidays.put("端午节", ChineseHolidayParserConfiguration::dragonBoatFestival);
        holidays.put("端午", ChineseHolidayParserConfiguration::dragonBoatFestival);
        holidays.put("中秋节", ChineseHolidayParserConfiguration::midAutumnFestival);
        holidays.put("中秋", ChineseHolidayParserConfiguration::midAutumnFestival);
        holidays.put("重阳节", ChineseHolidayParserConfiguration::doubleNinthFestival);

        holidays.put("母亲节", ChineseHolidayParserConfiguration::mothersDay);
        holidays.put("父亲节", ChineseHolidayParserConfiguration::fathersDay);
        holidays.put("感恩节", ChineseHolidayParserConfiguration::thanksgivingDay);

        return holidays;
    }

    private static LocalDateTime newYear(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime laborDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    private static LocalDateTime foolDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 1);
    }

    private static LocalDateTime christmasEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 24);
    }

    private static LocalDateTime christmasDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 25);
    }

    private static LocalDateTime treePlantDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 12);
    }

    private static LocalDateTime nationalDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 1);
    }

    private static LocalDateTime valentinesDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 14);
    }

    private static LocalDateTime teacherDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 10);
    }

    private static LocalDateTime childrenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 1);
    }

    private static LocalDateTime femaleDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 8);
    }

    private static LocalDateTime youthDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 4);
    }

    private static LocalDateTime armyDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 1);
    }

    private static LocalDateTime girlsDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 7);
    }

    private static LocalDateTime singlesDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime qingmingDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 5);
    }

    private static LocalDateTime halloweenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 31);
    }

    private static LocalDateTime springFestival(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime chineseNewYearEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime lanternFestival(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 15);
    }

    private static LocalDateTime dragonBoatFestival(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 5);
    }

    private static LocalDateTime midAutumnFestival(int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 15);
    }

    private static LocalDateTime doubleNinthFestival(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 9);
    }

    @Override
    public int getSwiftYear(String text) {

        String trimmedText = text.trim();
        int swift = -10;

        if (trimmedText.startsWith("明年")) {
            swift = 1;
        } else if (trimmedText.startsWith("去年")) {
            swift = -1;
        } else if (trimmedText.startsWith("今年")) {
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(String holiday) {
        return holiday.replace(" ", "");
    }
}
