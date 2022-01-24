package com.microsoft.recognizers.text.datetime.german.parsers;

import static java.lang.Integer.parseInt;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.time.YearMonth;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.function.IntFunction;
import java.util.regex.Pattern;
import java.util.stream.StreamSupport;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.IHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.HolidayFunctions;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.StringUtility;

public class HolidayParserGer extends GermanHolidayParserConfiguration implements IDateTimeParser {
    private final IHolidayParserConfiguration config;

    public HolidayParserGer(IHolidayParserConfiguration config) {
        this.config = config;
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
        return DateUtil.safeCreateFromMinValue(year, 11, getDayWithinRange(year, 11, 0
                , DayOfWeek.WEDNESDAY, 16, 22));
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
    public String getParserName() {
        return Constants.SYS_DATETIME_DATE;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        throw new UnsupportedOperationException();
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {

        LocalDateTime referenceDate = reference;
        Object value = null;

        if (er.getType().equals(getParserName())) {

            DateTimeResolutionResult innerResult = parseHolidayRegexMatch(er.getText(), referenceDate);

            if (innerResult.getSuccess()) {
                HashMap<String, String> futureResolution = new HashMap<>();
                futureResolution.put(TimeTypeConstants.DATE, DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getFutureValue()));
                innerResult.setFutureResolution(futureResolution);

                HashMap<String, String> pastResolution = new HashMap<>();
                pastResolution.put(TimeTypeConstants.DATE, DateTimeFormatUtil.formatDate((LocalDateTime)innerResult.getPastValue()));
                innerResult.setPastResolution(pastResolution);
                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(
                er.getStart(),
                er.getLength(),
                er.getText(),
                er.getType(),
                er.getData(),
                value,
                "",
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex()
        );

        return ret;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    private DateTimeResolutionResult parseHolidayRegexMatch(String text, LocalDateTime referenceDate) {

        for (Pattern pattern : this.config.getHolidayRegexList()) {
            ConditionalMatch match = RegexExtension.matchExact(pattern, text, true);
            if (match.getSuccess()) {
                // LUIS value string will be set in Match2Date method
                DateTimeResolutionResult ret = match2Date(match.getMatch().get(), referenceDate);

                return ret;
            }
        }

        return new DateTimeResolutionResult();
    }

    private DateTimeResolutionResult match2Date(Match match, LocalDateTime referenceDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        String holidayStr = this.config.sanitizeHolidayToken(match.getGroup("holiday").value.toLowerCase(Locale.ROOT));

        // get year (if exist)
        String yearStr = match.getGroup("year").value.toLowerCase();
        String orderStr = match.getGroup("order").value.toLowerCase();
        int year;
        boolean hasYear = false;

        if (!StringUtility.isNullOrEmpty(yearStr)) {
            year = parseInt(yearStr);
            hasYear = true;
        } else if (!StringUtility.isNullOrEmpty(orderStr)) {
            int swift = this.config.getSwiftYear((orderStr));
            if (swift < -1) {
                return  ret;
            }

            year = referenceDate.getYear() + swift;
            hasYear = true;
        } else {
            year = referenceDate.getYear();
        }

        String holidayKey = "";
        for (ImmutableMap.Entry<String, Iterable<String>> holidayPair : this.config.getHolidayNames().entrySet()) {
            if (StreamSupport.stream(holidayPair.getValue().spliterator(), false).anyMatch(name -> holidayStr.equals(name))) {
                holidayKey = holidayPair.getKey();
                break;
            }
        }

        String timexStr = "";
        if (!StringUtility.isNullOrEmpty(holidayKey)) {
            LocalDateTime value = referenceDate;
            IntFunction<LocalDateTime> fixedHolidaysFunction = FixedHolidays.get(holidayKey);
            IntFunction<LocalDateTime> variableHolidaysFunction = VariableHolidays.get(holidayKey);

            if (fixedHolidaysFunction != null) {
                value = fixedHolidaysFunction.apply(year);
                timexStr = String.format("-%02d-%02d", value.getMonthValue(), value.getDayOfMonth());
            }
            else {
                if (variableHolidaysFunction != null) {
                    value = variableHolidaysFunction.apply(year);
                    if (hasYear) {
                        timexStr = String.format("-%02d-%02d", value.getMonthValue(), value.getDayOfMonth());
                    }
                }
                else {
                    return ret;
                }
            }

            if (hasYear) {
                ret.setTimex(String.format("%04d", year) + timexStr);
                ret.setFutureValue(DateUtil.safeCreateFromMinValue(year, value.getMonthValue(), value.getDayOfMonth()));
                ret.setPastValue(DateUtil.safeCreateFromMinValue(year, value.getMonthValue(), value.getDayOfMonth()));
                ret.setSuccess(true);
                return ret;
            }

            ret.setTimex("XXXX" + timexStr);
            ret.setFutureValue(getFutureValue(value, referenceDate, holidayKey));
            ret.setPastValue(getPastValue(value, referenceDate, holidayKey));
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }

    private LocalDateTime getFutureValue(LocalDateTime value, LocalDateTime referenceDate, String holiday) {
        if (value.isBefore(referenceDate)) {
            IntFunction<LocalDateTime> function = AllHolidays.get(holiday);
            if (function != null) {
                return function.apply(value.getYear() + 1);
            }
        }

        return value;
    }

    private LocalDateTime getPastValue(LocalDateTime value, LocalDateTime referenceDate, String holiday) {

        if (value.isAfter(referenceDate) || value == referenceDate) {
            IntFunction<LocalDateTime> function = AllHolidays.get(holiday);
            if (function != null) {
                return function.apply(value.getYear() - 1);
            }
        }

        return value;
    }

    static final ImmutableMap<String, IntFunction<LocalDateTime>> FixedHolidays = ImmutableMap.<String, IntFunction<LocalDateTime>>builder()
            .put("assumptionofmary", HolidayParserGer::assumptionOfMary) // 15. August
            .put("germanunityday", HolidayParserGer::germanUnityDay) // 3. Oktober
            .put("reformationday", HolidayParserGer::reformationDay) // 31. Oktober
            .put("stmartinsday", HolidayParserGer::stMartinsDay) // 11. November
            .put("saintnicholasday", HolidayParserGer::saintNicholasDay) // 6. Dezember
            .put("biblicalmagiday", HolidayParserGer::biblicalMagiDay) // 6. Januar
            .put("walpurgisnight", HolidayParserGer::walpurgisNight) // 30. April
            .put("austriannationalday", HolidayParserGer::austrianNationalDay) // 26. Oktober
            .put("immaculateconception", HolidayParserGer::immaculateConception) // 8. Dezember
            .put("secondchristmasday", HolidayParserGer::secondChristmasDay) // 26. Dezember
            .put("berchtoldsday", HolidayParserGer::berchtoldDay) // 2. Januar
            .put("saintjosephsday", HolidayParserGer::saintJosephsDay) // 19. März
            .put("swissnationalday", HolidayParserGer::swissNationalDay) // 1. August
            .put("maosbirthday", HolidayParserGer::maoBirthday)
            .put("yuandan", HolidayParserGer::newYear)
            .put("teachersday", HolidayParserGer::teacherDay)
            .put("singleday", HolidayParserGer::singlesDay)
            .put("allsaintsday", HolidayParserGer::allHallowDay)
            .put("youthday", HolidayParserGer::youthDay)
            .put("childrenday", HolidayParserGer::childrenDay)
            .put("femaleday", HolidayParserGer::femaleDay)
            .put("treeplantingday", HolidayParserGer::treePlantDay)
            .put("arborday", HolidayParserGer::treePlantDay)
            .put("girlsday", HolidayParserGer::girlsDay)
            .put("whiteloverday", HolidayParserGer::whiteLoverDay)
            .put("loverday", HolidayParserGer::valentinesDay)
            .put("barbaratag", HolidayParserGer::barbaraTag)
            .put("augsburgerfriedensfest", HolidayParserGer::augsburgerFriedensFest)
            .put("johannistag", HolidayParserGer::johannisTag)
            .put("peterundpaul", HolidayParserGer::peterUndPaul)
            .put("firstchristmasday", HolidayParserGer::christmasDay)
            .put("xmas", HolidayParserGer::christmasDay)
            .put("newyear", HolidayParserGer::newYear)
            .put("newyearday", HolidayParserGer::newYear)
            .put("newyearsday", HolidayParserGer::newYear)
            .put("heiligedreikönige", HolidayParserGer::heiligeDreiKonige)
            .put("inaugurationday", HolidayParserGer::inaugurationDay)
            .put("groundhougday", HolidayParserGer::groundhogDay)
            .put("valentinesday", HolidayParserGer::valentinesDay)
            .put("stpatrickday", HolidayParserGer::stPatrickDay)
            .put("aprilfools", HolidayParserGer::foolDay)
            .put("stgeorgeday", HolidayParserGer::stGeorgeDay)
            .put("mayday", HolidayParserGer::mayday)
            .put("labour", HolidayParserGer::laborDay)
            .put("cincodemayoday", HolidayParserGer::cincoDeMayo)
            .put("baptisteday", HolidayParserGer::baptisteDay)
            .put("usindependenceday", HolidayParserGer::usaIndependenceDay)
            .put("independenceday", HolidayParserGer::usaIndependenceDay)
            .put("bastilleday", HolidayParserGer::bastilleDay)
            .put("halloweenday", HolidayParserGer::halloweenDay)
            .put("allhallowday", HolidayParserGer::allHallowDay)
            .put("allsoulsday", HolidayParserGer::allSoulsDay)
            .put("guyfawkesday", HolidayParserGer::guyFawkesDay)
            .put("veteransday", HolidayParserGer::veteransDay)
            .put("christmaseve", HolidayParserGer::christmasEve)
            .put("newyeareve", HolidayParserGer::newYearEve)
            .put("piday", HolidayParserGer::piDay)
            .put("beginningofsummer", HolidayParserGer::beginningOfSummer)
            .put("beginningofwinter", HolidayParserGer::beginningOfWinter)
            .put("beginningofspring", HolidayParserGer::beginningOfSpring)
            .put("beginningoffall", HolidayParserGer::beginningOfFall)
            .build();

    static final ImmutableMap<String, IntFunction<LocalDateTime>> VariableHolidays = ImmutableMap.<String, IntFunction<LocalDateTime>>builder()
            .put("fathers", HolidayParserGer::fathersDayOfYear)
            .put("easterday", HolidayParserGer::easterDay)
            .put("eastersunday", HolidayParserGer::easterDay)
            .put("eastermonday", HolidayParserGer::easterMondayOfYear)
            .put("eastersaturday", HolidayParserGer::easterSaturday)
            .put("weiberfastnacht", HolidayParserGer::weiberfastnacht)
            .put("carnival", HolidayParserGer::carnival)
            .put("ashwednesday", HolidayParserGer::ashWednesday)
            .put("palmsunday", HolidayParserGer::palmSunday)
            .put("goodfriday", HolidayParserGer::goodFriday)
            .put("ascensionofchrist", HolidayParserGer::ascensionOfChrist)
            .put("whitesunday", HolidayParserGer::whiteSunday)
            .put("whitemonday", HolidayParserGer::whiteMonday)
            .put("corpuschristi", HolidayParserGer::corpusChristi)
            .put("rosenmontag", HolidayParserGer::rosenmontag)
            .put("fastnacht", HolidayParserGer::fastnacht)
            .put("fastnachtssamstag", HolidayParserGer::fastnachtSaturday)
            .put("fastnachtssonntag", HolidayParserGer::fastnachtSunday)
            .put("holythursday", HolidayParserGer::holyThursday)
            .put("memorialdaygermany", HolidayParserGer::memorialDayGermany)
            .put("dayofrepentance", HolidayParserGer::dayOfRepentance)
            .put("totensonntag", HolidayParserGer::totenSonntag)
            .put("firstadvent", HolidayParserGer::firstAdvent)
            .put("secondadvent", HolidayParserGer::secondAdvent)
            .put("thirdadvent", HolidayParserGer::thirdAdvent)
            .put("fourthadvent", HolidayParserGer::fourthAdvent)
            .put("chedayofrepentance", HolidayParserGer::cheDayOfRepentance)
            .put("mothers", HolidayParserGer::mothersDay)
            .put("thanksgiving", HolidayParserGer::thanksgivingDay)
            .build();

    static final ImmutableMap<String, IntFunction<LocalDateTime>> AllHolidays = ImmutableMap
            .<String, IntFunction<LocalDateTime>>builder().putAll(FixedHolidays).putAll(VariableHolidays).build();
}
