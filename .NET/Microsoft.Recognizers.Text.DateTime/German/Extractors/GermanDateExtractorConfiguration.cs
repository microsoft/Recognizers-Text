using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Immutable;

using Microsoft.Recognizers.Text.DateTime.German.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateExtractorConfiguration : BaseOptionsConfiguration,IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(DateTimeDefinitions.MonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = 
            new Regex(DateTimeDefinitions.YearRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SingleWeekDayRegex =
            new Regex(DateTimeDefinitions.SingleWeekDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OnRegex = 
            new Regex(DateTimeDefinitions.OnRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(DateTimeDefinitions.RelaxedOnRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = 
            new Regex(DateTimeDefinitions.ThisRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastDateRegex = 
            new Regex(DateTimeDefinitions.LastDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextDateRegex = 
            new Regex(DateTimeDefinitions.NextDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex = 
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex =
            new Regex(DateTimeDefinitions.SpecialDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayWithNumRegex =
            new Regex(DateTimeDefinitions.SpecialDayWithNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeWeekDayRegex =
            new Regex(DateTimeDefinitions.RelativeWeekDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDate = 
            new Regex(DateTimeDefinitions.SpecialDate, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ForTheRegex =
            new Regex(DateTimeDefinitions.ForTheRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayAndDayOfMothRegex =
            new Regex(DateTimeDefinitions.WeekDayAndDayOfMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex =
            new Regex(
                DateTimeDefinitions.RelativeMonthRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrefixArticleRegex =
            new Regex(
                DateTimeDefinitions.PrefixArticleRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (Sonntag,)? 5. April 
            new Regex(DateTimeDefinitions.DateExtractor1, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sonntag,)? 5. April, 2016
            new Regex(DateTimeDefinitions.DateExtractor2, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sonntag,)? der 6. April, 2016
            new Regex(DateTimeDefinitions.DateExtractor3, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23.3.2017
            new Regex(DateTimeDefinitions.DateExtractor4, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23-3-2015
            new Regex(DateTimeDefinitions.DateExtractor5, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // am 1.3
            new Regex(DateTimeDefinitions.DateExtractor6, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 7/23
            new Regex(DateTimeDefinitions.DateExtractor7, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // am 24-12
            new Regex(DateTimeDefinitions.DateExtractor8, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23/7
            new Regex(DateTimeDefinitions.DateExtractor9, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            //Nächstes Jahr (im Sommer)?
            new Regex(DateTimeDefinitions.DateExtractor10, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 2015-12-23
            new Regex(DateTimeDefinitions.DateExtractorA, RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };


        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastDateRegex, NextDateRegex,
            SingleWeekDayRegex, WeekDayOfMonthRegex, SpecialDate
        };

        public static readonly Regex OfMonth = 
            new Regex(DateTimeDefinitions.OfMonth, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthEnd = 
            new Regex(DateTimeDefinitions.MonthEnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayEnd =
            new Regex(DateTimeDefinitions.WeekDayEnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearSuffix =
            new Regex(DateTimeDefinitions.YearSuffix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly ImmutableDictionary<string, int> DayOfWeek = 
            DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, int> MonthOfYear = 
            DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();

        public GermanDateExtractorConfiguration() : base(DateTimeOptions.None)
        {
            IntegerExtractor = Number.German.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.German.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new GermanNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
            UtilityConfiguration = new GermanDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        Regex IDateExtractorConfiguration.PrefixArticleRegex => PrefixArticleRegex;

        IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.DayOfWeek => DayOfWeek;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.MonthOfYear => MonthOfYear;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.WeekDayEnd => WeekDayEnd;

        Regex IDateExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateExtractorConfiguration.ForTheRegex => ForTheRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayOfMonthRegex => WeekDayAndDayOfMothRegex;

        Regex IDateExtractorConfiguration.RelativeMonthRegex => RelativeMonthRegex;

        Regex IDateExtractorConfiguration.WeekDayRegex => WeekDayRegex;

        Regex IDateExtractorConfiguration.YearSuffix => YearSuffix;

        Regex IDateExtractorConfiguration.LessThanRegex => LessThanRegex;

        Regex IDateExtractorConfiguration.MoreThanRegex => MoreThanRegex;
    }
}