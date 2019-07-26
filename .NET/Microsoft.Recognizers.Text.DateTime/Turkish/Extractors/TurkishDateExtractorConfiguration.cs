using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Text.DateTime.Turkish.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Turkish;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishDateExtractorConfiguration : BaseOptionsConfiguration, IDateExtractorConfiguration
    {

        public static readonly Regex MonthRegex =
            new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex SingleWeekDayRegex =
            new Regex(DateTimeDefinitions.SingleWeekDayRegex, RegexFlags);

        public static readonly Regex OnRegex =
            new Regex(DateTimeDefinitions.OnRegex, RegexFlags);

        public static readonly Regex RelaxedOnRegex =
            new Regex(DateTimeDefinitions.RelaxedOnRegex, RegexFlags);

        public static readonly Regex ThisRegex =
            new Regex(DateTimeDefinitions.ThisRegex, RegexFlags);

        public static readonly Regex LastDateRegex =
            new Regex(DateTimeDefinitions.LastDateRegex, RegexFlags);

        public static readonly Regex NextDateRegex =
            new Regex(DateTimeDefinitions.NextDateRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex =
            new Regex(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex RelativeWeekDayRegex =
            new Regex(DateTimeDefinitions.RelativeWeekDayRegex, RegexFlags);

        public static readonly Regex SpecialDate =
            new Regex(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex SpecialDayWithNumRegex =
            new Regex(DateTimeDefinitions.SpecialDayWithNumRegex, RegexFlags);

        public static readonly Regex ForTheRegex =
            new Regex(DateTimeDefinitions.ForTheRegex, RegexFlags);

        public static readonly Regex WeekDayAndDayOfMothRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayOfMonthRegex, RegexFlags);

        public static readonly Regex WeekDayAndDayRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex StrictRelativeRegex =
            new Regex(DateTimeDefinitions.StrictRelativeRegex, RegexFlags);

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        public static readonly Regex OfMonth =
            new Regex(DateTimeDefinitions.OfMonth, RegexFlags);

        public static readonly Regex MonthEnd =
            new Regex(DateTimeDefinitions.MonthEnd, RegexFlags);

        public static readonly Regex WeekDayEnd =
            new Regex(DateTimeDefinitions.WeekDayEnd, RegexFlags);

        public static readonly Regex YearSuffix =
            new Regex(DateTimeDefinitions.YearSuffix, RegexFlags);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex SinceYearSuffixRegex =
            new Regex(DateTimeDefinitions.SinceYearSuffixRegex, RegexFlags);

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex RangeConnectorSymbolRegex =
            new Regex(Definitions.BaseDateTime.RangeConnectorSymbolRegex, RegexFlags);

        public static readonly ImmutableDictionary<string, int> DayOfWeek =
            DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, int> MonthOfYear =
            DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.ImplicitDayRegex, RegexFlags);

        public TurkishDateExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            IntegerExtractor = Number.Turkish.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Turkish.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new TurkishNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new TurkishDurationExtractorConfiguration(this));
            UtilityConfiguration = new TurkishDatetimeUtilityConfiguration();

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
            var dateRegex4 = new Regex(DateTimeDefinitions.DateExtractor4, RegexFlags);

            // 23-3-2015
            var dateRegex5 = new Regex(DateTimeDefinitions.DateExtractor5, RegexFlags);

            // on (Sunday,)? 1.3
            var dateRegex6 = new Regex(DateTimeDefinitions.DateExtractor6, RegexFlags);

            // on (Sunday,)? 24-12
            var dateRegex8 = new Regex(DateTimeDefinitions.DateExtractor8, RegexFlags);

            // "(Sunday,)? 7/23, 2018", year part is required
            var dateRegex7L = new Regex(DateTimeDefinitions.DateExtractor7L, RegexFlags);

            // "(Sunday,)? 7/23", year part is not required
            var dateRegex7S = new Regex(DateTimeDefinitions.DateExtractor7S, RegexFlags);

            // "(Sunday,)? 23/7, 2018", year part is required
            var dateRegex9L = new Regex(DateTimeDefinitions.DateExtractor9L, RegexFlags);

            // "(Sunday,)? 23/7", year part is not required
            var dateRegex9S = new Regex(DateTimeDefinitions.DateExtractor9S, RegexFlags);

            // (Sunday,)? 2015-12-23
            var dateRegexA = new Regex(DateTimeDefinitions.DateExtractorA, RegexFlags);

            DateRegexList = new List<Regex>
            {
                // (Sunday,)? April 5 or (Sunday,)? April 5, 2016
                new Regex(DateTimeDefinitions.DateExtractor1, RegexFlags),

                // (Sunday,)? 6th of April
                new Regex(DateTimeDefinitions.DateExtractor3, RegexFlags),
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

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IEnumerable<Regex> ImplicitDateList { get; }

        IImmutableDictionary<string, int> IDateExtractorConfiguration.DayOfWeek => DayOfWeek;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.MonthOfYear => MonthOfYear;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.WeekDayEnd => WeekDayEnd;

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
    }
}