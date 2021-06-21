using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class HolidayParserGer : IDateTimeParser
    {

        // Refactor this class to follow framework across languages

        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Dictionary<string, Func<int, DateObject>> FixedHolidaysDict = new Dictionary<string, Func<int, DateObject>>
        {
                { "assumptionofmary", AssumptionOfMary }, // 15. August
                { "germanunityday", GermanUnityDay }, // 3. Oktober
                { "reformationday", ReformationDay }, // 31. Oktober
                { "stmartinsday", StMartinsDay }, // 11. November
                { "saintnicholasday", SaintNicholasDay }, // 6. Dezember
                { "biblicalmagiday", BiblicalMagiDay }, // 6. Januar
                { "walpurgisnight", WalpurgisNight }, // 30. April
                { "austriannationalday", AustrianNationalDay }, // 26. Oktober
                { "immaculateconception", ImmaculateConception }, // 8. Dezember
                { "secondchristmasday", SecondChristmasDay }, // 26. Dezember
                { "berchtoldsday", BerchtoldsDay }, // 2. Januar
                { "saintjosephsday", SaintJosephsDay }, // 19. März
                { "swissnationalday", SwissNationalDay }, // 1. August
                { "maosbirthday", MaoBirthday },
                { "yuandan", NewYear },
                { "teachersday", TeacherDay },
                { "singleday", SinglesDay },
                { "allsaintsday", AllHallowDay },
                { "youthday", YouthDay },
                { "childrenday", ChildrenDay },
                { "femaleday", FemaleDay },
                { "treeplantingday", TreePlantDay },
                { "arborday", TreePlantDay },
                { "girlsday", GirlsDay },
                { "whiteloverday", WhiteLoverDay },
                { "loverday", ValentinesDay },
                { "barbaratag", BarbaraTag },
                { "augsburgerfriedensfest", AugsburgerFriedensFest },
                { "johannistag", JohannisTag },
                { "peterundpaul", PeterUndPaul },
                { "firstchristmasday", ChristmasDay },
                { "xmas", ChristmasDay },
                { "newyear", NewYear },
                { "newyearday", NewYear },
                { "newyearsday", NewYear },
                { "heiligedreikönige", HeiligeDreiKönige },
                { "inaugurationday", InaugurationDay },
                { "groundhougday", GroundhogDay },
                { "valentinesday", ValentinesDay },
                { "stpatrickday", StPatrickDay },
                { "aprilfools", FoolDay },
                { "stgeorgeday", StGeorgeDay },
                { "mayday", Mayday },
                { "labour", LaborDay },
                { "cincodemayoday", CincoDeMayoday },
                { "baptisteday", BaptisteDay },
                { "usindependenceday", UsaIndependenceDay },
                { "independenceday", UsaIndependenceDay },
                { "bastilleday", BastilleDay },
                { "halloweenday", HalloweenDay },
                { "allhallowday", AllHallowDay },
                { "allsoulsday", AllSoulsday },
                { "guyfawkesday", GuyFawkesDay },
                { "veteransday", Veteransday },
                { "christmaseve", ChristmasEve },
                { "newyeareve", NewYearEve },
                { "piday", PiDay },
                { "beginningofsummer", BeginningOfSummer },
                { "beginningofwinter", BeginningOfWinter },
                { "beginningofspring", BeginningOfSpring },
                { "beginningoffall", BeginningOfFall },
        };

        public static readonly Dictionary<string, Func<int, DateObject>> HolidayFuncDict = new Dictionary
            <string, Func<int, DateObject>>
        {
            { "fathers", GetFathersDayOfYear },
            { "easterday", GetEasterDay },
            { "eastersunday", GetEasterDay },
            { "eastermonday", GetEasterMondayOfYear },
            { "eastersaturday", GetEasterSaturday },
            { "weiberfastnacht", GetWeiberfastnacht },
            { "carnival", GetCarnival },
            { "ashwednesday", GetAshWednesday },
            { "palmsunday", GetPalmSunday },
            { "goodfriday", GetGoodFriday },
            { "ascensionofchrist", GetAscensionOfChrist },
            { "whitesunday", GetWhiteSunday },
            { "whitemonday", GetWhiteMonday },
            { "corpuschristi", GetCorpusChristi },
            { "rosenmontag", GetRosenmontag },
            { "fastnacht", GetFastnacht },
            { "fastnachtssamstag", GetFastnachtSaturday },
            { "fastnachtssonntag", GetFastnachtSunday },
            { "holythursday", GetHolyThursday },
            { "memorialdaygermany", GetMemorialDayGermany },
            { "dayofrepentance", GetDayOfRepentance },
            { "totensonntag", GetTotenSonntag },
            { "firstadvent", GetFirstAdvent },
            { "secondadvent", GetSecondAdvent },
            { "thirdadvent", GetThirdAdvent },
            { "fourthadvent", GetFourthAdvent },
            { "chedayofrepentance", GetCheDayOfRepentance },
            { "mothers", GetMothersDayOfYear },
            { "thanksgiving", GetThanksgivingDayOfYear },
        };

        private readonly IHolidayParserConfiguration config;

        public HolidayParserGer(IHolidayParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;
            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseHolidayRegexMatch(er.Text, referenceDate);

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue) },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue) },
                    };
                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private static DateObject GetFutureValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value < referenceDate)
            {
                if (FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(1);
                }

                if (HolidayFuncDict.ContainsKey(holiday))
                {
                    value = HolidayFuncDict[holiday](referenceDate.Year + 1);
                }
            }

            return value;
        }

        private static DateObject GetPastValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value >= referenceDate)
            {
                if (FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(-1);
                }

                if (HolidayFuncDict.ContainsKey(holiday))
                {
                    value = HolidayFuncDict[holiday](referenceDate.Year - 1);
                }
            }

            return value;
        }

        private static DateObject AssumptionOfMary(int year) => new DateObject(year, 8, 15);

        private static DateObject GermanUnityDay(int year) => new DateObject(year, 10, 3);

        private static DateObject ReformationDay(int year) => new DateObject(year, 10, 31);

        private static DateObject StMartinsDay(int year) => new DateObject(year, 11, 11);

        private static DateObject SaintNicholasDay(int year) => new DateObject(year, 12, 6);

        private static DateObject BiblicalMagiDay(int year) => new DateObject(year, 1, 6);

        private static DateObject WalpurgisNight(int year) => new DateObject(year, 4, 30);

        private static DateObject AustrianNationalDay(int year) => new DateObject(year, 10, 26);

        private static DateObject ImmaculateConception(int year) => new DateObject(year, 2, 14);

        private static DateObject SecondChristmasDay(int year) => new DateObject(year, 12, 26);

        private static DateObject BerchtoldsDay(int year) => new DateObject(year, 1, 2);

        private static DateObject SaintJosephsDay(int year) => new DateObject(year, 3, 19);

        private static DateObject SwissNationalDay(int year) => new DateObject(year, 8, 1);

        private static DateObject LoverDay(int year) => new DateObject(year, 2, 14);

        private static DateObject LaborDay(int year) => new DateObject(year, 5, 1);

        private static DateObject MidautumnDay(int year) => new DateObject(year, 8, 15);

        private static DateObject SpringDay(int year) => new DateObject(year, 1, 1);

        private static DateObject LanternDay(int year) => new DateObject(year, 1, 15);

        private static DateObject QingMingDay(int year) => new DateObject(year, 4, 4);

        private static DateObject DragonBoatDay(int year) => new DateObject(year, 5, 5);

        private static DateObject ChsNationalDay(int year) => new DateObject(year, 10, 1);

        private static DateObject ChsMilBuildDay(int year) => new DateObject(year, 8, 1);

        private static DateObject ChongYangDay(int year) => new DateObject(year, 9, 9);

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject NewYearEve(int year) => new DateObject(year, 12, 31);

        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);

        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);

        private static DateObject ValentinesDay(int year) => new DateObject(year, 2, 14);

        private static DateObject WhiteLoverDay(int year) => new DateObject(year, 3, 14);

        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);

        private static DateObject GirlsDay(int year) => new DateObject(year, 3, 7);

        private static DateObject TreePlantDay(int year) => new DateObject(year, 3, 12);

        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);

        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);

        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);

        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 10);

        private static DateObject SinglesDay(int year) => new DateObject(year, 11, 11);

        private static DateObject MaoBirthday(int year) => new DateObject(year, 12, 26);

        private static DateObject InaugurationDay(int year) => new DateObject(year, 1, 20);

        private static DateObject GroundhogDay(int year) => new DateObject(year, 2, 2);

        private static DateObject StPatrickDay(int year) => new DateObject(year, 3, 17);

        private static DateObject StGeorgeDay(int year) => new DateObject(year, 4, 23);

        private static DateObject Mayday(int year) => new DateObject(year, 5, 1);

        private static DateObject CincoDeMayoday(int year) => new DateObject(year, 5, 5);

        private static DateObject BaptisteDay(int year) => new DateObject(year, 6, 24);

        private static DateObject UsaIndependenceDay(int year) => new DateObject(year, 7, 4);

        private static DateObject BastilleDay(int year) => new DateObject(year, 7, 14);

        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);

        private static DateObject AllHallowDay(int year) => new DateObject(year, 11, 1);

        private static DateObject AllSoulsday(int year) => new DateObject(year, 11, 2);

        private static DateObject GuyFawkesDay(int year) => new DateObject(year, 11, 5);

        private static DateObject Veteransday(int year) => new DateObject(year, 11, 11);

        private static DateObject PiDay(int year) => new DateObject(year, 3, 14);

        private static DateObject BeginningOfWinter(int year) => new DateObject(year, 12, 21);

        private static DateObject BeginningOfSummer(int year) => new DateObject(year, 6, 21);

        private static DateObject BeginningOfSpring(int year) => new DateObject(year, 3, 20);

        private static DateObject BeginningOfFall(int year) => new DateObject(year, 9, 23);

        private static DateObject BarbaraTag(int year) => new DateObject(year, 12, 4);

        private static DateObject AugsburgerFriedensFest(int year) => new DateObject(year, 8, 8);

        private static DateObject PeterUndPaul(int year) => new DateObject(year, 6, 29);

        private static DateObject JohannisTag(int year) => new DateObject(year, 6, 24);

        private static DateObject HeiligeDreiKönige(int year) => new DateObject(year, 1, 6);

        private static DateObject GetMothersDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 5, (from day in Enumerable.Range(1, 31)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 5, day).DayOfWeek == DayOfWeek.Sunday
                                                                     select day).ElementAt(1));
        }

        private static DateObject GetFathersDayOfYear(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 39);
        }

        private static DateObject GetMartinLutherKingDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 1, (from day in Enumerable.Range(1, 31)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 1, day).DayOfWeek == DayOfWeek.Monday
                                                                     select day).ElementAt(2));
        }

        private static DateObject GetWashingtonsBirthdayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 2, (from day in Enumerable.Range(1, 29)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 2, day).DayOfWeek == DayOfWeek.Monday
                                                                     select day).ElementAt(2));
        }

        private static DateObject GetCanberraDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 3, (from day in Enumerable.Range(1, 31)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 3, day).DayOfWeek == DayOfWeek.Monday
                                                                     select day).ElementAt(0));
        }

        private static DateObject GetMemorialDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 5, (from day in Enumerable.Range(1, 31)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 5, day).DayOfWeek == DayOfWeek.Monday
                                                                     select day).Last());
        }

        private static DateObject GetLabourDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 9, (from day in Enumerable.Range(1, 30)
                                                                     where DateObject.MinValue.SafeCreateFromValue(year, 9, day).DayOfWeek == DayOfWeek.Monday
                                                                     select day).ElementAt(0));
        }

        private static DateObject GetColumbusDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 10, (from day in Enumerable.Range(1, 31)
                                                                      where DateObject.MinValue.SafeCreateFromValue(year, 10, day).DayOfWeek == DayOfWeek.Monday
                                                                      select day).ElementAt(1));
        }

        private static DateObject GetThanksgivingDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 10, (from day in Enumerable.Range(1, 31)
                                                                      where DateObject.MinValue.SafeCreateFromValue(year, 10, day).DayOfWeek == DayOfWeek.Sunday
                                                                      select day).ElementAt(0));
        }

        private static DateObject GetEasterDay(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year);
        }

        private static DateObject GetEasterMondayOfYear(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 1);
        }

        private static DateObject GetCheDayOfRepentance(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 09, (from day in Enumerable.Range(1, 30)
                                                                      where DateObject.MinValue.SafeCreateFromValue(year, 9, day).DayOfWeek == DayOfWeek.Sunday
                                                                      select day).ElementAt(2));
        }

        private static DateObject GetFourthAdvent(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year);
        }

        private static DateObject GetThirdAdvent(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year, 7);
        }

        private static DateObject GetSecondAdvent(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year, 14);
        }

        private static DateObject GetFirstAdvent(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year, 21);
        }

        private static DateObject GetTotenSonntag(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year, 28);
        }

        private static DateObject GetDayOfRepentance(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 11, (from day in Enumerable.Range(16, 22)
                                                                      where DateObject.MinValue.SafeCreateFromValue(year, 11, day).DayOfWeek == DayOfWeek.Wednesday
                                                                      select day).ElementAt(0));
        }

        private static DateObject GetMemorialDayGermany(int year)
        {
            return HolidayFunctions.CalculateAdventDate(year, 35);
        }

        private static DateObject GetHolyThursday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -3);
        }

        private static DateObject GetFastnacht(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -47);
        }

        private static DateObject GetRosenmontag(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -48);
        }

        private static DateObject GetCorpusChristi(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 60);
        }

        private static DateObject GetWhiteSunday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 49);
        }

        private static DateObject GetWhiteMonday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 50);
        }

        private static DateObject GetAscensionOfChrist(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, 39);
        }

        private static DateObject GetGoodFriday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -2);
        }

        private static DateObject GetPalmSunday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -7);
        }

        private static DateObject GetAshWednesday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -46);
        }

        private static DateObject GetCarnival(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -49);
        }

        private static DateObject GetWeiberfastnacht(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -52);
        }

        private static DateObject GetEasterSaturday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -1);
        }

        private static DateObject GetFastnachtSaturday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -50);
        }

        private static DateObject GetFastnachtSunday(int year)
        {
            return HolidayFunctions.CalculateHolidayByEaster(year, -49);
        }

        private DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            var trimmedText = text.Trim();
            foreach (var regex in this.config.HolidayRegexList)
            {
                var offset = 0;
                var match = regex.Match(trimmedText);

                if (match.Success && match.Index == offset && match.Length == trimmedText.Length)
                {
                    // Value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }

            return new DateTimeResolutionResult();
        }

        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = this.config.SanitizeHolidayToken(match.Groups["holiday"].Value);

            // get year (if exist)
            var yearStr = match.Groups["year"].Value;
            var orderStr = match.Groups["order"].Value;
            int year;
            var hasYear = false;

            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr, CultureInfo.InvariantCulture);
                hasYear = true;
            }
            else if (!string.IsNullOrEmpty(orderStr))
            {
                var swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }

                year = referenceDate.Year + swift;
                hasYear = true;
            }
            else
            {
                year = referenceDate.Year;
            }

            string holidayKey = string.Empty;
            foreach (var holidayPair in this.config.HolidayNames)
            {
                if (holidayPair.Value.Contains(holidayStr))
                {
                    holidayKey = holidayPair.Key;
                    break;
                }
            }

            var timexStr = string.Empty;
            if (!string.IsNullOrEmpty(holidayKey))
            {
                DateObject value;
                if (FixedHolidaysDict.ContainsKey(holidayKey))
                {
                    value = FixedHolidaysDict[holidayKey](year);
                    timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                }
                else
                {
                    if (HolidayFuncDict.ContainsKey(holidayKey))
                    {
                        value = HolidayFuncDict[holidayKey](year);
                        if (hasYear)
                        {
                            timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                        }
                    }
                    else
                    {
                        return ret;
                    }
                }

                if (hasYear)
                {
                    ret.Timex = year.ToString("D4", CultureInfo.InvariantCulture) + timexStr;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, value.Month, value.Day);
                    ret.Success = true;
                    return ret;
                }

                ret.Timex = "XXXX" + timexStr;
                ret.FutureValue = GetFutureValue(value, referenceDate, holidayKey);
                ret.PastValue = GetPastValue(value, referenceDate, holidayKey);
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}