using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.German.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            RegexCache.Get(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex DayRegex =
            RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

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

        public static readonly Regex SpecialDayWithNumRegex =
            RegexCache.Get(DateTimeDefinitions.SpecialDayWithNumRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex RelativeWeekDayRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeWeekDayRegex, RegexFlags);

        public static readonly Regex SpecialDate =
            RegexCache.Get(DateTimeDefinitions.SpecialDate, RegexFlags);

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

        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastDateRegex, NextDateRegex,
            SingleWeekDayRegex, WeekDayOfMonthRegex, SpecialDate,
        };

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

        public static readonly ImmutableDictionary<string, int> DayOfWeek =
            DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, int> MonthOfYear =
            DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();

        public static readonly Regex BeforeAfterRegex =
            RegexCache.Get(DateTimeDefinitions.BeforeAfterRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanDateExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.German.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.German.OrdinalExtractor.GetInstance();
            NumberParser = new BaseNumberParser(new GermanNumberParserConfiguration(new BaseNumberOptionsConfiguration(numConfig)));

            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration(this));
            UtilityConfiguration = new GermanDatetimeUtilityConfiguration();

            // 3-23-2017
            var dateRegex4 = RegexCache.Get(DateTimeDefinitions.DateExtractor4, RegexFlags);

            // 23-3-2015
            var dateRegex5 = RegexCache.Get(DateTimeDefinitions.DateExtractor5, RegexFlags);

            // am 1.3
            var dateRegex6 = RegexCache.Get(DateTimeDefinitions.DateExtractor6, RegexFlags);

            // am 24-12
            var dateRegex8 = RegexCache.Get(DateTimeDefinitions.DateExtractor8, RegexFlags);

            // 7/23
            var dateRegex7 = RegexCache.Get(DateTimeDefinitions.DateExtractor7, RegexFlags);

            // 23/7
            var dateRegex9 = RegexCache.Get(DateTimeDefinitions.DateExtractor9, RegexFlags);

            // Nächstes Jahr (im Sommer)?
            var dateRegex10 = RegexCache.Get(DateTimeDefinitions.DateExtractor10, RegexFlags);

            // 2015-12-23
            var dateRegexA = RegexCache.Get(DateTimeDefinitions.DateExtractorA, RegexFlags);

            DateRegexList = new List<Regex>
            {
                // (Sonntag,)? 5. April
                RegexCache.Get(DateTimeDefinitions.DateExtractor1, RegexFlags),

                // (Sonntag,)? 5. April, 2016
                RegexCache.Get(DateTimeDefinitions.DateExtractor2, RegexFlags),

                // (Sonntag,)? der 6. April, 2016
                RegexCache.Get(DateTimeDefinitions.DateExtractor3, RegexFlags),
            };

            var enableDmy = DmyDateFormat ||
                            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;

            DateRegexList = DateRegexList.Concat(enableDmy ?
                new[] { dateRegex5, dateRegex8, dateRegex9, dateRegex4, dateRegex6, dateRegex7, dateRegex10, dateRegexA } :
                new[] { dateRegex4, dateRegex6, dateRegex7, dateRegex5, dateRegex8, dateRegex9, dateRegex10, dateRegexA });
        }

        public IEnumerable<Regex> DateRegexList { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        Regex IDateExtractorConfiguration.PrefixArticleRegex => PrefixArticleRegex;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

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