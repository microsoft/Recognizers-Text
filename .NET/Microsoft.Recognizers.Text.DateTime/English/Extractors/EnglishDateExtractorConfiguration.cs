using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.DateTime.English.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateExtractorConfiguration : BaseOptionsConfiguration, IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(DateTimeDefinitions.MonthRegex, RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexOptions.Singleline);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex SingleWeekDayRegex =
            new Regex(DateTimeDefinitions.SingleWeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex OnRegex =
            new Regex(DateTimeDefinitions.OnRegex, RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(DateTimeDefinitions.RelaxedOnRegex, RegexOptions.Singleline);

        public static readonly Regex ThisRegex =
            new Regex(DateTimeDefinitions.ThisRegex, RegexOptions.Singleline);

        public static readonly Regex LastDateRegex =
            new Regex(DateTimeDefinitions.LastDateRegex, RegexOptions.Singleline);

        public static readonly Regex NextDateRegex =
            new Regex(DateTimeDefinitions.NextDateRegex, RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex =
            new Regex(DateTimeDefinitions.SpecialDayRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeWeekDayRegex =
            new Regex(DateTimeDefinitions.RelativeWeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex SpecialDate =
            new Regex(DateTimeDefinitions.SpecialDate, RegexOptions.Singleline);

        public static readonly Regex SpecialDayWithNumRegex =
            new Regex(DateTimeDefinitions.SpecialDayWithNumRegex, RegexOptions.Singleline);

        public static readonly Regex ForTheRegex =
            new Regex(DateTimeDefinitions.ForTheRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayAndDayOfMothRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayOfMonthRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayAndDayRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexOptions.Singleline);

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexOptions.Singleline);

        public static readonly Regex OfMonth =
            new Regex(DateTimeDefinitions.OfMonth, RegexOptions.Singleline);

        public static readonly Regex MonthEnd =
            new Regex(DateTimeDefinitions.MonthEnd, RegexOptions.Singleline);

        public static readonly Regex WeekDayEnd =
            new Regex(DateTimeDefinitions.WeekDayEnd, RegexOptions.Singleline);

        public static readonly Regex YearSuffix =
            new Regex(DateTimeDefinitions.YearSuffix, RegexOptions.Singleline);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexOptions.Singleline);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexOptions.Singleline);

        public static readonly Regex SinceYearSuffixRegex =
            new Regex(DateTimeDefinitions.SinceYearSuffixRegex, RegexOptions.Singleline);

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexOptions.Singleline);

        public static readonly Regex RangeConnectorSymbolRegex =
            new Regex(Definitions.BaseDateTime.RangeConnectorSymbolRegex, RegexOptions.Singleline);

        public static readonly ImmutableDictionary<string, int> DayOfWeek =
            DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, int> MonthOfYear =
            DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.ImplicitDayRegex, RegexOptions.Singleline);

        public EnglishDateExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.English.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this));
            UtilityConfiguration = new EnglishDatetimeUtilityConfiguration();

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

            const RegexOptions dateRegexOption = RegexOptions.Singleline;

            // 3-23-2017
            var dateRegex4 = new Regex(DateTimeDefinitions.DateExtractor4, dateRegexOption);

            // 23-3-2015
            var dateRegex5 = new Regex(DateTimeDefinitions.DateExtractor5, dateRegexOption);

            // on (Sunday,)? 1.3
            var dateRegex6 = new Regex(DateTimeDefinitions.DateExtractor6, dateRegexOption);

            // on (Sunday,)? 24-12
            var dateRegex8 = new Regex(DateTimeDefinitions.DateExtractor8, dateRegexOption);

            // "(Sunday,)? 7/23, 2018", year part is required
            var dateRegex7L = new Regex(DateTimeDefinitions.DateExtractor7L, dateRegexOption);

            // "(Sunday,)? 7/23", year part is not required
            var dateRegex7S = new Regex(DateTimeDefinitions.DateExtractor7S, dateRegexOption);

            // "(Sunday,)? 23/7, 2018", year part is required
            var dateRegex9L = new Regex(DateTimeDefinitions.DateExtractor9L, dateRegexOption);

            // "(Sunday,)? 23/7", year part is not required
            var dateRegex9S = new Regex(DateTimeDefinitions.DateExtractor9S, dateRegexOption);

            // (Sunday,)? 2015-12-23
            var dateRegexA = new Regex(DateTimeDefinitions.DateExtractorA, dateRegexOption);

            DateRegexList = new List<Regex>
            {
                // ((this)? Sunday,)? April 5
                new Regex(DateTimeDefinitions.DateExtractor1, dateRegexOption),

                // ((this)? Sunday,)? April 5, 2016
                new Regex(DateTimeDefinitions.DateExtractor2, dateRegexOption),

                // (Sunday,)? 6th of April
                new Regex(DateTimeDefinitions.DateExtractor3, dateRegexOption),
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