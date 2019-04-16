using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Italian.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDateExtractorConfiguration : BaseOptionsConfiguration, IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(
                DateTimeDefinitions.MonthRegex,
                RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                DateTimeDefinitions.DayRegex,
                RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(
                DateTimeDefinitions.MonthNumRegex,
                RegexOptions.Singleline);

        public static readonly Regex YearRegex =
            new Regex(
                DateTimeDefinitions.YearRegex,
                RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                DateTimeDefinitions.WeekDayRegex,
                RegexOptions.Singleline);

        public static readonly Regex OnRegex =
            new Regex(
                DateTimeDefinitions.OnRegex,
                RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(
                DateTimeDefinitions.RelaxedOnRegex,
                RegexOptions.Singleline);

        public static readonly Regex ThisRegex =
            new Regex(
                DateTimeDefinitions.ThisRegex,
                RegexOptions.Singleline);

        public static readonly Regex LastRegex =
            new Regex(
                DateTimeDefinitions.LastDateRegex,
                RegexOptions.Singleline);

        public static readonly Regex NextRegex =
            new Regex(
                DateTimeDefinitions.NextDateRegex,
                RegexOptions.Singleline);

        public static readonly Regex UnitRegex =
            new Regex(
                DateTimeDefinitions.DateUnitRegex,
                RegexOptions.Singleline);

        // day before yesterday, day after tomorrow, next day, last day, the day yesterday, the day tomorrow
        public static readonly Regex SpecialDayRegex =
            new Regex(
                DateTimeDefinitions.SpecialDayRegex,
                RegexOptions.Singleline);

        public static readonly Regex SpecialDayWithNumRegex =
            new Regex(
                DateTimeDefinitions.SpecialDayWithNumRegex,
                RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex =
            new Regex(
                DateTimeDefinitions.DateUnitRegex,
                RegexOptions.Singleline);

        public static readonly Regex StrictWeekDay =
            new Regex(
                DateTimeDefinitions.StrictWeekDay,
                RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                DateTimeDefinitions.WeekDayOfMonthRegex,
                RegexOptions.Singleline);

        public static readonly Regex SpecialDate =
            new Regex(
                DateTimeDefinitions.SpecialDate,
                RegexOptions.Singleline);

        public static readonly Regex RelativeWeekDayRegex =
            new Regex(
                DateTimeDefinitions.RelativeWeekDayRegex,
                RegexOptions.Singleline);

        public static readonly Regex ForTheRegex =
            new Regex(DateTimeDefinitions.ForTheRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayAndDayOfMothRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayOfMonthRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayAndDayRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex =
            new Regex(
                DateTimeDefinitions.RelativeMonthRegex,
                RegexOptions.Singleline);

        public static readonly Regex PrefixArticleRegex =
            new Regex(
                DateTimeDefinitions.PrefixArticleRegex,
                RegexOptions.Singleline);

        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            StrictWeekDay, WeekDayOfMonthRegex, SpecialDate,
        };

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

        public static readonly Regex NonDateUnitRegex =
            new Regex(@"(?<unit>heure|heures|hrs|secondes|seconde|secs|sec|minutes|minute|mins)\b", RegexOptions.Singleline);

        public ItalianDateExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            IntegerExtractor = Number.Italian.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Italian.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new ItalianNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            UtilityConfiguration = new ItalianDatetimeUtilityConfiguration();

            const RegexOptions dateRegexOption = RegexOptions.Singleline;

            // 3-23-2017
            var dateRegex4 = new Regex(DateTimeDefinitions.DateExtractor4, dateRegexOption);

            // 23-3-2015
            var dateRegex5 = new Regex(DateTimeDefinitions.DateExtractor5, dateRegexOption);

            // on 1.3
            var dateRegex6 = new Regex(DateTimeDefinitions.DateExtractor6, dateRegexOption);

            // on 24-12
            var dateRegex8 = new Regex(DateTimeDefinitions.DateExtractor8, dateRegexOption);

            // 7/23
            var dateRegex7 = new Regex(DateTimeDefinitions.DateExtractor7, dateRegexOption);

            // 23/7
            var dateRegex9 = new Regex(DateTimeDefinitions.DateExtractor9, dateRegexOption);

            // 2015-12-23
            var dateRegexA = new Regex(DateTimeDefinitions.DateExtractorA, dateRegexOption);

            DateRegexList = new List<Regex>
            {
                // (Sunday,)? April 5
                new Regex(DateTimeDefinitions.DateExtractor1, dateRegexOption),

                // (Sunday,)? April 5, 2016
                new Regex(DateTimeDefinitions.DateExtractor2, dateRegexOption),

                // (Sunday,)? 6th of April
                new Regex(DateTimeDefinitions.DateExtractor3, dateRegexOption),
            };

            var enableDmy = DmyDateFormat ||
                            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;

            DateRegexList = DateRegexList.Concat(enableDmy
                ? new[] { dateRegex5, dateRegex8, dateRegex9, dateRegex4, dateRegex6, dateRegex7, dateRegexA }
                : new[] { dateRegex4, dateRegex6, dateRegex7, dateRegex5, dateRegex8, dateRegex9, dateRegexA });
        }

        public IEnumerable<Regex> DateRegexList { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.DayOfWeek => DayOfWeek;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.MonthOfYear => MonthOfYear;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.WeekDayEnd => WeekDayEnd;

        Regex IDateExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayOfMonthRegex => WeekDayAndDayOfMothRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayRegex => WeekDayAndDayRegex;

        Regex IDateExtractorConfiguration.ForTheRegex => ForTheRegex;

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
