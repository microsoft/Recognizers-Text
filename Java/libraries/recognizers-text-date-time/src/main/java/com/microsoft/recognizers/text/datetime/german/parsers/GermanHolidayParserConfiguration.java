// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.HolidayFunctions;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Optional;
import java.util.function.IntFunction;
import java.util.regex.Pattern;

public class GermanHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    private final Pattern thisPrefixRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern previousPrefixRegex;

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

        thisPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ThisPrefixRegex);
        nextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PreviousPrefixRegex);
    }

    private static LocalDateTime assumptionOfMary(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 15);
    }

    private static LocalDateTime germanUnityDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 3);
    }

    private static LocalDateTime reformationDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 31);
    }

    private static LocalDateTime stMartinsDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime saintNicholasDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 6);
    }

    private static LocalDateTime biblicalMagiDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 6);
    }

    private static LocalDateTime walpurgisNight(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 30);
    }

    private static LocalDateTime austrianNationalDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 26);
    }

    private static LocalDateTime immaculateConception(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 14);
    }

    private static LocalDateTime secondChristmasDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 26);
    }

    private static LocalDateTime berchtoldDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 2);
    }

    private static LocalDateTime saintJosephsDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 19);
    }

    private static LocalDateTime swissNationalDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 1);
    }

    private static LocalDateTime loverDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, 14);
    }

    private static LocalDateTime laborDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    private static LocalDateTime midAutumnDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 15);
    }

    private static LocalDateTime springDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 1);
    }

    private static LocalDateTime lanternDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 15);
    }

    private static LocalDateTime qingMingDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 4, 4);
    }

    private static LocalDateTime dragonBoatDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 5);
    }

    private static LocalDateTime chsNationalDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, 1);
    }

    private static LocalDateTime chsMilBuildDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 1);
    }

    private static LocalDateTime chongYangDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 9);
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

    private static LocalDateTime cincoDeMayo(final int year) {
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

    private static LocalDateTime allSoulsDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 2);
    }

    private static LocalDateTime guyFawkesDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 5);
    }

    private static LocalDateTime veteransDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, 11);
    }

    private static LocalDateTime piDay(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 14);
    }

    private static LocalDateTime beginningOfWinter(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 21);
    }

    private static LocalDateTime beginningOfSummer(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 21);
    }

    private static LocalDateTime beginningOfSpring(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, 20);
    }

    private static LocalDateTime beginningOfFall(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, 22);
    }

    private static LocalDateTime barbaraTag(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 12, 4);
    }

    private static LocalDateTime augsburgerFriedensFest(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 8, 8);
    }

    private static LocalDateTime peterUndPaul(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 29);
    }

    private static LocalDateTime johannisTag(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, 24);
    }

    private static LocalDateTime heiligeDreiKonige(final int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, 6);
    }

    private static LocalDateTime fathersDayOfYear(final int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 39);
    }

    private static LocalDateTime easterDay(final int year) {
        return HolidayFunctions.calculateHolidayByEaster(year);
    }

    private static LocalDateTime easterMondayOfYear(final int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 1);
    }

    private static LocalDateTime cheDayOfRepentance(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, getDay(year, 9, 2, DayOfWeek.SUNDAY));
    }

    private static LocalDateTime fourthAdvent(int year) {
        return HolidayFunctions.calculateAdventDate(year);
    }

    private static LocalDateTime thirdAdvent(int year) {
        return HolidayFunctions.calculateAdventDate(year, 7);
    }

    private static LocalDateTime secondAdvent(int year) {
        return HolidayFunctions.calculateAdventDate(year, 14);
    }

    private static LocalDateTime firstAdvent(int year) {
        return HolidayFunctions.calculateAdventDate(year, 21);
    }

    private static LocalDateTime totenSonntag(int year) {
        return HolidayFunctions.calculateAdventDate(year, 28);
    }

    private static LocalDateTime dayOfRepentance(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, getDay(year, 9, 0, DayOfWeek.WEDNESDAY));
    }

    private static LocalDateTime memorialDayGermany(int year) {
        return HolidayFunctions.calculateAdventDate(year, 35);
    }

    private static LocalDateTime holyThursday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -3);
    }

    private static LocalDateTime fastnacht(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -47);
    }

    private static LocalDateTime rosenmontag(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -48);
    }

    private static LocalDateTime corpusChristi(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 60);
    }

    private static LocalDateTime whiteSunday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 49);
    }

    private static LocalDateTime whiteMonday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 50);
    }

    private static LocalDateTime ascensionOfChrist(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, 39);
    }

    private static LocalDateTime goodFriday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -2);
    }

    private static LocalDateTime palmSunday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -7);
    }

    private static LocalDateTime ashWednesday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -46);
    }

    private static LocalDateTime carnival(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -49);
    }

    private static LocalDateTime weiberfastnacht(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -52);
    }

    private static LocalDateTime easterSaturday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -1);
    }

    private static LocalDateTime fastnachtSaturday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -50);
    }

    private static LocalDateTime fastnachtSunday(int year) {
        return HolidayFunctions.calculateHolidayByEaster(year, -49);
    }

    @Override
    public int getSwiftYear(String text) {

        String trimmedText = StringUtility.trimStart(StringUtility.trimEnd(text)).toLowerCase(Locale.ROOT);
        int swift = -10;
        Optional<Match> matchNextPrefixRegex = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchPreviousPrefixRegex = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchThisPrefixRegex = Arrays.stream(RegExpUtility.getMatches(thisPrefixRegex, trimmedText)).findFirst();

        if (matchNextPrefixRegex.isPresent()) {
            swift = 1;
        } else if (matchPreviousPrefixRegex.isPresent()) {
            swift = -1;
        } else if (matchThisPrefixRegex.isPresent()) {
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(String holiday) {
        return holiday.replace(" ", "")
                .replace("-", "")
                .replace("'", "");
    }

    @Override
    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {
        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>(super.initHolidayFuncs());
        holidays.put("assumptionofmary", GermanHolidayParserConfiguration::assumptionOfMary); // 15. August
        holidays.put("germanunityday", GermanHolidayParserConfiguration::germanUnityDay); // 3. Oktober
        holidays.put("reformationday", GermanHolidayParserConfiguration::reformationDay); // 31. Oktober
        holidays.put("stmartinsday", GermanHolidayParserConfiguration::stMartinsDay); // 11. November
        holidays.put("saintnicholasday", GermanHolidayParserConfiguration::saintNicholasDay); // 6. Dezember
        holidays.put("biblicalmagiday", GermanHolidayParserConfiguration::biblicalMagiDay); // 6. Januar
        holidays.put("walpurgisnight", GermanHolidayParserConfiguration::walpurgisNight); // 30. April
        holidays.put("austriannationalday", GermanHolidayParserConfiguration::austrianNationalDay); // 26. Oktober
        holidays.put("immaculateconception", GermanHolidayParserConfiguration::immaculateConception); // 8. Dezember
        holidays.put("secondchristmasday", GermanHolidayParserConfiguration::secondChristmasDay); // 26. Dezember
        holidays.put("berchtoldsday", GermanHolidayParserConfiguration::berchtoldDay); // 2. Januar
        holidays.put("saintjosephsday", GermanHolidayParserConfiguration::saintJosephsDay); // 19. März
        holidays.put("swissnationalday", GermanHolidayParserConfiguration::swissNationalDay); // 1. August
        holidays.put("maosbirthday", GermanHolidayParserConfiguration::maoBirthday);
        holidays.put("yuandan", GermanHolidayParserConfiguration::newYear);
        holidays.put("teachersday", GermanHolidayParserConfiguration::teacherDay);
        holidays.put("singleday", GermanHolidayParserConfiguration::singlesDay);
        holidays.put("allsaintsday", GermanHolidayParserConfiguration::allHallowDay);
        holidays.put("youthday", GermanHolidayParserConfiguration::youthDay);
        holidays.put("childrenday", GermanHolidayParserConfiguration::childrenDay);
        holidays.put("femaleday", GermanHolidayParserConfiguration::femaleDay);
        holidays.put("treeplantingday", GermanHolidayParserConfiguration::treePlantDay);
        holidays.put("arborday", GermanHolidayParserConfiguration::treePlantDay);
        holidays.put("girlsday", GermanHolidayParserConfiguration::girlsDay);
        holidays.put("whiteloverday", GermanHolidayParserConfiguration::whiteLoverDay);
        holidays.put("loverday", GermanHolidayParserConfiguration::valentinesDay);
        holidays.put("barbaratag", GermanHolidayParserConfiguration::barbaraTag);
        holidays.put("augsburgerfriedensfest", GermanHolidayParserConfiguration::augsburgerFriedensFest);
        holidays.put("johannistag", GermanHolidayParserConfiguration::johannisTag);
        holidays.put("peterundpaul", GermanHolidayParserConfiguration::peterUndPaul);
        holidays.put("firstchristmasday", GermanHolidayParserConfiguration::christmasDay);
        holidays.put("xmas", GermanHolidayParserConfiguration::christmasDay);
        holidays.put("newyear", GermanHolidayParserConfiguration::newYear);
        holidays.put("newyearday", GermanHolidayParserConfiguration::newYear);
        holidays.put("newyearsday", GermanHolidayParserConfiguration::newYear);
        holidays.put("heiligedreikönige", GermanHolidayParserConfiguration::heiligeDreiKonige);
        holidays.put("inaugurationday", GermanHolidayParserConfiguration::inaugurationDay);
        holidays.put("groundhougday", GermanHolidayParserConfiguration::groundhogDay);
        holidays.put("valentinesday", GermanHolidayParserConfiguration::valentinesDay);
        holidays.put("stpatrickday", GermanHolidayParserConfiguration::stPatrickDay);
        holidays.put("aprilfools", GermanHolidayParserConfiguration::foolDay);
        holidays.put("stgeorgeday", GermanHolidayParserConfiguration::stGeorgeDay);
        holidays.put("mayday", GermanHolidayParserConfiguration::mayday);
        holidays.put("labour", GermanHolidayParserConfiguration::laborDay);
        holidays.put("cincodemayoday", GermanHolidayParserConfiguration::cincoDeMayo);
        holidays.put("baptisteday", GermanHolidayParserConfiguration::baptisteDay);
        holidays.put("usindependenceday", GermanHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("independenceday", GermanHolidayParserConfiguration::usaIndependenceDay);
        holidays.put("bastilleday", GermanHolidayParserConfiguration::bastilleDay);
        holidays.put("halloweenday", GermanHolidayParserConfiguration::halloweenDay);
        holidays.put("allhallowday", GermanHolidayParserConfiguration::allHallowDay);
        holidays.put("allsoulsday", GermanHolidayParserConfiguration::allSoulsDay);
        holidays.put("guyfawkesday", GermanHolidayParserConfiguration::guyFawkesDay);
        holidays.put("veteransday", GermanHolidayParserConfiguration::veteransDay);
        holidays.put("christmaseve", GermanHolidayParserConfiguration::christmasEve);
        holidays.put("newyeareve", GermanHolidayParserConfiguration::newYearEve);
        holidays.put("piday", GermanHolidayParserConfiguration::piDay);
        holidays.put("beginningofsummer", GermanHolidayParserConfiguration::beginningOfSummer);
        holidays.put("beginningofwinter", GermanHolidayParserConfiguration::beginningOfWinter);
        holidays.put("beginningofspring", GermanHolidayParserConfiguration::beginningOfSpring);
        holidays.put("beginningoffall", GermanHolidayParserConfiguration::beginningOfFall);

        holidays.put("fathers", GermanHolidayParserConfiguration::fathersDayOfYear);
        holidays.put("easterday", GermanHolidayParserConfiguration::easterDay);
        holidays.put("eastersunday", GermanHolidayParserConfiguration::easterDay);
        holidays.put("eastermonday", GermanHolidayParserConfiguration::easterMondayOfYear);
        holidays.put("eastersaturday", GermanHolidayParserConfiguration::easterSaturday);
        holidays.put("weiberfastnacht", GermanHolidayParserConfiguration::weiberfastnacht);
        holidays.put("carnival", GermanHolidayParserConfiguration::carnival);
        holidays.put("ashwednesday", GermanHolidayParserConfiguration::ashWednesday);
        holidays.put("palmsunday", GermanHolidayParserConfiguration::palmSunday);
        holidays.put("goodfriday", GermanHolidayParserConfiguration::goodFriday);
        holidays.put("ascensionofchrist", GermanHolidayParserConfiguration::ascensionOfChrist);
        holidays.put("whitesunday", GermanHolidayParserConfiguration::whiteSunday);
        holidays.put("whitemonday", GermanHolidayParserConfiguration::whiteMonday);
        holidays.put("corpuschristi", GermanHolidayParserConfiguration::corpusChristi);
        holidays.put("rosenmontag", GermanHolidayParserConfiguration::rosenmontag);
        holidays.put("fastnacht", GermanHolidayParserConfiguration::fastnacht);
        holidays.put("fastnachtssamstag", GermanHolidayParserConfiguration::fastnachtSaturday);
        holidays.put("fastnachtssonntag", GermanHolidayParserConfiguration::fastnachtSunday);
        holidays.put("holythursday", GermanHolidayParserConfiguration::holyThursday);
        holidays.put("memorialdaygermany", GermanHolidayParserConfiguration::memorialDayGermany);
        holidays.put("dayofrepentance", GermanHolidayParserConfiguration::dayOfRepentance);
        holidays.put("totensonntag", GermanHolidayParserConfiguration::totenSonntag);
        holidays.put("firstadvent", GermanHolidayParserConfiguration::firstAdvent);
        holidays.put("secondadvent", GermanHolidayParserConfiguration::secondAdvent);
        holidays.put("thirdadvent", GermanHolidayParserConfiguration::thirdAdvent);
        holidays.put("fourthadvent", GermanHolidayParserConfiguration::fourthAdvent);
        holidays.put("chedayofrepentance", GermanHolidayParserConfiguration::cheDayOfRepentance);
        holidays.put("mothers", GermanHolidayParserConfiguration::mothersDay);
        holidays.put("thanksgiving", GermanHolidayParserConfiguration::thanksgivingDay);

        return holidays;
    }

}
