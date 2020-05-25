package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.HolidayFunctions;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Optional;
import java.util.function.IntFunction;

public class SpanishHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    public SpanishHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(SpanishHolidayExtractorConfiguration.HolidayRegexList);

        HashMap<String, Iterable<String>> holidayNamesMap = new HashMap<>();
        for (Map.Entry<String, String[]> entry : SpanishDateTime.HolidayNames.entrySet()) {
            if (entry.getValue() instanceof String[]) {
                holidayNamesMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        this.setHolidayNames(ImmutableMap.copyOf(holidayNamesMap));

        HashMap<String, String> variableHolidaysTimexMap = new HashMap<>();
        for (Map.Entry<String, String> entry : SpanishDateTime.VariableHolidaysTimexDictionary.entrySet()) {
            if (entry.getValue() instanceof String) {
                variableHolidaysTimexMap.put(entry.getKey(), entry.getValue());
            }
        }
        this.setVariableHolidaysTimexDictionary(ImmutableMap.copyOf(variableHolidaysTimexMap));
    }

    @Override
    public int getSwiftYear(String text) {

        String trimmedText = StringUtility
            .trimStart(StringUtility.trimEnd(text)).toLowerCase(Locale.ROOT);
        int swift = -10;
        Optional<Match> matchNextPrefixRegex = Arrays.stream(RegExpUtility.getMatches(
            SpanishDatePeriodParserConfiguration.nextPrefixRegex, text)).findFirst();
        Optional<Match> matchPastPrefixRegex = Arrays.stream(RegExpUtility.getMatches(
            SpanishDatePeriodParserConfiguration.previousPrefixRegex, text)).findFirst();
        Optional<Match> matchThisPrefixRegex = Arrays.stream(RegExpUtility.getMatches(
            SpanishDatePeriodParserConfiguration.thisPrefixRegex, text)).findFirst();
        if (matchNextPrefixRegex.isPresent() && matchNextPrefixRegex.get().length == text.trim().length()) {
            swift = 1;
        } else if (matchPastPrefixRegex.isPresent() && matchPastPrefixRegex.get().length == text.trim().length()) {
            swift = -1;
        } else if (matchThisPrefixRegex.isPresent() && matchThisPrefixRegex.get().length == text.trim().length()) {
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(String holiday) {
        return holiday.replace(" ", "")
            .replace("á", "a")
            .replace("é", "e")
            .replace("í", "i")
            .replace("ó", "o")
            .replace("ú", "u");
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {

        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.initHolidayFuncs());
        holidays.put("padres", SpanishHolidayParserConfiguration::fathersDay);
        holidays.put("madres", SpanishHolidayParserConfiguration::mothersDay);
        holidays.put("acciondegracias", SpanishHolidayParserConfiguration::thanksgivingDay);
        holidays.put("trabajador", SpanishHolidayParserConfiguration::internationalWorkersDay);
        holidays.put("delaraza", SpanishHolidayParserConfiguration::columbusDay);
        holidays.put("memoria", SpanishHolidayParserConfiguration::memorialDay);
        holidays.put("pascuas", SpanishHolidayParserConfiguration::pascuas);
        holidays.put("navidad", SpanishHolidayParserConfiguration::christmasDay);
        holidays.put("nochebuena", SpanishHolidayParserConfiguration::christmasEve);
        holidays.put("añonuevo", SpanishHolidayParserConfiguration::newYear);
        holidays.put("nochevieja", SpanishHolidayParserConfiguration::newYearEve);
        holidays.put("yuandan", SpanishHolidayParserConfiguration::newYear);
        holidays.put("maestro", SpanishHolidayParserConfiguration::teacherDay);
        holidays.put("todoslossantos", SpanishHolidayParserConfiguration::halloweenDay);
        holidays.put("niño", SpanishHolidayParserConfiguration::childrenDay);
        holidays.put("mujer", SpanishHolidayParserConfiguration::femaleDay);

        return holidays;
    }

    private static LocalDateTime newYear(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime newYearEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 31);
    }

    private static LocalDateTime christmasDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 25);
    }

    private static LocalDateTime christmasEve(int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 24);
    }

    private static LocalDateTime femaleDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 8);
    }

    private static LocalDateTime childrenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 1);
    }

    private static LocalDateTime halloweenDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 31);
    }

    private static LocalDateTime teacherDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 10);
    }

    private static LocalDateTime pascuas(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year);
    }
}
