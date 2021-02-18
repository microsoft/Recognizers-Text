using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.DateTime.Hindi.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Hindi;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
   public class HindiDateExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            RegexCache.Get(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex MonthNumRegex =
            RegexCache.Get(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex YearRegex =
            RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex WeekDayRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex SingleWeekDayRegex =
            RegexCache.Get(DateTimeDefinitions.SingleWeekDayRegex, RegexFlags);

        public static readonly Regex OnRegex =
            RegexCache.Get(DateTimeDefinitions.OnRegex, RegexFlags);

        public static readonly Regex RelaxedOnRegex =
            RegexCache.Get(DateTimeDefinitions.RelaxedOnRegex, RegexFlags);

        public static readonly Regex ThisRegex =
            RegexCache.Get(DateTimeDefinitions.ThisRegex, RegexFlags);

        public static readonly Regex LastDateRegex =
            RegexCache.Get(DateTimeDefinitions.LastDateRegex, RegexFlags);

        public static readonly Regex NextDateRegex =
            RegexCache.Get(DateTimeDefinitions.NextDateRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex =
            RegexCache.Get(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex RelativeWeekDayRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeWeekDayRegex, RegexFlags);

        public static readonly Regex SpecialDate =
            RegexCache.Get(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex SpecialDayWithNumRegex =
            RegexCache.Get(DateTimeDefinitions.SpecialDayWithNumRegex, RegexFlags);

        public static readonly Regex ForTheRegex =
            RegexCache.Get(DateTimeDefinitions.ForTheRegex, RegexFlags);

        public static readonly Regex WeekDayAndDayOfMothRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayAndDayOfMonthRegex, RegexFlags);

        public static readonly Regex WeekDayAndDayRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayAndDayRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex StrictRelativeRegex =
            RegexCache.Get(DateTimeDefinitions.StrictRelativeRegex, RegexFlags);

        public static readonly Regex PrefixArticleRegex =
            RegexCache.Get(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        public static readonly Regex OfMonth =
            RegexCache.Get(DateTimeDefinitions.OfMonth, RegexFlags);

        public static readonly Regex MonthEnd =
            RegexCache.Get(DateTimeDefinitions.MonthEnd, RegexFlags);

        public static readonly Regex WeekDayEnd =
            RegexCache.Get(DateTimeDefinitions.WeekDayEnd, RegexFlags);

        public static readonly Regex WeekDayStart =
            RegexCache.Get(DateTimeDefinitions.WeekDayStart, RegexFlags);

        public static readonly Regex YearSuffix =
            RegexCache.Get(DateTimeDefinitions.YearSuffix, RegexFlags);

        public static readonly Regex LessThanRegex =
            RegexCache.Get(DateTimeDefinitions.LessThanRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            RegexCache.Get(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex SinceYearSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.SinceYearSuffixRegex, RegexFlags);

        public static readonly Regex RangeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex RangeConnectorSymbolRegex =
            RegexCache.Get(Definitions.BaseDateTime.RangeConnectorSymbolRegex, RegexFlags);

        public static readonly Regex BeforeAfterRegex =
            RegexCache.Get(DateTimeDefinitions.BeforeAfterRegex, RegexFlags);

        public static readonly ImmutableDictionary<string, int> DayOfWeek =
            DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, int> MonthOfYear =
            DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DayRegex =
            RegexCache.Get(DateTimeDefinitions.ImplicitDayRegex, RegexFlags);

        public HindiDateExtractorConfiguration(IDateTimeOptionsConfiguration config)
           : base(config)
        {

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.Hindi.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Hindi.OrdinalExtractor.GetInstance();

            NumberParser = new BaseIndianNumberParser(new HindiNumberParserConfiguration(numConfig));

            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new HindiHolidayExtractorConfiguration(this));
            UtilityConfiguration = new HindiDatetimeUtilityConfiguration();

            ImplicitDateList = new List<Regex>
            {
                // extract "12" from "on 12"
                OnRegex,

                // extract "12th" from "on/at/in 12th"
                RelaxedOnRegex,

                // "the day before yesterday", "previous day", "today", "yesterday", "tomorrow"
                SpecialDayRegex,

                // "this Monday", "Tuesday of this week"
                ThisRegex,

                // "last/previous Monday", "Monday of last week"
                LastDateRegex,

                // "next/following Monday", "Monday of next week"
                NextDateRegex,

                // "Sunday", "Weds"
                SingleWeekDayRegex,

                // "2nd Monday of April"
                WeekDayOfMonthRegex,

                // "on the 12th"
                SpecialDate,

                // "two days from today", "five days from tomorrow"
                SpecialDayWithNumRegex,

                // "three Monday from now"
                RelativeWeekDayRegex,
            };

            if ((Options & DateTimeOptions.CalendarMode) != 0)
            {
                ImplicitDateList = ImplicitDateList.Concat(new[] { DayRegex });
            }

            // 3-23-2017
            var dateRegex4 = RegexCache.Get(DateTimeDefinitions.DateExtractor4, RegexFlags);

            // 23-3-2015
            var dateRegex5 = RegexCache.Get(DateTimeDefinitions.DateExtractor5, RegexFlags);

            // on (Sunday,)? 1.3
            var dateRegex6 = RegexCache.Get(DateTimeDefinitions.DateExtractor6, RegexFlags);

            // on (Sunday,)? 24-12
            var dateRegex8 = RegexCache.Get(DateTimeDefinitions.DateExtractor8, RegexFlags);

            // "(Sunday,)? 7/23, 2018", year part is required
            var dateRegex7L = RegexCache.Get(DateTimeDefinitions.DateExtractor7L, RegexFlags);

            // "(Sunday,)? 7/23", year part is not required
            var dateRegex7S = RegexCache.Get(DateTimeDefinitions.DateExtractor7S, RegexFlags);

            // "(Sunday,)? 23/7, 2018", year part is required
            var dateRegex9L = RegexCache.Get(DateTimeDefinitions.DateExtractor9L, RegexFlags);

            // "(Sunday,)? 23/7", year part is not required
            var dateRegex9S = RegexCache.Get(DateTimeDefinitions.DateExtractor9S, RegexFlags);

            // (Sunday,)? 2015-12-23
            var dateRegexA = RegexCache.Get(DateTimeDefinitions.DateExtractorA, RegexFlags);

            DateRegexList = new List<Regex>
            {
                // (Sunday,)? April 5 or (Sunday,)? April 5, 2016
                RegexCache.Get(DateTimeDefinitions.DateExtractor1, RegexFlags),

                // (Sunday,)? 6th of April
                RegexCache.Get(DateTimeDefinitions.DateExtractor3, RegexFlags),
            };

            var enableDmy = DmyDateFormat ||
                            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;

            DateRegexList = DateRegexList.Concat(enableDmy ?
                new[] { dateRegex5, dateRegex8, dateRegex9L, dateRegex9S, dateRegex4, dateRegex6, dateRegex7L, dateRegex7S, dateRegexA } :
                new[] { dateRegex4, dateRegex6, dateRegex7L, dateRegex7S, dateRegex5, dateRegex8, dateRegex9L, dateRegex9S, dateRegexA });
        }

        public IEnumerable<Regex> DateRegexList { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IEnumerable<Regex> ImplicitDateList { get; }

        IImmutableDictionary<string, int> IDateExtractorConfiguration.DayOfWeek => DayOfWeek;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.MonthOfYear => MonthOfYear;

        bool IDateExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.WeekDayEnd => WeekDayEnd;

        Regex IDateExtractorConfiguration.WeekDayStart => WeekDayStart;

        Regex IDateExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateExtractorConfiguration.ForTheRegex => ForTheRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayOfMonthRegex => WeekDayAndDayOfMothRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayRegex => WeekDayAndDayRegex;

        Regex IDateExtractorConfiguration.RelativeMonthRegex => RelativeMonthRegex;

        Regex IDateExtractorConfiguration.StrictRelativeRegex => StrictRelativeRegex;

        Regex IDateExtractorConfiguration.WeekDayRegex => WeekDayRegex;

        Regex IDateExtractorConfiguration.PrefixArticleRegex => PrefixArticleRegex;

        Regex IDateExtractorConfiguration.YearSuffix => YearSuffix;

        Regex IDateExtractorConfiguration.LessThanRegex => LessThanRegex;

        Regex IDateExtractorConfiguration.MoreThanRegex => MoreThanRegex;

        Regex IDateExtractorConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDateExtractorConfiguration.SinceYearSuffixRegex => SinceYearSuffixRegex;

        Regex IDateExtractorConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDateExtractorConfiguration.RangeConnectorSymbolRegex => RangeConnectorSymbolRegex;

        Regex IDateExtractorConfiguration.BeforeAfterRegex => BeforeAfterRegex;
    }
}
