package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.StringUtility;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishHolidayExtractorConfiguration;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.function.IntFunction;
import java.util.stream.Collectors;

public class EnglishHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public EnglishHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(EnglishHolidayExtractorConfiguration.HolidayRegexList);

        HashMap<String, Iterable<String>> newMap =new HashMap<>();
        for (Map.Entry<String, String[]> entry : EnglishDateTime.HolidayNames.entrySet()) {
            if(entry.getValue() instanceof String[]){
                newMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        this.setHolidayNames(ImmutableMap.copyOf(newMap));
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> InitHolidayFuncs() {

        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.InitHolidayFuncs());
        holidays.put("mayday", EnglishHolidayParserConfiguration::Mayday);
        holidays.put("yuandan", EnglishHolidayParserConfiguration::NewYear);
        holidays.put("newyear", EnglishHolidayParserConfiguration::NewYear);
        holidays.put("youthday", EnglishHolidayParserConfiguration::YouthDay);
        holidays.put("girlsday", EnglishHolidayParserConfiguration::GirlsDay);
        holidays.put("xmas", EnglishHolidayParserConfiguration::ChristmasDay);
        holidays.put("newyearday", EnglishHolidayParserConfiguration::NewYear);
        holidays.put("aprilfools", EnglishHolidayParserConfiguration::FoolDay);
        holidays.put("easterday", EnglishHolidayParserConfiguration::EasterDay);
        holidays.put("newyearsday", EnglishHolidayParserConfiguration::NewYear);
        holidays.put("femaleday", EnglishHolidayParserConfiguration::FemaleDay);
        holidays.put("singleday", EnglishHolidayParserConfiguration::SinglesDay);
        holidays.put("newyeareve", EnglishHolidayParserConfiguration::NewYearEve);
        holidays.put("arborday", EnglishHolidayParserConfiguration::TreePlantDay);
        holidays.put("loverday", EnglishHolidayParserConfiguration::ValentinesDay);
        holidays.put("christmas", EnglishHolidayParserConfiguration::ChristmasDay);
        holidays.put("teachersday", EnglishHolidayParserConfiguration::TeacherDay);
        holidays.put("stgeorgeday", EnglishHolidayParserConfiguration::StGeorgeDay);
        holidays.put("baptisteday", EnglishHolidayParserConfiguration::BaptisteDay);
        holidays.put("bastilleday", EnglishHolidayParserConfiguration::BastilleDay);
        holidays.put("allsoulsday", EnglishHolidayParserConfiguration::AllSoulsday);
        holidays.put("veteransday", EnglishHolidayParserConfiguration::Veteransday);
        holidays.put("childrenday", EnglishHolidayParserConfiguration::ChildrenDay);
        holidays.put("maosbirthday", EnglishHolidayParserConfiguration::MaoBirthday);
        holidays.put("allsaintsday", EnglishHolidayParserConfiguration::HalloweenDay);
        holidays.put("stpatrickday", EnglishHolidayParserConfiguration::StPatrickDay);
        holidays.put("halloweenday", EnglishHolidayParserConfiguration::HalloweenDay);
        holidays.put("allhallowday", EnglishHolidayParserConfiguration::AllHallowDay);
        holidays.put("guyfawkesday", EnglishHolidayParserConfiguration::GuyFawkesDay);
        holidays.put("christmaseve", EnglishHolidayParserConfiguration::ChristmasEve);
        holidays.put("groundhougday", EnglishHolidayParserConfiguration::GroundhogDay);
        holidays.put("whiteloverday", EnglishHolidayParserConfiguration::WhiteLoverDay);
        holidays.put("valentinesday", EnglishHolidayParserConfiguration::ValentinesDay);
        holidays.put("treeplantingday", EnglishHolidayParserConfiguration::TreePlantDay);
        holidays.put("cincodemayoday", EnglishHolidayParserConfiguration::CincoDeMayoday);
        holidays.put("inaugurationday", EnglishHolidayParserConfiguration::InaugurationDay);
        holidays.put("independenceday", EnglishHolidayParserConfiguration::UsaIndependenceDay);
        holidays.put("usindependenceday", EnglishHolidayParserConfiguration::UsaIndependenceDay);

        return holidays;
    }

    private static LocalDateTime EasterDay(int year) { return DateUtil.minValue(); }
    private static LocalDateTime Mayday(int year) { return DateUtil.safeCreateFromMinValue(year, 5, 1); }
    private static LocalDateTime NewYear(int year) { return DateUtil.safeCreateFromMinValue(year, 1, 1); }
    private static LocalDateTime FoolDay(int year) { return DateUtil.safeCreateFromMinValue(year, 4, 1); }
    private static LocalDateTime GirlsDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, 7); }
    private static LocalDateTime YouthDay(int year) { return DateUtil.safeCreateFromMinValue(year, 5, 4); }
    private static LocalDateTime FemaleDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, 8); }
    private static LocalDateTime ChildrenDay(int year) { return DateUtil.safeCreateFromMinValue(year, 6, 1); }
    private static LocalDateTime TeacherDay(int year) { return DateUtil.safeCreateFromMinValue(year, 9, 10); }
    private static LocalDateTime GroundhogDay(int year) { return DateUtil.safeCreateFromMinValue(year, 2, 2); }
    private static LocalDateTime StGeorgeDay(int year) { return DateUtil.safeCreateFromMinValue(year, 4, 23); }
    private static LocalDateTime BaptisteDay(int year) { return DateUtil.safeCreateFromMinValue(year, 6, 24); }
    private static LocalDateTime BastilleDay(int year) { return DateUtil.safeCreateFromMinValue(year, 7, 14); }
    private static LocalDateTime AllSoulsday(int year) { return DateUtil.safeCreateFromMinValue(year, 11, 2); }
    private static LocalDateTime SinglesDay(int year) { return DateUtil.safeCreateFromMinValue(year, 11, 11); }
    private static LocalDateTime NewYearEve(int year) { return DateUtil.safeCreateFromMinValue(year, 12, 31); }
    private static LocalDateTime TreePlantDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, 12); }
    private static LocalDateTime StPatrickDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, 17); }
    private static LocalDateTime AllHallowDay(int year) { return DateUtil.safeCreateFromMinValue(year, 11, 1); }
    private static LocalDateTime GuyFawkesDay(int year) { return DateUtil.safeCreateFromMinValue(year, 11, 5); }
    private static LocalDateTime Veteransday(int year) { return DateUtil.safeCreateFromMinValue(year, 11, 11); }
    private static LocalDateTime MaoBirthday(int year) { return DateUtil.safeCreateFromMinValue(year, 12, 26); }
    private static LocalDateTime ValentinesDay(int year) { return DateUtil.safeCreateFromMinValue(year, 2, 14); }
    private static LocalDateTime WhiteLoverDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, 14); }
    private static LocalDateTime CincoDeMayoday(int year) { return DateUtil.safeCreateFromMinValue(year, 5, 5); }
    private static LocalDateTime HalloweenDay(int year) { return DateUtil.safeCreateFromMinValue(year, 10, 31); }
    private static LocalDateTime ChristmasEve(int year) { return DateUtil.safeCreateFromMinValue(year, 12, 24); }
    private static LocalDateTime ChristmasDay(int year) { return DateUtil.safeCreateFromMinValue(year, 12, 25); }
    private static LocalDateTime InaugurationDay(int year) { return DateUtil.safeCreateFromMinValue(year, 1, 20); }
    private static LocalDateTime UsaIndependenceDay(int year) { return DateUtil.safeCreateFromMinValue(year, 7, 4); }

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
